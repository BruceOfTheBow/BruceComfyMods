namespace ComfyGizmo;

using UnityEngine;

using static PluginConfig;

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

  public void DisplayModeChangeHudMessage() {
    if (MessageHud.m_instance) {
      MessageHud.m_instance.ShowMessage(MessageHud.MessageType.TopLeft, $"Switched to {GetModeName()} mode.");
    }
  }

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

  protected abstract string GetModeName();

  protected float GetAngle() {
    return 180f / SnapDivisions.Value;
  }
}
