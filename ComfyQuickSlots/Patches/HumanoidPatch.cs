namespace ComfyQuickSlots;

using HarmonyLib;

[HarmonyPatch(typeof(Humanoid))]
static class HumanoidPatch {
  [HarmonyPrefix]
  [HarmonyPatch(nameof(Humanoid.EquipItem))]
  static bool EquipItemPrefix(Humanoid __instance, ref bool __result, ItemDrop.ItemData item) {
    if (QuickSlotsManager.IsArmor(item)) {
      Vector2i armorSlot = QuickSlotsManager.GetArmorSlot(item);

      if (QuickSlotsManager.IsArmorTypeEquipped(__instance, item)) {
        ItemDrop.ItemData swapItem = QuickSlotsManager.GetArmorItemToSwap(__instance, item);
        QuickSlotsManager.UnequipItem(__instance, swapItem);
        QuickSlotsManager.EquipItem(__instance, item);
        QuickSlotsManager.MoveArmorItemToSlot(__instance, item, armorSlot.x, armorSlot.y);

        __result = true;
        return false;
      }

      QuickSlotsManager.EquipItem(__instance, item);

      if (!(item.m_gridPos.x == armorSlot.x && item.m_gridPos.y == armorSlot.y)) {
        QuickSlotsManager.MoveArmorItemToSlot(__instance, item, armorSlot.x, armorSlot.y);
      }

      __result = true;
      return false;
    }

    return true;
  }

  [HarmonyPostfix]
  [HarmonyPatch(nameof(Humanoid.EquipItem))]
  static void EquipItemPostfix(Humanoid __instance, ItemDrop.ItemData item) {
    if (__instance != null && item != null) {
      __instance.GetInventory().Changed();
    }
  }

  [HarmonyPrefix]
  [HarmonyPatch(nameof(Humanoid.UnequipItem))]
  static bool UnequipItemPrefix(Humanoid __instance, ItemDrop.ItemData item, bool triggerEquipEffects) {
    if (item != null) {
      if (QuickSlotsManager.IsArmor(item) && item.m_equipped) {
        if (!QuickSlotsManager.HaveEmptyInventorySlot(__instance.GetInventory())) {
          __instance.Message(MessageHud.MessageType.Center, "Inventory full. Item not unequipped.");
          return false;
        }

        QuickSlotsManager.UnequipItem(__instance, item);
        Vector2i emptyLoc = QuickSlotsManager.GetEmptyInventorySlot(__instance.GetInventory(), true);
        QuickSlotsManager.MoveArmorItemToSlot(__instance, item, emptyLoc.x, emptyLoc.y);
        return false;
      }
    }
    return true;
  }
}
