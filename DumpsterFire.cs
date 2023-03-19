using BepInEx;

using HarmonyLib;

using System.Reflection;

namespace DumpsterFire {
  [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
  public class DumpsterFire : BaseUnityPlugin {
    public const string PluginGuid = "bruce.valheim.comfy.dumpsterfire";
    public const string PluginName = "DumpsterFire";
    public const string PluginVersion = "1.0.0";

    Harmony _harmony;

    public void Awake() {
      _harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), harmonyInstanceId: PluginGuid);
    }

    public void OnDestroy() {
      _harmony?.UnpatchSelf();
    }
  }
}