namespace Hygge;

using HarmonyLib;

using static PluginConfig;

[HarmonyPatch(typeof(Hud))]
public class HudPatch {

  [HarmonyPostfix]
  [HarmonyPatch(nameof(Hud.SetupPieceInfo))]
  static void SetupPieceInfoPostfix(Hud __instance, Piece piece) {
    if (IsModEnabled.Value) {
      ProcessPieceInfo(piece, __instance.m_hoveredPiece, Player.m_localPlayer);
    }
  }

  static void ProcessPieceInfo(Piece piece, Piece hoveredPiece, Player player) {
    if (piece && piece.m_comfort == 0) {
      ComfortPanelManager.ToggleOff();
      return;
    }

    if (hoveredPiece && hoveredPiece.m_comfort != 0) {
      ComfortPanelManager.ToggleOn(hoveredPiece);
      ComfortPanelManager.Update(hoveredPiece);
      return;
    }

    if (player
        && player.m_placementGhost
        && player.m_placementGhost.TryGetComponent(out Piece placementPiece)
        && placementPiece.m_comfort != 0) {
      ComfortPanelManager.ToggleOn(placementPiece);
      ComfortPanelManager.Update(placementPiece);
      return;
    }

    ComfortPanelManager.ToggleOff();
  }

  [HarmonyPostfix]
  [HarmonyPatch(nameof(Hud.OnDestroy))]
  static void OnDestroyPostfix(Hud __instance) {
    if (IsModEnabled.Value) {
      ComfortPanelManager.DestroyPanel();
    }
  }
}
