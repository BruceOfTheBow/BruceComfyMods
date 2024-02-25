using BepInEx;
using HarmonyLib;
using System.Reflection;

using static PassWard.PluginConfig;

namespace PassWard {
  [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
  public sealed class PassWard : BaseUnityPlugin {
    public const string PluginGuid = "bruce.valheim.mods.passward";
    public const string PluginName = "PassWard";
    public const string PluginVersion = "1.0.0";

    Harmony _harmony;

    public static readonly int WardHash = "guard_stone".GetStableHashCode();
    public static readonly int PasswordZdoFieldHash = "passward.password".GetStableHashCode();
    public static readonly string SetPasswordInputText = "Set password";

    void Awake() {
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

      MessageHud.instance.ShowMessage(MessageHud.MessageType.TopLeft, "message");
    }
  }
}
