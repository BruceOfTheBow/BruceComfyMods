using BepInEx.Configuration;

namespace HomieHeadcount {
  public class PluginConfig {
    public static ConfigEntry<bool> IsModEnabled { get; private set; }
    public static ConfigEntry<KeyboardShortcut> ToggleHomiePanel { get; private set; }

    public static void BindConfig(ConfigFile config) {
      IsModEnabled = config.Bind("_Global", "isModEnabled", true, "Globally enable or disable this mod.");

      IsModEnabled.OnSettingChanged(
         () => {
           PanelManager.Hide();
           PanelManager.Destroy();
         });
    }
  }
}
