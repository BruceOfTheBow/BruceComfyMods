using System.Collections.Generic;
using System.Linq;

using static Pintervention.Pintervention;

namespace Pintervention {
  public class PinOwnerManager {
    public static List<long> ForeignPinOwners { get; private set; } = new();
    static List<long> _filteredPinOwners = new();
    static HashSet<Minimap.PinData> _localPlayerPins = new();

    public static bool Initialize() {
      if (!Minimap.instance || !Player.m_localPlayer || IsInitialized()) {
        return false;
      }

      UpdatePinOwners();
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

      if(_filteredPinOwners.Any()) {
        _filteredPinOwners.Clear();
        _filteredPinOwners = new();
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

      if (!_filteredPinOwners.Any()) { 
        return; 
      }

      foreach (long pid in _filteredPinOwners) {
        List<Minimap.PinData> pinsToFilter = GetPinsByPid(pid);

        foreach (Minimap.PinData pin in pinsToFilter) {
          Minimap.instance.DestroyPinMarker(pin);
        }
      }
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
      if (!_filteredPinOwners.Contains(pid)) {
        return false;
      }

      return true;
    }

    public static void ToggleFilter(long pid) {
      if (_filteredPinOwners.Contains(pid)) {
        _filteredPinOwners.Remove(pid);
        return;
      }

      _filteredPinOwners.Add(pid);
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
  }
}
