using HarmonyLib;
using UnityEngine;

namespace ComfyFirstPerson;

using static PluginConfig;

[HarmonyPatch(typeof(Player))]
public static class PlayerPatch {
  [HarmonyPrefix]
  [HarmonyPatch(nameof(Player.TestGhostClipping))]
  static bool TestGhostClippingPrefix(ref Player __instance, ref GameObject ghost, ref float maxPenetration) {
    if (!IsModEnabled.Value) {
      return true;
    }

    return false;
  }
}
