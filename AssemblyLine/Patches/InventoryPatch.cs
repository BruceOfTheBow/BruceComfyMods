namespace AssemblyLine;

using HarmonyLib;

[HarmonyPatch(typeof(Inventory))]
static class InventoryPatch {
  [HarmonyPostfix]
  [HarmonyPatch(nameof(Inventory.Changed))]
  static void OnInventoryChangedPostfix(Inventory __instance) {
    if (Player.m_localPlayer == null
        || Player.m_localPlayer.GetInventory() != __instance
        || !InventoryGui.IsVisible()) {
      return;
    }

    CraftingManager.SetRequirementText();
  }
}
