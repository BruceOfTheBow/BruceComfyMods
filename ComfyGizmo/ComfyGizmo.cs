namespace ComfyGizmo;

using System;
using System.Reflection;

using BepInEx;
using BepInEx.Logging;

using HarmonyLib;

using SoftReferenceableAssets;

using static PluginConfig;

[BepInPlugin(PluginGUID, PluginName, PluginVersion)]
public sealed class ComfyGizmo : BaseUnityPlugin {
  public const string PluginGUID = "bruce.valheim.comfymods.gizmo";
  public const string PluginName = "ComfyGizmo";
  public const string PluginVersion = "1.16.0";

  public static ManualLogSource LogSource { get; private set; }

  void Awake() {
    LogSource = Logger;
    BindConfig(Config);

    // Required to load Shader assets if Jotunn is not installed.
    try {
      Runtime.MakeAllAssetsLoadable();
    } catch (Exception exception) {
      LogSource.LogError($"Could not make ComfyGizmo assets loadable; gizmos may be unavailable. {exception}");
    }

    try {
      Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), harmonyInstanceId: PluginGUID);
    } catch (Exception exception) {
      LogSource.LogError($"Could not apply ComfyGizmo patches; the plugin will stay inactive. {exception}");
    }
  }
}
