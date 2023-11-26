using HarmonyLib;

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
