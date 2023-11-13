
using HarmonyLib;

using static Gizmo.ComfyGizmo;

namespace Gizmo.Patches {
  [HarmonyPatch(typeof(Game))]
  static class GamePatch {
    [HarmonyPostfix]
    [HarmonyPatch(nameof(Game.Start))]
    static void StartPostfix() {
      UnityEngine.GameObject.Destroy(GizmoRoot);
      GizmoRoot = CreateGizmoRoot();

      UnityEngine.GameObject.Destroy(ComfyGizmoObj);
      ComfyGizmoObj = new("ComfyGizmo");
      _comfyGizmoRoot = ComfyGizmoObj.transform;
      LocalFrame = false;
    }
  }
}
