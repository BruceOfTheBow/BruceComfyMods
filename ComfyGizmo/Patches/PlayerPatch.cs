namespace ComfyGizmo;

using System;
using System.Collections.Generic;
using System.Reflection.Emit;

using HarmonyLib;

using UnityEngine;

using static PluginConfig;

[HarmonyPatch(typeof(Player))]
static class PlayerPatch {
  [HarmonyPostfix]
  [HarmonyPatch(nameof(Player.UpdatePlacement))]
  static void UpdatePlacementPostfix(Player __instance, bool takeInput) {
    RotationManager.HideGizmos();
    RotationManager.ShowGizmos(__instance);

    if (!takeInput || !__instance.m_buildPieces || Hud.IsPieceSelectionVisible()) {
      return;
    }

    if (ZInput.GetKeyDown(SnapDivisionIncrementKey.Value.MainKey) || ZInput.GetButtonDown(SnapDivisionIncrementButton.Value)) {
      RotationManager.IncreaseSnapDivisions();
    }

    if (ZInput.GetKeyDown(SnapDivisionDecrementKey.Value.MainKey) || ZInput.GetButtonDown(SnapDivisionDecrementButton.Value)) {
      RotationManager.DecreaseSnapDivisions();
    }

    if ((ZInput.GetKeyDown(CopyPieceRotationKey.Value.MainKey) || ZInput.GetButtonDown(CopyPieceRotationButton.Value)) && __instance.m_hoveringPiece != null) {
      RotationManager.MatchPieceRotation(__instance.m_hoveringPiece);
    }

    if (ZInput.GetKeyDown(ChangeRotationModeKey.Value.MainKey) || ZInput.GetButtonDown(ChangeRotationModeButton.Value)) {
      IsLocalFrameEnabled.Value = !IsLocalFrameEnabled.Value;
    }

    RotationManager.ResetScales();

    if (ZInput.GetKey(ResetAllRotationKey.Value.MainKey) || ZInput.GetButton(ResetAllRotationButton.Value)) {
      RotationManager.ResetRotation();
      return;
    }

    if (ZInput.GamepadActive && ZInput.InputLayout == InputLayout.Default && !ZInput.GetButton("JoyRotate"))
      return;

    Vector3 rotationAxis = GetRotationAxis();

    if (ZInput.GetKey(ResetRotationKey.Value.MainKey) || ZInput.GetButton(ResetRotationButton.Value)) {
      RotationManager.ResetAxis(rotationAxis);
    }
    
    rotationAxis *= GetSign();
    RotationManager.Rotate(rotationAxis);
  }

  private static float GetCombinedTriggerRotation()
  {
    if (ZInput.GetButton("JoyRotate"))
    {
      return -1;
    }
    
    return ZInput.GetButton("JoyRotateRight") ? 1 : 0;
  }
  
  private static float GetSign() {
    if (ZInput.GamepadActive)
    {
      float rotationSpeedRatio = 2;
      return ZInput.InputLayout switch
      {
        InputLayout.Alternative1 or InputLayout.Alternative2 => GetCombinedTriggerRotation() / rotationSpeedRatio,
        _ => ZInput.GetJoyRightStickX(true) / rotationSpeedRatio
      };
    }
    return Math.Sign(ZInput.GetMouseScrollWheel());
  }

  private static Vector3 GetRotationAxis() {
    if (ZInput.GetKey(XRotationKey.Value.MainKey) || ZInput.GetButton(XRotationButton.Value))
    {
      RotationManager.SetActiveXScale(1.5f);
      return Vector3.right;
    }

    if (ZInput.GetKey(ZRotationKey.Value.MainKey) || ZInput.GetButton(ZRotationButton.Value)) {
      RotationManager.SetActiveZScale(1.5f);
      return Vector3.forward;
    }

    RotationManager.SetActiveYScale(1.5f);
    return Vector3.up;
  }

  [HarmonyPostfix]
  [HarmonyPatch(nameof(Player.SetupPlacementGhost))]
  static void SetupPlacementGhostPostfix(Player __instance) {
    RotationManager.OnSetupPlacementGhost(__instance.m_placementGhost);
  }

  [HarmonyPrefix]
  [HarmonyPatch(nameof(Player.UpdatePlacementGhost))]
  static void UpdatePlacementGhostPrefix(Player __instance) {
    if (ZInput.GetKeyDown(SelectTargetPieceKey.Value.MainKey) || ZInput.GetButton(SelectTargetPieceButton.Value) && HasValidTargetPiece(__instance)) {
      HammerTableManager.SelectTargetPiece(__instance);
    }
  }

  static bool HasValidTargetPiece(Player player) {
    return player
        && player.GetHoveringPiece()
        && player.m_buildPieces
        && player.m_buildPieces.m_availablePieces != default;
  }

  [HarmonyTranspiler]
  [HarmonyPatch(nameof(Player.UpdatePlacementGhost))]
  static IEnumerable<CodeInstruction> UpdatePlacementGhostTranspiler(IEnumerable<CodeInstruction> instructions) {
    return new CodeMatcher(instructions)
        .Start()
        .MatchStartForward(
            new CodeMatch(OpCodes.Ldfld, AccessTools.Field(typeof(Player), nameof(Player.m_placeRotation))),
            new CodeMatch(OpCodes.Conv_R4),
            new CodeMatch(OpCodes.Mul),
            new CodeMatch(OpCodes.Ldc_R4),
            new CodeMatch(OpCodes.Call),
            new CodeMatch(OpCodes.Stloc_S))
        .ThrowIfInvalid($"Could not patch Player.UpdatePlacementGhost()! (place-rotation)")
        .Advance(offset: 5)
        .InsertAndAdvance(
            new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(PlayerPatch), nameof(PlaceRotationDelegate))))
        .InstructionEnumeration();
  }

  static Quaternion PlaceRotationDelegate(Quaternion rotation) {
    if (RotationManager.TryGetRotation(out Quaternion gizmoRotation)) {
      return gizmoRotation;
    }

    return rotation;
  }
}
