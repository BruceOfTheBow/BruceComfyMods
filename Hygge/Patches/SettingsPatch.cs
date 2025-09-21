namespace Hygge;

using HarmonyLib;

using Valheim.SettingsGui;

using static PluginConfig;

[HarmonyPatch(typeof(GraphicsSettings))]
static class SettingsPatch {
  [HarmonyPostfix]
  [HarmonyPatch(nameof(GraphicsSettings.ApplyResolution))]
  public static void ApplyResolutionPostfix(GraphicsSettings __instance) {
    if (IsModEnabled.Value) {
      ComfortPanelManager.DestroyPanel();
    }
  }
}
