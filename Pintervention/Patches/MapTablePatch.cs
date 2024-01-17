using System.Collections.Generic;

using HarmonyLib;
using Pintervention.Core;
using static Pintervention.Pintervention;
using static Pintervention.PluginConfig;
using static PrivilegeManager;

namespace Pintervention.Patches {
  [HarmonyPatch(typeof(MapTable))]
  static class MapTablePatch {
    static readonly string _nameHashBase = "pintervention.pid_name";
    static readonly char _seperator = '_';

    static int _currentIteratorInt = 0;

    [HarmonyPrefix]
    [HarmonyPatch(nameof(MapTable.OnRead))]
    static bool OnReadPostfix(MapTable __instance, ItemDrop.ItemData item, ref bool __result) {
      if (!IsModEnabled.Value
          || !__instance
          || !__instance.m_nview
          || !__instance.m_nview.IsValid()
          || item != null) {

        return true;
      }

      LoadPlayerNames(__instance.m_nview.GetZDO());

      if (RevealMapOnRead.Value && TakePinsOnRead.Value) {
        return true;
      }

      SharedMapDataManager.ReadMapData(__instance.m_nview.GetZDO());
      __result = false;
      return false;
      
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(MapTable.OnWrite))]
    static void OnWritePostfix(MapTable __instance) {
      if (!IsModEnabled.Value
          || !__instance
          || !__instance.m_nview
          || !__instance.m_nview.IsValid()
          || !Player.m_localPlayer) {
        return;
      }

      __instance.m_nview.GetZDO()
          .Set(GetNextEmptyHashCode(__instance.m_nview.GetZDO()), GetStoredValue(Player.m_localPlayer));
    }

    static void LoadPlayerNames(ZDO zdo) {
      string storedValue = "not empty";

      while (storedValue != string.Empty) {
        storedValue = zdo.GetString(GetNextHashCode(), string.Empty);

        if (storedValue == string.Empty) {
          continue;
        }

        KeyValuePair<long, string> kvp = ParsePidNamePair(storedValue);

        if (kvp.Equals(default(KeyValuePair<long, string>))) {
          LogWarning($"Unable to parse name at index {_currentIteratorInt - 1} with value {storedValue}.");
          continue;
        }

        ForeignPinManager.AddPlayerName(kvp.Key, kvp.Value);
      }

      ResetIterator();
    }

    static string GetStoredValue(Player localPlayer) {
      return $"{localPlayer.GetPlayerID()}{_seperator}{localPlayer.GetPlayerName()}";
    }

    static KeyValuePair<long, string> ParsePidNamePair(string storedValue) {
      string[] values = storedValue.Split(_seperator);

      if (values.Length !=2 || !long.TryParse(values[0], out long pid)) {
        return default(KeyValuePair<long, string>);
      }

      return new KeyValuePair<long, string>(pid, values[1]);
    }

    static int GetNextHashCode() {
      return GetNumberedHashCode(++_currentIteratorInt);
    }
    
    static void ResetIterator() {
      _currentIteratorInt = 0;
    }
    
    static int GetNextEmptyHashCode(ZDO zdo) {
      int i = 0;
      string check = "not empty";

      while (check != string.Empty) {
        check = zdo.GetString(GetNumberedHashCode(i), string.Empty);
        i++;
      }

      return GetNumberedHashCode(i);
    }

    static int GetNumberedHashCode(int i) {
      return $"{_nameHashBase}{i}".GetStableHashCode();
    }
  }
}
