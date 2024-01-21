using HarmonyLib;

using System.Collections.Generic;

using static Pintervention.Pintervention;
using static Pintervention.PluginConfig;

namespace Pintervention {
  [HarmonyPatch(typeof(Minimap))]
  static class MinimapPatch {
    static HashSet<string> _generatedPinNames = new() {
      "StartTemple",
      "Vendor_BlackForest",
      "Hildir_camp",
      "$enemy_eikthyr",
      "$enemy_gdking",
      "$enemy_bonemass",
      "$enemy_dragon",
      "$enemy_goblinking",
      "$enemy_queen",
      "$hud_pin_hildir1",
      "$hud_pin_hilder2",
      "$hud_pin_hildir3"
    };

    [HarmonyPostfix]
    [HarmonyPatch(nameof(Minimap.Update))]
    static void UpdatePostfix(Minimap __instance) {
      if (!IsModEnabled.Value 
            || !Minimap.IsOpen()
            || !__instance
            || !__instance.m_largeRoot
            || !DisplayFilterPanel.Value.IsDown()) {

        return;
      }

      ForeignPinManager.Initialize();
      PlayerFilterPanelManager.ToggleFilterPanel();
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(Minimap.UpdatePins))]
    static void UpdatePinsPostfix(Minimap __instance) {
      if (!IsModEnabled.Value
            || !Minimap.IsOpen()
            || !__instance
            || !__instance.m_largeRoot) {

        return;
      }

      ForeignPinManager.FilterPins();
      PlayerFilterPanelManager.UpdatePinCounts();
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(Minimap.AddPin))]
    static void AddPinPostfix(Minimap __instance, Minimap.PinData __result) {
      if (!IsModEnabled.Value
            || !__instance
            || !Player.m_localPlayer
            || !ForeignPinManager.IsLocationPin(__result)
            || __result.m_ownerID != 0L) {

        return;
      }

      __result.m_ownerID = Player.m_localPlayer.GetPlayerID();
    }
  }
}
