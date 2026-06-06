namespace ComfyGizmo;

using BepInEx.Configuration;

using ComfyLib;

using UnityEngine;

public static class PluginConfig {
  public static ConfigEntry<int> SnapDivisions { get; private set; }

  public static ConfigEntry<bool> ShowGizmoPrefab { get; private set; }
  public static ConfigEntry<bool> ShowKeyHints { get; private set; }
  public static ConfigEntry<bool> ResetRotationOnModeChange { get; private set; }
  public static ConfigEntry<bool> ResetRotationOnSnapDivisionChange { get; private set; }
  public static ConfigEntry<bool> IsLocalFrameEnabled { get; private set; }
  public static ConfigEntry<bool> IsOldRotationEnabled { get; private set; }
  public static ConfigEntry<bool> IsRoofModeEnabled { get; private set; }

  public static ConfigEntry<bool> IgnoreTerrainOpPrefab { get; private set; }
  public static ToggleStringListConfigEntry IgnorePrefabNameList { get; private set; }

  public static readonly int MinSnapDivisions = 2;
  public static readonly int MaxSnapDivisions = 256;

  public static void BindConfig(ConfigFile config) {
    SnapDivisions =
        config.BindInOrder(
            "Gizmo",
            "snapDivisions",
            16,
            "Number of snap angles per 180 degrees. Vanilla uses 8.",
            new AcceptableValueRange<int>(MinSnapDivisions, MaxSnapDivisions));

    SnapDivisions.OnSettingChanged(RotationManager.ResetRotationConditional);

    BindKeysConfig(config);
    BindJoystickConfig(config);
    BindGizmoColorsConfig(config);

    ShowGizmoPrefab =
        config.BindInOrder(
            "UI",
            "showGizmoPrefab",
            true,
            "Show the Gizmo prefab in placement mode.");

    ShowKeyHints =
        config.BindInOrder(
            "UI",
            "showKeyHints",
            true,
            "Show ComfyGizmo control hints in the build key-hint bar. Adapts to keyboard or controller.");

    ResetRotationOnSnapDivisionChange =
        config.BindInOrder(
            "Reset",
            "resetOnSnapDivisionChange",
            true,
            "Resets the piece's rotation on snap division change.");

    ResetRotationOnModeChange =
        config.BindInOrder(
            "Reset",
            "resetOnModeChange",
            true,
            "Resets the piece's rotation on mode switch.");

    IsRoofModeEnabled =
        config.BindInOrder(
            "Modes",
            "isRoofModeEnabled",
            false,
            "Enables roof mode which allows corner roof piece rotation 45 deg compared to normal rotation.");

    IsRoofModeEnabled.OnSettingChanged(
        () => {           
          RotationManager.ResetRotation();
          RotationManager.OnModeChange(Player.m_localPlayer);
        });

    IsOldRotationEnabled =
        config.BindInOrder(
            "Modes",
            "isOldRotationModeEnabled",
            false,
            "Enables pre Gizmo-v1.4.0 rotation scheme.");

    IsOldRotationEnabled.OnSettingChanged(() => RotationManager.OnModeChange(Player.m_localPlayer));

    IsLocalFrameEnabled =
        config.BindInOrder(
            "Modes",
            "isLocalFrameModeEnabled",
            false,
            "Enables localFrame rotation mode. Allows rotation around the piece's Y-axis rather than world-Y.");

    IsLocalFrameEnabled.OnSettingChanged(
        () => {
          RotationManager.OnModeChange(Player.m_localPlayer);
          RotationManager.DisableLocalFrameMode();
        });

    IgnoreTerrainOpPrefab =
        config.BindInOrder(
            "Ignored",
            "ignoreTerrainOpPrefab",
            false,
            "If enabled, Gizmo rotation will be ignored for terrain-modifying prefabs.");

    IgnorePrefabNameList =
        new ToggleStringListConfigEntry(
            config,
            "Ignored",
            "ignorePrefabNameList",
            string.Empty,
            "List of prefab names to ignore for Gizmo rotation.");

    IgnorePrefabNameList.SettingChanged += OnIgnorePrefabNameListChanged;
    OnIgnorePrefabNameListChanged(default, IgnorePrefabNameList.ToggledStringValues());
  }

  static void OnIgnorePrefabNameListChanged(object sender, string[] values) {
    RotationManager.SetIgnoredPrefabNames(values);
  }

  public static ConfigEntry<KeyboardShortcut> XRotationKey { get; private set; }
  public static ConfigEntry<KeyboardShortcut> ZRotationKey { get; private set; }
  public static ConfigEntry<KeyboardShortcut> ResetRotationKey { get; private set; }
  public static ConfigEntry<KeyboardShortcut> ResetAllRotationKey { get; private set; }
  public static ConfigEntry<KeyboardShortcut> ChangeRotationModeKey { get; private set; }
  public static ConfigEntry<KeyboardShortcut> CopyPieceRotationKey { get; private set; }
  public static ConfigEntry<KeyboardShortcut> SelectTargetPieceKey { get; private set; }
  public static ConfigEntry<KeyboardShortcut> SnapDivisionIncrementKey { get; private set; }
  public static ConfigEntry<KeyboardShortcut> SnapDivisionDecrementKey { get; private set; }

  public static void BindKeysConfig(ConfigFile config) {
    XRotationKey =
        config.BindInOrder(
            "Keys",
            "xRotationKey",
            new KeyboardShortcut(KeyCode.LeftShift),
            "Hold this key to rotate on the x-axis/plane (red circle).");

    ZRotationKey =
        config.BindInOrder(
            "Keys",
            "zRotationKey",
            new KeyboardShortcut(KeyCode.LeftAlt),
            "Hold this key to rotate on the z-axis/plane (blue circle).");

    ResetRotationKey =
        config.BindInOrder(
            "Keys",
            "resetRotationKey",
            new KeyboardShortcut(KeyCode.V),
            "Press this key to reset the selected axis to zero rotation.");

    ResetAllRotationKey =
        config.BindInOrder(
            "Keys",
            "resetAllRotationKey",
            KeyboardShortcut.Empty,
            "Press this key to reset _all axis_ rotations to zero rotation.");

    ChangeRotationModeKey =
        config.BindInOrder(
            "Keys",
            "changeRotationMode",
            new KeyboardShortcut(KeyCode.BackQuote),
            "Press this key to toggle rotation modes.");

    CopyPieceRotationKey =
        config.BindInOrder(
            "Keys",
            "copyPieceRotation",
            KeyboardShortcut.Empty,
            "Press this key to copy targeted piece's rotation.");

    SelectTargetPieceKey =
        config.BindInOrder(
            "Keys",
            "selectTargetPieceKey",
            new KeyboardShortcut(KeyCode.P),
            "Selects target piece to be used.");

    SnapDivisionIncrementKey =
        config.BindInOrder(
            "Keys",
            "snapDivisionIncrement",
            new KeyboardShortcut(KeyCode.PageUp),
            "Doubles snap divisions from current.");

    SnapDivisionDecrementKey =
        config.BindInOrder(
            "Keys",
            "snapDivisionDecrement",
            new KeyboardShortcut(KeyCode.PageDown),
            "Doubles snap divisions from current.");
  }

  public static ConfigEntry<bool> JoystickRotationEnabled { get; private set; }
  public static ConfigEntry<float> JoystickRotationDeadzone { get; private set; }
  public static ConfigEntry<bool> JoystickRotationInvert { get; private set; }
  public static ConfigEntry<bool> JoystickVerticalInvert { get; private set; }
  public static ConfigEntry<float> JoystickRotationRepeatDelay { get; private set; }
  public static ConfigEntry<string> JoystickGizmoButton { get; private set; }
  public static ConfigEntry<float> JoystickResetHoldSeconds { get; private set; }

  public static void BindJoystickConfig(ConfigFile config) {
    JoystickRotationEnabled =
        config.BindInOrder(
            "Joystick",
            "joystickRotationEnabled",
            true,
            "Use the controller's right joystick to rotate pieces while building. "
                + "Left/right = yaw (Y), up/down = pitch (X) or roll (Z) when toggled.");

    JoystickRotationDeadzone =
        config.BindInOrder(
            "Joystick",
            "joystickRotationDeadzone",
            0.5f,
            "How far the right joystick must be pushed before a rotation is triggered.",
            new AcceptableValueRange<float>(0.1f, 0.95f));

    JoystickRotationInvert =
        config.BindInOrder(
            "Joystick",
            "joystickRotationInvert",
            false,
            "Invert the left/right (yaw) right-joystick rotation direction.");

    JoystickVerticalInvert =
        config.BindInOrder(
            "Joystick",
            "joystickVerticalInvert",
            false,
            "Invert the up/down (pitch and roll) right-joystick rotation direction.");

    JoystickRotationRepeatDelay =
        config.BindInOrder(
            "Joystick",
            "joystickRotationRepeatDelay",
            0.1f,
            "Seconds between repeated rotations while the right joystick is held. Lower is faster.",
            new AcceptableValueRange<float>(0.01f, 1f));

    JoystickGizmoButton =
        config.BindInOrder(
            "Joystick",
            "joystickGizmoButton",
            "JoyButtonX",
            "Gamepad button for gizmo actions while building: tap to toggle the up/down stick between "
                + "pitch (X) and roll (Z), hold to reset all rotation. Uses Valheim's Joy* button names. "
                + "Default 'JoyButtonX' is free during building in every controller layout (unlike the "
                + "right-stick click, which cycles snap points).");

    JoystickResetHoldSeconds =
        config.BindInOrder(
            "Joystick",
            "joystickResetHoldSeconds",
            0.4f,
            "How long to hold the gizmo button to reset all rotation.",
            new AcceptableValueRange<float>(0.2f, 1.5f));
  }

  public static ConfigEntry<Color> XGizmoColor { get; private set; }
  public static ConfigEntry<Color> YGizmoColor { get; private set; }
  public static ConfigEntry<Color> ZGizmoColor { get; private set; }

  public static ConfigEntry<float> XEmissionColorFactor { get; private set; }
  public static ConfigEntry<float> YEmissionColorFactor { get; private set; }
  public static ConfigEntry<float> ZEmissionColorFactor { get; private set; }

  public static void BindGizmoColorsConfig(ConfigFile config) {
    XGizmoColor =
        config.BindInOrder(
            "GizmoColors - X",
            "xGizmoColor",
            new Color(1, 0, 0, 0.502f),
            "Sets the color of x gizmo (rotations about x axis).");

    XGizmoColor.OnSettingChanged(Gizmos.SetAllXColors);

    YGizmoColor =
       config.BindInOrder(
            "GizmoColors - Y",
            "yGizmoColor",
            new Color(0, 1, 0, 0.502f),
            "Sets the color of y gizmo (rotations about y axis).");

    YGizmoColor.OnSettingChanged(Gizmos.SetAllYColors);

    ZGizmoColor =
       config.BindInOrder(
            "GizmoColors - Z",
            "zGizmoColor",
            new Color(0, 0, 1, 0.502f),
            "Sets the color of z gizmo (rotations about z axis).");

    ZGizmoColor.OnSettingChanged(Gizmos.SetAllZColors);

    XEmissionColorFactor =
        config.BindInOrder(
            "GizmoColors - X",
            "emissionColorFactorX",
            1f,
            "Factor to multiply the target color by and set as emission color.",
            new AcceptableValueRange<float>(0f, 3f));

    XEmissionColorFactor.OnSettingChanged(Gizmos.SetAllXColors);

    YEmissionColorFactor =
        config.BindInOrder(
            "GizmoColors - Y",
            "emissionColorFactorY",
            1f,
            "Factor to multiply the target color by and set as emission color.",
            new AcceptableValueRange<float>(0f, 3f));

    YEmissionColorFactor.OnSettingChanged(Gizmos.SetAllYColors);

    ZEmissionColorFactor =
        config.BindInOrder(
            "GizmoColors - Z",
            "emissionColorFactorZ",
            1f,
            "Factor to multiply the target color by and set as emission color.",
            new AcceptableValueRange<float>(0f, 3f));

    ZEmissionColorFactor.OnSettingChanged(Gizmos.SetAllZColors);
  }
}
