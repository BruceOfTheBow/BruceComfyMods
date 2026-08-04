namespace ComfyFirstPerson.Patches;

using HarmonyLib;

using UnityEngine;

using static ComfyFirstPerson;
using static PluginConfig;

[HarmonyPatch(typeof(GameCamera))]
public static class GameCameraPatch {

  [HarmonyPostfix]
  [HarmonyPatch(nameof(GameCamera.Awake))]
  static void Postfix(ref GameCamera __instance) {
    if (FPVManager.IsInitialized()) {
      return;
    }

    FPVManager.Initialize();
  }

  [HarmonyPostfix]
  [HarmonyPatch(nameof(GameCamera.UpdateCamera))]
  static void Postfix(ref GameCamera __instance, float dt) {
    if (!IsModEnabled.Value
        || __instance == null
        || Player.m_localPlayer == null 
        || Player.m_localPlayer.m_buildPieces
        || (Player.m_localPlayer.IsDead() && Player.m_localPlayer.GetRagdoll())) {
      return;
    }

    if (Input.GetKeyDown(ToggleKey.Value.MainKey)) {
      FPVManager.ToggleFirstPerson();
    }

    if (!FPVManager.IsFirstPerson()) {
      return;
    }

    FPVManager.SetCameraPosition();

    if (!FPVManager.CanChangePOV()) {
      return;
    }

    if (ZInput.GetKeyDown(RaiseKey.Value.MainKey)) {
      FPVManager.IncreaseFOV();
    } 
      
    if (ZInput.GetKeyDown(LowerKey.Value.MainKey)) {
      FPVManager.DecreaseFOV();
    }
  }
}