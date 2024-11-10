namespace AddAllFuel;

public static class SmelterExtensions {
  public static string GetFuelItemName(this Smelter smelter) {
    return smelter.m_fuelItem ? smelter.m_fuelItem.m_itemData.m_shared.m_name : string.Empty;
  }

  public static bool RepeatAddFuel(
      this Smelter smelter, Switch sw, Humanoid user, ItemDrop.ItemData item, int count) {
    bool result = false;

    for (int i = 0; i < count; i++) {
      bool onAddFuelResult = smelter.OnAddFuel(sw, user, item);

      if (!onAddFuelResult) {
        return result;
      }

      result |= onAddFuelResult;
    }

    return result;
  }

  public static bool RepeatAddOre(
      this Smelter smelter, Switch sw, Humanoid user, ItemDrop.ItemData item, int count) {
    bool result = false;

    for (int i = 0; i < count; i++) {
      bool onAddOreResult = smelter.OnAddOre(sw, user, item);

      if (!onAddOreResult) {
        return result;
      }

      result |= onAddOreResult;
    }

    return result;
  }

  public static bool TryFindCookableItem(this Smelter smelter, Inventory inventory, out ItemDrop.ItemData item) {
    foreach (Smelter.ItemConversion itemConversion in smelter.m_conversion) {
      string fromItemName = itemConversion.m_from.m_itemData.m_shared.m_name;

      if (SmelterManager.ExcludeCookableItems.Contains(fromItemName)) {
        continue;
      }

      item = inventory.GetItem(fromItemName);

      if (item != default) {
        return true;
      }
    }

    item = default;
    return false;
  }
}
