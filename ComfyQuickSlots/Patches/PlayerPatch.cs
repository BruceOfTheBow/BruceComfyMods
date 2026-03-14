namespace ComfyQuickSlots;

using HarmonyLib;

[HarmonyPatch(typeof(Player))]
static class PlayerPatch {
  [HarmonyPrefix]
  [HarmonyPatch(nameof(Player.Awake))]
  static void AwakePrefix(Player __instance) {
    QuickSlotsManager.SetupPlayerInventory(__instance.m_inventory);
  }


  [HarmonyPostfix]
  [HarmonyPatch(nameof(Player.Load))]
  static void LoadPostFix(Player __instance) {
    if (__instance.m_knownTexts.ContainsKey(QuickSlotsManager.PlayerDataKey)) {
      ZPackage pkg = new(__instance.m_knownTexts[QuickSlotsManager.PlayerDataKey]);
      __instance.GetInventory().Load(pkg);

      // Clear stale equipment references left over from the native Player.Load.
      // inventory.Load() replaced all ItemData objects, so the old equipment refs
      // now point to orphaned instances. Without clearing these, mods like
      // AdventureBackpacks bind their data to the wrong item, causing backpack
      // contents to be lost on save/load.
      __instance.m_helmetItem = null;
      __instance.m_chestItem = null;
      __instance.m_legItem = null;
      __instance.m_shoulderItem = null;
      __instance.m_utilityItem = null;

      // Equip armor via Humanoid.EquipItem (instead of QuickSlotsManager.EquipItem)
      // so that all Harmony postfixes from other mods fire correctly.
      for (int i = 0; i < 5; i++) {
        var item = __instance.GetInventory().GetItemAt(i, 4);
        if (item != null) {
          __instance.EquipItem(item);
        }
      }

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

  [HarmonyPrefix]
  [HarmonyPatch(nameof(Player.CreateTombStone))]
  static void CreateTombStonePrefix(Player __instance) {
    TombStoneManager.CreateTombStone(__instance);
  }

  [HarmonyPostfix]
  [HarmonyPatch(nameof(Player.CreateTombStone))]
  static void CreateTombStonePostfix(Player __instance) {
    Inventory playerInventory = __instance.GetInventory();

    playerInventory.m_height = QuickSlotsManager.Rows;
    playerInventory.m_width = QuickSlotsManager.Columns;
  }
}
