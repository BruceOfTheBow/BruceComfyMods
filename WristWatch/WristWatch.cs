using BepInEx;
using HarmonyLib;
using System.Reflection;

namespace WristWatch {
  [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
  public sealed class WristWatch : BaseUnityPlugin {
    public const string PluginGuid = "bruce.valheim.comfymods.wristwatch";
    public const string PluginName = "WristWatch";
    public const string PluginVersion = "1.0.0";

    Harmony _harmony;

    void Awake() {
      _harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), harmonyInstanceId: PluginGuid);
    }

    void OnDestroy() {
      _harmony?.UnpatchSelf();
    }
  }
}
