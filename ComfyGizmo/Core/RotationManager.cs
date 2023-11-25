using System;
using UnityEngine;

using static ComfyGizmo.ComfyGizmo;
using static ComfyGizmo.PluginConfig;

namespace ComfyGizmo {
  public class RotationManager {
    private static bool _localFrame = false;
    private static Vector3 _eulerAngles = Vector3.zero;
    private static Quaternion _roofRotation = Quaternion.identity;

    public static void MatchPieceRotation(Piece target) {
      if (!_localFrame) {
        _eulerAngles = target.GetComponent<Transform>().eulerAngles;
        ResetRotations();
        Rotate(_eulerAngles);
        return;
      }

      InternalRotator.SetRotation(target.GetComponent<Transform>().localRotation);
      Gizmos.SetRotation(target.GetComponent<Transform>().localRotation);
    }

    public static void Rotate(Vector3 rotationAxis) {
      if (_localFrame) {
        RotateLocalFrame(rotationAxis);
        return;
      }

      if (IsRoofModeEnabled.Value && IsCornerRoofPieceSelected()) {
        RotateRoofMode(rotationAxis);
        return;
      }

      RotateDefault(rotationAxis);
    }

    public static void RotateDefault(Vector3 rotationAxis) {
      _eulerAngles += rotationAxis * GetAngle();

      InternalRotator.SetLocalRotation(Quaternion.Euler(_eulerAngles));
      Gizmos.SetComponentLocalRotations(_eulerAngles);
    }

    public static void RotateLocalFrame(Vector3 rotationAxis) {
      Quaternion rotation = Quaternion.AngleAxis(GetAngle(), rotationAxis);

      InternalRotator.ApplyRotation(rotation);
      Gizmos.ApplyRotation(rotation);
    }

    public static void RotateRoofMode(Vector3 rotationAxis) {
      Vector3 tiltedAxis = ConvertAxisRoofMode(rotationAxis);

      InternalRotator.ApplyLocalRotation(Quaternion.AngleAxis(GetAngle(), tiltedAxis));
      Gizmos.ApplyLocalRotation(Quaternion.AngleAxis(GetAngle(), rotationAxis));
      _roofRotation *= Quaternion.AngleAxis(GetAngle(), tiltedAxis);
    }

    public static void Offset() {
      _roofRotation = Quaternion.AngleAxis(-45f, Vector3.up);
    }

    private static Vector3 ConvertAxisRoofMode(Vector3 rotationAxis) {
      return Quaternion.Euler(0, 45f, 0) * rotationAxis;
    }

    public static void ResetRotations() {
      if (_localFrame) {
        ResetRotationsLocalFrame();
        return;
      }

      ResetDefaultRotations();

      if (IsRoofModeEnabled.Value) {
        _roofRotation = Quaternion.identity;
        Offset();
      }
    }

    public static void ResetRotationsLocalFrame() {
      InternalRotator.SetRotation(Quaternion.identity);
      Gizmos.SetRotation(Quaternion.identity);
    }

    public static void ResetDefaultRotations() {
      _eulerAngles = Vector3.zero;
      InternalRotator.SetLocalRotation(Quaternion.Euler(_eulerAngles));
      Gizmos.SetComponentLocalRotations(_eulerAngles);
    }

    public static void ResetAxis(Vector3 axis) {
      if (_localFrame) {
        InternalRotator.SetAxisRotation(0f, axis);
        Gizmos.SetAxisRotation(0f, axis);
      }

      InternalRotator.SetLocalAxisRotation(0f, axis);
      Gizmos.SetLocalAxisRotation(0f, axis);
    }

    public static void ChangeRotationMode() {
      if (_localFrame) {
        ChangeToDefault();
        ToggleLocalFrame();
        return;
      }

      ChangeToLocalFrame();
      ToggleLocalFrame();
    }

    public static bool IsLocalFrameEnabled() {
      return _localFrame;
    }

    public static Quaternion GetTranspilerRotation() {
      if (IsRoofModeEnabled.Value) {
        return _roofRotation;
      }

      if (!NewGizmoRotation.Value) {
        return Gizmos.GetXGizmoRoot().rotation;
      }

      return InternalRotator.GetInternalRotatorRoot().rotation;
    }

    private static void ChangeToDefault() {
      MessageHud.instance.ShowMessage(MessageHud.MessageType.TopLeft, "Default rotation mode enabled");

      if (ResetRotationOnModeChange.Value) {
        ResetRotationsLocalFrame();
        return;
      }

      _eulerAngles = InternalRotator.GetEulerAngles();
      Gizmos.SetComponentLocalRotations(_eulerAngles);
    }

    private static void ChangeToLocalFrame() {
      MessageHud.instance.ShowMessage(MessageHud.MessageType.TopLeft, "Local frame rotation mode enabled");

      if (ResetRotationOnModeChange.Value) {
        ResetDefaultRotations();
        return;
      } 

      Quaternion currentRotation = Gizmos.GetRotation();
      Gizmos.SetComponentLocalRotations(Vector3.zero);
      Gizmos.SetRotation(currentRotation);
    }

    private static void ToggleLocalFrame() {
      _localFrame = !_localFrame;
    }

    private static float GetAngle() {
      return 180f / SnapDivisions.Value;
    }

    public static void IncreaseSnapDivisions() {
      if (SnapDivisions.Value * 2 > MaxSnapDivisions) {
        return;
      }
      
      MessageHud.instance.ShowMessage(MessageHud.MessageType.TopLeft, $"Snap divisions increased to {SnapDivisions.Value * 2}");
      SnapDivisions.Value = SnapDivisions.Value * 2;

      if (!ResetRotationOnSnapDivisionChange.Value) {
        return;
      }

      if (!_localFrame) {
        ResetDefaultRotations();
      }

      ResetRotationsLocalFrame();
    }

    public static void DecreaseSnapDivisions() {
      if (Math.Floor(SnapDivisions.Value / 2f) != SnapDivisions.Value / 2f || SnapDivisions.Value / 2 < MinSnapDivisions) {
        return;
      }
      
      MessageHud.instance.ShowMessage(MessageHud.MessageType.TopLeft, $"Snap divisions decreased to {SnapDivisions.Value / 2}");
      SnapDivisions.Value = SnapDivisions.Value / 2;

      if (!ResetRotationOnSnapDivisionChange.Value) {
        return;
      }

      if (!_localFrame) {
        ResetDefaultRotations();
      }

      ResetRotationsLocalFrame();
    }
  }
}
