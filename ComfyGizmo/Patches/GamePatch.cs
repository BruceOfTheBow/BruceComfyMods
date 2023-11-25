
using HarmonyLib;
using UnityEngine;

namespace ComfyGizmo.Patches {
  [HarmonyPatch(typeof(Game))]
  static class GamePatch {
    [HarmonyPostfix]
    [HarmonyPatch(nameof(Game.Start))]
    static void StartPostfix() {
      Gizmos.Destroy();
      Gizmos.Initialize();
      Gizmos.SetComponentLocalRotations(Vector3.zero);
      Gizmos.SetRotation(Quaternion.identity);

      InternalRotator.Destroy();
      InternalRotator.Initialize();
    }
  }
}
