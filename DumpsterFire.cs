using BepInEx;
using BepInEx.Logging;

using HarmonyLib;

using System.Reflection;

using static DumpsterFire.PluginConfig;

namespace DumpsterFire {
  [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
  public class DumpsterFire : BaseUnityPlugin {
    public const string PluginGuid = "bruce.valheim.comfy.dumpsterfire";
    public const string PluginName = "DumpsterFire";
    public const string PluginVersion = "1.0.0";

    internal static ManualLogSource _logger;

    Harmony _harmony;

    public void Awake() {
      _logger = Logger;

      BindConfig(Config);

      _harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), harmonyInstanceId: PluginGuid);
    }

    public void OnDestroy() {
      _harmony?.UnpatchSelf();
    }
  }
}