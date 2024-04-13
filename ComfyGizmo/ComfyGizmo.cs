using System.Reflection;

using BepInEx;

using HarmonyLib;

using static ComfyGizmo.PluginConfig;

namespace ComfyGizmo {
  [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
  public class ComfyGizmo : BaseUnityPlugin {
    public const string PluginGUID = "bruce.valheim.comfymods.gizmo";
    public const string PluginName = "ComfyGizmo";
    public const string PluginVersion = "1.9.1";

    Harmony _harmony;

    void Awake() {
      BindConfig(Config);

      Gizmos.LoadGizmoPrefab();

      _harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), harmonyInstanceId: PluginGUID);
    }

    void OnDestroy() {
      _harmony?.UnpatchSelf();
    }
  }
}
