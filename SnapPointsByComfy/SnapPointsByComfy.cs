namespace SnapPointsByComfy;

using System;
using System.Globalization;
using System.Reflection;

using BepInEx;
using BepInEx.Logging;

using HarmonyLib;

using static PluginConfig;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public sealed class SnapPointsByComfy : BaseUnityPlugin {
  public const string PluginGuid = "bruce.comfy.valheim.snappointsbycomfy";
  public const string PluginName = "SnapPointsByComfy";
  public const string PluginVersion = "1.0.0";

  static ManualLogSource _logger;

  void Awake() {
    _logger = Logger;

    BindConfig(Config);

    Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), harmonyInstanceId: PluginGuid);
  }

  public static void SendModeChangeMessage() {
    Player.m_localPlayer.Message(MessageHud.MessageType.TopLeft, string.Format("Snap Points Made Easy: Switched to {0}", SnapModeSelected.Value), 0, null);
  }

  public static void ToggleModEnabled() {
    IsModEnabled.Value = !IsModEnabled.Value;
  }

  public static void LogInfo(object obj) {
    _logger.LogInfo($"[{DateTime.Now.ToString(DateTimeFormatInfo.InvariantInfo)}] {obj}");
  }

  public static void LogError(object obj) {
    _logger.LogError($"[{DateTime.Now.ToString(DateTimeFormatInfo.InvariantInfo)}] {obj}");
  }
}
