using HarmonyLib;

namespace HomieHeadcount {
  [HarmonyPatch(typeof(Tameable))]
  static class TameablePatch {
    [HarmonyPrefix]
    [HarmonyPatch(nameof(Tameable.UnSummon))]
    public static void UnsummonPrefix(Tameable __instance) {
      if (!__instance.gameObject
          || !HomieCounter.Contains(__instance)) {

        return;
      }

      HomieCounter.Remove(__instance);
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(Tameable.OnDeath))]
    public static void OnDeathPrefix(Tameable __instance) {
      if (!__instance.gameObject
          || !HomieCounter.Contains(__instance)) {

        return;
      }
      
      HomieCounter.Remove(__instance);
    }
  }
}
