using HarmonyLib;

using static ComfyQuickSlots.ComfyQuickSlots;

namespace ComfyQuickSlots {
  [HarmonyPatch(typeof(TombStone))]
  static class TombstonePatch {
    [HarmonyPrefix]
    [HarmonyPatch(nameof(TombStone.GiveBoost))]
    static bool GiveBoostPrefix(TombStone __instance) {
      if (__instance.m_nview.m_zdo.GetBool(IsAdditionalTombstoneField)) {
        return false;
      }

      return true;
    }
  }
}
