using HarmonyLib;

using static Pintervention.PluginConfig;

namespace Pintervention {
  [HarmonyPatch(typeof(Minimap))]
  static class MinimapPatch {
    [HarmonyPostfix]
    [HarmonyPatch(nameof(Minimap.Update))]
    static void UpdatePostfix(Minimap __instance) {
      if (!IsModEnabled.Value 
            || !__instance
            || !DisplayFilterPanel.Value.IsDown()) {

        return;
      }

      PlayerFilterPanelManager.ToggleFilterPanel();
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(Minimap.UpdatePins))]
    static void UpdatePinsPostfix(Minimap __instance) {
      if (!IsModEnabled.Value
            || !__instance) {

        return;
      }

      ForeignPinManager.FilterPins();
    }
  }
}
