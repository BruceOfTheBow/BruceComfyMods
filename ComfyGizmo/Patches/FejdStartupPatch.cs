using HarmonyLib;

using static ComfyGizmo.ComfyGizmo;

namespace ComfyGizmo.Patches {
  [HarmonyPatch(typeof(FejdStartup))]
  static class FejdStartupPatch {

    [HarmonyPostfix]
    [HarmonyPatch(nameof(FejdStartup.Awake))]
    static void AwakePostfix() {
      if (IsSearsCatalogEnabled()) {
        int searsCatalogColumnCount = GetBuildPanelColumns();

        if (searsCatalogColumnCount != -1) {
          ColumnCount = searsCatalogColumnCount;
        }
      }
    }
  }
}
