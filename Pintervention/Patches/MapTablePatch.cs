using System.Collections.Generic;
using System.Linq;
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

      if (!__instance.m_nview.IsOwner()) {
        return;
      }

      AddNamesToTable(__instance);

      SendTableZdoUpdate(__instance);
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(MapTable.GetReadHoverText))]
    static void GetReadHoverTextPostfix(MapTable __instance, ref string __result) {
      __result = 
            Localization.instance.Localize(__instance.m_name + "\n[<color=yellow><b>$KEY_Use</b></color>]") 
            + $" Interact"
            + $"\n[{GetBoolUnicodeCharacter(ReadPinsOnInteract.Value)}] Read pins"
            + $"\n[{GetBoolUnicodeCharacter(ReadRevealedMapOnInteract.Value)}] Read revealed map";
    }

    static string GetBoolUnicodeCharacter(bool enabled) {
      if (enabled) {
        return "<color=green><b>\u2713</b></color>";
      }

      return "<color=red><b>X</b></color>";
    }

    static void SendTableZdoUpdate(MapTable __instance) {
      ZDOID tableZdoId = __instance.m_nview.GetZDO().m_uid;
      List<ZNetPeer> zNetPeers 
          = ZNet.instance.m_peers
            .Where(x => Player.s_players.Select(x => x.m_nview.GetZDO().m_uid).Contains(x.m_characterID))
            .ToList();

      foreach (ZNetPeer zNetPeer in zNetPeers) {
        ZDOMan.instance.ForceSendZDO(zNetPeer.m_uid, tableZdoId);
      }
    }

    static bool ClaimOwnership(MapTable table) {
      if (!table
          || !table.m_nview
          || !table.m_nview.IsValid()) {
        return false;
      }

      Log("Claimed table ownership to write name data.");

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

    static KeyValuePair<long, string> ParsePidNamePair(string storedValue) {
      string[] values = storedValue.Split(_seperator);

      if (values.Length !=2 || !long.TryParse(values[0], out long pid)) {
        return default(KeyValuePair<long, string>);
      }

      return new KeyValuePair<long, string>(pid, values[1]);
    }
    
    static bool IsNameOnTable(MapTable table, long pid, string name) {
      ZDO zdo = table.m_nview.GetZDO();
      string valueToStore = GetStoredValue(pid, name);

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

    static void AddNamesToTable(MapTable table) {
      Dictionary<long, string> namesToWrite = NameManager.PlayerNamesById;

      if (!namesToWrite.ContainsKey(Player.m_localPlayer.GetPlayerID())) {
        namesToWrite.Add(Player.m_localPlayer.GetPlayerID(), Player.m_localPlayer.GetPlayerName());
      }      

      foreach (KeyValuePair<long, string> nameByPid in NameManager.PlayerNamesById) {
        if (IsNameOnTable(table, nameByPid.Key, nameByPid.Value) 
              || nameByPid.Value.StartsWith("Unknown")) {
          continue;
        }

        table.m_nview.GetZDO().Set(GetNextEmptyHashCode(table.m_nview.GetZDO()),
            GetStoredValue(nameByPid.Key, nameByPid.Value));       
      }
    }

    static string GetStoredValue(long pid, string name) {
      return $"{pid}{_seperator}{name}";
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
