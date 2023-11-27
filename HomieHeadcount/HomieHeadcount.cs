using BepInEx;
using HarmonyLib;
using System.Reflection;

using static HomieHeadcount.PluginConfig;

namespace HomieHeadcount {
  [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
  public class HomieHeadcount : BaseUnityPlugin {
    public const string PluginGuid = "bruce.valheim.comfymods.homieheadcount";
    public const string PluginName = "HomieHeadcount";
    public const string PluginVersion = "1.0.0";

    Harmony _harmony;

    public static int SkeletonAiNameHashCode = "Skeleton_Friendly(Clone)".GetStableHashCode();

    void Awake() {
      BindConfig(Config);

      _harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), harmonyInstanceId: PluginGuid);
    }

    void OnDestroy() {
      _harmony?.UnpatchSelf();
    }
  }
}
