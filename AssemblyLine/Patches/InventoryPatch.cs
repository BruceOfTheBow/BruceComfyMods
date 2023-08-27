using HarmonyLib;

using static AssemblyLine.Patches.InventoryGuiPatch;

namespace AssemblyLine.Patches {
  [HarmonyPatch(typeof(Inventory))]
  internal class InventoryPatch {
    [HarmonyPostfix]
    [HarmonyPatch(nameof(Inventory.Changed))]
    public static void OnInventoryChangedPostfix(Inventory __instance) {
      if (!InventoryGui.IsVisible() && Player.m_localPlayer == null) {
        return;
      }
      SetRequirementText();
    }
  }
}
