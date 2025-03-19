namespace Pintervention;

using System;
using System.Globalization;
using System.Reflection;

using BepInEx;
using BepInEx.Logging;

using HarmonyLib;

using static PluginConfig;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public sealed class Pintervention : BaseUnityPlugin {
  public const string PluginGuid = "bruce.valheim.comfymods.pintervention";
  public const string PluginName = "Pintervention";
  public const string PluginVersion = "1.3.0";

  static ManualLogSource _logger;

  void Awake() {
    _logger = Logger;
    BindConfig(Config);

    Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), harmonyInstanceId: PluginGuid);
  }

  public static void MessageLocalPlayer(string message) {
    if (!Player.m_localPlayer) {
      return;
    }

    Player.m_localPlayer.Message(MessageHud.MessageType.Center, message, 0, null);
  }

  public static void LogInfo<T>(T obj) {
    _logger.LogInfo($"[{DateTime.Now.ToString(DateTimeFormatInfo.InvariantInfo)}] {obj}");
  }

  public static void LogWarning<T>(T obj) {
    _logger.LogWarning($"[{DateTime.Now.ToString(DateTimeFormatInfo.InvariantInfo)}] {obj}");
  }
}
