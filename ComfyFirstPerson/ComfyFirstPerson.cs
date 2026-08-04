namespace ComfyFirstPerson;

using BepInEx;
using HarmonyLib;
using System.Reflection;
using UnityEngine;

using static PluginConfig;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public sealed class ComfyFirstPerson : BaseUnityPlugin {
  public const string PluginGuid = "bruce.comfy.valheim.firstperson";
  public const string PluginName = "ComfyFirstPerson";
  public const string PluginVersion = "1.0.0";

  void Awake() {
    BindConfig(Config);

    Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), harmonyInstanceId: PluginGuid);
  }

  public static void ShowMessage(string message) {
    if (!MessageHud.m_instance) {
      return;
    }

    MessageHud.m_instance.ShowMessage(MessageHud.MessageType.TopLeft, message);
  }
}
