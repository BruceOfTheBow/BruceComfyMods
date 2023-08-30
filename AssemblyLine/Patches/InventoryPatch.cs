using HarmonyLib;

using static AssemblyLine.Patches.InventoryGuiPatch;

namespace AssemblyLine.Patches {
  [HarmonyPatch(typeof(Inventory))]
  internal class InventoryPatch {
    [HarmonyPostfix]
    [HarmonyPatch(nameof(Inventory.Changed))]
    public static void OnInventoryChangedPostfix(Inventory __instance) {
      if (Player.m_localPlayer == null 
          || Player.m_localPlayer.GetInventory() != __instance 
          || !InventoryGui.IsVisible()) {

        return;
      }

      SetRequirementText();
    }
  }
}
