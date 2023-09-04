using BepInEx.Configuration;

namespace QuietWindmills {
  public class PluginConfig {
    public static ConfigEntry<bool> IsModEnabled { get; private set; }
    public static ConfigEntry<bool> QuietWhenEmpty { get; private set; }
    public static ConfigEntry<float> MaxVolume { get; private set; }

    public static void BindConfig(ConfigFile config) {
      IsModEnabled = config.Bind("_Global", "isModEnabled", true, "Globally enable or disable this mod.");

      QuietWhenEmpty = config.Bind("Volume", "quietWhenEmpty", true, "Disables windmill sfx when windmills are not in use.");

      MaxVolume =
          config.Bind(
              "Volume",
              "windmillVolume",
              0.5f,
              new ConfigDescription("Max volume for windmills. Default is 1",
              new AcceptableValueRange<float>(0f, 1f)));
    }
  }
}
