namespace Aurality;

using BepInEx;

using HarmonyLib;

using System.Reflection;

using static PluginConfig;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public sealed class Aurality : BaseUnityPlugin {
  public const string PluginGuid = "bruce.comfy.valheim.aurality";
  public const string PluginName = "Aurality";
  public const string PluginVersion = "1.1.0";

  Harmony _harmony;

  void Awake() {
    BindConfig(Config);    

    _harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), harmonyInstanceId: PluginGuid);
  }

  void OnDestroy() {
    _harmony?.UnpatchSelf();
  }
}
