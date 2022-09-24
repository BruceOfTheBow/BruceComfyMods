using BepInEx;

using HarmonyLib;
using BepInEx.Logging;
using System.Reflection;

using static BatchDeposit.PluginConfig;
namespace BatchDeposit {
  [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
  public class ComfyBatchDeposit : BaseUnityPlugin {
    public const string PluginGuid = "com.bruce.valheim.comfybatchdeposit";
    public const string PluginName = "ComfyBatchDeposit";
    public const string PluginVersion = "1.0.0";

    static ManualLogSource _logger;
    static bool _debug = true;

    Harmony _harmony;

    public void Awake() {
      _harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), harmonyInstanceId: PluginGuid);
      _logger = Logger;
      BindConfig(Config);
    }

    public void OnDestroy() {
      _harmony?.UnpatchSelf();
    }

    public static void log(string message) {
      if (_debug) {
        _logger.LogInfo(message);
      }
    }
  }
}