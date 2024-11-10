namespace AddAllFuel;

public static class ZDOExtensions {
  public static readonly int[] SlotHashes = [
    "slot0".GetStableHashCode(),
    "slot1".GetStableHashCode(),
    "slot2".GetStableHashCode(),
    "slot3".GetStableHashCode(),
    "slot4".GetStableHashCode(),
    "slot5".GetStableHashCode(),
    "slot6".GetStableHashCode(),
    "slot7".GetStableHashCode(),
    "slot8".GetStableHashCode(),
    "slot9".GetStableHashCode(),
  ];

  public static readonly int[] SlotStatusHashes = [
    "slotstatus0".GetStableHashCode(),
    "slotstatus1".GetStableHashCode(),
    "slotstatus2".GetStableHashCode(),
    "slotstatus3".GetStableHashCode(),
    "slotstatus4".GetStableHashCode(),
    "slotstatus5".GetStableHashCode(),
    "slotstatus6".GetStableHashCode(),
    "slotstatus7".GetStableHashCode(),
    "slotstatus8".GetStableHashCode(),
    "slotstatus9".GetStableHashCode(),
  ];

  public static bool TryGetSlotString(this ZDO zdo, int slotIndex, out string value) {
    if (ZDOExtraData.s_strings.TryGetValue(zdo.m_uid, out BinarySearchDictionary<int, string> values)
        && values.TryGetValue(SlotHashes[slotIndex], out value)) {
      return true;
    }

    value = default;
    return false;
  }

  public static bool TryGetSlotFloat(this ZDO zdo, int slotIndex, out float value) {
    if (ZDOExtraData.s_floats.TryGetValue(zdo.m_uid, out BinarySearchDictionary<int, float> values)
      && values.TryGetValue(SlotHashes[slotIndex], out value)) {
      return true;
    }

    value = default;
    return false;
  }

  public static bool TryGetSlotStatusInt(this ZDO zdo, int slotIndex, out int value) {
    if (ZDOExtraData.s_ints.TryGetValue(zdo.m_uid, out BinarySearchDictionary<int, int> values)
        && values.TryGetValue(SlotStatusHashes[slotIndex], out value)) {
      return true;
    }

    value = default;
    return false;
  }
}
