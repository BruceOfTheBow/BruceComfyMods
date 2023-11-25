using HarmonyLib;

using static ComfyGizmo.ComfyGizmo;

namespace ComfyGizmo.Patches {
  [HarmonyPatch(typeof(FejdStartup))]
  static class FejdStartupPatch {

    [HarmonyPostfix]
    [HarmonyPatch(nameof(FejdStartup.Awake))]
    static void AwakePostfix() {
      HammerTableManager.Initialize();
    }
  }
}
