using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ComfyGizmo {
  public class DefaultRotator : AbstractRotator {
    public DefaultRotator() {
      _name = "Default Rotator";
      _gizmos = Gizmos.CreateGizmos();
      ResetRotation();
    }

    public override void Rotate(Vector3 rotationAxis) {
      _eulerAngles += rotationAxis * GetAngle();
      _gizmos.SetLocalRotation(_eulerAngles);
    }

    public override void ResetRotation() {
      _eulerAngles = Vector3.zero;
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

      _gizmos.SetLocalRotation(_eulerAngles);
    }

    public override void MatchPieceRotation(Piece target) {
      ResetRotation();
      _eulerAngles = target.GetComponent<Transform>().eulerAngles;
      Rotate(Vector3.zero);
    }


    protected override Gizmos GetGizmos() {
      return _gizmos;
    }

    public override Quaternion GetRotation() {
      return _gizmos.GetXGizmoRoot().rotation;
    }
  }
}
