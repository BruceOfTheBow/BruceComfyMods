using BepInEx;
using BepInEx.Logging;

using HarmonyLib;

using UnityEngine.PostProcessing;

using System.Reflection;

using static LensCap.PluginConfig;

namespace LensCap {
  [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
  public class LensCap : BaseUnityPlugin {
    public const string PluginGuid = "bruce.valheim.comfymods.lenscap";
    public const string PluginName = "LensCap";
    public const string PluginVersion = "1.0.0";

    private static float _defaultIntensity = 0f;

    static ManualLogSource _logger;

    Harmony _harmony;

    public void Awake() {
      BindConfig(Config);

      _logger = Logger;

      IsModEnabled.SettingChanged +=
        (sender, eventArgs) => {
          if (IsModEnabled.Value) {
            RemoveLensDirt();
            return;
          }

          EnableLensDirt();
        };

      _harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), harmonyInstanceId: PluginGuid);
    }

    public void OnDestroy() {
      _harmony?.UnpatchSelf();
    }

    public static void EnableLensDirt() {
      if (GameCamera.instance == null || !GameCamera.instance.gameObject.TryGetComponent(out PostProcessingBehaviour ppb)) {
        return;
      }

      ppb.profile.bloom.m_Settings.lensDirt.intensity = _defaultIntensity;
    }

    public static void RemoveLensDirt() {
      if (GameCamera.instance == null || !GameCamera.instance.gameObject.TryGetComponent(out PostProcessingBehaviour ppb)) {
        return;
      }

      ppb.profile.bloom.m_Settings.lensDirt.intensity = 0f;
    }

    public static void GetDefaultIntensity() {
      if (GameCamera.instance == null || !GameCamera.instance.gameObject.TryGetComponent(out PostProcessingBehaviour ppb)) {
        return;
      }

      _defaultIntensity = ppb.profile.bloom.m_Settings.lensDirt.intensity;
    }

    public static void Log(string message) {
      _logger.LogInfo(message);
    }
  }
}