using HarmonyLib;

using static Pintervention.PluginConfig;

namespace Pintervention {
  [HarmonyPatch(typeof(ZNet))]
  static class ZNetPatch {
    [HarmonyPrefix]
    [HarmonyPatch(nameof(ZNet.OnDestroy))]
    static void OnDestroyPrefix() {
      if (!IsModEnabled.Value) {
        return;
      }

      PinOwnerManager.WriteFilteredPlayersToFile();
      PinOwnerManager.Clear();      
    }
  }
}
