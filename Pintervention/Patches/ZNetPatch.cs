namespace Pintervention;

using HarmonyLib;

using static PluginConfig;

[HarmonyPatch(typeof(ZNet))]
static class ZNetPatch {
  [HarmonyPrefix]
  [HarmonyPatch(nameof(ZNet.OnDestroy))]
  static void OnDestroyPrefix() {
    if (IsModEnabled.Value) {
      PinOwnerManager.WriteFilteredPlayersToFile();
      PinOwnerManager.Clear();
    }
  }
}
