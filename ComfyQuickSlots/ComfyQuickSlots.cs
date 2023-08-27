using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

using static ComfyQuickSlots.PluginConfig;

namespace ComfyQuickSlots {
  [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
  public class ComfyQuickSlots : BaseUnityPlugin {
    public const string PluginGuid = "com.bruce.valheim.comfyquickslots";
    public const string PluginName = "ComfyQuickSlots";
    public const string PluginVersion = "1.3.0";

    public const string playerDataKey = "ComfyQuickSlotsInventory";
    private static ConfigFile configFile = new ConfigFile(Path.Combine(Paths.ConfigPath, "ComfyQuickSlots.cfg"), true);

    static ManualLogSource _logger;
    Harmony _harmony;

    public static readonly string PlayerInventoryName = "ComfyQuickSlotsInventory";

    private static bool _debug = false;

    static Assembly assem = typeof(ComfyQuickSlots).Assembly;
    public static string fpath = assem.Location;
    public static string path = fpath.Remove(fpath.Length - 13);
    public static bool GKInstalled;

    public const int quickSlotsCount = 3;

    public const int rows = 5;
    public const int columns = 8;
    public static readonly List<ItemDrop.ItemData.ItemType> ArmorSlotTypes = new List<ItemDrop.ItemData.ItemType>() {
            ItemDrop.ItemData.ItemType.Helmet,
            ItemDrop.ItemData.ItemType.Chest,
            ItemDrop.ItemData.ItemType.Legs,
            ItemDrop.ItemData.ItemType.Shoulder,
            ItemDrop.ItemData.ItemType.Utility
        };
    public static Vector2i helmetSlot = new Vector2i(0, 4);
    public static Vector2i chestSlot = new Vector2i(1, 4);
    public static Vector2i legsSlot = new Vector2i(2, 4);
    public static Vector2i shoulderSlot = new Vector2i(3, 4);
    public static Vector2i utilitySlot = new Vector2i(4, 4);

    static Vector2i _quickSlot1 = new Vector2i(5, 4);
    static Vector2i _quickSlot2 = new Vector2i(6, 4);
    static Vector2i _quickSlot3 = new Vector2i(7, 4);

    public static List<Vector2i> armorSlots = new List<Vector2i>() { helmetSlot, chestSlot, legsSlot, shoulderSlot, utilitySlot };
    static List<Vector2i> _quickSlots = new List<Vector2i>() { _quickSlot1, _quickSlot2, _quickSlot3 };

    public static bool firstLoad = false;
    public static bool onMenuLoad = false;
    public static List<ItemDrop.ItemData> initialEquippedArmor = new List<ItemDrop.ItemData>();

    public void Awake() {
      _harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), harmonyInstanceId: PluginGuid);
      _logger = Logger;

      BindConfig(Config);
    }

    public void OnDestroy() {
      _harmony?.UnpatchSelf();
    }

    public void Update() {
      var player = Player.m_localPlayer;
      if (player == null || !player.TakeInput() || !EnableQuickslots.Value) {
        return;
      }

      ItemDrop.ItemData item = null;

      if (Input.GetKeyDown(QuickSlot3.Value)) {
        item = Player.m_localPlayer.GetInventory().GetItemAt(7, 4);
      }
      if (Input.GetKeyDown(QuickSlot2.Value)) {
        item = Player.m_localPlayer.GetInventory().GetItemAt(6, 4);
      }
      if (Input.GetKeyDown(QuickSlot1.Value)) {
        item = Player.m_localPlayer.GetInventory().GetItemAt(5, 4);
        
      }

      if (item == null || Player.m_localPlayer.IsEquipActionQueued(item)) {
        return;
      }
      Player.m_localPlayer.UseItem(null, item, false);
    }

    public static void log(string message) {
      if (_debug) {
        _logger.LogInfo(message);
      }
    }

    public static bool HaveEmptyInventorySlot(Inventory inventory) {
      for (int i = 0; i < columns; i++) {
        for (int j = 0; j < rows; j++) {
          if (inventory.GetItemAt(i, j) == null && j != 4) {
            return true;
          } else {
            if (i > 4 && j == 4 && inventory.GetItemAt(i, j) == null) {
              return true;
            }
          }
        }
      }
      return false;
    }
    public static Vector2i GetEmptyInventorySlot(Inventory inventory, bool topFirst) {
      if (topFirst) {
        for (int j = 0; j < rows; j++) {
          for (int i = 0; i < columns; i++) {
            if (inventory.GetItemAt(i, j) == null && j != 4) {
              return new Vector2i(i, j);
            } else {
              if (i > 4 && j == 4 && inventory.GetItemAt(i, j) == null) {
                return new Vector2i(i, j);
              }
            }
          }
        }
      } else {
        for (int j = rows - 1; j >= 0; j--) {
          for (int i = 0; i < columns; i++) {
            if (inventory.GetItemAt(i, j) == null && j != 4) {
              return new Vector2i(i, j);
            } else {
              if (i > 4 && j == 4 && inventory.GetItemAt(i, j) == null) {
                return new Vector2i(i, j);
              }
            }
          }
        }
      }

      return new Vector2i(-1, -1);
    }
    public static bool IsArmor(ItemDrop.ItemData item) {
      if (item.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Helmet || item.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Chest || item.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Legs || item.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Shoulder || item.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Utility) {
        return true;
      }
      return false;
    }

    public static bool IsArmorSlot(Vector2i loc) {
      if (armorSlots.Contains(loc)) {
        return true;
      }
      return false;
    }
    public static bool IsQuickSlot(Vector2i loc) {
      if (_quickSlots.Contains(loc)) {
        return true;
      }
      return false;
    }

    public static Vector2i GetArmorSlot(ItemDrop.ItemData item) {
      return armorSlots[GetArmorTypeIndex(item)];
    }

    public static int GetArmorSlotIndex(Vector2i loc) {
      return armorSlots.IndexOf(loc);
    }

    public static int GetArmorTypeIndex(ItemDrop.ItemData item) {
      return ArmorSlotTypes.IndexOf(item.m_shared.m_itemType);
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

    public static void EquipItem(Humanoid humanoid, ItemDrop.ItemData item) {
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
      log($"Equipped {item.m_shared.m_name}");
      item.m_equipped = true;
      humanoid.SetupEquipment();
      humanoid.TriggerEquipEffect(item);
    }

    public static void EquipAndAddItem(Humanoid humanoid, ItemDrop.ItemData item) {
      humanoid.m_inventory.AddItem(item);
      EquipItem(humanoid, item);
    }

    public static bool IsArmorTypeEquipped(Humanoid humanoid, ItemDrop.ItemData item) {
      ItemDrop.ItemData armorItem = null;
      if (item.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Helmet) {
        armorItem = humanoid.m_helmetItem;
      } else if (item.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Chest) {
        armorItem = humanoid.m_chestItem;
      } else if (item.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Legs) {
        armorItem = humanoid.m_legItem;
      } else if (item.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Shoulder) {
        armorItem = humanoid.m_shoulderItem;
      } else if (item.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Utility) {
        armorItem = humanoid.m_utilityItem;
      }

      if (armorItem == null) {
        return false;
      }
      return true;
    }

    public static ItemDrop.ItemData GetArmorItemToSwap(Humanoid humanoid, ItemDrop.ItemData item) {
      if (item.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Helmet) {
        return humanoid.m_helmetItem;
      } else if (item.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Chest) {
        return humanoid.m_chestItem;
      } else if (item.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Legs) {
        return humanoid.m_legItem;
      } else if (item.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Shoulder) {
        return humanoid.m_shoulderItem;
      } else if (item.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Utility) {
        return humanoid.m_utilityItem;
      }
      return null;
    }

    public static bool SwapArmorItems(Humanoid humanoid, ItemDrop.ItemData itemToMove, ItemDrop.ItemData itemInArmorSlot, int armorSlotX, int armorSlotY) {
      ComfyQuickSlots.log($"Swapping {itemToMove.m_shared.m_name} in {itemToMove.m_gridPos.x},{itemToMove.m_gridPos.y} with {itemInArmorSlot} in {armorSlotX},{armorSlotY}");
      Vector2i otherSlot = itemToMove.m_gridPos;
      itemToMove.m_gridPos = new Vector2i(armorSlotX, armorSlotY);
      itemInArmorSlot.m_gridPos = otherSlot;
      humanoid.GetInventory().Changed();
      return false;
    }

    public static bool MoveArmorItemToSlot(Humanoid humanoid, ItemDrop.ItemData item, int x, int y) {
      ItemDrop.ItemData itemInArmorSlot = humanoid.GetInventory().GetItemAt(x, y);
      if (itemInArmorSlot != null && !itemInArmorSlot.Equals(item)) {
        return SwapArmorItems(humanoid, item, itemInArmorSlot, x, y);
      } else {
        item.m_gridPos = new Vector2i(x, y);
        if (!humanoid.GetInventory().m_inventory.Contains(item)) {
          humanoid.GetInventory().AddItem(item);
          humanoid.GetInventory().Changed();
          return true;
        }
      }
      return false;
    }

    public static bool AddItemToSlot(Humanoid humanoid, ItemDrop.ItemData item, int x, int y) {
      humanoid.GetInventory().m_inventory.Add(item);
      item.m_gridPos = new Vector2i(x, y);
      return true;
    }

    public static bool AddItemToExistingStacks(Inventory inventory, ItemDrop.ItemData item) {
      int i = 0;
      if (item.m_shared.m_maxStackSize > 1) {
        while (i < item.m_stack) {
          ItemDrop.ItemData itemData = inventory.FindFreeStackItem(item.m_shared.m_name, item.m_quality);
          if (itemData != null) {
            itemData.m_stack++;
            i++;
          } else {
            item.m_stack = item.m_stack - i;
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

    public static bool EquipArmorInArmorSlots(Player player) {
      for (int i = 0; i < 5; i++) {
        if (player.GetInventory().GetItemAt(i, 4) != null) {
          EquipItem(player, player.GetInventory().GetItemAt(i, 4));
        }
      }
      return true;
    }

    public static bool UnequipAllArmor(Player player) {
      for (int i = 0; i < 5; i++) {
        ItemDrop.ItemData item = player.GetInventory().GetItemAt(i, 4);
        if (item != null) {
          UnequipItem(player, item);
        }
      }
      return true;
    }

    public static void GetAllArmorFirst(Player player) {
      if (player.m_helmetItem != null) {
        initialEquippedArmor.Add(player.m_helmetItem);
      }
      if (player.m_chestItem != null) {
        initialEquippedArmor.Add(player.m_chestItem);
      }
      if (player.m_legItem != null) {
        initialEquippedArmor.Add(player.m_legItem);
      }
      if (player.m_shoulderItem != null) {
        initialEquippedArmor.Add(player.m_shoulderItem);
      }
      if (player.m_utilityItem != null) {
        initialEquippedArmor.Add(player.m_utilityItem);
      }
    }

    public static bool Save(Player player) {
      log($"Saving inventory data to {playerDataKey}");
      ZPackage pkg = new ZPackage();
      player.GetInventory().Save(pkg);
      if (player.m_knownTexts.ContainsKey(playerDataKey)) {
        player.m_knownTexts[playerDataKey] = pkg.GetBase64();
      } else {
        player.m_knownTexts.Add(playerDataKey, pkg.GetBase64());
      }

      return true;
    }

    public static int ItemCountInInventory(ItemDrop.ItemData item) {
      int count = 0;
      for (int j = 0; j < rows; j++) {
        for (int i = 0; i < columns; i++) {
          if (Player.m_localPlayer.GetInventory().GetItemAt(i, j).m_shared.m_name == item.m_shared.m_name) {
            count++;
          }
        }
      }
      return count;
    }

    public static bool IsSimilarItemEquipped(ItemDrop.ItemData item) {
      foreach (ItemDrop.ItemData itemCheck in Player.m_localPlayer.GetInventory().GetEquippedItems()) {
        if (item.m_shared.m_name.Equals(itemCheck.m_shared.m_name)) {
          return true;
        }
      }
      return false;
    }
  }
}