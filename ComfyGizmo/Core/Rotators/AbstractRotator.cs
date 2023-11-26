using UnityEngine;
using static ComfyGizmo.PluginConfig;

namespace ComfyGizmo {
  
  public abstract class AbstractRotator {
    protected Gizmos _gizmos;
    protected GhostGizmo _ghostGizmo;
    protected string _name;

    protected Vector3 _eulerAngles = Vector3.zero;
    public abstract void Rotate(Vector3 rotationAxis);

    public abstract void ResetRotation();

    public abstract void ResetAxis(Vector3 axis);

    public abstract void MatchPieceRotation(Piece target);

    public abstract Quaternion GetRotation();


    public void Destroy() {
      DestroyGizmos();
      DestroyGhostGizmo();
    }

    private void DestroyGizmos() {
      if (_gizmos == null) {
        return;
      }

      _gizmos.Destroy();
    }

    private void DestroyGhostGizmo() {
      if (_ghostGizmo == null ) {
        return;
      }

      _ghostGizmo.Destroy();
    }

    protected abstract Gizmos GetGizmos();

    public void ShowGizmos(Player player) {
      GetGizmos().Show(player);
    }

    public void HideGizmos() {
      GetGizmos().Hide();
    }

    public void ResetScales() {
      GetGizmos().ResetScale();
    }

    public void SetXScale(float scale) {
      GetGizmos().SetXScale(scale);
    }

    public void SetYScale(float scale) {
      GetGizmos().SetYScale(scale);
    }

    public void SetZScale(float scale) {
      GetGizmos().SetZScale(scale);
    }

    protected float GetAngle() {
      return 180f / SnapDivisions.Value;
    }
  }
}
