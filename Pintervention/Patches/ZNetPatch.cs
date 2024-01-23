using HarmonyLib;

using static Pintervention.PluginConfig;

namespace Pintervention {
  [HarmonyPatch(typeof(ZNet))]
  static class ZNetPatch {
    [HarmonyPostfix]
    [HarmonyPatch(nameof(ZNet.LoadWorld))]
    static void LoadWorldPostfix() {
      if (!IsModEnabled.Value) {
        return;
      }

      NameManager.LoadPlayerNames();
    }
  }
}
