using System.IO;
using System.Reflection;
using UnityEngine;

using static ComfyGizmo.PluginConfig;

namespace ComfyGizmo {
  public class Gizmos {
    static Gizmos _instance; 

    GameObject _gizmo;
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

    public static void Initialize() {
      _instance = new Gizmos();
    }

    public Gizmos() {
      CreateGizmos();
    }

    private void CreateGizmos() {
      _gizmo = LoadGizmoPrefab();
      _gizmoRoot = UnityEngine.GameObject.Instantiate(_gizmo).transform;

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

    public static void Destroy() {
      if (_instance == null) {
        return;
      }

      UnityEngine.GameObject.Destroy(_instance._gizmoRoot);
      UnityEngine.GameObject.Destroy(_instance._gizmo);
      _instance = null;
    }

    public static void Show(Player player) {
      if (!player.m_placementMarkerInstance) {
        return;
      }

      _instance.SetActive(player);
      _instance.SetPosition(player.m_placementMarkerInstance.transform.position + (Vector3.up * 0.5f));
    }

    public static Quaternion GetRotation() {
      return _instance._gizmoRoot.transform.rotation;
    }

    public static Transform GetXGizmoRoot() {
      return _instance._xGizmoRoot;
    }

    public static void HideGizmos() {

    }

    public static void ApplyRotation(Quaternion rotation) {
      _instance._gizmoRoot.rotation *= rotation;
    }

    public static void ApplyLocalRotation(Quaternion rotation) {
      _instance._gizmoRoot.localRotation *= rotation;
    }

    public static void SetComponentLocalRotations(Vector3 eulerAngles) {
      _instance._xGizmoRoot.localRotation = Quaternion.Euler(eulerAngles.x, 0f, 0f);
      _instance._yGizmoRoot.localRotation = Quaternion.Euler(0f, eulerAngles.y, 0f);
      _instance._zGizmoRoot.localRotation = Quaternion.Euler(0f, 0f, eulerAngles.z);
    }

    public static void ResetRotations() {
      _instance._gizmoRoot.rotation = Quaternion.identity;
    }

    public static void SetRotation(Quaternion rotation) {
      _instance.SetRootRotation(rotation);
    }

    public static void SetAxisRotation(float angle, Vector3 axis) {
      _instance._gizmoRoot.rotation = Quaternion.AngleAxis(angle, axis);
    }

    public static void SetLocalAxisRotation(float angle, Vector3 axis) {
      _instance._gizmoRoot.localRotation = Quaternion.AngleAxis(angle, axis);
    }

    public static void ResetScale() {
      SetLocalScale(1f);
    }

    public static void SetLocalScale(float scale) {
      SetXScale(scale);
      SetYScale(scale);
      SetZScale(scale);
    }

    public static void SetXScale(float scale) {
      _instance._xGizmo.localScale = Vector3.one * scale;
    }

    public static void SetYScale(float scale) {
      _instance._yGizmo.localScale = Vector3.one * scale;
    }

    public static void SetZScale(float scale) {
      _instance._zGizmo.localScale = Vector3.one * scale;
    }

    public static void SetXColor() {
      _instance.SetXGizmoColor();
    }

    public static void SetYColor() {
      _instance.SetYGizmoColor();
    }

    public static void SetZColor() {
      _instance.SetZGizmoColor();
    }

    public void SetRootRotation(Quaternion rotation) {
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

    static GameObject LoadGizmoPrefab() {
      AssetBundle bundle = AssetBundle.LoadFromMemory(
          GetResource(Assembly.GetExecutingAssembly(), "ComfyGizmo.Resources.gizmos"));

      GameObject prefab = bundle.LoadAsset<GameObject>("GizmoRoot");
      bundle.Unload(unloadAllLoadedObjects: false);

      return prefab;
    }

    static byte[] GetResource(Assembly assembly, string resourceName) {
      Stream stream = assembly.GetManifestResourceStream(resourceName);

      byte[] data = new byte[stream.Length];
      stream.Read(data, offset: 0, count: (int)stream.Length);

      return data;
    }
  }
}
