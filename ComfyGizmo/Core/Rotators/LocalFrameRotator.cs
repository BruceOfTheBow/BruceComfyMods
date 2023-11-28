using UnityEngine;

using static ComfyGizmo.PluginConfig;

namespace ComfyGizmo {
  public class LocalFrameRotator : AbstractRotator {
    public LocalFrameRotator() {
      _name = "Local Frame Rotator";
      _ghostGizmo = GhostGizmo.CreateGhostGizmo();
      _gizmos = Gizmos.CreateGizmos();
      ResetRotation();
    }

    public override void Rotate(Vector3 rotationAxis) {
      Quaternion rotation = Quaternion.AngleAxis(GetAngle(), rotationAxis);

      _ghostGizmo.ApplyRotation(rotation);
      _gizmos.ApplyRotation(rotation);
    }

    public override void ResetRotation() {
      _ghostGizmo.SetRotation(Quaternion.identity);
      _gizmos.SetRotation(Quaternion.identity);
    }

    public override void ResetAxis(Vector3 axis) {
      _ghostGizmo.SetAxisRotation(0f, axis);
      _gizmos.SetAxisRotation(0f, axis);
    }

    public override void MatchPieceRotation(Piece target) {
      _ghostGizmo.SetRotation(target.GetComponent<Transform>().localRotation);
      _gizmos.SetRotation(target.GetComponent<Transform>().localRotation);
    }

    protected override string GetModeName() {
      return "local frame rotation";
    }

    protected override Gizmos GetGizmos() {
      return _gizmos;
    }

    public override Quaternion GetRotation() {
      if (!IsOldRotationEnabled.Value) {
        return _gizmos.GetXGizmoRoot().rotation;
      }

      return _ghostGizmo.GetRotation();
    }
  }
}
