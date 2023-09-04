using System;
using HarmonyLib;

using UnityEngine;

using static Aurality.Aurality;
using static Aurality.PluginConfig;

namespace Aurality.Patches {
  [HarmonyPatch(typeof(ZSFX))]
  static class ZSFXPatch {
    [HarmonyPostfix]
    [HarmonyPatch(nameof(ZSFX.Awake))]
    static void ZSFXAwakePostix(ZSFX __instance) {
      if (!IsModEnabled.Value || !__instance || !__instance.m_audioSource) {
        return;
      }

      if (IsBuzzingDisabled.Value && BuzzPrefabs.Contains(__instance.name)) { 
        __instance.m_audioSource.mute = true;
      }

      if (IsBuzzingDisabled.Value && __instance.m_audioClips != null && Array.Find(__instance.m_audioClips, x => x.name.Equals("Insect_Wasp_WingsLoop3"))) {
        __instance.m_audioSource.mute = true;
      }
    }
  }
}
