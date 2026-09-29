//namespace ComfyQuickSlots;

//using System.Collections.Generic;
//using System.IO;

//using UnityEngine;

//using static PluginConfig;

//public static class TombStoneManager {

//  public static void CreateTombStone(Player player) {
//    Inventory playerInventory = player.GetInventory();

//    playerInventory.m_height = 4;
//    playerInventory.m_width = 8;

//    // Log items in TombStone on death for auditing and tracking purposes.
//    InventoryLogger.LogInventoryToFile(
//        playerInventory, Path.Combine(LogFilesPath.Value, $"{player.GetPlayerID()}.csv"));


//    if (ZoneSystem.instance.GetGlobalKey(GlobalKeys.DeathKeepEquip)) {
//      // ...
//    } else {
//      QuickSlotsManager.UnequipAllArmor(player);
//      player.UnequipAllItems();
//    }

//    List<ItemDrop.ItemData> playerItems = [.. playerInventory.GetAllItems()];

//    foreach (ItemDrop.ItemData item in playerItems) {
//      if (item.m_gridPos.y >= 4 && !item.m_equipped) {
//        graveInventory.AddItem(item);
//        playerInventory.RemoveItem(item);
//      }
//    }
//  }