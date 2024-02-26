using BepInEx;
using HarmonyLib;
using System.Globalization;
using System;
using System.Reflection;

using static PassWard.PluginConfig;
using BepInEx.Logging;

namespace PassWard {
  [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
  public sealed class PassWard : BaseUnityPlugin {
    public const string PluginGuid = "bruce.valheim.mods.passward";
    public const string PluginName = "PassWard";
    public const string PluginVersion = "1.0.0";

    Harmony _harmony;

    internal static ManualLogSource _logger;

    public static readonly int WardHash = "guard_stone".GetStableHashCode();
    public static readonly int PasswordZdoFieldHash = "passward.password".GetStableHashCode();

    public static readonly string SetPasswordInputText = "Set password";
    public static readonly string ChangePasswordInputText = "Change password";
    public static readonly string EnterPasswordInputText = "Enter password";

    void Awake() {
      _logger = Logger;

      BindConfig(Config);

      _harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), harmonyInstanceId: PluginGuid);
    }

    void OnDestroy() {
      _harmony?.UnpatchSelf();
    }

    public static void ShowMessage(string message) {
      if (!MessageHud.instance) {
        return;
      }

      MessageHud.instance.ShowMessage(MessageHud.MessageType.TopLeft, message);
    }

    public static void LogError(object o) {
      _logger.LogError($"[{DateTime.Now.ToString(DateTimeFormatInfo.InvariantInfo)}] {o}");
    }
  }
}
