using BepInEx.Configuration;

using UnityEngine;

namespace DumpsterFire {
  public class PluginConfig {
    public static ConfigEntry<bool> IsModEnabled { get; private set; }
    public static ConfigEntry<bool> IsAudioEnabled { get; private set; }
    public static ConfigEntry<KeyboardShortcut> DeleteKey { get; private set; }


    public static void BindConfig(ConfigFile config) {
      IsModEnabled = config.Bind("_Global", "isModEnabled", true, "Globally enable or disable this mod.");

      IsAudioEnabled = config.Bind("Audio", "isAudioEnabled", true, "Enables fire burn audio on dumpster firing item.");

      DeleteKey =
        config.Bind(
          "Hotkeys",
          "deleteKey",
          new KeyboardShortcut(KeyCode.Delete),
          "Key to delete item when selected.");
    }
  }
}
