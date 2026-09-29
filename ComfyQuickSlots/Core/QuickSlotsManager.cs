namespace ComfyQuickSlots;

using System.Collections.Generic;
using System.Linq;

public static class QuickSlotsManager {
  public const string PlayerInventoryName = "ComfyQuickSlotsInventory";
  public const int Columns = 8;
  private static int _rows = 5;


  public static void SetupPlayerInventory(Player player) {
    player.m_inventory.m_name = PlayerInventoryName;
    player.m_inventory.m_width = Columns;

    if (player.TryGetUniqueKeyValue("invrows", out string rowStr) && int.TryParse(rowStr, out int rows)) {
      SetRows(rows + 1);
    }

    player.m_inventory.m_height = _rows;
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
  public static bool IsPurchasingItem = false;

  public static void RefreshBindings() {
    ShouldRefreshPlayerGrid = true;
  }

  public static void EquipArmorInArmorSlots(Player player) {
    foreach (Vector2i armorSlot in ArmorSlots) {
      ItemDrop.ItemData item = player.GetInventory().GetItemAt(armorSlot.x, armorSlot.y);

      if (item == null) {
        continue;
      }

      player.EquipItem(item);
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

  public static void LoadSavedData(Player player) {
    foreach (ItemDrop.ItemData item in player.GetInventory().m_inventory) {
      if (item.IsEquipable() && !IsArmor(item) && item.m_equipped) {
        player.EquipItem(item);
      }
    }

    if (player.m_knownTexts.ContainsKey(PlayerDataKey)) {
      // Clear armor references for compatibility
      player.m_helmetItem = null;
      player.m_chestItem = null;
      player.m_legItem = null;
      player.m_shoulderItem = null;
      player.m_utilityItem = null;

      player.GetInventory().Load(new ZPackage(player.m_knownTexts[PlayerDataKey]));
      EquipArmorInArmorSlots(player);
      player.GetInventory().Changed();
      
      return;
    }

    FirstLoad = true;

    foreach (ItemDrop.ItemData armorPiece in player.m_inventory.m_inventory.Where(x => x.m_equipped && IsArmor(x)).ToList()) {
      player.EquipItem(armorPiece);
    }

    MoveEquippedArmorToArmorSlots();
    player.GetInventory().Changed();
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

  public static void MoveQuickSlotItems(Player player) {
    if (player == null || player.GetInventory() == null) {
      return;
    }

    foreach (Vector2i quickSlot in QuickSlots) {
      ItemDrop.ItemData item = player.GetInventory().GetItemAt(quickSlot.x, quickSlot.y - 1);

      if (item == null) {
        continue;
      }

      item.m_gridPos = quickSlot;
    }

    player.GetInventory().Changed();
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

    foreach (Vector2i armorSlot in ArmorSlots) {
      ItemDrop.ItemData item = playerInventory.GetItemAt(armorSlot.x, armorSlot.y);

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
      if (IsArmorSlot(itemData.m_gridPos)) {
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
      if (IsArmorSlot(itemData.m_gridPos)) {
        continue;
      }

      emptySlots--;
    }

    return emptySlots > 0;
  }

  public static int FindFreeNonEquipmentStackSpace(Inventory inventory, string itemName, int worldLevel) {
    int stackSpace = 0;

    foreach (ItemDrop.ItemData itemData in inventory.m_inventory) {
      if (IsArmorSlot(itemData.m_gridPos)) {
        continue;
      }

      if (itemData.m_shared.m_name == itemName && itemData.m_worldLevel == worldLevel) {
        stackSpace += (itemData.m_shared.m_maxStackSize - itemData.m_stack);
      }
    }

    return stackSpace;
  }

  public static int GetEmptyInventorySlots(Inventory inventory) {
    int emptySlots = (inventory.m_width * inventory.m_height) - 5;

    foreach (ItemDrop.ItemData itemData in inventory.m_inventory) {
      if (IsArmorSlot(itemData.m_gridPos)) {
        continue;
      }

      emptySlots--;
    }

    return emptySlots;
  }

  public static int GetRows() {
    return _rows;
  }

  public static int GetRowIndex() {
    return _rows - 1;
  }

  public static void SetRows(int rows) {
    _rows = rows;
    PositionQuickslots();
  }

  public static void PositionQuickslots() {
    HelmetSlot = new Vector2i(0, GetRowIndex());
    ChestSlot = new Vector2i(1, GetRowIndex());
    LegsSlot = new Vector2i(2, GetRowIndex());
    ShoulderSlot = new Vector2i(3, GetRowIndex());
    UtilitySlot = new Vector2i(4, GetRowIndex());

    QuickSlot1 = new Vector2i(5, GetRowIndex());
    QuickSlot2 = new Vector2i(6, GetRowIndex());
    QuickSlot3 = new Vector2i(7, GetRowIndex());

    ArmorSlots = [HelmetSlot, ChestSlot, LegsSlot, ShoulderSlot, UtilitySlot];
    QuickSlots = [QuickSlot1, QuickSlot2, QuickSlot3];
  }
}
