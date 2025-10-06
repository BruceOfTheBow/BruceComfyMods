namespace ComfyQuickSlots;

using System.Collections.Generic;
using System.IO;

using UnityEngine;

using static PluginConfig;

public static class TombStoneManager {  
  public static readonly int IsAdditionalTombStoneHash = "cqs.tombstone".GetStableHashCode();

  public static void CreateTombStone(Player player) {
    Inventory playerInventory = player.GetInventory();

    playerInventory.m_height = 4;
    playerInventory.m_width = 8;

    // Log items in TombStone on death for auditing and tracking purposes.
    InventoryLogger.LogInventoryToFile(
        playerInventory, Path.Combine(LogFilesPath.Value, $"{player.GetPlayerID()}.csv"));

    GameObject additionalTombstone = CreateAdditionalTombStone(player);
    SetupAdditionalTombStone(additionalTombstone.GetComponent<TombStone>());

    Container graveContainer = additionalTombstone.GetComponent<Container>();
    Inventory graveInventory = graveContainer.GetInventory();

    if (ZoneSystem.instance.GetGlobalKey(GlobalKeys.DeathKeepEquip)) {
      // ...
    } else {
      QuickSlotsManager.UnequipAllArmor(player);
      player.UnequipAllItems();
    }

    List<ItemDrop.ItemData> playerItems = [.. playerInventory.GetAllItems()];

    foreach (ItemDrop.ItemData item in playerItems) {
      if (item.m_gridPos.y >= 4 && !item.m_equipped) {
        graveInventory.AddItem(item);
        playerInventory.RemoveItem(item);
      }
    }
  }

  static GameObject CreateAdditionalTombStone(Player player) {
    GameObject additionalTombstone =
        Object.Instantiate(
            player.m_tombstone, player.GetCenterPoint() + Vector3.up * 1.25f, player.transform.rotation);

    additionalTombstone.transform.localScale -= new Vector3(0.5f, 0.5f, 0.5f);

    return additionalTombstone;
  }

  static void SetupAdditionalTombStone(TombStone tombStone) {
    PlayerProfile profile = Game.instance.GetPlayerProfile();
    ZDO tombStoneZDO = tombStone.m_nview.m_zdo;

    tombStoneZDO.Set(ZDOVars.s_owner, profile.GetPlayerID());
    tombStoneZDO.Set(ZDOVars.s_ownerName, profile.GetName());
    tombStoneZDO.Set(IsAdditionalTombStoneHash, true);
  }

  public static bool IsAdditionalTombStone(TombStone tombStone) {
    return tombStone.m_nview.m_zdo.GetBool(IsAdditionalTombStoneHash);
  }
}
