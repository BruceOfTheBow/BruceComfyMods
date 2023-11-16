using HarmonyLib;

using static HomeFurnishings.PluginConfig;

namespace HomeFurnishings.Patches {
  [HarmonyPatch(typeof(Hud))]
  public class HudPatch {
    private static int _lastPieceHashCode;

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
        _lastPieceHashCode = "".GetStableHashCode();
        return;
      }

      if (__instance.m_hoveredPiece && piece.m_comfort != 0) {
        ComfortPanelManager.ToggleOn(piece);
      }
    }

    //[HarmonyPostfix]
    //[HarmonyPatch(nameof(Hud.UpdateBuild))]
    //static void UpdateBuildPostfix(ref Hud __instance, Piece piece) {
      
    //}
  }
}
