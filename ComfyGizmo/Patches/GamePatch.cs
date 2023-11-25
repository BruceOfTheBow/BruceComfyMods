
using HarmonyLib;
using UnityEngine;

using static ComfyGizmo.PluginConfig;

namespace ComfyGizmo.Patches {
  [HarmonyPatch(typeof(Game))]
  static class GamePatch {
    [HarmonyPostfix]
    [HarmonyPatch(nameof(Game.Start))]
    static void StartPostfix() {
      Gizmos.Destroy();
      Gizmos.Initialize();
      Gizmos.SetComponentLocalRotations(Vector3.zero);

      InternalRotator.Destroy();
      InternalRotator.Initialize();

      if (!IsRoofModeEnabled.Value) {
        Gizmos.SetRotation(Quaternion.identity);
      }

      Gizmos.SetRotation(Quaternion.identity);
      Gizmos.ApplyRotation(Quaternion.AngleAxis(45f, Vector3.up));
    }
  }
}
