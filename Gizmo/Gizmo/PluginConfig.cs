using BepInEx.Configuration;

using UnityEngine;

namespace Gizmo {
  public static class PluginConfig {
    public static ConfigEntry<int> SnapDivisions { get; private set; }
    public static ConfigEntry<KeyboardShortcut> XRotationKey;
    public static ConfigEntry<KeyboardShortcut> ZRotationKey;
    public static ConfigEntry<KeyboardShortcut> ResetRotationKey;
    public static ConfigEntry<KeyboardShortcut> ResetAllRotationKey;
    public static ConfigEntry<KeyboardShortcut> ChangeRotationModeKey;
    public static ConfigEntry<KeyboardShortcut> CopyPieceRotationKey;
    public static ConfigEntry<KeyboardShortcut> SnapDivisionIncrementKey;
    public static ConfigEntry<KeyboardShortcut> SnapDivisionDecrementKey;

    public static ConfigEntry<bool> ShowGizmoPrefab;
    public static ConfigEntry<bool> ResetRotationOnModeChange;
    public static ConfigEntry<bool> ResetRotationOnSnapDivisionChange;
    public static ConfigEntry<bool> NewGizmoRotation;

    public static ConfigEntry<Color> XGizmoColor { get; private set; }
    public static ConfigEntry<Color> YGizmoColor { get; private set; }
    public static ConfigEntry<Color> ZGizmoColor { get; private set; }

    public static ConfigEntry<float> XEmissionColorFactor { get; private set; }
    public static ConfigEntry<float> YEmissionColorFactor { get; private set; }
    public static ConfigEntry<float> ZEmissionColorFactor { get; private set; }

    public static int MaxSnapDivisions = 256;
    public static int MinSnapDivisions = 2;

    public static void BindConfig(ConfigFile config) {
      SnapDivisions =
          config.Bind(
              "Gizmo",
              "snapDivisions",
              16,
              new ConfigDescription(
                  "Number of snap angles per 180 degrees. Vanilla uses 8.",
                 new AcceptableValueRange<int>(MinSnapDivisions, MaxSnapDivisions)));

      XRotationKey =
          config.Bind(
              "Keys",
              "xRotationKey",
              new KeyboardShortcut(KeyCode.LeftShift),
              "Hold this key to rotate on the x-axis/plane (red circle).");

      ZRotationKey =
          config.Bind(
              "Keys",
              "zRotationKey",
              new KeyboardShortcut(KeyCode.LeftAlt),
              "Hold this key to rotate on the z-axis/plane (blue circle).");

      ResetRotationKey =
          config.Bind(
              "Keys",
              "resetRotationKey",
              new KeyboardShortcut(KeyCode.V),
              "Press this key to reset the selected axis to zero rotation.");

      ResetAllRotationKey =
          config.Bind(
              "Keys",
              "resetAllRotationKey",
              KeyboardShortcut.Empty,
              "Press this key to reset _all axis_ rotations to zero rotation.");


      ChangeRotationModeKey =
          config.Bind(
              "Keys",
              "changeRotationMode",
              new KeyboardShortcut(KeyCode.BackQuote),
              "Press this key to toggle rotation modes.");

      CopyPieceRotationKey =
          config.Bind(
              "Keys",
              "copyPieceRotation",
              KeyboardShortcut.Empty,
              "Press this key to copy targeted piece's rotation.");

      SnapDivisionIncrementKey =
          config.Bind(
              "Keys",
              "snapDivisionIncrement",
              new KeyboardShortcut(KeyCode.PageUp),
              "Doubles snap divisions from current.");

      SnapDivisionDecrementKey =
          config.Bind(
              "Keys",
              "snapDivisionDecrement",
              new KeyboardShortcut(KeyCode.PageDown),
              "Doubles snap divisions from current.");

      XGizmoColor =
          config.Bind(
              "GizmoColors - X",
              "xGizmoColor",
              new Color(1,0,0, 0.502f),
              "Sets the color of x gizmo (rotations about x axis).");

      YGizmoColor =
         config.Bind(
            "GizmoColors - Y",
            "yGizmoColor",
            new Color(0, 1, 0, 0.502f),
            "Sets the color of y gizmo (rotations about y axis).");

      ZGizmoColor =
         config.Bind(
            "GizmoColors - Z",
            "zGizmoColor",
            new Color(0, 0, 1, 0.502f),
            "Sets the color of z gizmo (rotations about z axis).");

      XEmissionColorFactor =
          config.Bind(
            "GizmoColors - X",
            "emissionColorFactorX",
            0.4f,
            new ConfigDescription(
                  "Factor to multiply the target color by and set as emission color.",
                  new AcceptableValueRange<float>(0f, 3f)));

      YEmissionColorFactor =
          config.Bind(
            "GizmoColors - Y",
            "emissionColorFactorY",
            0.4f,
            new ConfigDescription(
                  "Factor to multiply the target color by and set as emission color.",
                  new AcceptableValueRange<float>(0f, 3f)));

      ZEmissionColorFactor =
          config.Bind(
            "GizmoColors - Z",
            "emissionColorFactorZ",
            0.4f,
            new ConfigDescription(
                  "Factor to multiply the target color by and set as emission color.",
                  new AcceptableValueRange<float>(0f, 3f)));

      ShowGizmoPrefab = config.Bind("UI", "showGizmoPrefab", true, "Show the Gizmo prefab in placement mode.");

      ResetRotationOnSnapDivisionChange = config.Bind("Reset", "resetOnSnapDivisionChange", true, "Resets the piece's rotation on snap division change.");
      ResetRotationOnModeChange = config.Bind("Reset", "resetOnModeChange", true, "Resets the piece's rotation on mode switch.");

      NewGizmoRotation = config.Bind("Rotation Mode", "newGizmoRotation", false, "Enables post Gizmo v1.2.0 rotation scheme. Restart required for changes to take effect.");
    }
  }
}
