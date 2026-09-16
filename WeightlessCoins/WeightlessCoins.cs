namespace WeightlessCoins;

using BepInEx;
using HarmonyLib;
using System.Reflection;

using static PluginConfig;
 
[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public sealed class WeightlessCoins : BaseUnityPlugin {
  public const string PluginGuid = "bruce.valheim.weightlesscoins";
  public const string PluginName = "WeightlessCoins";
  public const string PluginVersion = "1.0.0";

  Harmony _harmony;

  void Awake() {
    BindConfig(Config);

    _harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), harmonyInstanceId: PluginGuid);
  }

  void OnDestroy() {
    _harmony?.UnpatchSelf();
  }
}
