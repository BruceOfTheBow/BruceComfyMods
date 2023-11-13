using HarmonyLib;

using static Gizmo.ComfyGizmo;

namespace Gizmo.Patches {
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
