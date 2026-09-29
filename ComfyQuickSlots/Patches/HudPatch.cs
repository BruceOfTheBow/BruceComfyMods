namespace ComfyQuickSlots;

using HarmonyLib;

using static PluginConfig;
using static HotkeyBarManager;

[HarmonyPatch(typeof(Hud))]
static class HudPatch {
  [HarmonyPostfix]
  [HarmonyPatch(nameof(Hud.Awake))]
  static void AwakePostfix(Hud __instance) {
    QuickSlotsHotkeyBar = __instance.GetComponentInChildren<HotkeyBar>();
  }

  [HarmonyPostfix]
  [HarmonyPatch(nameof(Hud.Update))]
  static void UpdatePostfix(Hud __instance) {
    if (!IsModEnabled.Value || !EnableQuickslots.Value || QuickSlotsHotkeyBarGameObject) {
      return;
    }

    CreateHotkeyBarGameObject(__instance);
  }
}
