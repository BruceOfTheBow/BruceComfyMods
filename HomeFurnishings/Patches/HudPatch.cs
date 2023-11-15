using HarmonyLib;

using static HomeFurnishings.PluginConfig;

namespace HomeFurnishings.Patches {
  [HarmonyPatch(typeof(Hud))]
  public class HudPatch {
    [HarmonyPostfix]
    [HarmonyPatch(nameof(Hud.SetupPieceInfo))]
    static void SetupPieceInfoPostfix(ref Hud __instance, Piece piece) {
      if (!IsModEnabled.Value 
          || !__instance
          || !__instance.m_buildHud) {

        return;
      }

      if (!__instance.m_hoveredPiece) {
        ComfortPanelManager.ToggleOff();
        return;
      }

      if (__instance.m_hoveredPiece) {
        ComfortPanelManager.ToggleOn(piece);
      }
    }
  }
}
