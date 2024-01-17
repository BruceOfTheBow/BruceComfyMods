using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;

using static Pintervention.PluginConfig;

namespace Pintervention {
  [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
  public class Pintervention : BaseUnityPlugin {
    public const string PluginGuid = "bruce.valheim.comfymods.pintervention";
    public const string PluginName = "Pintervention";
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

    public static void MessageLocalPlayer(string message) {
      if (!Player.m_localPlayer) {
        return;
      }

      Player.m_localPlayer.Message(MessageHud.MessageType.Center, message, 0, null);
    }

    public static void Log(string message) {
      _logger.LogInfo(message);
    }

    public static void LogWarning(string message) {
      _logger.LogWarning(message);
    }
  }
}
