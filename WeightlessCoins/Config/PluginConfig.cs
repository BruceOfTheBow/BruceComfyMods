namespace WeightlessCoins;

using BepInEx.Configuration;

using ComfyLib;


public static class PluginConfig {
  public static ConfigEntry<bool> IsModEnabled { get; private set; }

  public static ConfigEntry<float> CoinWeightScale { get; private set; }



  public static void BindConfig(ConfigFile config) {
    IsModEnabled =
        config.BindInOrder(
            "_Global",
            "isModEnabled",
            true,
            "Globally enable or disable this mod.");

    CoinWeightScale =
        config.BindInOrder(
          "Scale",
          "coinWeightScale",
          0.0f,
          "Multiplies the coin's base weight by this value.");
  }
}
