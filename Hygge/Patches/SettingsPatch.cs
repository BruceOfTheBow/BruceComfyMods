using Fishlabs.Valheim;
using HarmonyLib;

using static Hygge.PluginConfig;

namespace Hygge.Patches {
  [HarmonyPatch(typeof(GraphicsSettings))]
  static class SettingsPatch {
    [HarmonyPostfix]
    [HarmonyPatch(nameof(GraphicsSettings.ApplyResolution))]
    public static void ApplyResolutionPostfix(ref GraphicsSettings __instance) {
      if (!IsModEnabled.Value || !ComfortPanelManager.ComfortPanel?.Panel) {
        return;
      }

      ComfortPanelManager.DestroyPanel();
    }
  }
}
    