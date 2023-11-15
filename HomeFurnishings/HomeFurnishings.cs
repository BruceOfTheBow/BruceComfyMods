using BepInEx;
using HarmonyLib;
using System.Reflection;

using static HomeFurnishings.PluginConfig;

namespace HomeFurnishings {
  [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
  public class HomeFurnishings : BaseUnityPlugin {
    public const string PluginGuid = "bruce.valheim.comfymods.homefurnishings";
    public const string PluginName = "HomeFurnishings";
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
