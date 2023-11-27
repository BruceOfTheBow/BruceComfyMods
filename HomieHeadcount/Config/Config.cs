using BepInEx.Configuration;
using UnityEngine;

namespace HomieHeadcount {
  public class PluginConfig {
    public static ConfigEntry<bool> IsModEnabled { get; private set; }
    public static ConfigEntry<KeyboardShortcut> ToggleHomiePanel { get; private set; }

    public static void BindConfig(ConfigFile config) {
      IsModEnabled = config.Bind("_Global", "isModEnabled", true, "Globally enable or disable this mod.");

      ToggleHomiePanel =
        config.Bind(
            "Keys",
            "toggleHomiePanel",
            new KeyboardShortcut(KeyCode.None),
            "Toggle homie panel displaying information about your homies.");
    }
  }
}
