using System.Collections.Generic;
using System.IO;
using System.Reflection;

using UnityEngine;

using static ComfyGizmo.PluginConfig;

namespace ComfyGizmo {
  public class Gizmos {
    static GameObject _prefab;
    readonly static List<Gizmos> _gizmoInstances = new();

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
      _gizmo = GameObject.Instantiate(_prefab);
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

    public  void SetYScale(float scale) {
      _yGizmo.localScale = Vector3.one * scale;
    }

    public  void SetZScale(float scale) {
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
      _xMaterial.SetColor("_Color", XGizmoColor.Value * XEmissionColorFactor.Value);
    }

    public void SetYGizmoColor() {
      _yMaterial.SetColor("_Color", YGizmoColor.Value * YEmissionColorFactor.Value);
    }

    public void SetZGizmoColor() {
      _zMaterial.SetColor("_Color", ZGizmoColor.Value * ZEmissionColorFactor.Value);
    }

    public void Destroy() {
      UnityEngine.GameObject.Destroy(_gizmo);
    }

    private void SetupComponentsAndRoots() {
      _gizmoRoot = _gizmo.transform;

      _xGizmo = _gizmoRoot.Find("YRoot/ZRoot/XRoot/X");
      _yGizmo = _gizmoRoot.Find("YRoot/Y");
      _zGizmo = _gizmoRoot.Find("YRoot/ZRoot/Z");

      _xMaterial = _xGizmo.gameObject.GetComponent<Renderer>().material;
      _yMaterial = _yGizmo.gameObject.GetComponent<Renderer>().material;
      _zMaterial = _zGizmo.gameObject.GetComponent<Renderer>().material;

      SetXGizmoColor();
      SetYGizmoColor();
      SetZGizmoColor();

      _xGizmoRoot = _gizmoRoot.Find("YRoot/ZRoot/XRoot");
      _yGizmoRoot = _gizmoRoot.Find("YRoot");
      _zGizmoRoot = _gizmoRoot.Find("YRoot/ZRoot");
    }

    public static GameObject LoadGizmoPrefab() {
      AssetBundle bundle = AssetBundle.LoadFromMemory(
          GetResource(Assembly.GetExecutingAssembly(), "ComfyGizmo.Resources.gizmos"));

      _prefab = bundle.LoadAsset<GameObject>("GizmoRoot");
      bundle.Unload(unloadAllLoadedObjects: false);

      return _prefab;
    }

    static byte[] GetResource(Assembly assembly, string resourceName) {
      Stream stream = assembly.GetManifestResourceStream(resourceName);

      byte[] data = new byte[stream.Length];
      stream.Read(data, offset: 0, count: (int)stream.Length);

      return data;
    }
  }
}
