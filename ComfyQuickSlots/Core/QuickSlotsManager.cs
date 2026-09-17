namespace ComfyQuickSlots;

using System.Collections.Generic;

public static class QuickSlotsManager {
  public const string PlayerInventoryName = "ComfyQuickSlotsInventory";
  public const int Columns = 8;
  private static int _rows = 5;


  public static void SetupPlayerInventory(Inventory inventory) {
    inventory.m_name = PlayerInventoryName;
    inventory.m_width = Columns;
  }

  public const string PlayerDataKey = "ComfyQuickSlotsInventory";
  public const int QuickSlotsCount = 3;

  public static readonly List<ItemDrop.ItemData.ItemType> ArmorSlotTypes = [
    ItemDrop.ItemData.ItemType.Helmet,
    ItemDrop.ItemData.ItemType.Chest,
    ItemDrop.ItemData.ItemType.Legs,
    ItemDrop.ItemData.ItemType.Shoulder,
    ItemDrop.ItemData.ItemType.Utility,
  ];

  public static bool FirstLoad = false;
  public static List<ItemDrop.ItemData> InitialEquippedArmor = [];

  public static Vector2i HelmetSlot = new(0, 4);
  public static Vector2i ChestSlot = new(1, 4);
  public static Vector2i LegsSlot = new(2, 4);
  public static Vector2i ShoulderSlot = new(3, 4);
  public static Vector2i UtilitySlot = new(4, 4);

  public static Vector2i QuickSlot1 = new(5, 4);
  public static Vector2i QuickSlot2 = new(6, 4);
  public static Vector2i QuickSlot3 = new(7, 4);

  public static List<Vector2i> ArmorSlots = [HelmetSlot, ChestSlot, LegsSlot, ShoulderSlot, UtilitySlot];
  public static List<Vector2i> QuickSlots = [QuickSlot1, QuickSlot2, QuickSlot3];

  public static bool OnMenuLoad = false;

  public static bool ShouldRefreshPlayerGrid { get; set; }

  public static void RefreshBindings() {
    ShouldRefreshPlayerGrid = true;
  }

  public static bool AddItemToExistingStacks(Inventory inventory, ItemDrop.ItemData item) {
    int i = 0;
    if (item.m_shared.m_maxStackSize > 1) {
      while (i < item.m_stack) {
        ItemDrop.ItemData itemData =
            inventory.FindFreeStackItem(item.m_shared.m_name, item.m_quality, item.m_worldLevel);

        if (itemData != null) {
          itemData.m_stack++;
          i++;
        } else {
          item.m_stack -= i;
          Vector2i vector2i = GetEmptyInventorySlot(inventory, true);
          if (vector2i.x >= 0) {
            item.m_gridPos = vector2i;
            inventory.m_inventory.Add(item);
            return true;
          }
          return false;
        }
      }
    }
    return false;
  }

  public static bool AddItemToSlot(Humanoid humanoid, ItemDrop.ItemData item, int x, int y) {
    humanoid.GetInventory().m_inventory.Add(item);
    item.m_gridPos = new Vector2i(x, y);
    return true;
  }

  public static void EquipAndAddItem(Humanoid humanoid, ItemDrop.ItemData item) {
    humanoid.m_inventory.AddItem(item);
    EquipItem(humanoid, item);
  }

  public static void EquipArmorInArmorSlots(Player player) {
    foreach (Vector2i armorSlot in ArmorSlots) {
      EquipItem(player, player.GetInventory().GetItemAt(armorSlot.x, armorSlot.y));
    }
  }

  public static void EquipItem(Humanoid humanoid, ItemDrop.ItemData item) {
    if (item == null) {
      return;
    }

    if (item.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Helmet) {
      humanoid.m_helmetItem = item;
    } else if (item.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Chest) {
      humanoid.m_chestItem = item;
    } else if (item.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Legs) {
      humanoid.m_legItem = item;
    } else if (item.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Shoulder) {
      humanoid.m_shoulderItem = item;
    } else if (item.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Utility) {
      humanoid.m_utilityItem = item;
    }

    item.m_equipped = true;
    humanoid.SetupEquipment();
    humanoid.TriggerEquipEffect(item);
  }

  public static void GetAllArmorFirst(Player player) {
    if (player.m_helmetItem != null) {
      InitialEquippedArmor.Add(player.m_helmetItem);
    }
    if (player.m_chestItem != null) {
      InitialEquippedArmor.Add(player.m_chestItem);
    }
    if (player.m_legItem != null) {
      InitialEquippedArmor.Add(player.m_legItem);
    }
    if (player.m_shoulderItem != null) {
      InitialEquippedArmor.Add(player.m_shoulderItem);
    }
    if (player.m_utilityItem != null) {
      InitialEquippedArmor.Add(player.m_utilityItem);
    }
  }

  public static ItemDrop.ItemData GetArmorItemToSwap(Humanoid humanoid, ItemDrop.ItemData item) {
    ItemDrop.ItemData.ItemType itemType = item.m_shared.m_itemType;

    return itemType switch {
      ItemDrop.ItemData.ItemType.Helmet => humanoid.m_helmetItem,
      ItemDrop.ItemData.ItemType.Chest => humanoid.m_chestItem,
      ItemDrop.ItemData.ItemType.Legs => humanoid.m_legItem,
      ItemDrop.ItemData.ItemType.Shoulder => humanoid.m_shoulderItem,
      ItemDrop.ItemData.ItemType.Utility => humanoid.m_utilityItem,
      _ => default,
    };
  }

  public static Vector2i GetArmorSlot(ItemDrop.ItemData item) {
    return ArmorSlots[GetArmorTypeIndex(item)];
  }

  public static int GetArmorSlotIndex(Vector2i loc) {
    return ArmorSlots.IndexOf(loc);
  }

  public static int GetArmorTypeIndex(ItemDrop.ItemData item) {
    return ArmorSlotTypes.IndexOf(item.m_shared.m_itemType);
  }

  public static Vector2i GetEmptyInventorySlot(Inventory inventory, bool topFirst) {
    if (topFirst) {
      for (int j = 0; j < _rows; j++) {
        for (int i = 0; i < Columns; i++) {
          Vector2i slot = new Vector2i(i, j);

          if (inventory.GetItemAt(i, j) == null && !IsArmorSlot(slot)) {
            return slot;
          }
        }
      }
    }

    for (int j = _rows - 1; j >= 0; j--) {
      for (int i = 0; i < Columns; i++) {
        Vector2i slot = new Vector2i(i, j);

        if (inventory.GetItemAt(i, j) == null && !IsArmorSlot(slot)) {
          return slot;
        }
      }
    }

    return new Vector2i(-1, -1);
  }

  public static bool HaveEmptyInventorySlot(Inventory inventory) {
    for (int i = 0; i < Columns; i++) {
      for (int j = 0; j < _rows; j++) {
        if (inventory.GetItemAt(i, j) == null && !IsArmorSlot(new Vector2i(i, j))) {
          return true;
        }
      }
    }

    return false;
  }

  public static bool IsArmor(ItemDrop.ItemData item) {
    ItemDrop.ItemData.ItemType itemType = item.m_shared.m_itemType;

    return itemType switch {
      ItemDrop.ItemData.ItemType.Helmet => true,
      ItemDrop.ItemData.ItemType.Chest => true,
      ItemDrop.ItemData.ItemType.Legs => true,
      ItemDrop.ItemData.ItemType.Shoulder => true,
      ItemDrop.ItemData.ItemType.Utility => true,
      _ => false,
    };
  }

  public static bool IsArmorSlot(Vector2i loc) {
    if (ArmorSlots.Contains(loc)) {
      return true;
    }

    return false;
  }

  public static bool IsArmorTypeEquipped(Humanoid humanoid, ItemDrop.ItemData item) {
    ItemDrop.ItemData.ItemType itemType = item.m_shared.m_itemType;

    return itemType switch {
      ItemDrop.ItemData.ItemType.Helmet => humanoid.m_helmetItem != null,
      ItemDrop.ItemData.ItemType.Chest => humanoid.m_chestItem != null,
      ItemDrop.ItemData.ItemType.Legs => humanoid.m_legItem != null,
      ItemDrop.ItemData.ItemType.Shoulder => humanoid.m_shoulderItem != null,
      ItemDrop.ItemData.ItemType.Utility => humanoid.m_utilityItem != null,
      _ => false,
    };
  }

  public static bool IsQuickSlot(Vector2i loc) {
    if (QuickSlots.Contains(loc)) {
      return true;
    }
    return false;
  }

  public static bool IsSimilarItemEquipped(ItemDrop.ItemData item) {
    foreach (ItemDrop.ItemData itemCheck in Player.m_localPlayer.GetInventory().GetEquippedItems()) {
      if (item.m_shared.m_name.Equals(itemCheck.m_shared.m_name)) {
        return true;
      }
    }
    return false;
  }

  public static int ItemCountInInventory(Inventory inventory, ItemDrop.ItemData item) {
    string itemName = item.m_shared.m_name;
    int count = 0;

    for (int j = 0; j < _rows; j++) {
      for (int i = 0; i < Columns; i++) {
        ItemDrop.ItemData inventoryItem = inventory.GetItemAt(i, j);

        if (inventoryItem != null && inventoryItem.m_shared.m_name == itemName) {
          count++;
        }
      }
    }

    return count;
  }

  public static void MoveArmorItemToSlot(Humanoid humanoid, ItemDrop.ItemData item, int x, int y) {
    if (item == null) {
      return;
    }

    ItemDrop.ItemData itemInArmorSlot = humanoid.GetInventory().GetItemAt(x, y);

    if (itemInArmorSlot != null && !itemInArmorSlot.Equals(item)) {
      SwapArmorItems(humanoid, item, itemInArmorSlot, x, y);
      return;
    } 

    item.m_gridPos = new Vector2i(x, y);

    if (!humanoid.GetInventory().m_inventory.Contains(item)) {
      humanoid.GetInventory().AddItem(item);
      humanoid.GetInventory().Changed();
    }
  }

  public static void MoveEquippedArmorToArmorSlots() {
    if (Player.m_localPlayer == null) {
      return;
    }

    MoveArmorItemToSlot(Player.m_localPlayer, Player.m_localPlayer.m_helmetItem, HelmetSlot.x, HelmetSlot.y);
    MoveArmorItemToSlot(Player.m_localPlayer, Player.m_localPlayer.m_chestItem, ChestSlot.x, ChestSlot.y);
    MoveArmorItemToSlot(Player.m_localPlayer, Player.m_localPlayer.m_legItem, LegsSlot.x, LegsSlot.y);
    MoveArmorItemToSlot(Player.m_localPlayer, Player.m_localPlayer.m_shoulderItem, ShoulderSlot.x, ShoulderSlot.y);
    MoveArmorItemToSlot(Player.m_localPlayer, Player.m_localPlayer.m_utilityItem, UtilitySlot.x, UtilitySlot.y);
}

  public static bool Save(Player player) {
    ZPackage pkg = new();
    player.GetInventory().Save(pkg);

    if (player.m_knownTexts.ContainsKey(PlayerDataKey)) {
      player.m_knownTexts[PlayerDataKey] = pkg.GetBase64();
    } else {
      player.m_knownTexts.Add(PlayerDataKey, pkg.GetBase64());
    }

    return true;
  }

  public static bool SwapArmorItems(
      Humanoid humanoid,
      ItemDrop.ItemData itemToMove,
      ItemDrop.ItemData itemInArmorSlot,
      int armorSlotX,
      int armorSlotY) {
    Vector2i otherSlot = itemToMove.m_gridPos;
    itemToMove.m_gridPos = new Vector2i(armorSlotX, armorSlotY);
    itemInArmorSlot.m_gridPos = otherSlot;
    humanoid.GetInventory().Changed();

    return false;
  }

  public static bool UnequipAllArmor(Player player) {
    Inventory playerInventory = player.GetInventory();

    for (int i = 0; i < 5; i++) {
      ItemDrop.ItemData item = playerInventory.GetItemAt(i, 4);

      if (item != null) {
        UnequipItem(player, item);
      }
    }

    return true;
  }

  public static bool UnequipItem(Humanoid player, ItemDrop.ItemData item) {
    if (player.m_helmetItem == item) {
      player.m_helmetItem = null;
      item.m_equipped = false;

      player.SetupEquipment();
      player.TriggerEquipEffect(item);

      return true;
    }

    if (player.m_chestItem == item) {
      player.m_chestItem = null;
      item.m_equipped = false;

      player.SetupEquipment();
      player.TriggerEquipEffect(item);

      return true;
    }

    if (player.m_legItem == item) {
      player.m_legItem = null;
      item.m_equipped = false;

      player.SetupEquipment();
      player.TriggerEquipEffect(item);

      return true;
    }

    if (player.m_shoulderItem == item) {
      player.m_shoulderItem = null;
      item.m_equipped = false;

      player.SetupEquipment();
      player.TriggerEquipEffect(item);

      return true;
    }

    if (player.m_utilityItem == item) {
      player.m_utilityItem = null;
      item.m_equipped = false;

      player.SetupEquipment();
      player.TriggerEquipEffect(item);

      return true;
    }

    return false;
  }

  public static bool CanAddItem(Inventory inventory, ItemDrop.ItemData item, int stack) {
    int emptySlots = (inventory.m_width * inventory.m_height) - 5;
    int stackSpace = 0;

    string itemName = item.m_shared.m_name;
    int worldLevel = item.m_worldLevel;

    foreach (ItemDrop.ItemData itemData in inventory.m_inventory) {
      Vector2i gridPos = itemData.m_gridPos;

      if (gridPos.y == 4 && gridPos.x <= 4) {
        continue;
      }

      emptySlots--;

      if (itemData.m_shared.m_name == itemName && itemData.m_worldLevel == worldLevel) {
        stackSpace += (itemData.m_shared.m_maxStackSize - itemData.m_stack);
      }
    }

    if (stack <= 0) {
      stack = item.m_stack;
    }

    return emptySlots > 0 || (stackSpace + (emptySlots * item.m_shared.m_maxStackSize)) >= stack;
  }

  public static bool HasEmptyNonEquipmentSlot(Inventory inventory) {
    int emptySlots = (inventory.m_width * inventory.m_height) - 5;

    foreach (ItemDrop.ItemData itemData in inventory.m_inventory) {
      Vector2i gridPos = itemData.m_gridPos;

      if (gridPos.y == 4 && gridPos.x <= 4) {
        continue;
      }

      emptySlots--;
    }

    return emptySlots > 0;
  }

  public static int FindFreeNonEquipmentStackSpace(Inventory inventory, string itemName, int worldLevel) {
    int stackSpace = 0;

    foreach (ItemDrop.ItemData itemData in inventory.m_inventory) {
      Vector2i gridPos = itemData.m_gridPos;

      if (gridPos.y == 4 && gridPos.x <= 4) {
        continue;
      }

      if (itemData.m_shared.m_name == itemName && itemData.m_worldLevel == worldLevel) {
        stackSpace += (itemData.m_shared.m_maxStackSize - itemData.m_stack);
      }
    }

    return stackSpace;
  }

  public static int GetRows() {
    return _rows;
  }
  public static void SetRows(int rows) {
    if (rows == _rows) {
      return;
    }

    _rows = rows;
    PositionQuickslots();
  }

  public static void PositionQuickslots() {
    HelmetSlot.y = _rows - 1;
    ChestSlot.y = _rows - 1;
    LegsSlot.y = _rows - 1;
    ShoulderSlot.y = _rows - 1;
    UtilitySlot.y = _rows - 1;

    QuickSlot1.y = _rows - 1;
    QuickSlot2.y = _rows - 1;
    QuickSlot3.y = _rows - 1;

    ArmorSlots = [HelmetSlot, ChestSlot, LegsSlot, ShoulderSlot, UtilitySlot];
    QuickSlots = [QuickSlot1, QuickSlot2, QuickSlot3];
  }
}
