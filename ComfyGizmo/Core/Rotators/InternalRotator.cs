﻿namespace ComfyGizmo;

using UnityEngine;

public sealed class InternalRotator : AbstractRotator {
  public InternalRotator() {
    _name = "Internal Rotator";
    _ghostGizmo = GhostGizmo.CreateGhostGizmo();
    _gizmos = Gizmos.CreateGizmos();
    ResetRotation();
  }

  public override void Rotate(Vector3 rotationAxis) {
    _eulerAngles += rotationAxis * GetAngle();
    _ghostGizmo.SetLocalRotation(Quaternion.Euler(_eulerAngles));
    _gizmos.SetLocalRotation(_eulerAngles);
  }

  public override void ResetRotation() {
    _eulerAngles = Vector3.zero;
    _ghostGizmo.SetLocalRotation(Quaternion.Euler(_eulerAngles));
    _gizmos.SetLocalRotation(_eulerAngles);
  }

  public override void ResetAxis(Vector3 axis) {
    if (axis == Vector3.up) {
      _eulerAngles.y = 0f;
    }

    if (axis == Vector3.right) {
      _eulerAngles.x = 0f;
    }

    if (axis == Vector3.forward) {
      _eulerAngles.z = 0f;
    }

    _ghostGizmo.SetLocalRotation(Quaternion.Euler(_eulerAngles));
    _gizmos.SetLocalRotation(_eulerAngles);
  }

  public override void MatchPieceRotation(Piece target) {
    ResetRotation();
    _eulerAngles = target.GetComponent<Transform>().eulerAngles;
    Rotate(Vector3.zero);
  }

  protected override string GetModeName() {
    return "old rotation";
  }

  protected override Gizmos GetGizmos() {
    return _gizmos;
  }

  public override Quaternion GetRotation() {
    return _ghostGizmo.GetRotation();
  }
}
