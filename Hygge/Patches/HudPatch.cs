using HarmonyLib;

using static Hygge.PluginConfig;

namespace Hygge.Patches {
  [HarmonyPatch(typeof(Hud))]
  public class HudPatch {

    [HarmonyPostfix]
    [HarmonyPatch(nameof(Hud.SetupPieceInfo))]
    static void SetupPieceInfoPostfix(ref Hud __instance, Piece piece) {
      if (!IsModEnabled.Value) {
        return;
      }

      if (piece && piece.m_comfort == 0) {
        ComfortPanelManager.ToggleOff();
        return;
      }

      if (__instance.m_hoveredPiece &&  __instance.m_hoveredPiece.m_comfort != 0) {
        ComfortPanelManager.ToggleOn(__instance.m_hoveredPiece);
        ComfortPanelManager.Update(__instance.m_hoveredPiece);

        return;
      }

      if (Player.m_localPlayer
          && Player.m_localPlayer.m_placementGhost
          && Player.m_localPlayer.m_placementGhost.TryGetComponent(out Piece placePiece)
          && placePiece.m_comfort != 0) {

        ComfortPanelManager.ToggleOn(placePiece);
        ComfortPanelManager.Update(placePiece);

        return;
      }

      ComfortPanelManager.ToggleOff();
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(Hud.OnDestroy))]
    static void OnDestroyPostfix(ref Hud __instance) {
      if (!IsModEnabled.Value || !ComfortPanelManager.ComfortPanel?.Panel) {
        return;
      }

      ComfortPanelManager.DestroyPanel();
    }
  }
}
