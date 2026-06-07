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
  static void UpdatePlacementPostfix(Player __instance, bool takeInput, float dt) {
    RotationManager.HideGizmos();
    RotationManager.ShowGizmos(__instance);

    if (!takeInput || !__instance.m_buildPieces || Hud.IsPieceSelectionVisible()) {
      return;
    }

    if (ZInput.GetKeyDown(SnapDivisionIncrementKey.Value.MainKey)) {
      RotationManager.IncreaseSnapDivisions();
    }

    if (ZInput.GetKeyDown(SnapDivisionDecrementKey.Value.MainKey)) {
      RotationManager.DecreaseSnapDivisions();
    }

    if (ZInput.GetKeyDown(CopyPieceRotationKey.Value.MainKey) && __instance.m_hoveringPiece != null) {
      RotationManager.MatchPieceRotation(__instance.m_hoveringPiece);
    }

    if (ZInput.GetKeyDown(ChangeRotationModeKey.Value.MainKey)) {
      IsLocalFrameEnabled.Value = !IsLocalFrameEnabled.Value;
    }

    if (HandleGizmoButton()) {
      return;
    }

    RotationManager.ResetScales();

    if (ZInput.GetKey(ResetAllRotationKey.Value.MainKey)) {
      RotationManager.ResetRotation();
      return;
    }

    Vector3 rotationAxis = GetRotationAxis();

    if (ZInput.GetKey(ResetRotationKey.Value.MainKey)) {
      RotationManager.ResetAxis(rotationAxis);
    }

    int scrollSign = Math.Sign(ZInput.GetMouseScrollWheel());

    if (scrollSign != 0) {
      _joystickRotationTimer = 0f;
      RotationManager.Rotate(rotationAxis * scrollSign);
      return;
    }

    if (TryGetJoystickRotation(dt, out Vector3 joystickRotation)) {
      RotationManager.Rotate(joystickRotation);
    }
  }

  // Initial hold time (seconds) before the right joystick starts auto-repeating
  // rotations, matching vanilla's building-rotation feel.
  const float JoystickRotationHoldDelay = 0.25f;

  static float _joystickRotationTimer = 0f;

  // Up/down stick rotates pitch (X) by default; tapping the gizmo button switches it to roll (Z).
  static bool _joystickVerticalIsRoll = false;

  static float _gizmoButtonDownTime = -1f;
  static bool _gizmoButtonResetFired = false;

  // One gamepad button drives both gizmo actions: a quick tap toggles the up/down
  // stick between pitch and roll, holding it resets all rotation. This deliberately
  // avoids the right-stick click, which cycles snap points in every controller layout.
  // Returns true if a reset happened this frame.
  private static bool HandleGizmoButton() {
    if (!JoystickRotationEnabled.Value || !ZInput.IsGamepadActive()) {
      _gizmoButtonDownTime = -1f;
      _gizmoButtonResetFired = false;
      return false;
    }

    string button = JoystickGizmoButton.Value;

    if (string.IsNullOrWhiteSpace(button)) {
      return false;
    }

    if (ZInput.GetButtonDown(button)) {
      _gizmoButtonDownTime = Time.time;
      _gizmoButtonResetFired = false;
    }

    bool didReset = false;

    if (!_gizmoButtonResetFired
        && _gizmoButtonDownTime >= 0f
        && ZInput.GetButton(button)
        && Time.time - _gizmoButtonDownTime >= JoystickResetHoldSeconds.Value) {
      RotationManager.ResetRotation();
      ShowMessage(GizmoLocalization.Translate(GizmoLocalization.ResetMessageToken));
      _gizmoButtonResetFired = true;
      didReset = true;
    }

    if (ZInput.GetButtonUp(button)) {
      bool wasTap =
          !_gizmoButtonResetFired
          && _gizmoButtonDownTime >= 0f
          && Time.time - _gizmoButtonDownTime < JoystickResetHoldSeconds.Value;

      if (wasTap) {
        _joystickVerticalIsRoll = !_joystickVerticalIsRoll;
        ShowMessage(
            GizmoLocalization.Translate(
                _joystickVerticalIsRoll
                    ? GizmoLocalization.RollMessageToken
                    : GizmoLocalization.PitchMessageToken));
      }

      _gizmoButtonDownTime = -1f;
      _gizmoButtonResetFired = false;
    }

    return didReset;
  }

  // Right-stick rotation: left/right -> yaw (Y), up/down -> pitch (X) or roll (Z).
  // The dominant stick axis wins so a diagonal push never rotates two axes at once.
  private static bool TryGetJoystickRotation(float dt, out Vector3 rotation) {
    rotation = Vector3.zero;

    if (!JoystickRotationEnabled.Value || !ZInput.IsGamepadActive()) {
      _joystickRotationTimer = 0f;
      return false;
    }

    float stickX = ZInput.GetJoyRightStickX();
    float stickY = ZInput.GetJoyRightStickY();
    float deadzone = JoystickRotationDeadzone.Value;

    if (Mathf.Abs(stickX) <= deadzone && Mathf.Abs(stickY) <= deadzone) {
      _joystickRotationTimer = 0f;
      return false;
    }

    Vector3 axis;
    float stickValue;
    bool invert;

    if (Mathf.Abs(stickX) >= Mathf.Abs(stickY)) {
      axis = Vector3.up;
      stickValue = stickX;
      invert = JoystickRotationInvert.Value;
    } else {
      axis = _joystickVerticalIsRoll ? Vector3.forward : Vector3.right;
      stickValue = stickY;
      invert = JoystickVerticalInvert.Value;
    }

    HighlightAxis(axis);

    int sign = GetJoystickRepeatSign(stickValue, dt);

    if (sign == 0) {
      return false;
    }

    if (invert) {
      sign = -sign;
    }

    rotation = axis * sign;
    return true;
  }

  // One tick the moment the stick passes the deadzone, then a brief hold delay,
  // then auto-repeating ticks while it stays held (mirrors vanilla cadence).
  private static int GetJoystickRepeatSign(float stickValue, float dt) {
    int sign = (stickValue > 0f) ? 1 : -1;

    if (_joystickRotationTimer <= 0f) {
      _joystickRotationTimer = JoystickRotationHoldDelay;
      return sign;
    }

    _joystickRotationTimer -= dt;

    if (_joystickRotationTimer <= 0f) {
      _joystickRotationTimer = JoystickRotationRepeatDelay.Value;
      return sign;
    }

    return 0;
  }

  private static void HighlightAxis(Vector3 axis) {
    RotationManager.ResetScales();

    if (axis == Vector3.right) {
      RotationManager.SetActiveXScale(1.5f);
    } else if (axis == Vector3.forward) {
      RotationManager.SetActiveZScale(1.5f);
    } else {
      RotationManager.SetActiveYScale(1.5f);
    }
  }

  private static void ShowMessage(string message) {
    if (MessageHud.m_instance) {
      MessageHud.m_instance.ShowMessage(MessageHud.MessageType.TopLeft, message);
    }
  }

  private static Vector3 GetRotationAxis() {
    if (ZInput.GetKey(XRotationKey.Value.MainKey)) {
      RotationManager.SetActiveXScale(1.5f);
      return Vector3.right;
    }

    if (ZInput.GetKey(ZRotationKey.Value.MainKey)) {
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
    if (ZInput.GetKeyDown(SelectTargetPieceKey.Value.MainKey) && HasValidTargetPiece(__instance)) {
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
