using UnityEngine;

namespace ComfyGizmo {
  public class InternalRotator {
    private static InternalRotator _instance;

    private GameObject _prefab;
    private Transform _root;

    public static void Initialize() {
      _instance = new InternalRotator();
    }

    public static void Destroy() {
      if (_instance == null) {
        return;
      }

      UnityEngine.GameObject.Destroy(_instance._prefab);
      UnityEngine.GameObject.Destroy(_instance._root);
      _instance = null;
    }

    public InternalRotator() {
      _prefab = new("ComfyGizmo");
      _root = _prefab.transform;
    }

    public static void ApplyRotation(Quaternion rotation) {
      _instance._root.rotation *= rotation;
    }

    public static void ApplyLocalRotation(Quaternion rotation) {
      _instance._root.localRotation *= rotation;
    }

    public static void SetRotation(Quaternion rotation) {
      _instance._root.rotation = rotation;
    }

    public static void SetLocalRotation(Quaternion rotation) {
      _instance._root.localRotation = rotation;
    }

    public static void SetAxisRotation(float angle, Vector3 axis) {
      _instance._root.rotation = Quaternion.AngleAxis(angle, axis);
    }

    public static void SetLocalAxisRotation(float angle, Vector3 axis) {
      _instance._root.localRotation = Quaternion.AngleAxis(angle, axis);
    }

    public static Vector3 GetEulerAngles() {
      return _instance._root.transform.eulerAngles;
    }

    public static Transform GetInternalRotatorRoot() {
      return _instance._root;
    }
  }
}
