using System.Collections.Generic;
using System.Linq;

using static Pintervention.Pintervention;

namespace Pintervention {
  public class ForeignPinManager {
    static List<long> _foreignPinOwners = new();
    static List<long> _filteredPinOwners = new();

    static Dictionary<long, string> _playerNamesById = new();

    public static bool Initialize() {
      if (!Minimap.instance) {
        return false;
      }

      SetForeignPinOwners();
      SetLocalPlayerName();
      SetForeignPlayerNames();
      
      return true;
    }

    static void SetLocalPlayerName() {
      if (!Player.m_localPlayer || _playerNamesById.ContainsKey(Player.m_localPlayer.GetPlayerID())) {
        return;
      }

      _playerNamesById.Add(Player.m_localPlayer.GetPlayerID(), Player.m_localPlayer.GetPlayerName());
    }

    static void SetForeignPinOwners() {
      if (!Minimap.instance) {
        return;
      }

      _foreignPinOwners = Minimap.instance.m_pins.Select(x => x.m_ownerID).Distinct().ToList();
    }

    public static bool IsLocationPin(Minimap.PinData pin) {
      if (!Minimap.instance || !ZoneSystem.instance) {
        return false;
      }

      List<ZoneSystem.LocationInstance> nearbyLocations 
          = ZoneSystem.instance.m_locationInstances
          .Where(x => Vector2i.Distance(x.Key, new Vector2i((int)pin.m_pos.x, (int)pin.m_pos.z)) < 1f)
          .Select(x => x.Value)
          .ToList();
      
      if (!nearbyLocations.Any()) {
        return false;
      }

      Log($"{nearbyLocations.Count()} locations found for pin {pin.m_name}");
      return true;
    }

    static void SetForeignPlayerNames() {
      // Load data from saved file?

      if (_foreignPinOwners == null || !_foreignPinOwners.Any()) {
        return;
      }

      Dictionary<long, ZDO> zdosByPid = GetZDOsByPlayerId(_foreignPinOwners);

      foreach (KeyValuePair<long, ZDO> zdoByPid in zdosByPid) {
        string name = zdoByPid.Value.GetString(ZDOVars.s_playerName, string.Empty);

        if (string.IsNullOrEmpty(name)) {
          continue;
        }

        _playerNamesById.Add(zdoByPid.Key, name);
      }
    }
    
    public static void Clear() {
      if (_foreignPinOwners.Any()) {
        _foreignPinOwners.Clear();
        _foreignPinOwners = new();
      }

      if(_filteredPinOwners.Any()) {
        _filteredPinOwners.Clear();
        _filteredPinOwners = new();
      }
    }

    public static long GetForeignPinOwnerAtIndex(int index) {
      return _foreignPinOwners[index];
    }

    public static List<long> GetForeignPinOwners() {
      return _foreignPinOwners;
    }

    public static int GetForeignPinOwnersCount() {
      return _foreignPinOwners.Count;
    }

    public static int GetPinCountByOwner(long pid) {
      if (!Minimap.instance) {
        return 0;
      }

      return Minimap.instance.m_pins.Where(x => x.m_ownerID == pid).Count();
    }

    public static string GetPlayerNameById(long pid) {
      if (_playerNamesById.ContainsKey(pid)) {
        return _playerNamesById[pid];
      }

      // Get next Unknown Player count

      return pid.ToString();
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

    public static void FilterPins() {
      if (!IsInitialized()) {
        Initialize();
      }

      if (!_filteredPinOwners.Any()) { 
        return; 
      }

      foreach (long pid in _filteredPinOwners) {
        List<Minimap.PinData> pinsToFilter = Minimap.instance.m_pins.Where(x => x.m_ownerID == pid).ToList();

        foreach (Minimap.PinData pin in pinsToFilter) {
          Minimap.instance.DestroyPinMarker(pin);
        }
      }
    }

    public static void ToggleFilter(long pid) {
      if (_filteredPinOwners.Contains(pid)) {
        _filteredPinOwners.Remove(pid);
        return;
      }

      _filteredPinOwners.Add(pid);
    }

    public static bool IsInitialized() {
      if (_foreignPinOwners == null) {
        return false;
      }

      return true;
    }

    public static void AddPlayerName(long pid, string name) {
      if (_playerNamesById.ContainsKey(pid)) {
        return;
      }

      _playerNamesById.Add(pid, name);
    }
  }
}
