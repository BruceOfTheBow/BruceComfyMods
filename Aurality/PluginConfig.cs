using BepInEx.Configuration;

namespace Aurality {
  public static class PluginConfig {
    public static ConfigEntry<bool> IsModEnabled { get; private set; }
    public static ConfigEntry<bool> IsBuzzingDisabled { get; private set; }
    public static void BindConfig(ConfigFile config) {
      IsModEnabled =
          config.Bind("_Global", "isModEnabled", true, "Globally enable or disable this mod (restart required).");

      IsBuzzingDisabled =
        config.Bind("audioDisables", "buzzingDisabled", true, "Removes sfx from deathsquitoes for those sensitive to buzzing.");
    }
  }
}
