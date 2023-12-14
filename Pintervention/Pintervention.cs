using BepInEx;
using HarmonyLib;
using System.Reflection;

using static Pintervention.PluginConfig;

namespace Pintervention {
  [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
  public class Pintervention : BaseUnityPlugin {
    public const string PluginGuid = "bruce.valheim.comfymods.pintervention";
    public const string PluginName = "Pintervention";
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
}
