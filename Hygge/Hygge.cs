using BepInEx;
using HarmonyLib;
using System.Reflection;

using static Hygge.PluginConfig;

namespace Hygge {
  [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
  public class Hygge : BaseUnityPlugin {
    public const string PluginGuid = "bruce.valheim.comfymods.hygge";
    public const string PluginName = "Hygge";
    public const string PluginVersion = "1.3.0";

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
