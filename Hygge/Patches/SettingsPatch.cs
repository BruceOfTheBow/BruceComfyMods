using HarmonyLib;

using static Hygge.PluginConfig;

namespace Hygge.Patches {
  [HarmonyPatch(typeof(Settings))]
  static class SettingsPatch {
    [HarmonyPostfix]
    [HarmonyPatch(nameof(Settings.ApplyMode))]
    public static void ApplyModePostfix(ref Settings __instance) {
      if (!IsModEnabled.Value || !ComfortPanelManager.ComfortPanel?.Panel) {
        return;
      }

      ComfortPanelManager.DestroyPanel();
    }
  }
}
    