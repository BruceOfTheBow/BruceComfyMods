namespace ComfyQuickSlots;

using HarmonyLib;

[HarmonyPatch(typeof(TombStone))]
static class TombStonePatch {
  [HarmonyPrefix]
  [HarmonyPatch(nameof(TombStone.GiveBoost))]
  static bool GiveBoostPrefix(TombStone __instance) {
    if (TombStoneManager.IsAdditionalTombStone(__instance)) {
      return false;
    }

    return true;
  }
}
