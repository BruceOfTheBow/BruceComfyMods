using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

using UnityEngine;

using static Gizmo.PluginConfig;

namespace Gizmo {
  [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
  public class ComfyGizmo : BaseUnityPlugin {
    public const string PluginGUID = "bruce.valheim.comfymods.gizmo";
    public const string PluginName = "ComfyGizmo";
    public const string PluginVersion = "1.9.0";

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

    public static Piece LastSelected;
    public static int CurrentPieceIndex = -1;
    public static Dictionary<string, Vector2Int> PieceLocations = new();
    public static int _cachedAvailablePieceCount = -1;

    public static int ColumnCount = 15;
    private static readonly string _searsCatalogGUID = "redseiko.valheim.searscatalog";
    public static BaseUnityPlugin SearsCatalog;
    public static ConfigEntry<int> SearsCatalogColumns;

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

    public static bool IsHammerTableChanged(Player player) {
      if (!player || !player.m_buildPieces || player.m_buildPieces.m_availablePieces == null) {
        return false;
      }
      int currentPieceCount = 0;

      for (int i = 0; i < player.m_buildPieces.m_availablePieces.Count; i++) {
        currentPieceCount += player.m_buildPieces.m_availablePieces[i].Count;
      }

      if (currentPieceCount == _cachedAvailablePieceCount) {
        return false;
      }

      return true;
    }

    public static void CacheHammerTable(Player player) {
      PieceTable hammerPieceTable = player.m_buildPieces;
      _cachedAvailablePieceCount = 0;
      PieceLocations = new();

      for (int i = 0; i < hammerPieceTable.m_availablePieces.Count; i++) {
        List<Piece> categoryPieces = hammerPieceTable.m_availablePieces[i];

        for (int j = 0; j < categoryPieces.Count; j++) {
          if (!PieceLocations.ContainsKey(GetPieceIdentifier(categoryPieces[j]))) {
            PieceLocations.Add(GetPieceIdentifier(categoryPieces[j]), new Vector2Int(i, j));
            _cachedAvailablePieceCount++;
          }
        }
      }
    }

    public static void SetSelectedPiece(Player player, Piece piece) {
      Vector2Int pieceLocation = PieceLocations[GetPieceIdentifier(piece)];
      Piece.PieceCategory previousCategory = player.m_buildPieces.m_selectedCategory;

      player.m_buildPieces.m_selectedCategory = (Piece.PieceCategory)pieceLocation.x;
      player.SetSelectedPiece(new Vector2Int(pieceLocation.y % ColumnCount, pieceLocation.y / ColumnCount));
      player.SetupPlacementGhost();

      if (previousCategory != player.m_buildPieces.m_selectedCategory) {
        Hud.instance.UpdatePieceList(player, new Vector2Int(pieceLocation.y % 15, pieceLocation.y / 15), (Piece.PieceCategory)pieceLocation.x, true);
      }
    }

    public static BaseUnityPlugin GetSearsCatalogPlugin() {
      IEnumerable<BaseUnityPlugin> loadedPlugins = GetLoadedPlugins();

      if (loadedPlugins == null) {
        return null;
      }

      Dictionary<string, BaseUnityPlugin> plugins 
          = loadedPlugins
              .Where(plugin => plugin.Info.Metadata.GUID == _searsCatalogGUID)
              .ToDictionary(plugin => plugin.Info.Metadata.GUID);

      if (plugins.TryGetValue(_searsCatalogGUID, out BaseUnityPlugin plugin)) {
        return plugin;
      }

      return null;
    }

    private static IEnumerable<BaseUnityPlugin> GetLoadedPlugins() {
      return BepInEx.Bootstrap.Chainloader.PluginInfos
                    .Where(x => x.Value != null && x.Value.Instance != null)
                    .Select(x => x.Value.Instance);
    }

    public static bool IsSearsCatalogEnabled() {
      SearsCatalog = GetSearsCatalogPlugin();

      if (!SearsCatalog) {
        return false;
      }

      return true;
    }

    public static int GetBuildPanelColumns() {
      if (SearsCatalog.Config.TryGetEntry(new ConfigDefinition("BuildHud.Panel", "buildHudPanelColumns"), out ConfigEntry<int> columns)) {
        SearsCatalogColumns = columns;
        return columns.Value;
      }

      return -1;
    }

    public static string GetPieceIdentifier(Piece piece) {
      return piece.m_name + piece.m_description;
    }
  }
}
