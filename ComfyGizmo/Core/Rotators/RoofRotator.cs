namespace ComfyGizmo;

using System.Collections.Generic;

using UnityEngine;

public sealed class RoofRotator : AbstractRotator {
  Quaternion _roofRotation;

  public static readonly HashSet<string> RoofCornerPieceNames = [
    "wood_roof_ocorner",
    "wood_roof_ocorner_45",
    "wood_roof_icorner",
    "wood_roof_icorner_45",

    "darkwood_roof_ocorner",
    "darkwood_roof_ocorner_45",
    "darkwood_roof_icorner",
    "darkwood_roof_icorner_45"
  ];

  public RoofRotator() {
    _name = "Roof Rotator";
    _ghostGizmo = GhostGizmo.CreateGhostGizmo();
    _gizmos = Gizmos.CreateGizmos();
    ResetRotation();
  }

  public override void Rotate(Vector3 rotationAxis) {
    _eulerAngles += rotationAxis * GetAngle();
    Vector3 tiltedAxis = ConvertAxisRoofMode(rotationAxis);

    _gizmos.SetLocalRotation(_eulerAngles);
    _roofRotation *= Quaternion.AngleAxis(GetAngle(), tiltedAxis);
  }

  public override void ResetRotation() {
    _eulerAngles = Vector3.zero;

    _gizmos.SetLocalRotation(_eulerAngles);
    _roofRotation = GetBaseRotation();
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

    _gizmos.SetLocalRotation(_eulerAngles);
    _roofRotation = GetBaseRotation() * Quaternion.Euler(_eulerAngles);
  }

  public override void MatchPieceRotation(Piece target) {
    ResetRotation();
    _eulerAngles = (GetInverseBaseRotation() * Quaternion.Euler(target.transform.eulerAngles)).eulerAngles;

    _gizmos.SetLocalRotation(_eulerAngles);
    _roofRotation = Quaternion.Euler(target.transform.eulerAngles);
  }

  protected override string GetModeName() {
    return "roof rotation";
  }

  protected override Gizmos GetGizmos() {
    return _gizmos;
  }

  public override Quaternion GetRotation() {
    return _roofRotation;
  }

  private Vector3 ConvertAxisRoofMode(Vector3 rotationAxis) {
    return Quaternion.Euler(0, -45f, 0) * rotationAxis;
  }

  private Quaternion GetBaseRotation() {
    return Quaternion.AngleAxis(45f, Vector3.up);
  }

  private Quaternion GetInverseBaseRotation() {
    return Quaternion.AngleAxis(-45f, Vector3.up);
  }

  public static bool IsCornerRoofPieceSelected() {
    return
        Player.m_localPlayer
        && Player.m_localPlayer.m_placementGhost
        && RoofCornerPieceNames.Contains(Utils.GetPrefabName(Player.m_localPlayer.m_placementGhost.name));
  }
}
