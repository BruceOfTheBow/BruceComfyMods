using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;

using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

using UnityEngine;

using static Gizmo.PluginConfig;

namespace Gizmo {
  [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
  public class ComfyGizmo : BaseUnityPlugin {
    public const string PluginGUID = "com.rolopogo.gizmo.comfy";
    public const string PluginName = "ComfyGizmo";
    public const string PluginVersion = "1.6.0";

    public static GameObject GizmoPrefab = null;
    public static Transform GizmoRoot;

    public static Transform XGizmo;
    public static Transform YGizmo;
    public static Transform ZGizmo;

    public static Transform XGizmoRoot;
    public static Transform YGizmoRoot;
    public static Transform ZGizmoRoot;

    public static GameObject ComfyGizmoObj;
    public static Transform _comfyGizmoRoot;

    public static Vector3 EulerAngles;
    static float _rotation;

    public static bool LocalFrame;

    static float _snapAngle;

    static Material _xMaterial;
    static Material _yMaterial;
    static Material _zMaterial;



    Harmony _harmony;

    public void Awake() {
      BindConfig(Config);

      SnapDivisions.SettingChanged += (sender, eventArgs) => _snapAngle = 180f / SnapDivisions.Value;
      _snapAngle = 180f / SnapDivisions.Value;

      GizmoPrefab = LoadGizmoPrefab();

      XGizmoColor.SettingChanged +=
        (sender, eventArgs) => {
          SetXGizmoColor();
      };

      YGizmoColor.SettingChanged +=
        (sender, eventArgs) => {
          SetYGizmoColor();
        };

      ZGizmoColor.SettingChanged +=
        (sender, eventArgs) => {
          SetZGizmoColor();
        };

      XEmissionColorFactor.SettingChanged +=
        (sender, eventArgs) => {
          SetXGizmoColor();
        };

      YEmissionColorFactor.SettingChanged +=
        (sender, eventArgs) => {
          SetYGizmoColor();
        };

      ZEmissionColorFactor.SettingChanged +=
        (sender, eventArgs) => {
          SetZGizmoColor();
        };

      _harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), harmonyInstanceId: PluginGUID);
    }

    public void OnDestroy() {
      _harmony?.UnpatchSelf();
    }

    public static void Rotate() {
      if (Input.GetKey(ResetAllRotationKey.Value.MainKey)) {
        ResetRotations();
      } else if (Input.GetKey(XRotationKey.Value.MainKey)) {
        HandleAxisInput(ref EulerAngles.x, XGizmo);
      } else if (Input.GetKey(ZRotationKey.Value.MainKey)) {
        HandleAxisInput(ref EulerAngles.z, ZGizmo);
      } else {
        HandleAxisInput(ref EulerAngles.y, YGizmo);
      }

      ComfyGizmoObj.transform.localRotation = Quaternion.Euler(EulerAngles);
      RotateGizmoComponents(EulerAngles);
    }

    public static void RotateLocalFrame() {
      if (Input.GetKey(ResetAllRotationKey.Value.MainKey)) {
        ResetRotationsLocalFrame();
        return;
      }

      _rotation = 0f;
      Vector3 rotVector;

      if (Input.GetKey(XRotationKey.Value.MainKey)) {
        XGizmo.localScale = Vector3.one * 1.5f;
        rotVector = Vector3.right;
        HandleAxisInputLocalFrame(ref _rotation, rotVector, XGizmo);
      } else if (Input.GetKey(ZRotationKey.Value.MainKey)) {
        ZGizmo.localScale = Vector3.one * 1.5f;
        rotVector = Vector3.forward;
        HandleAxisInputLocalFrame(ref _rotation, rotVector, ZGizmo);
      } else {
        YGizmo.localScale = Vector3.one * 1.5f;
        rotVector = Vector3.up;
        HandleAxisInputLocalFrame(ref _rotation, rotVector, YGizmo);
      }

      RotateAxes(_rotation, rotVector);
    }

    public static void RotateAxes(float rotation, Vector3 rotVector) {
      ComfyGizmoObj.transform.rotation *= Quaternion.AngleAxis(rotation, rotVector);
      GizmoRoot.rotation *= Quaternion.AngleAxis(rotation, rotVector);
    }

    public static void HandleAxisInput(ref float rotation, Transform gizmo) {
      gizmo.localScale = Vector3.one * 1.5f;
      rotation += Math.Sign(Input.GetAxis("Mouse ScrollWheel")) * _snapAngle;

      if (Input.GetKey(ResetRotationKey.Value.MainKey)) {
        rotation = 0f;
      }
    }

    public static void HandleAxisInputLocalFrame(ref float rotation, Vector3 rotVector, Transform gizmo) {
      gizmo.localScale = Vector3.one * 1.5f;
      rotation = Math.Sign(Input.GetAxis("Mouse ScrollWheel")) * _snapAngle;

      if (Input.GetKey(ResetRotationKey.Value.MainKey)) {
        rotation = 0f;
        ResetRotationLocalFrameAxis(rotVector);
      }
    }
    public static void MatchPieceRotation(Piece target) {
      if (LocalFrame) {
        ComfyGizmoObj.transform.rotation = target.GetComponent<Transform>().localRotation;
        GizmoRoot.rotation = target.GetComponent<Transform>().localRotation;
      } else {
        EulerAngles = target.GetComponent<Transform>().eulerAngles;
        Rotate();
      }
    }
    public static void ResetRotations() {
      EulerAngles = Vector3.zero;
      ComfyGizmoObj.transform.localRotation = Quaternion.Euler(Vector3.zero);
      RotateGizmoComponents(Vector3.zero);
    }

    public static void ResetGizmoComponents() {
      EulerAngles = Vector3.zero;
      RotateGizmoComponents(Vector3.zero);
    }

    public static void ResetGizmoRoot() {
      GizmoRoot.rotation = Quaternion.AngleAxis(0f, Vector3.up);
      GizmoRoot.rotation = Quaternion.AngleAxis(0f, Vector3.right);
      GizmoRoot.rotation = Quaternion.AngleAxis(0f, Vector3.forward);
    }

    public static void RotateGizmoComponents(Vector3 eulerAngles) {
      XGizmoRoot.localRotation = Quaternion.Euler(eulerAngles.x, 0f, 0f);
      YGizmoRoot.localRotation = Quaternion.Euler(0f, eulerAngles.y, 0f);
      ZGizmoRoot.localRotation = Quaternion.Euler(0f, 0f, eulerAngles.z);
    }

    public static void ResetRotationsLocalFrame() {
      ResetRotationLocalFrameAxis(Vector3.up);
      ResetRotationLocalFrameAxis(Vector3.right);
      ResetRotationLocalFrameAxis(Vector3.forward);
    }

    public static void ResetRotationLocalFrameAxis(Vector3 axis) {
      ComfyGizmoObj.transform.rotation = Quaternion.AngleAxis(0f, axis);
      GizmoRoot.rotation = Quaternion.AngleAxis(0f, axis);
    }

    static GameObject LoadGizmoPrefab() {
      AssetBundle bundle = AssetBundle.LoadFromMemory(
          GetResource(Assembly.GetExecutingAssembly(), "Gizmo.Resources.gizmos"));

      GameObject prefab = bundle.LoadAsset<GameObject>("GizmoRoot");
      bundle.Unload(unloadAllLoadedObjects: false);

      return prefab;
    }

    static byte[] GetResource(Assembly assembly, string resourceName) {
      Stream stream = assembly.GetManifestResourceStream(resourceName);

      byte[] data = new byte[stream.Length];
      stream.Read(data, offset: 0, count: (int) stream.Length);

      return data;
    }

    public static Transform CreateGizmoRoot() {
      GizmoRoot = Instantiate(GizmoPrefab).transform;

      // ??? Something about quaternions.
      XGizmo = GizmoRoot.Find("YRoot/ZRoot/XRoot/X");
      YGizmo = GizmoRoot.Find("YRoot/Y");
      ZGizmo = GizmoRoot.Find("YRoot/ZRoot/Z");

      _xMaterial = XGizmo.gameObject.GetComponent<Renderer>().material;
      _yMaterial = YGizmo.gameObject.GetComponent<Renderer>().material;
      _zMaterial = ZGizmo.gameObject.GetComponent<Renderer>().material;

      SetXGizmoColor();
      SetYGizmoColor();
      SetZGizmoColor();

      XGizmoRoot = GizmoRoot.Find("YRoot/ZRoot/XRoot");
      YGizmoRoot = GizmoRoot.Find("YRoot");
      ZGizmoRoot = GizmoRoot.Find("YRoot/ZRoot");

      return GizmoRoot.transform;
    }

    public static void SetXGizmoColor() {
      _xMaterial.SetColor("_Color", XGizmoColor.Value * XEmissionColorFactor.Value);
    }

    public static void SetYGizmoColor() {
      _yMaterial.SetColor("_Color", YGizmoColor.Value * YEmissionColorFactor.Value);
    }

    public static void SetZGizmoColor() {
      _zMaterial.SetColor("_Color", ZGizmoColor.Value * ZEmissionColorFactor.Value);
      
    }
  }
}
