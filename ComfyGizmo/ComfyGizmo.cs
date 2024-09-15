namespace ComfyGizmo;

using System.Reflection;

using BepInEx;

using HarmonyLib;

using SoftReferenceableAssets;

using static PluginConfig;

[BepInPlugin(PluginGUID, PluginName, PluginVersion)]
public sealed class ComfyGizmo : BaseUnityPlugin {
  public const string PluginGUID = "bruce.valheim.comfymods.gizmo";
  public const string PluginName = "ComfyGizmo";
  public const string PluginVersion = "1.11.0";

  void Awake() {
    BindConfig(Config);

    // Required to load Shader assets if Jotunn is not installed.
    Runtime.MakeAllAssetsLoadable();

    Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), harmonyInstanceId: PluginGUID);
  }
}
