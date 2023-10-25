using BepInEx.Configuration;

namespace Abacus {
  public class PluginConfig {
    public static ConfigEntry<bool> IsModEnabled { get; private set; }
    public static ConfigEntry<int> SmallIncrement10 { get; private set; }
    public static ConfigEntry<int> LargeIncrement10 { get; private set; }
    public static ConfigEntry<int> SmallIncrement20 { get; private set; }
    public static ConfigEntry<int> LargeIncrement20 { get; private set; }
    public static ConfigEntry<int> SmallIncrement30 { get; private set; }
    public static ConfigEntry<int> LargeIncrement30 { get; private set; }
    public static ConfigEntry<int> SmallIncrement50 { get; private set; }
    public static ConfigEntry<int> LargeIncrement50 { get; private set; }

    public static ConfigEntry<int> SmallIncrement100 { get; private set; }
    public static ConfigEntry<int> LargeIncrement100 { get; private set; }
    public static ConfigEntry<int> SmallIncrement999 { get; private set; }
    public static ConfigEntry<int> LargeIncrement999 { get; private set; }


    public static void BindConfig(ConfigFile config) {
      IsModEnabled = config.Bind("_Global", "isModEnabled", true, "Globally enable or disable this mod.");

      SmallIncrement10 =
        config.Bind(
          "Increment10",
          "smallIncrement",
          1,
          new ConfigDescription(
              "Small increment for max stack size of 10.",
              new AcceptableValueRange<int>(1, 2)));


      LargeIncrement10 =
          config.Bind(
            "Increment10",
            "largeIncrement",
            2,
            new ConfigDescription(
                "Large increment for max stack size of 10.",
                new AcceptableValueRange<int>(2, 9)));

      SmallIncrement20 =
        config.Bind(
          "Increment20",
          "smallIncrement",
          5,
          new ConfigDescription(
              "Small increment for max stack size of 20.",
              new AcceptableValueRange<int>(1, 5)));


      LargeIncrement20 =
          config.Bind(
            "Increment20",
            "largeIncrement",
            10,
            new ConfigDescription(
                "Large increment for max stack size of 20.",
                new AcceptableValueRange<int>(5, 19)));


      SmallIncrement30 =
        config.Bind(
          "Increment30",
          "smallIncrement",
          5,
          new ConfigDescription(
              "Small increment for max stack size of 30.",
              new AcceptableValueRange<int>(1, 5)));


      LargeIncrement30 =
        config.Bind(
          "Increment30",
          "largeIncrement",
          10,
          new ConfigDescription(
              "Large increment for max stack size of 30.",
              new AcceptableValueRange<int>(5, 29)));

      SmallIncrement50 =
        config.Bind(
          "Increment50",
          "smallIncrement",
          5,
          new ConfigDescription(
              "Small increment for max stack size of 50.",
              new AcceptableValueRange<int>(1, 10)));


      LargeIncrement50 =
          config.Bind(
            "Increment50",
            "largeIncrement",
            10,
            new ConfigDescription(
                "Large increment for max stack size of 50.",
                new AcceptableValueRange<int>(10, 50)));

      SmallIncrement100 =
        config.Bind(
          "Increment100",
          "smallIncrement",
          10,
          new ConfigDescription(
              "Small increment for max stack size of 100.",
              new AcceptableValueRange<int>(1, 25)));


      LargeIncrement100 =
          config.Bind(
            "Increment100",
            "largeIncrement",
            25,
            new ConfigDescription(
                "Large increment for max stack size of 100.",
                new AcceptableValueRange<int>(25, 99)));

      SmallIncrement999 =
          config.Bind(
            "Increment999",
            "smallIncrement",
            25,
            new ConfigDescription(
                "Small increment for gold splitting.",
                new AcceptableValueRange<int>(5, 100)));


      LargeIncrement999 =
          config.Bind(
            "Increment999",
            "largeIncrement",
            100,
            new ConfigDescription(
                "Large increment for gold spliting.",
                new AcceptableValueRange<int>(100, 999)));
    }
  }
}
