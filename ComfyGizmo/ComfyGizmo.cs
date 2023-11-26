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

    Harmony _harmony;

    public void Awake() {
      BindConfig(Config);

      Gizmos.LoadGizmoPrefab();

      XGizmoColor.SettingChanged +=
        (sender, eventArgs) => {
          Gizmos.SetAllXColors();
      };

      YGizmoColor.SettingChanged +=
        (sender, eventArgs) => {
          Gizmos.SetAllYColors();
        };

      ZGizmoColor.SettingChanged +=
        (sender, eventArgs) => {
          Gizmos.SetAllZColors();
        };

      XEmissionColorFactor.SettingChanged +=
        (sender, eventArgs) => {
          Gizmos.SetAllXColors();
        };

      YEmissionColorFactor.SettingChanged +=
        (sender, eventArgs) => {
          Gizmos.SetAllYColors();
        };

      ZEmissionColorFactor.SettingChanged +=
        (sender, eventArgs) => {
          Gizmos.SetAllZColors();
        };

      IsLocalFrameEnabled.SettingChanged +=
        (sender, eventArgs) => {
          RotationManager.OnModeChange(Player.m_localPlayer);

          if (IsLocalFrameEnabled.Value) {
            IsRoofModeEnabled.Value = false;
          }
        };

      IsOldRotationEnabled.SettingChanged +=
        (sender, eventArgs) => {
          RotationManager.OnModeChange(Player.m_localPlayer);
        };

      IsRoofModeEnabled.SettingChanged +=
        (sender, eventArgs) => {
          RotationManager.ResetRotation();
          RotationManager.OnModeChange(Player.m_localPlayer);
        };

      _harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), harmonyInstanceId: PluginGUID);
    }

    public void OnDestroy() {
      _harmony?.UnpatchSelf();
    }
  }
}
