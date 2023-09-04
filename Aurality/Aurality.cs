using BepInEx;

using HarmonyLib;

using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using static Aurality.PluginConfig;

namespace Aurality {
  [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
  public class Aurality : BaseUnityPlugin {
    public const string PluginGuid = "bruce.comfy.valheim.aurality";
    public const string PluginName = "Aurality";
    public const string PluginVersion = "1.0.1";

    Harmony _harmony;

    public static readonly List<string> BuzzPrefabs = new List<string> { "sfx_deathsquito_attack(Clone)" };

    public void Awake() {
      BindConfig(Config);    

      _harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), harmonyInstanceId: PluginGuid);
    }

    public void OnDestroy() {
      _harmony?.UnpatchSelf();
    }
  }
}