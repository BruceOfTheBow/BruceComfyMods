namespace RadialRebind;

using BepInEx.Configuration;

using UnityEngine;

using ComfyLib;


public static class PluginConfig {
  public static ConfigEntry<bool> IsModEnabled { get; private set; }
  public static ConfigEntry<KeyCode> OpenRadialShortcut { get; private set; }

  public static void BindConfig(ConfigFile config) {
    IsModEnabled =
        config.BindInOrder(
            "_Global",
            "isModEnabled",
            true,
            "Globally enable or disable this mod.");

    OpenRadialShortcut =
        config.BindInOrder(
            "Hotkeys",
            "openRadialShortcut",
            KeyCode.G,
            "Action keyboard shortcut.");

    OpenRadialShortcut.OnSettingChanged(() => { BindManager.RebindOpenRadial(ZInput.m_instance); });
  }
}
