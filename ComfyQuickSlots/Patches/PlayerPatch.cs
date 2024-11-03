namespace ComfyQuickSlots;

using HarmonyLib;

[HarmonyPatch(typeof(Player))]
static class PlayerPatch {
  [HarmonyPrefix]
  [HarmonyPatch(nameof(Player.Awake))]
  static void AwakePrefix(ref Player __instance) {
    __instance.m_inventory.m_name = "ComfyQuickSlotsInventory";
    __instance.m_inventory.m_height = QuickSlotsManager.Rows;
    __instance.m_inventory.m_width = QuickSlotsManager.Columns;
  }

  [HarmonyPostfix]
  [HarmonyPatch(nameof(Player.Load))]
  static void LoadPostFix(Player __instance) {
    if (__instance.m_knownTexts.ContainsKey(QuickSlotsManager.PlayerDataKey)) {
      ZPackage pkg = new(__instance.m_knownTexts[QuickSlotsManager.PlayerDataKey]);
      __instance.GetInventory().Load(pkg);
      QuickSlotsManager.EquipArmorInArmorSlots(__instance);
      __instance.GetInventory().Changed();
    } else {
      QuickSlotsManager.FirstLoad = true;

      foreach (ItemDrop.ItemData armorPiece in QuickSlotsManager.InitialEquippedArmor) {
        QuickSlotsManager.UnequipItem(__instance, armorPiece);
        __instance.GetInventory().AddItem(armorPiece);
        __instance.EquipItem(armorPiece);
        Vector2i armorSlot = QuickSlotsManager.GetArmorSlot(armorPiece);
        QuickSlotsManager.MoveArmorItemToSlot(__instance, armorPiece, armorSlot.x, armorSlot.y);
        __instance.GetInventory().Changed();
      }

      QuickSlotsManager.InitialEquippedArmor.Clear();
    }

    foreach (ItemDrop.ItemData item in __instance.GetInventory().m_inventory) {
      if (item.IsEquipable() && !QuickSlotsManager.IsArmor(item) && item.m_equipped) {
        __instance.EquipItem(item);
      }
    }
  }

  [HarmonyPrefix]
  [HarmonyPatch(nameof(Player.Save))]
  static bool SavePrefix(Player __instance) {
    QuickSlotsManager.FirstLoad = false;

    return QuickSlotsManager.Save(__instance);
  }

  // Prevents interaction with item stands and armor stands while item is equipping
  [HarmonyPrefix]
  [HarmonyPatch(nameof(Player.UseHotbarItem))]
  static bool UseHotbarItemPrefix(Player __instance, int index) {
    ItemDrop.ItemData itemAt = __instance.m_inventory.GetItemAt(index - 1, 0);
    if (__instance.IsEquipActionQueued(itemAt)) {
      return false;
    }

    return true;
  }
}
