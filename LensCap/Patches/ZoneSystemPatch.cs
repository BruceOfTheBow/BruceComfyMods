using HarmonyLib;

using static LensCap.LensCap;
using static LensCap.PluginConfig;

namespace LensCap {
  [HarmonyPatch(typeof(ZoneSystem))]
  static class ZoneSystemPatch {
    [HarmonyPostfix]
    [HarmonyPatch(nameof(ZoneSystem.Start))]
    static void StartPostfix(ref ZoneSystem __instance) {
      if (!IsModEnabled.Value) {
        return;
      }

      GetDefaultIntensity();
      RemoveLensDirt();
    }
  }
}
