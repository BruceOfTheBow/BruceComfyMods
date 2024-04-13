using System;
using System.Collections.Generic;
using System.Reflection.Emit;

using HarmonyLib;

using UnityEngine;

using static ComfyGizmo.PluginConfig;

namespace ComfyGizmo.Patches {
  [HarmonyPatch(typeof(Player))]
  static class PlayerPatch {
    [HarmonyPrefix]
    [HarmonyPatch(nameof(Player.UpdatePlacementGhost))]
    static void UpdatePlacementGhostPrefix(ref Player __instance) {
      if (!Input.GetKeyDown(SelectTargetPieceKey.Value.MainKey)
            || !__instance 
            || !__instance.m_buildPieces 
            || __instance.m_buildPieces.m_availablePieces == null
            || !__instance.GetHoveringPiece()) {

        return;
      }

      HammerTableManager.SelectTargetPiece(__instance);
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(Player.UpdatePlacement))]
    static void UpdatePlacementPostfix(ref Player __instance, ref bool takeInput) {
      RotationManager.HideGizmos();
      RotationManager.ShowGizmos(__instance);

      if (!__instance.m_buildPieces
            || !takeInput
            || Hud.IsPieceSelectionVisible()) {
        return;
      }

      if (Input.GetKeyDown(SnapDivisionIncrementKey.Value.MainKey)) {
        RotationManager.IncreaseSnapDivisions();
      }

      if (Input.GetKeyDown(SnapDivisionDecrementKey.Value.MainKey)) {
        RotationManager.DecreaseSnapDivisions();
      }

      if (Input.GetKeyDown(CopyPieceRotationKey.Value.MainKey) && __instance.m_hoveringPiece != null) {
        RotationManager.MatchPieceRotation(__instance.m_hoveringPiece);
      }

      if (Input.GetKeyDown(ChangeRotationModeKey.Value.MainKey)) {
        IsLocalFrameEnabled.Value = !IsLocalFrameEnabled.Value;
      }

      RotationManager.ResetScales();

      if (Input.GetKey(ResetAllRotationKey.Value.MainKey)) {
        RotationManager.ResetRotation();
        return;
      }

      Vector3 rotationAxis = GetRotationAxis();

      if (Input.GetKey(ResetRotationKey.Value.MainKey)) {
        RotationManager.ResetAxis(rotationAxis);
      }

      rotationAxis *= GetSign();
      RotationManager.Rotate(rotationAxis);
    }
    
    private static int GetSign() {
      return Math.Sign(Input.GetAxis("Mouse ScrollWheel"));
    }

    private static Vector3 GetRotationAxis() {
      if (Input.GetKey(XRotationKey.Value.MainKey)) {
        RotationManager.SetActiveXScale(1.5f);
        return Vector3.right;
      }

      if (Input.GetKey(ZRotationKey.Value.MainKey)) {
        RotationManager.SetActiveZScale(1.5f);
        return Vector3.forward;
      }

      RotationManager.SetActiveYScale(1.5f);
      return Vector3.up;
    }

    [HarmonyTranspiler]
    [HarmonyPatch(nameof(Player.UpdatePlacementGhost))]
    static IEnumerable<CodeInstruction> UpdatePlacementGhostTranspiler(IEnumerable<CodeInstruction> instructions) {
      return new CodeMatcher(instructions)
        .MatchForward(
            useEnd: false,
            new CodeMatch(OpCodes.Ldfld, AccessTools.Field(typeof(Player), nameof(Player.m_placeRotation))),
            new CodeMatch(OpCodes.Conv_R4),
            new CodeMatch(OpCodes.Mul),
            new CodeMatch(OpCodes.Ldc_R4),
            new CodeMatch(OpCodes.Call),
            new CodeMatch(OpCodes.Stloc_S))
        .Advance(offset: 5)
        .InsertAndAdvance(Transpilers.EmitDelegate<Func<Quaternion, Quaternion>>(_ => RotationManager.GetRotation()))
        .InstructionEnumeration();
    }
  }
}
