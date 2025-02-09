namespace KnightsTemplar;

using HarmonyLib;

using static PluginConfig;

[HarmonyPatch(typeof(InventoryGui))]
static class InventoryGuiPatch {
  [HarmonyPostfix]
  [HarmonyPatch(nameof(InventoryGui.CanRepair))]
  static void CanRepairPostfix(ItemDrop.ItemData item, ref bool __result) {
    if (!__result && IsModEnabled.Value && RepairController.CanRepairItem(Player.m_localPlayer, item)) {
      __result = true;
    }
  }
}
