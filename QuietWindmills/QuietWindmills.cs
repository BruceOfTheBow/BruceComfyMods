using BepInEx;

using HarmonyLib;

using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using static QuietWindmills.PluginConfig;

namespace QuietWindmills {
  [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
  public class QuietWindmills : BaseUnityPlugin {
    public const string PluginGuid = "bruce.valheim.comfymods.quietwindmills";
    public const string PluginName = "QuietWindmills";
    public const string PluginVersion = "1.0.0";

    Harmony _harmony;

    public static float DefaultPropellerSpeed = -600f;
    public static float DefaultMaxVolume = 1f;

    static int _windmillHash = "windmill".GetStableHashCode();

    public void Awake() {
      BindConfig(Config);

      IsModEnabled.SettingChanged +=
       (sender, eventArgs) => {
         if (!IsModEnabled.Value) {
           RestoreDefaults();
         }
       };

      QuietWhenEmpty.SettingChanged +=
        (sender, eventArgs) => {
          if (!QuietWhenEmpty.Value) {
            SetWindmillValues(DefaultPropellerSpeed, MaxVolume.Value);
          }
        };

      _harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), harmonyInstanceId: PluginGuid);
    }

    public void OnDestroy() {
      _harmony?.UnpatchSelf();
    }

    static void RestoreDefaults() {
      SetWindmillValues(DefaultPropellerSpeed, DefaultMaxVolume);
    }

    static void SetWindmillValues(float speed, float maxVol) {
      if (ZNetScene.instance == null) {
        return;
      }

      List<ZNetView> windmillViews = ZNetScene.instance.m_instances.Values.Where(x => x.GetZDO().m_prefab == _windmillHash).ToList();
      foreach (ZNetView zNetView in windmillViews) {
        if (!zNetView.gameObject.TryGetComponent(out Windmill windmill)) {
          continue;
        }

        windmill.m_propellerRotationSpeed = speed;
        windmill.m_maxVol = maxVol;
      }
    }
  }
}