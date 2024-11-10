namespace AddAllFuel;

public static class ShieldGeneratorExtensions {
  public static bool IsMatchingFuelItem(this ShieldGenerator shieldGenerator, string itemName) {
    foreach (ItemDrop itemDrop in shieldGenerator.m_fuelItems) {
      if (itemDrop.m_itemData.m_shared.m_name == itemName) {
        return true;
      }
    }

    return false;
  }

  public static ItemDrop.ItemData GetMatchingFuelItem(this ShieldGenerator shieldGenerator, Inventory inventory) {
    foreach (ItemDrop itemDrop in shieldGenerator.m_fuelItems) {
      ItemDrop.ItemData item = inventory.GetItem(itemDrop.m_itemData.m_shared.m_name);

      if (item != default) {
        return item;
      }
    }

    return default;
  }

  public static bool RepeatAddFuel(
      this ShieldGenerator shieldGenerator, Switch sw, Humanoid user, ItemDrop.ItemData item, int count) {
    bool result = false;

    for (int i = 0; i < count; i++) {
      bool onAddFuelResult = shieldGenerator.OnAddFuel(sw, user, item);

      if (!onAddFuelResult) {
        return result;
      }

      result |= onAddFuelResult;
    }

    return result;
  }
}
