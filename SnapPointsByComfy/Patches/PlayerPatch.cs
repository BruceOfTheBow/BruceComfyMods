namespace SnapPointsByComfy;

using System.Collections.Generic;

using HarmonyLib;

using UnityEngine;

using static PluginConfig;
using static SnapPointsByComfy;


[HarmonyPatch(typeof(Player))]
static class PlayerPatch {
  [HarmonyPostfix]
  [HarmonyPatch(nameof(Player.PieceRayTest))]
  public static void PieceRayTestPostfix(Player __instance, ref bool __result, ref Vector3 point, ref Vector3 normal, ref Piece piece, ref Heightmap heightmap, ref Collider waterSurface, bool water) {
    SnapManager.LastRayPiece = piece;
  }

  [HarmonyPostfix]
  [HarmonyPatch(nameof(Player.UpdatePlacementGhost))]
  private static void UpdatePlacementGhostPostfix(GameObject ___m_placementGhost, bool flashGuardStone) {
    if (ZInput.GetKeyDown(EnableModToggleKey.Value.MainKey)) {
      ToggleModEnabled();
    }

    if (!IsModEnabled.Value || ___m_placementGhost == null) {
      return;
    }

    if (ZInput.GetKeyDown(SnapModeToggleKey.Value.MainKey)) {
      SnapManager.ToggleSnapMode();
      SendModeChangeMessage();
    }

    Piece selectedPiece = ___m_placementGhost.GetComponent<Piece>();

    if (selectedPiece == null) {
      return;
    }

    if (!SnapManager.LastRayPiece) {
      return;
    }

    if (SnapManager.Target != SnapManager.LastRayPiece.transform) {
      SnapManager.SetTarget(SnapManager.LastRayPiece.transform);

      if (ResetOnPieceChange.Value) {
        SnapManager.ResetTargetSnapPoint();
      }

      SnapManager.ClampTargetSnapPoint();
    }

    
    
    if (SnapManager.Source != selectedPiece.transform ) {
      SnapManager.SetSource(selectedPiece.transform);

      if (ResetOnPieceChange.Value) {
        SnapManager.ResetSourceSnapPoint();
      }

      SnapManager.ClampSourceSnapPoint();
    }

    if (!SnapManager.IsValidSnapPoints()) {
      return;
    }

    if (ZInput.GetKeyDown(IterateSourceSnapPointKey.Value.MainKey)) {
      SnapManager.NextSourceSnapPoint();
    }

    if (ZInput.GetKeyDown(IterateTargetSnapPointKey.Value.MainKey)) {
      SnapManager.NextTargetSnapPoint();
    }

    ___m_placementGhost.transform.position += SnapManager.GetOffset();
  }
}
