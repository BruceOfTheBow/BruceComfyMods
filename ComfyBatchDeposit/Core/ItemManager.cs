namespace ComfyBatchDeposit;

using System.Collections.Generic;

public sealed class ItemManager {
  public static bool IsDropping = false;
  public static List<ItemDrop.ItemData> GetAllItemsOfType(Inventory inventory, ItemDrop.ItemData item) {
    List<ItemDrop.ItemData> allItems = new();

    if (item == null) {
      return allItems;
    }

    foreach (ItemDrop.ItemData checkItem in inventory.GetAllItems()) {
      if (checkItem == null
            || checkItem.m_shared.m_name != item.m_shared.m_name
            || checkItem == item) {
        continue;
      }

      allItems.Add(checkItem);
    }

    return allItems;
  }
}
