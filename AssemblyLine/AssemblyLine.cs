using System;
using System.Globalization;

using BepInEx;
using BepInEx.Logging;

using HarmonyLib;

using System.Reflection;

using static AssemblyLine.PluginConfig;

namespace AssemblyLine {
  [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
  public class AssemblyLine : BaseUnityPlugin {
    public const string PluginGuid = "bruce.valheim.comfy.assemblyline";
    public const string PluginName = "AssemblyLine";
    public const string PluginVersion = "1.1.0";

    static ManualLogSource _logger;
    Harmony _harmony;

    public void Awake() {
      _logger = Logger;

      BindConfig(Config);

      _harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), harmonyInstanceId: PluginGuid);
    }

    public void OnDestroy() {
      _harmony?.UnpatchSelf();
    }

    public static void LogInfo(string message) {
      Chat.m_instance.AddString(message);
      _logger.LogInfo($"[{DateTime.Now.ToString(DateTimeFormatInfo.InvariantInfo)}] {message}");
    }
  }
}