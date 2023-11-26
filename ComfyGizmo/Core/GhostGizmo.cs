using UnityEngine;

namespace ComfyGizmo {
  public class GhostGizmo {

    readonly GameObject _prefab;
    readonly Transform _root;

    public static GhostGizmo CreateGhostGizmo() {
      return new GhostGizmo();
    }

    public GhostGizmo() {
      _prefab = new("ComfyGizmo");
      _root = _prefab.transform;
    }

    public void Destroy() {
      UnityEngine.GameObject.Destroy(_prefab);
    }

    public void ApplyRotation(Quaternion rotation) {
      _root.rotation *= rotation;
    }

    public void ApplyLocalRotation(Quaternion rotation) {
      _root.localRotation *= rotation;
    }

    public void SetRotation(Quaternion rotation) {
      _root.rotation = rotation;
    }
    public Quaternion GetRotation() {
      return _root.rotation;
    }

    public void SetLocalRotation(Quaternion rotation) {
      _root.localRotation = rotation;
    }

    public  void SetAxisRotation(float angle, Vector3 axis) {
      _root.rotation = Quaternion.AngleAxis(angle, axis);
    }

    public void SetLocalAxisRotation(float angle, Vector3 axis) {
      _root.localRotation = Quaternion.AngleAxis(angle, axis);
    }
  }
}
