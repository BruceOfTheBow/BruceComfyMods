namespace RadialRebind;

using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Globalization;
using System.Reflection;

using static PluginConfig;


[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public sealed class RadialRebind : BaseUnityPlugin {
  public const string PluginGuid = "bruce.valheim.radialrebind";
  public const string PluginName = "RadialRebind";
  public const string PluginVersion = "1.0.0";

  Harmony _harmony;

  static ManualLogSource _logger;

  void Awake() {
    BindConfig(Config);

    _logger = Logger;

    _harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), harmonyInstanceId: PluginGuid);
  }

  void OnDestroy() {
    _harmony?.UnpatchSelf();
  }

  public static void LogInfo(object obj) {
    _logger.LogInfo($"[{DateTime.Now.ToString(DateTimeFormatInfo.InvariantInfo)}] {obj}");
  }
}
