
using HarmonyLib;

namespace ComfyGizmo.Patches {
  [HarmonyPatch(typeof(Game))]
  static class GamePatch {
    [HarmonyPostfix]
    [HarmonyPatch(nameof(Game.Start))]
    static void StartPostfix() {
      RotationManager.Initialize();
    }
  }
}
