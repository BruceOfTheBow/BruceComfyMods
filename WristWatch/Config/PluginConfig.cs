using BepInEx.Configuration;

using static WristWatch.ConfigFileExtensions;

namespace WristWatch {
  public class PluginConfig {
    public static ConfigEntry<bool> IsModEnabled { get; private set; }
    public static ConfigEntry<bool> ReadPinsOnInteract { get; private set; }
    public static ConfigEntry<bool> ReadRevealedMapOnInteract { get; private set; }

    public static void BindConfig(ConfigFile config) {
      IsModEnabled = config.Bind("_Global", "isModEnabled", true, "Globally enable or disable this mod.");

      IsModEnabled.OnSettingChanged(ClockManager.Toggle);

      ReadPinsOnInteract =
          config.Bind(
              "CartographyTable",
              "readPinsOnInteract",
              true,
              "Allows not taking pins when reading from cartography table.");

      ReadRevealedMapOnInteract =
          config.Bind(
              "CartographyTable",
              "readRevealedMapOnInteract",
              true,
              "Allows not taking shared map data when reading from cartography table.");
    }
  }
}
