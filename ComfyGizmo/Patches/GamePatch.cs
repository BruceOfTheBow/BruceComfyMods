namespace ComfyGizmo;

using HarmonyLib;

[HarmonyPatch(typeof(Game))]
static class GamePatch {
  [HarmonyPostfix]
  [HarmonyPatch(nameof(Game.Start))]
  static void StartPostfix() {
    RotationManager.Initialize();
  }
}
