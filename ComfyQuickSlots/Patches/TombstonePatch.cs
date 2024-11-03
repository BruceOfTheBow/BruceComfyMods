namespace ComfyQuickSlots;

using HarmonyLib;

[HarmonyPatch(typeof(TombStone))]
static class TombstonePatch {
  [HarmonyPrefix]
  [HarmonyPatch(nameof(TombStone.GiveBoost))]
  static bool GiveBoostPrefix(TombStone __instance) {
    if (__instance.m_nview.m_zdo.GetBool(QuickSlotsManager.IsAdditionalTombstoneField)) {
      return false;
    }

    return true;
  }
}
