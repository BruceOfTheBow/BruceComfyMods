namespace SnapPointsByComfy;

using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using static PluginConfig;

public static class SnapManager {
  public static Piece LastRayPiece = null;

  public static Transform Source = null;
  public static Transform Target = null;

  public static List<Transform> _sourceSnapPoints = null;
  public static List<Transform> _targetSnapPoints = null;

  static int _targetSnapPointIndex = 0;
  static int _sourceSnapPointIndex = 0;

  public static void ClampSourceSnapPoint() {
    _sourceSnapPointIndex = Math.Min(_sourceSnapPointIndex, Math.Max(_sourceSnapPoints.Count - 1, 0));
  }

  public static void ClampTargetSnapPoint() {
    _targetSnapPointIndex = Math.Min(_targetSnapPointIndex, Math.Max(_targetSnapPoints.Count - 1, 0));
  }

  public static List<Transform> GetSnapPoints(Transform piece) {
    List<Transform> snapPoints = new ();

    if (piece == null) {
      return snapPoints;
    }

    for (int i = 0; i < piece.childCount; i++) {
      Transform child = piece.GetChild(i);

      if (child.CompareTag("snappoint")) {
        snapPoints.Add(child);
      }
    }

    if (IsCenterSnapPointEnabled.Value) {
      snapPoints.Add(piece.transform);
    }
    
    return snapPoints;
  }

  public static Vector3 GetOffset() {
    if (SnapModeSelected.Value == SnapMode.Closest) {
      if (!Player.m_localPlayer.m_placementMarkerInstance) {
        return Vector3.zero;
      }

      return _targetSnapPoints.OrderBy((Transform snapPoint) => Vector3.Distance(Player.m_localPlayer.m_placementMarkerInstance.transform.position, snapPoint.position)).First().position - _sourceSnapPoints[_sourceSnapPointIndex].position;
    }

    return _targetSnapPoints[_targetSnapPointIndex].position - _sourceSnapPoints[_sourceSnapPointIndex].position;
  }

  public static bool IsValidSnapPoints() {
    return _targetSnapPoints.Any() && _sourceSnapPoints.Any();
  }

  public static void NextSourceSnapPoint() {
    _sourceSnapPointIndex++;

    if (_sourceSnapPointIndex >= GetSnapPoints(Source).Count) {
      _sourceSnapPointIndex = 0;
    }
  }

  public static void NextTargetSnapPoint() {
    _targetSnapPointIndex++;

    if (_targetSnapPointIndex >= GetSnapPoints(Target).Count) {
      _targetSnapPointIndex = 0;
    }
  }

  public static void RebuildSnapPoints() {
    _sourceSnapPoints = GetSnapPoints(Source);
    _sourceSnapPointIndex = 0;

    _targetSnapPoints = GetSnapPoints(Target);
    _targetSnapPointIndex = 0;
  }

  public static void ResetSourceSnapPoint() {
    _sourceSnapPointIndex = 0;
  }

  public static void ResetTargetSnapPoint() {
    _targetSnapPointIndex = 0;
  }

  public static void SetSource(Transform parent) {
    Source = parent;
    _sourceSnapPoints = GetSnapPoints(Source);
  }

  public static void SetTarget(Transform parent) {
    Target = parent;
    _targetSnapPoints = GetSnapPoints(Target);
  }

  public static void ToggleSnapMode() {
    if (SnapModeSelected.Value == SnapMode.Cycle) {
      SnapModeSelected.Value = SnapMode.Closest;
      return;
    }

    SnapModeSelected.Value = SnapMode.Cycle;
  }

  public enum SnapMode {
    Cycle,
    Closest
  }
}
