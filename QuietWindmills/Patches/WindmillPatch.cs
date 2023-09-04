using HarmonyLib;

using static QuietWindmills.PluginConfig;
using static QuietWindmills.QuietWindmills;

namespace QuietWindmills {
  [HarmonyPatch(typeof(Windmill))]
  static class WindmillPatch {

    [HarmonyPrefix]
    [HarmonyPatch(nameof(Windmill.Update))]
    static void WindmillUpdatePrefix(Windmill __instance) {
      if (!IsModEnabled.Value
            || __instance == null
            || !__instance.TryGetComponent(out Smelter smelter)) {

        return;
      }

      if (smelter.IsActive()) {
        __instance.m_propellerRotationSpeed = DefaultPropellerSpeed;
        __instance.m_maxVol = MaxVolume.Value;
        return;
      }

      if (!QuietWhenEmpty.Value) {
        return;
      }

      __instance.m_propellerRotationSpeed = 0f;
      __instance.m_maxVol = 0f;
    }
  }
}
