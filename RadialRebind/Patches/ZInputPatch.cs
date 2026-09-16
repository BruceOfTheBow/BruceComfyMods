namespace RadialRebind;

using HarmonyLib;

using static PluginConfig;

[HarmonyPatch(typeof(ZInput))]
static class ZInputPatch {

  [HarmonyPostfix]
  [HarmonyPatch(nameof(ZInput.Load))]
  static void LoadPostfix(ZInput __instance) {
    if (!IsModEnabled.Value || __instance == null) {
      return;
    }

    BindManager.RebindOpenRadial(__instance);
  }
}
