namespace ComfyGizmo;

using System;
using System.Collections.Generic;

using ComfyLib;

using UnityEngine;

using static PluginConfig;

public sealed class Gizmos {
  public static readonly Lazy<GameObject> GizmoPrefab = new(LoadGizmoPrefab);
  
  readonly static List<Gizmos> _gizmoInstances = [];

  readonly GameObject _gizmo;
  Transform _gizmoRoot;

  Transform _xGizmo;
  Transform _yGizmo;
  Transform _zGizmo;

  Transform _xGizmoRoot;
  Transform _yGizmoRoot;
  Transform _zGizmoRoot;

  Material _xMaterial;
  Material _yMaterial;
  Material _zMaterial;

  public static Gizmos CreateGizmos() {
    Gizmos gizmos = new();
    _gizmoInstances.Add(gizmos);

    return gizmos;
  }
  public static void ResetAllScales() {
    foreach (Gizmos gizmos in _gizmoInstances) {
      gizmos.ResetScale();
    }
  }

  public static void SetAllXColors() {
    foreach (Gizmos gizmos in _gizmoInstances) {
      gizmos.SetXGizmoColor();
    }
  }

  public static void SetAllYColors() {
    foreach (Gizmos gizmos in _gizmoInstances) {
      gizmos.SetYGizmoColor();
    }
  }

  public static void SetAllZColors() {
    foreach (Gizmos gizmos in _gizmoInstances) {
      gizmos.SetZGizmoColor();
    }
  }

  public Gizmos() {
    _gizmo = UnityEngine.Object.Instantiate(GizmoPrefab.Value);
    SetupComponentsAndRoots();
  }

  public void Show(Player player) {
    if (!player.m_placementMarkerInstance) {
      return;
    }

    SetActive(player);
    SetPosition(player.m_placementMarkerInstance.transform.position + (Vector3.up * 0.5f));
  }

  public void Hide() {
    _gizmoRoot.gameObject.SetActive(false);
  }

  public Transform GetXGizmoRoot() {
    return _xGizmoRoot;
  }

  public void ApplyRotation(Quaternion rotation) {
    _gizmoRoot.rotation *= rotation;
  }

  public void SetLocalRotation(Vector3 eulerAngles) {
    _xGizmoRoot.localRotation = Quaternion.Euler(eulerAngles.x, 0f, 0f);
    _yGizmoRoot.localRotation = Quaternion.Euler(0f, eulerAngles.y, 0f);
    _zGizmoRoot.localRotation = Quaternion.Euler(0f, 0f, eulerAngles.z);
  }

  public void SetAxisRotation(float angle, Vector3 axis) {
    _gizmoRoot.rotation = Quaternion.AngleAxis(angle, axis);
  }

  public void SetLocalAxisRotation(float angle, Vector3 axis) {
    _gizmoRoot.localRotation = Quaternion.AngleAxis(angle, axis);
  }

  public void ResetScale() {
    SetLocalScale(1f);
  }

  public void SetLocalScale(float scale) {
    SetXScale(scale);
    SetYScale(scale);
    SetZScale(scale);
  }

  public void SetXScale(float scale) {
    _xGizmo.localScale = Vector3.one * scale;
  }

  public void SetYScale(float scale) {
    _yGizmo.localScale = Vector3.one * scale;
  }

  public void SetZScale(float scale) {
    _zGizmo.localScale = Vector3.one * scale;
  }

  public void SetXColor() {
    SetXGizmoColor();
  }

  public void SetYColor() {
    SetYGizmoColor();
  }

  public void SetZColor() {
    SetZGizmoColor();
  }

  public void SetRotation(Quaternion rotation) {
    _gizmoRoot.rotation = rotation;
  }

  public void SetPosition(Vector3 position) {
    _gizmoRoot.position = position;
  }

  public void SetActive(Player player) {
    _gizmoRoot.gameObject.SetActive(ShowGizmoPrefab.Value && player.m_placementMarkerInstance.activeSelf);
  }

  public void SetXGizmoColor() {
    _xMaterial.color = XGizmoColor.Value * XEmissionColorFactor.Value;
  }

  public void SetYGizmoColor() {
    _yMaterial.color = YGizmoColor.Value * YEmissionColorFactor.Value;
  }

  public void SetZGizmoColor() {
    _zMaterial.color = ZGizmoColor.Value * ZEmissionColorFactor.Value;
  }

  public void Destroy() {
    UnityEngine.Object.Destroy(_gizmo);
  }

  private void SetupComponentsAndRoots() {
    _gizmoRoot = _gizmo.transform;

    _xGizmo = _gizmoRoot.Find("YRoot/ZRoot/XRoot/X");
    _yGizmo = _gizmoRoot.Find("YRoot/Y");
    _zGizmo = _gizmoRoot.Find("YRoot/ZRoot/Z");

    _xMaterial = _xGizmo.gameObject.GetComponent<Renderer>().material;
    SetupMaterial(_xMaterial);

    _yMaterial = _yGizmo.gameObject.GetComponent<Renderer>().material;
    SetupMaterial(_yMaterial);

    _zMaterial = _zGizmo.gameObject.GetComponent<Renderer>().material;
    SetupMaterial(_zMaterial);

    SetXGizmoColor();
    SetYGizmoColor();
    SetZGizmoColor();

    _xGizmoRoot = _gizmoRoot.Find("YRoot/ZRoot/XRoot");
    _yGizmoRoot = _gizmoRoot.Find("YRoot");
    _zGizmoRoot = _gizmoRoot.Find("YRoot/ZRoot");
  }

  static void SetupMaterial(Material material) {
    material.shader = AssetUtils.GetShader(AssetUtils.StandardShader);
    material.DisableKeyword("_EMISSION");
    material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.EmissiveIsBlack;
  }

  public static GameObject LoadGizmoPrefab() {
    return AssetUtils.LoadAsset<GameObject>("ComfyGizmo.Resources.gizmos", "GizmoRoot");
  }
}
