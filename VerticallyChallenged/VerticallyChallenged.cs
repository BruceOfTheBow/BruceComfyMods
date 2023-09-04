using BepInEx;

using HarmonyLib;

using System.Reflection;

using static VerticallyChallenged.PluginConfig;

namespace VerticallyChallenged {
  [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
  public class VerticallyChallenged : BaseUnityPlugin {
    public const string PluginGuid = "bruce.valheim.comfymods.verticallychalleneged";
    public const string PluginName = "VerticallyChallenged";
    public const string PluginVersion = "1.0.0";

    Harmony _harmony;

    public void Awake() {
      BindConfig(Config);

      _harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), harmonyInstanceId: PluginGuid);
    }

    public void OnDestroy() {
      _harmony?.UnpatchSelf();
    }
  }
}