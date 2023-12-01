using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;

using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using static AddAllFuel.PluginConfig;

namespace AddAllFuel {
  [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
  public class AddAllFuel : BaseUnityPlugin {
    public const string PluginGuid = "bruceofthebow.valheim.AddAllFuel";
    public const string PluginName = "ComfyAddAllFuel";
    public const string PluginVersion = "1.8.0";

    private static readonly bool _debug = true;
    private static readonly List<string> ExcludeNames = new List<string>() { "$item_finewood" };

    static ManualLogSource _logger;

    Harmony _harmony;

    private void Awake() {
      BindConfig(Config);

      _logger = Logger;

      _harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), harmonyInstanceId: PluginGuid);
    }

    void OnDestroy() {
      _harmony?.UnpatchSelf();
    }

    public static ItemDrop.ItemData FindCookableItem(Smelter __instance, Inventory inventory) {
      IEnumerable<string> names = null;
      if (ExcludeFinewood.Value) {
        names = __instance.m_conversion.
                      Where(n => !ExcludeNames.Contains(n.m_from.m_itemData.m_shared.m_name)).
                      Select(n => n.m_from.m_itemData.m_shared.m_name);
      } else {
        names = __instance.m_conversion.Select(n => n.m_from.m_itemData.m_shared.m_name);
      }

      if (names == null) {
        return null;
      } 
       

      foreach (string name in names) {
        ItemDrop.ItemData item = inventory?.GetItem(name, -1, false);

        if (item != null) {
          return item;
        }
          
      }
      return null;
    }
  }
}