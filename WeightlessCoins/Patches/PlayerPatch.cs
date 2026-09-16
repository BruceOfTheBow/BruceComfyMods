using HarmonyLib;

namespace WeightlessCoins;

[HarmonyPatch(typeof(Player))]
static class PlayerPatch {
  [HarmonyPostfix]
  [HarmonyPatch(nameof(Player.Awake))]
  static void PlayerAwakePostfix(Player __instance) {
    WeightManager.UpdateCoinsWeight(__instance.GetInventory());
  }
}
