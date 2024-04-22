namespace Aurality;

using HarmonyLib;

using static PluginConfig;

[HarmonyPatch(typeof(ZSFX))]
static class ZSFXPatch {
  [HarmonyPostfix]
  [HarmonyPatch(nameof(ZSFX.Awake))]
  static void ZSFXAwakePostix(ZSFX __instance) {
    if (IsModEnabled.Value && IsBuzzingDisabled.Value && ZSFXUtils.IsBuzzingSfx(__instance)) {
      __instance.m_audioSource.mute = true;
    }
  }
}
