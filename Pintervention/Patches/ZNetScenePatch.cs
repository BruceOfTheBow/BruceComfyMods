using HarmonyLib;

using static Pintervention.PluginConfig;

namespace Pintervention {
  [HarmonyPatch(typeof(ZNetScene))]
  static class ZNetScenePatch {
    [HarmonyPostfix]
    [HarmonyPatch(nameof(ZNetScene.Awake))]
    static void AwakePostfix() {
      if (!IsModEnabled.Value) {
        return;
      }

      NameManager.LoadPlayerNames();
      PinOwnerManager.LoadFilteredPinOwners();
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(ZNetScene.OnDestroy))]
    static void OnDestroyPostfix() {
      PinOwnerManager.Clear();

      if (!IsModEnabled.Value) {
        return;
      }

      PinOwnerManager.WriteFilteredPlayersToFile();
    }
  }
}
