namespace AddAllFuel;

public static class CookingStationExtensions {
  public static string GetFuelItemName(this CookingStation cookingStation) {
    return cookingStation.m_fuelItem ? cookingStation.m_fuelItem.m_itemData.m_shared.m_name : string.Empty;
  }

  public static bool TryGetFreeSlot(this CookingStation cookingStation, out int slotIndex) {
    ZDO zdo = cookingStation.m_nview.m_zdo;

    for (int i = 0; i < cookingStation.m_slots.Length; i++) {
      if (!zdo.TryGetSlotString(i, out string value) || string.IsNullOrEmpty(value)) {
        slotIndex = i;
        return true;
      }
    }

    slotIndex = default;
    return false;
  }

  public static int GetFreeSlotCount(this CookingStation cookingStation) {
    ZDO zdo = cookingStation.m_nview.m_zdo;
    int freeSlotCount = 0;

    for (int i = 0; i < cookingStation.m_slots.Length; i++) {
      if (!zdo.TryGetSlotString(i, out string value) || string.IsNullOrEmpty(value)) {
        freeSlotCount++;
      }
    }

    return freeSlotCount;
  }

  public static bool HaveDoneItemByStatus(this CookingStation cookingStation) {
    ZNetView netView = cookingStation.m_nview;

    if (!netView.IsValid()) {
      return false;
    }

    ZDO zdo = netView.m_zdo;

    for (int i = 0; i < cookingStation.m_slots.Length; i++) {
      if (zdo.TryGetSlotStatusInt(i, out int slotStatus) && slotStatus != (int) CookingStation.Status.NotDone) {
        return true;
      }
    }

    return false;
  }

  public static int GetDoneItemCount(this CookingStation cookingStation) {
    ZNetView netView = cookingStation.m_nview;

    if (!netView.IsValid()) {
      return 0;
    }

    ZDO zdo = netView.m_zdo;
    int doneItemCount = 0;

    for (int i = 0; i < cookingStation.m_slots.Length; i++) {
      if (zdo.TryGetSlotStatusInt(i, out int slotStatus) && slotStatus != (int) CookingStation.Status.NotDone) {
        doneItemCount++;
      }
    }

    return doneItemCount;
  }

  public static bool RepeatInteract(this CookingStation cookingStation, Humanoid user, int count) {
    bool result = false;

    for (int i = 0; i < count; i++) {
      bool onInteractResult = cookingStation.OnInteract(user);

      if (!onInteractResult) {
        return result;
      }

      result |= onInteractResult;
    }

    return result;
  }

  public static bool RepeatUseItem(
      this CookingStation cookingStation, Humanoid user, ItemDrop.ItemData item, int count) {
    bool result = false;

    for (int i = 0; i < count; i++) {
      bool onUseItemResult = cookingStation.OnUseItem(user, item);

      if (!onUseItemResult) {
        return result;
      }

      result |= onUseItemResult;
    }

    return result;
  }

  public static bool RepeatAddFuel(
      this CookingStation cookingStation, Switch sw, Humanoid user, ItemDrop.ItemData item, int count) {
    bool result = false;

    for (int i = 0; i < count; i++) {
      bool onAddFuelSwitchResult = cookingStation.OnAddFuelSwitch(sw, user, item);

      if (!onAddFuelSwitchResult) {
        return result;
      }

      result |= onAddFuelSwitchResult;
    }

    return result;
  }
}
