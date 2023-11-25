using System.Collections.Generic;
using System.Reflection;

using BepInEx;

using UnityEngine;

using HarmonyLib;

using static ComfyGizmo.PluginConfig;

namespace ComfyGizmo {
  [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
  public class ComfyGizmo : BaseUnityPlugin {
    public const string PluginGUID = "bruce.valheim.comfymods.gizmo";
    public const string PluginName = "ComfyGizmo";
    public const string PluginVersion = "1.9.0";

    private static readonly List<int> _roofCornerPieceHashCodes = new() {
      "wood_roof_ocorner".GetStableHashCode(),
      "wood_roof_ocorner_45".GetStableHashCode(),
      "wood_roof_icorner".GetStableHashCode(),
      "wood_roof_icorner_45".GetStableHashCode(),

      "darkwood_roof_ocorner".GetStableHashCode(),
      "darkwood_roof_ocorner_45".GetStableHashCode(),
      "darkwood_roof_icorner".GetStableHashCode(),
      "darkwood_roof_icorner_45".GetStableHashCode()
    };

    Harmony _harmony;

    public void Awake() {
      BindConfig(Config);

      Gizmos.Initialize();

      XGizmoColor.SettingChanged +=
        (sender, eventArgs) => {
          Gizmos.SetXColor();
      };

      YGizmoColor.SettingChanged +=
        (sender, eventArgs) => {
          Gizmos.SetYColor();
        };

      ZGizmoColor.SettingChanged +=
        (sender, eventArgs) => {
          Gizmos.SetZColor();
        };

      XEmissionColorFactor.SettingChanged +=
        (sender, eventArgs) => {
          Gizmos.SetXColor();
        };

      YEmissionColorFactor.SettingChanged +=
        (sender, eventArgs) => {
          Gizmos.SetYColor();
        };

      ZEmissionColorFactor.SettingChanged +=
        (sender, eventArgs) => {
          Gizmos.SetZColor();
        };

      IsRoofModeEnabled.SettingChanged +=
        (sender, eventArgs) => {
          RotationManager.ResetRotations();

          if (IsRoofModeEnabled.Value) {
            RotationManager.Offset();
            return;
          }

          Gizmos.ResetRotations();          
        };

      _harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), harmonyInstanceId: PluginGUID);
    }

    public void OnDestroy() {
      _harmony?.UnpatchSelf();
    }
  
    public static bool IsCornerRoofPieceSelected() {
      if (Player.m_localPlayer == null || Player.m_localPlayer.m_placementGhost == null) {
        return false;
      }

      if (!_roofCornerPieceHashCodes.Contains(Player.m_localPlayer.m_placementGhost.name.Replace("(Clone)","").GetStableHashCode())) {
        return false;
      }

      return true;
    }
  }
}
