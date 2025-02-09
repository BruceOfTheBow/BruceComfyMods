namespace KnightsTemplar;

using BepInEx;

using HarmonyLib;

using System.Reflection;

using static PluginConfig;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public sealed class KnightsTemplar : BaseUnityPlugin {
  public const string PluginGuid = "bruce.valehim.comfytools.knightstemplar";
  public const string PluginName = "KnightsTemplar";
  public const string PluginVersion = "1.3.0";

  void Awake() {
    BindConfig(Config);

    Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), harmonyInstanceId: PluginGuid);
  }
}
