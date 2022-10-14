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
    public const string PluginVersion = "1.0.2";

    static ManualLogSource _logger;

    Harmony _harmony;

    public void Awake() {
      _harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), harmonyInstanceId: PluginGuid);
      _logger = Logger;
      BindConfig(Config);
    }

    public void OnDestroy() {
      _harmony?.UnpatchSelf();
    }
  }
}