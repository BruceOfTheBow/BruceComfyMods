namespace ComfyFirstPerson;

using BepInEx.Configuration;

using ComfyLib;

using UnityEngine;

public static class PluginConfig {
  public static ConfigEntry<bool> IsModEnabled { get; private set; }

  public static ConfigEntry<bool> IsFirstPersonEnabled { get; private set; }
  public static ConfigEntry<float> DefaultFov { get; private set; }
  public static ConfigEntry<KeyboardShortcut> ToggleKey { get; private set; }
  public static ConfigEntry<KeyboardShortcut> RaiseKey { get; private set; }
  public static ConfigEntry<KeyboardShortcut> LowerKey { get; private set; }
  

  public static void BindConfig(ConfigFile config) {
    IsModEnabled = config.Bind("_Global", "isModEnabled", true, "Globally enable or disable this mod.");
    IsFirstPersonEnabled = config.Bind("Enable", "isFirstPersonEnabled", false, "Enable first person view.");
    
    DefaultFov = config.Bind("FOV", "defaultFov", 90.0f, "Default FOV when in first person mode.");


    ToggleKey =
      config.BindInOrder(
          "Keys",
          "toggleKey",
          new KeyboardShortcut(KeyCode.F8),
          "Hold this key to rotate on the x-axis/plane (red circle).");

    RaiseKey =
        config.BindInOrder(
            "Keys",
            "raiseKey",
            new KeyboardShortcut(KeyCode.PageUp),
            "Hold this key to rotate on the z-axis/plane (blue circle).");

    LowerKey =
        config.BindInOrder(
            "Keys",
            "lowerKey",
            new KeyboardShortcut(KeyCode.PageDown),
            "Press this key to reset the selected axis to zero rotation.");
  }
}