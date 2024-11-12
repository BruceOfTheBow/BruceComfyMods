namespace AssemblyLine;

using System;
using System.Globalization;
using System.Reflection;

using BepInEx;
using BepInEx.Logging;

using ComfyLib;

using HarmonyLib;

using static PluginConfig;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public sealed class AssemblyLine : BaseUnityPlugin {
  public const string PluginGuid = "bruce.valheim.comfy.assemblyline";
  public const string PluginName = "AssemblyLine";
  public const string PluginVersion = "1.2.1";

  static ManualLogSource _logger;

  void Awake() {
    _logger = Logger;
    BindConfig(Config);

    Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), harmonyInstanceId: PluginGuid);
  }

  public static void LogInfo(object obj) {
    _logger.LogInfo($"[{DateTime.Now.ToString(DateTimeFormatInfo.InvariantInfo)}] {obj}");
    Chat.m_instance.AddMessage(obj);
  }
}
