using BepInEx;
using System.Collections.Generic;
using System.Linq;

namespace Pintervention {
  public class ForeignPinManager {
    static List<long> _foreignPinOwners = new();
    static List<long> _filteredPinOwners = new();

    static Dictionary<long, string> _playerNamesById = new();

    public static bool Update() {
      if (!Minimap.instance) {
        return false;
      }

      UpdateLocalPlayerName();
      UpdateForeignPinOwners();
      
      return true;
    }

    static void UpdateLocalPlayerName() {
      if (Player.m_localPlayer == null || _playerNamesById.ContainsKey(Player.m_localPlayer.GetPlayerID())) {
        return;
      }

      _playerNamesById.Add(Player.m_localPlayer.GetPlayerID(), Player.m_localPlayer.GetPlayerName());
    }

    static void UpdateForeignPinOwners() {
      _foreignPinOwners = Minimap.instance.m_pins.Select(x => x.m_ownerID).Distinct().ToList();

      foreach (long foreignPinOwner in _foreignPinOwners) {
        string name
            = Minimap.instance.m_pins
                .Where(x => x.m_ownerID == foreignPinOwner)
                .Select(x => x.m_author)
                .First();
        
        if (name.IsNullOrWhiteSpace() || _playerNamesById.ContainsKey(foreignPinOwner)) {
          continue;
        }

        _playerNamesById.Add(foreignPinOwner, name);
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
      if (_foreignPinOwners == null) {
        Update();
      }

      return _foreignPinOwners[index];
    }

    public static List<long> GetForeignPinOwners() {
      if (_foreignPinOwners == null) {
        Update();
      }

      return _foreignPinOwners;
    }

    public static int GetPinCountByOwner(long pid) {
      if (Minimap.instance == null) {
        return 0;
      }

      if (_foreignPinOwners == null) {
        Update();
      }

      return Minimap.instance.m_pins.Where(x => x.m_ownerID == pid).Count();
    }

    public static List<Minimap.PinData> GetPinsByOwner(long pid) {
      if (_foreignPinOwners == null) {
        Update();
      }

      return Minimap.m_instance.m_pins.Where(x => x.m_ownerID == pid).ToList();
    }

    public static string GetPlayerNameById(long pid) {
      if (_playerNamesById.ContainsKey(pid)) {
        return _playerNamesById[pid];
      }

      // Nope this searched by UID
      if (!ZNet.instance) {
        return pid.ToString();
      }

      ZNetPeer peer = ZNet.instance.GetPeer(pid);

      if (peer == null) {
        return pid.ToString();
      }

      return peer.m_playerName;
    }

    public static void FilterPins() {
      if (!_foreignPinOwners.Any()) {
        Update();
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

    public static void AddPlayerName(long pid, string name) {
      if (_playerNamesById.ContainsKey(pid)) {
        return;
      }

      _playerNamesById.Add(pid, name);
    }
  }
}
