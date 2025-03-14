﻿namespace ComfyGizmo;

using System;
using System.Collections.Generic;

using UnityEngine;

using static PluginConfig;

public static class RotationManager {
  static DefaultRotator _defaultRotator;
  static InternalRotator _internalRotator;
  static LocalFrameRotator _localFrameRotator;
  static RoofRotator _roofRotator;

  public static void Initialize() {
    _defaultRotator = new DefaultRotator();
    _internalRotator = new InternalRotator();
    _localFrameRotator = new LocalFrameRotator();
    _roofRotator = new RoofRotator();
  }

  public static void Rotate(Vector3 rotationAxis) {
    _defaultRotator.Rotate(rotationAxis);
    _internalRotator.Rotate(rotationAxis);
    _localFrameRotator.Rotate(rotationAxis);
    _roofRotator.Rotate(rotationAxis);
  }

  public static void ResetRotation() {
    _defaultRotator.ResetRotation();
    _internalRotator.ResetRotation();
    _localFrameRotator.ResetRotation();
    _roofRotator.ResetRotation();
  }

  public static void ResetRotationConditional() {
    if (!ResetRotationOnSnapDivisionChange.Value) {
      return;
    }

    ResetRotation();
  }

  public static void ResetAxis(Vector3 axis) {
    _defaultRotator.ResetAxis(axis);
    _internalRotator.ResetAxis(axis);
    _localFrameRotator.ResetAxis(axis);
    _roofRotator.ResetAxis(axis);
  }

  public static void MatchPieceRotation(Piece target) {
    _defaultRotator.MatchPieceRotation(target);
    _internalRotator.MatchPieceRotation(target);
    _localFrameRotator.MatchPieceRotation(target);
    _roofRotator.MatchPieceRotation(target);
  }

  public static Quaternion GetRotation() {
    return GetActiveRotator().GetRotation();
  }

  public static void ShowGizmos(Player player) {
    GetActiveRotator().ShowGizmos(player);
  }

  public static void HideGizmos() {
    _defaultRotator.HideGizmos();
    _internalRotator.HideGizmos();
    _localFrameRotator.HideGizmos();
    _roofRotator.HideGizmos();
  }

  public static void ResetScales() {
    _defaultRotator.ResetScales();
    _internalRotator.ResetScales();
    _localFrameRotator.ResetScales();
    _roofRotator.ResetScales();
  }

  public static void SetActiveXScale(float scale) {
    GetActiveRotator().SetXScale(scale);
  }

  public static void SetActiveYScale(float scale) {
    GetActiveRotator().SetYScale(scale);
  }

  public static void SetActiveZScale(float scale) {
    GetActiveRotator().SetZScale(scale);
  }

  public static void OnModeChange(Player player) {
    if (ResetRotationOnModeChange.Value) {
      ResetRotation();
    }

    HideGizmos();
    GetActiveRotator().DisplayModeChangeHudMessage();
    GetActiveRotator().ShowGizmos(player);
  }
  
  private static AbstractRotator GetActiveRotator() {
    if (IsRoofModeEnabled.Value && RoofRotator.IsCornerRoofPieceSelected()) {
      return _roofRotator;
    }

    if (IsLocalFrameEnabled.Value) {
      return _localFrameRotator;
    }

    if (IsOldRotationEnabled.Value) {
      return _internalRotator;
    }

    return _defaultRotator;
  }

  public static void DestroyRotators() {
    if (_defaultRotator != null) {
      _defaultRotator.Destroy();
      _defaultRotator = null;
    }

    if (_internalRotator != null) {
      _internalRotator.Destroy();
      _internalRotator = null;
    }

    if (_localFrameRotator != null) {
      _localFrameRotator.Destroy();
      _localFrameRotator = null;
    }

    if (_roofRotator != null) {
      _roofRotator.Destroy();
      _roofRotator = null;
    }
  }

  public static void DisableLocalFrameMode() {
    if (IsLocalFrameEnabled.Value) {
      IsRoofModeEnabled.Value = false;

      ShowMessage($"Disabled roof mode.");
    }
  }

  public static void IncreaseSnapDivisions() {
    if (SnapDivisions.Value * 2 > MaxSnapDivisions) {
      return;
    }

    ShowMessage($"Snap divisions increased to {SnapDivisions.Value * 2}");
    SnapDivisions.Value = SnapDivisions.Value * 2;

    ResetRotationConditional();
  }

  public static void DecreaseSnapDivisions() {
    if (Math.Floor(SnapDivisions.Value / 2f) != (SnapDivisions.Value / 2f)
        || SnapDivisions.Value / 2 < MinSnapDivisions) {
      return;
    }
    
    ShowMessage($"Snap divisions decreased to {SnapDivisions.Value / 2}");
    SnapDivisions.Value = SnapDivisions.Value / 2;

    ResetRotationConditional();
  }

  static void ShowMessage(string message) {
    if (MessageHud.m_instance) {
      MessageHud.m_instance.ShowMessage(MessageHud.MessageType.TopLeft, message);
    }
  }

  public static bool IsTerrainOpPrefab { get; private set; }
  public static bool IsIgnoredPrefab { get; private set; }
  public static HashSet<string> IgnoredPrefabNames { get; private set; } = [];

  public static void SetIgnoredPrefabNames(IEnumerable<string> values) {
    IgnoredPrefabNames.Clear();

    foreach (string value in values) {
      string prefabName = value.Trim();

      if (!string.IsNullOrEmpty(prefabName)) {
        IgnoredPrefabNames.Add(prefabName);
      }
    }
  }

  public static bool ShouldIgnorePrefabByName(GameObject prefab) {
    string prefabName = Utils.GetPrefabName(prefab.name);

    return
        !string.IsNullOrEmpty(prefabName)
        && IgnoredPrefabNames.Count > 0
        && IgnoredPrefabNames.Contains(prefabName);
  }

  public static void OnSetupPlacementGhost(GameObject placementGhost) {
    if (placementGhost) {
      IsTerrainOpPrefab = placementGhost.TryGetComponent(out TerrainOp _);
      IsIgnoredPrefab = ShouldIgnorePrefabByName(placementGhost);
    } else {
      IsTerrainOpPrefab = false;
      IsIgnoredPrefab = false;
    }
  }

  public static bool TryGetRotation(out Quaternion rotation) {
    if (IsIgnoredPrefab || (IsTerrainOpPrefab && IgnoreTerrainOpPrefab.Value)) {
      rotation = Quaternion.identity;
      return false;
    }

    rotation = GetRotation();
    return true;
  }
}
