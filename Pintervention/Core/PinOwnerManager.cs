using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using static Pintervention.FileUtils;
using static Pintervention.Pintervention;

namespace Pintervention {
  public class PinOwnerManager {
    public static List<long> ForeignPinOwners { get; private set; } = new();
    public static List<long> FilteredPinOwners { get; private set; } = new ();
    static HashSet<Minimap.PinData> _localPlayerPins = new();

    public static bool Initialize() {
      if (!Minimap.instance || !Player.m_localPlayer || IsInitialized()) {
        return false;
      }

      UpdatePinOwners();
      NameManager.LoadPlayerNames();
      LoadFilteredPinOwners();

      return true;
    }

    public static void UpdatePinOwners() {
      FindPinOwners();
      AddAllLocalPlayerPins();
    }

    public static void AddAllLocalPlayerPins() {
      if (!Minimap.instance || !Player.m_localPlayer) {
        return;
      }

      foreach (Minimap.PinData pin in Minimap.instance.m_pins) {
        if (pin.m_ownerID != 0L
            || IsWorldPin(pin)) {

          continue;
        }

        AddLocalPlayerPin(pin);
      }
    }

    public static void FindPinOwners() {
      if (!Minimap.instance) {
        return;
      }

      ForeignPinOwners = Minimap.instance.m_pins.Select(x => x.m_ownerID).Distinct().ToList();
      ForeignPinOwners.Add(Player.m_localPlayer.GetPlayerID());
      ForeignPinOwners.Sort();
    }

    public static bool IsWorldPin(Minimap.PinData pin) {
      if (!Minimap.instance) {
        return true;
      }

      if (Minimap.instance.m_locationPins.Values.Contains(pin) 
          || Minimap.instance.m_playerPins.Contains(pin)
          || Minimap.instance.m_shoutPins.Contains(pin)
          || Minimap.instance.m_randEventPin == pin) {
        return true;
      }

      return false;
    }
    
    public static void Clear() {
      if (ForeignPinOwners.Any()) {
        ForeignPinOwners.Clear();
        ForeignPinOwners = new();
      }

      if(FilteredPinOwners.Any()) {
        FilteredPinOwners.Clear();
        FilteredPinOwners = new();
      }
    }

    public static long GetForeignPinOwnerAtIndex(int index) {
      return ForeignPinOwners[index];
    }


    public static Dictionary<long, ZDO> GetZDOsByPlayerId(List<long> playerIds) {
      Dictionary<ZDOID, ZDO> zdosById = ZDOMan.s_instance.m_objectsByID;
      HashSet<long> ids = new(playerIds);
      Dictionary<long, ZDO> zdosByPid = new();

      foreach (ZNetPeer netPeer in ZNet.m_instance.m_peers) {
        if (netPeer.m_characterID == ZDOID.None
            || !zdosById.TryGetValue(netPeer.m_characterID, out ZDO zdo)
            || !ZDOExtraData.GetLong(zdo.m_uid, ZDOVars.s_playerID, out long playerId)
            || !ids.Contains(playerId)) {
          continue;
        }

        zdosByPid.Add(playerId, zdo);
        ids.Remove(playerId);

        if (ids.Count <= 0) {
          break;
        }
      }

      return zdosByPid;
    }

    public static void RemoveForeignPinOwner(long pid) {
      if (ForeignPinOwners.Contains(pid)) {
        ForeignPinOwners.Remove(pid);
      }
    }

    public static void FilterPins() {
      if (!IsInitialized()) {
        Initialize();
      }

      if (!FilteredPinOwners.Any()) { 
        return; 
      }

      foreach (long pid in FilteredPinOwners) {
        List<Minimap.PinData> pinsToFilter = GetPinsByPid(pid);

        foreach (Minimap.PinData pin in pinsToFilter) {
          Minimap.instance.DestroyPinMarker(pin);
        }
      }
    }

    public static void SortPinOwnersByName() {
      ForeignPinOwners = ForeignPinOwners.OrderBy(x => NameManager.GetPlayerNameById(x)).ToList();
    }

    public static List<Minimap.PinData> GetPinsByPid(long pid) {
      if (pid == 0L) {
        AddAllLocalPlayerPins();

        return Minimap.instance.m_pins
            .Where(x => x.m_ownerID == pid && !_localPlayerPins.Contains(x))
            .ToList();
      }

      if (pid == Player.m_localPlayer.GetPlayerID()) {
        return _localPlayerPins.ToList();
      }

      return Minimap.instance.m_pins.Where(x => x.m_ownerID == pid).ToList();
    }

    public static bool IsFiltered(long pid) {
      if (!FilteredPinOwners.Contains(pid)) {
        return false;
      }

      return true;
    }

    public static void ToggleFilter(long pid) {
      if (FilteredPinOwners.Contains(pid)) {
        FilteredPinOwners.Remove(pid);
        return;
      }

      FilteredPinOwners.Add(pid);
    }

    public static bool IsInitialized() {
      if (!ForeignPinOwners.Any()) {
        return false;
      }

      return true;
    }

    public static void AddLocalPlayerPin(Minimap.PinData pin) {
      if (_localPlayerPins.Contains(pin)) {
        return;
      }

      _localPlayerPins.Add(pin);
    }

    public static void RemoveLocalPlayerPin(Minimap.PinData pin) {
      _localPlayerPins.Remove(pin);
    }

    public static string PidToRow(int hashedPid) {
      return string.Join(
          ",",
          hashedPid.ToString()
      );
    }

    public static void WriteFilteredPlayersToFile() {
      if (!FilteredPinOwners.Any()) {
        return;
      }

      if (!Directory.Exists(GetPath())) {
        Directory.CreateDirectory(GetPath());
      }

      using StreamWriter writer = File.CreateText(GetFilteredFilename());

      writer.AutoFlush = true;

      foreach (long pid in FilteredPinOwners) {
        writer.WriteLine(PidToRow(HashPid(pid)));
      }
    }

    public static void LoadFilteredPinOwners() {
      ReadFilteredPinOwnersFromFile();
    }

    public static void ReadFilteredPinOwnersFromFile() {
      if (!ZNet.instance || ZNet.instance.GetWorldUID().Equals(default)) {
        LogWarning("Could not read saved filtered pin owners from file as ZNet instance is null.");
        return;
      }

      if (!File.Exists(GetFilteredFilename())) {
        Log($"No saved filtered pin owners to load from {GetFilteredFilename()}.");
        return;
      }

      Log($"Loading saved filter state from {GetFilteredFilename()}");

      List<long> loadedHashedPids = new();

      using (var reader = new StreamReader(GetFilteredFilename())) {
        while (!reader.EndOfStream) {
          var line = reader.ReadLine();
          var values = line.Split(',');

          int hashedPid = Int32.Parse(values[0]);

          loadedHashedPids.Add(hashedPid);
        }
      }

      foreach (long pid in ForeignPinOwners) {
        if (!loadedHashedPids.Contains(HashPid(pid))) {
          continue;
        }

        if (FilteredPinOwners.Contains(pid)) {
          continue;
        }

        FilteredPinOwners.Add(pid);
      }
    }
  }
}
