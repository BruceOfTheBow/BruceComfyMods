namespace ComfyQuickSlots;

using HarmonyLib;


[HarmonyPatch(typeof(Player))]
static class PlayerPatch {
  [HarmonyPrefix]
  [HarmonyPatch(nameof(Player.Awake))]
  static void AwakePrefix(Player __instance) {
    QuickSlotsManager.SetupPlayerInventory(__instance);
  }

  [HarmonyPostfix]
  [HarmonyPatch(nameof(Player.Load))]
  static void LoadPostFix(Player __instance) {
    QuickSlotsManager.SetupPlayerInventory(__instance);
    QuickSlotsManager.LoadSavedData(__instance);
  }

  [HarmonyPrefix]
  [HarmonyPatch(nameof(Player.Save))]
  static bool SavePrefix(Player __instance) {
    QuickSlotsManager.FirstLoad = false;

    return QuickSlotsManager.Save(__instance);
  }

  [HarmonyPostfix]
  [HarmonyPatch(nameof(Player.SetInventorySize))]
  static void SetInventorySizePostfix(Player __instance, int rows) {
    QuickSlotsManager.MoveEquippedArmorToArmorSlots();
    QuickSlotsManager.ShouldRefreshPlayerGrid = true;

    if (QuickSlotsManager.IsPurchasingItem) {
      QuickSlotsManager.MoveQuickSlotItems(__instance);
      return;
    }
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
    if (ZoneSystem.instance.GetGlobalKey(GlobalKeys.DeathKeepEquip) || ZoneSystem.instance.GetGlobalKey(GlobalKeys.DeathDeleteUnequipped)) {
      return;
    }

    QuickSlotsManager.UnequipAllArmor(__instance);
  }
}
