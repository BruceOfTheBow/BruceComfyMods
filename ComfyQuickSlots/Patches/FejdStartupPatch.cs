namespace ComfyQuickSlots;

using HarmonyLib;

[HarmonyPatch(typeof(FejdStartup))]
static class FejdStartupPatch {
  [HarmonyPrefix]
  [HarmonyPatch(nameof(FejdStartup.SetupCharacterPreview))]
  static void SetupCharacterPreviewPrefix() {
    QuickSlotsManager.InitialEquippedArmor.Clear();
    QuickSlotsManager.FirstLoad = false;
  }
}
