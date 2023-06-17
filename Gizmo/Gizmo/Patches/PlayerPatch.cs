using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;

using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

using UnityEngine;

using static Gizmo.ComfyGizmo;
using static Gizmo.PluginConfig;

namespace Gizmo.Patches {
  [HarmonyPatch(typeof(Player))]
  internal class PlayerPatch {
    

    [HarmonyTranspiler]
    [HarmonyPatch(nameof(Player.UpdatePlacementGhost))]
    static IEnumerable<CodeInstruction> UpdatePlacementGhostTranspiler(IEnumerable<CodeInstruction> instructions) {
      if (NewGizmoRotation.Value) {
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
          .InsertAndAdvance(Transpilers.EmitDelegate<Func<Quaternion, Quaternion>>(_ => _comfyGizmoRoot.rotation))
          .InstructionEnumeration();
      } else {
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
        .InsertAndAdvance(Transpilers.EmitDelegate<Func<Quaternion, Quaternion>>(_ => XGizmoRoot.rotation))
        .InstructionEnumeration();
      }

    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(Player.UpdatePlacement))]
    static void UpdatePlacementPostfix(ref Player __instance, ref bool takeInput) {
      if (__instance.m_placementMarkerInstance) {
        GizmoRoot.gameObject.SetActive(ShowGizmoPrefab.Value && __instance.m_placementMarkerInstance.activeSelf);
        GizmoRoot.position = __instance.m_placementMarkerInstance.transform.position + (Vector3.up * 0.5f);
      }

      if (!__instance.m_buildPieces || !takeInput) {
        return;
      }

      if (Input.GetKeyDown(SnapDivisionIncrementKey.Value.MainKey)) {
        if (SnapDivisions.Value * 2 <= MaxSnapDivisions) {
          MessageHud.instance.ShowMessage(MessageHud.MessageType.TopLeft, $"Snap divisions increased to {SnapDivisions.Value * 2}");
          SnapDivisions.Value = SnapDivisions.Value * 2;
          if (ResetRotationOnSnapDivisionChange.Value) {
            if (LocalFrame) {
              ResetRotationsLocalFrame();
            } else {
              ResetRotations();
            }
            return;
          }
        }
      }

      if (Input.GetKeyDown(SnapDivisionDecrementKey.Value.MainKey)) {
        if (Math.Floor(SnapDivisions.Value / 2f) == SnapDivisions.Value / 2f && SnapDivisions.Value / 2 >= MinSnapDivisions) {
          MessageHud.instance.ShowMessage(MessageHud.MessageType.TopLeft, $"Snap divisions decreased to {SnapDivisions.Value / 2}");
          SnapDivisions.Value = SnapDivisions.Value / 2;
          if (ResetRotationOnSnapDivisionChange.Value) {
            if (LocalFrame) {
              ResetRotationsLocalFrame();
            } else {
              ResetRotations();
            }
            return;
          }
        }
      }

      if (Input.GetKey(CopyPieceRotationKey.Value.MainKey) && __instance.m_hoveringPiece != null) {
        MatchPieceRotation(__instance.m_hoveringPiece);
      }

      // Change Rotation Frames
      if (Input.GetKeyDown(ChangeRotationModeKey.Value.MainKey)) {
        if (LocalFrame) {
          MessageHud.instance.ShowMessage(MessageHud.MessageType.TopLeft, "Default rotation mode enabled");
          if (ResetRotationOnModeChange.Value) {
            ResetRotationsLocalFrame();
          } else {
            EulerAngles = ComfyGizmoObj.transform.eulerAngles;
            ResetGizmoRoot();
            RotateGizmoComponents(EulerAngles);
          }

        } else {
          MessageHud.instance.ShowMessage(MessageHud.MessageType.TopLeft, "Local frame rotation mode enabled");
          if (ResetRotationOnModeChange.Value) {
            ResetRotations();
          } else {
            Quaternion currentRotation = _comfyGizmoRoot.rotation;
            ResetGizmoComponents();
            GizmoRoot.rotation = currentRotation;
          }
        }

        LocalFrame = !LocalFrame;
        return;
      }

      XGizmo.localScale = Vector3.one;
      YGizmo.localScale = Vector3.one;
      ZGizmo.localScale = Vector3.one;

      if (!LocalFrame) {
        Rotate();
      } else {
        RotateLocalFrame();
      }
    }
  }
}
