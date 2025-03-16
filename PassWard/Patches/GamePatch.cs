namespace PassWard;

using HarmonyLib;

using static PluginConfig;

[HarmonyPatch(typeof(Game))]
static class GamePatch {
  [HarmonyPostfix]
  [HarmonyPatch(nameof(Game.Start))]
  static void StartPostfix() {
    if (IsModEnabled.Value) {
      WardManager.ClearCachedPermittedPlayerNames();
    }
  }

  [HarmonyPrefix]
  [HarmonyPatch(nameof(Game.OnDestroy))]
  static void OnDestroyPrefix() {
    if (IsModEnabled.Value) {
      WardManager.ClearCachedPermittedPlayerNames();
    }
  }
}
