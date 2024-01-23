using System.Collections.Generic;

using HarmonyLib;
using Pintervention.Core;
using static Pintervention.Pintervention;
using static Pintervention.PluginConfig;

namespace Pintervention.Patches {
  [HarmonyPatch(typeof(MapTable))]
  static class MapTablePatch {
    static readonly string _nameHashBase = "pintervention.pid_name";
    static readonly char _seperator = '_';

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

      if (ReadRevealedMapOnInteract.Value && ReadPinsOnInteract.Value) {
        return true;
      }

      SharedMapDataManager.ReadMapData(__instance.m_nview.GetZDO());
      __result = false;
      return false;
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(MapTable.OnRead))]
    static void OnReadPostfix(MapTable __instance, ItemDrop.ItemData item) {
      if (!ReadPinsOnInteract.Value) {
        return;
      }

      PinOwnerManager.FindPinOwners();
      NameManager.WriteNamesToFile();
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

      if (!ClaimOwnership(__instance)) {
        return;
      }

      if (IsNameOnTable(__instance)) {
        return;
      }

      AddNameToTable(__instance);
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(MapTable.GetReadHoverText))]
    static void GetReadHoverTextPostfix(MapTable __instance, ref string __result) {
      __result = 
            Localization.instance.Localize(__instance.m_name + "\n[<color=yellow><b>$KEY_Use</b></color>]") + " Interact"
            + $"\n[{GetBoolUnicodeCharacter(ReadPinsOnInteract.Value)}] Read pins"
            + $"\n[{GetBoolUnicodeCharacter(ReadRevealedMapOnInteract.Value)}] Read revealed map";
    }

    static string GetBoolUnicodeCharacter(bool enabled) {
      if (enabled) {
        return "<color=green><b>\u2713</b></color>";
      }

      return "<color=red><b>X</b></color>";
    }

    static bool ClaimOwnership(MapTable table) {
      if (!table
          || !table.m_nview
          || !table.m_nview.IsValid()) {
        return false;
      }

      table.m_nview.ClaimOwnership();
      return true;
    }

    static void LoadPlayerNames(ZDO zdo) {
      string storedValue = "not empty";
      int i = 0;

      while (storedValue != string.Empty) {
        storedValue = zdo.GetString(GetNumberedHashCode(i), string.Empty);

        if (storedValue == string.Empty) {
          continue;
        }

        i++;
        KeyValuePair<long, string> kvp = ParsePidNamePair(storedValue);

        if (kvp.Equals(default(KeyValuePair<long, string>))) {
          LogWarning($"Unable to parse name at index {i} with value {storedValue}.");
          continue;
        }

        NameManager.AddPlayerName(kvp.Key, kvp.Value);
      }
    }

    static string GetStoredValue() {
      return $"{Player.m_localPlayer.GetPlayerID()}{_seperator}{Player.m_localPlayer.GetPlayerName()}";
    }

    static KeyValuePair<long, string> ParsePidNamePair(string storedValue) {
      string[] values = storedValue.Split(_seperator);

      if (values.Length !=2 || !long.TryParse(values[0], out long pid)) {
        return default(KeyValuePair<long, string>);
      }

      return new KeyValuePair<long, string>(pid, values[1]);
    }
    
    static bool IsNameOnTable(MapTable table) {
      ZDO zdo = table.m_nview.GetZDO();
      string valueToStore = GetStoredValue();

      int i = 0;
      string check = "not empty";
      while (check != string.Empty) {
        check = zdo.GetString(GetNumberedHashCode(i), string.Empty);

        if (check == valueToStore) {
          return true;
        }

        i++;
      }

      Log($"Name {Player.m_localPlayer.GetPlayerName()} not found on map table.");
      return false;
    }

    static void AddNameToTable(MapTable table) {
      table.m_nview.ClaimOwnership();
      table.m_nview.GetZDO().Set(GetNextEmptyHashCode(table.m_nview.GetZDO()), GetStoredValue());
      Log($"Adding {GetStoredValue()} to table.");
    }
    
    static int GetNextEmptyHashCode(ZDO zdo) {
      int i = 0;
      string check = "not empty";

      while (check != string.Empty) {
        check = zdo.GetString(GetNumberedHashCode(i), string.Empty);

        if (check == string.Empty) {
          continue;
        }

        i++;
      }

      return GetNumberedHashCode(i);
    }

    static int GetNumberedHashCode(int i) {
      return $"{_nameHashBase}{i}".GetStableHashCode();
    }
  }
}
