using HarmonyLib;

using static VerticallyChallenged.PluginConfig;

namespace VerticallyChallenged.Patches {
  [HarmonyPatch(typeof(Attack))]
  static class AttackPatch {
    [HarmonyPrefix]
    [HarmonyPatch(nameof(Attack.DoMeleeAttack))]
    static void DoMeleeAttackPrefix(ref Attack __instance) {
      if (!IsModEnabled.Value 
          || Player.m_localPlayer == null
          || !Player.m_localPlayer.TryGetComponent(out Character character)
          || __instance.m_character != character) {

        return;
      }

      __instance.m_maxYAngle = 180f;
      __instance.m_attackHeight = 1f;
    }
  }
}
