using System.Collections.Generic;
using System.Linq;
using BepInEx.Configuration;
using HarmonyLib;
using HomieHeadcount.Core;
using UnityEngine;

using static HomieHeadcount.PluginConfig;

namespace HomieHeadcount {
  [HarmonyPatch(typeof(Tameable))]
  static class TameablePatch {
    [HarmonyPrefix]
    [HarmonyPatch(nameof(Tameable.UnSummon))]
    public static void UnsummonPrefix(Tameable __instance) {
      if (!IsModEnabled.Value
          || !__instance.gameObject
          || !HomieCounter.Contains(__instance)) {

        return;
      }

      HomieCounter.Remove(__instance);
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(Tameable.OnDeath))]
    public static void OnDeathPrefix(Tameable __instance) {
      if (!IsModEnabled.Value
          || !__instance.gameObject
          || !HomieCounter.Contains(__instance)) {

        return;
      }
      
      HomieCounter.Remove(__instance);
    }
  }
}
