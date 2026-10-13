namespace SnapPointsByComfy;

using BepInEx.Configuration;

using ComfyLib;

using UnityEngine;

public static class PluginConfig {
  public static ConfigEntry<bool> IsModEnabled { get; private set; }
  public static ConfigEntry<bool> IsCenterSnapPointEnabled { get; private set; }
  public static ConfigEntry<bool> ResetOnPieceChange { get; private set; }
  public static ConfigEntry<SnapManager.SnapMode> SnapModeSelected { get; private set; }
  public static ConfigEntry<KeyboardShortcut> EnableModToggleKey { get; private set; }
  public static ConfigEntry<KeyboardShortcut> IterateSourceSnapPointKey { get; private set; }
  public static ConfigEntry<KeyboardShortcut> IterateTargetSnapPointKey { get; private set; }
  public static ConfigEntry<KeyboardShortcut> SnapModeToggleKey { get; private set; }

  public static void BindConfig(ConfigFile config) {
    IsModEnabled =
        config.BindInOrder(
            "_Global",
            "isModEnabled",
            true,
            "Globally enable or disable this mod.");

    IsCenterSnapPointEnabled =
        config.BindInOrder(
            "Options",
            "isCenterSnapPointEnabled",
            true,
            "Enables an additional snap point at the center of each piece.");

    ResetOnPieceChange =
        config.BindInOrder(
            "Options",
            "resetOnPieceChange",
            false,
            "Reset the current snap point when a new piece is selected.");

    SnapModeSelected =
        config.BindInOrder(
            "Mode",
            "sourcePieceModeSelected",
            SnapManager.SnapMode.Cycle,
            "Mode for handling snap points for the piece currently being placed (Cycle or Closest).");

    EnableModToggleKey =
        config.BindInOrder(
            "KeyBinds",
            "enableModToggleKey",
            new KeyboardShortcut(KeyCode.RightControl),
            "Key bind to enable/disable the mod.");

    IterateSourceSnapPointKey =
        config.BindInOrder(
            "KeyBinds",
            "iterateSourceSnapPointsKey",
            new KeyboardShortcut(KeyCode.LeftControl),
            "Key bind to iterate through the snap points of the piece currently being placed.");

    IterateTargetSnapPointKey =
        config.BindInOrder(
            "KeyBinds",
            "iterateTargetSnapPointKey",
            new KeyboardShortcut(KeyCode.LeftAlt),
            "Key bind to iterate through the snap points of the piece currently being targeted.");

    SnapModeToggleKey =
        config.BindInOrder(
            "KeyBinds",
            "enableClosestSourceSnapPointKey",
            new KeyboardShortcut(KeyCode.CapsLock),
            "Key bind to toggle between closest snap point enabled or disabled for the piece being placed.");

    IsCenterSnapPointEnabled.OnSettingChanged(SnapManager.RebuildSnapPoints);
  }
}
