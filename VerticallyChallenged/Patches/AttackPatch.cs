using HarmonyLib;

using static VerticallyChallenged.PluginConfig;

namespace VerticallyChallenged.Patches {
  [HarmonyPatch(typeof(Attack))]
  static class AttackPatch {
    [HarmonyPrefix]
    [HarmonyPatch(nameof(Attack.DoMeleeAttack))]
    static void DoMeleeAttackPrefix(ref Attack __instance) {
      if (!IsModEnabled.Value) {
        return;
      }

      __instance.m_maxYAngle = 180f;
      __instance.m_attackHeight = 1f;
    }
  }
}
