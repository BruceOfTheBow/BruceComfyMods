using System.Collections.Generic;
using UnityEngine;

namespace ComfyGizmo.Core.Rotators {
  public class RoofRotator : AbstractRotator {
    Quaternion _roofRotation;

    private static readonly List<int> _roofCornerPieceHashCodes = new() {
      "wood_roof_ocorner".GetStableHashCode(),
      "wood_roof_ocorner_45".GetStableHashCode(),
      "wood_roof_icorner".GetStableHashCode(),
      "wood_roof_icorner_45".GetStableHashCode(),

      "darkwood_roof_ocorner".GetStableHashCode(),
      "darkwood_roof_ocorner_45".GetStableHashCode(),
      "darkwood_roof_icorner".GetStableHashCode(),
      "darkwood_roof_icorner_45".GetStableHashCode()
    };

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
      _eulerAngles = (GetInverseBaseRotation() * Quaternion.Euler(target.GetComponent<Transform>().eulerAngles)).eulerAngles;

      _gizmos.SetLocalRotation(_eulerAngles);
      _roofRotation = Quaternion.Euler(target.GetComponent<Transform>().eulerAngles);
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
      if (Player.m_localPlayer == null || Player.m_localPlayer.m_placementGhost == null) {
        return false;
      }

      if (!_roofCornerPieceHashCodes.Contains(Player.m_localPlayer.m_placementGhost.name.Replace("(Clone)", "").GetStableHashCode())) {
        return false;
      }

      return true;
    }
  }
}
