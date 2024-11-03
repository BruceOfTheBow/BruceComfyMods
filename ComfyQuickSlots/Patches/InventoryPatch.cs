namespace ComfyQuickSlots;

using HarmonyLib;

[HarmonyPatch(typeof(Inventory))]
static class InventoryPatch {
  [HarmonyPrefix]
  [HarmonyPatch(nameof(Inventory.AddItem), typeof(ItemDrop.ItemData), typeof(int), typeof(int), typeof(int))]
  static bool AddItemPrefix(Inventory __instance, ItemDrop.ItemData item, int amount, int x, int y) {
    if (item != null) {
      if (__instance.m_name == "ComfyQuickSlotsInventory") {
        if (QuickSlotsManager.FirstLoad && QuickSlotsManager.IsArmor(item) && item.m_equipped) {
          if (!QuickSlotsManager.InitialEquippedArmor.Contains((item))) {
            QuickSlotsManager.InitialEquippedArmor.Add(item);
          }
          return false;
        }

        if (item.m_equipped && QuickSlotsManager.IsArmor(item) && Player.m_localPlayer != null) {
          QuickSlotsManager.UnequipItem(Player.m_localPlayer, item);
          Vector2i armorSlot = QuickSlotsManager.GetArmorSlot(item);
          if (x == armorSlot.x && y == armorSlot.y) {
            return true;
          }
          return false;
        }

        if (Player.m_localPlayer == null) {
          return true;
        }

        if (x < 5 && y == 4) {
          return false;
        }
      }
    }

    return true;
  }

  [HarmonyPrefix]
  [HarmonyPatch(
      nameof(Inventory.MoveItemToThis),
      typeof(Inventory), typeof(ItemDrop.ItemData), typeof(int), typeof(int), typeof(int))]
  static bool MoveItemToThisPrefix(Inventory __instance, ref bool __result, int x, int y) {
    if (__instance.m_name == "ComfyQuickSlotsInventory") {
      if (x < 5 && y == 4) {
        return false;
      }
    }

    return true;
  }

  [HarmonyPrefix]
  [HarmonyPatch(nameof(Inventory.FindEmptySlot))]
  static bool FindEmptySlotPrefix(Inventory __instance, ref Vector2i __result, bool topFirst) {
    if (__instance.m_name == "ComfyQuickSlotsInventory") {
      __result = QuickSlotsManager.GetEmptyInventorySlot(__instance, topFirst);
      return false;
    }

    return true;
  }

  [HarmonyPrefix]
  [HarmonyPatch(nameof(Inventory.HaveEmptySlot))]
  public static bool HaveEmptySlotPrefix(Inventory __instance, ref bool __result) {
    if (__instance.m_name == "ComfyQuickSlotsInventory") {
      __result = QuickSlotsManager.HaveEmptyInventorySlot(__instance);
      return false;
    }

    return true;
  }
}
