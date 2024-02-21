//This file is courtesy of the creator of the original Valheim Inventory Slots mod :)
using System.Collections.Generic;
using System.IO;

using HarmonyLib;

using UnityEngine;

using static ComfyQuickSlots.PluginConfig;
using static ComfyQuickSlots.ComfyQuickSlots;

namespace ComfyQuickSlots {
  [HarmonyPatch(typeof(Player))]
  static class TombstonePatcher {
    [HarmonyPrefix]
    [HarmonyPatch(nameof(Player.CreateTombStone))]
    static void CreateTombStonePrefix(Player __instance) {
      // Log Items in tombstone on death for auditing and tracking purposes
      Directory.CreateDirectory(LogFilesPath.Value);
      string filename = __instance.GetPlayerID() + ".csv";
      InventoryLogger.LogInventoryToFile(__instance.GetInventory(), Path.Combine(LogFilesPath.Value, filename));

      Player that = __instance;
      UnequipAllArmor(__instance);

      GameObject additionalTombstone = CreateAdditionalTombstone(that);
      SetupAdditionalTombstone(additionalTombstone.GetComponent<TombStone>());

      Container graveContainer = additionalTombstone.GetComponent<Container>();
      Inventory graveInventory = graveContainer.GetInventory();

      that.UnequipAllItems();

      Inventory playerInventory = that.GetInventory();
      List<ItemDrop.ItemData> playerItems = new(playerInventory.GetAllItems());

      foreach (var item in playerItems) {
        if (item.m_gridPos.y >= 4 && !item.m_equipped) {
          graveInventory.AddItem(item);
          playerInventory.RemoveItem(item);
        }
      }

      __instance.GetInventory().m_height = 4;
      __instance.GetInventory().m_width = 8;
    }

    static GameObject CreateAdditionalTombstone(Player player) {
      GameObject additionalTombstone =
          UnityEngine.Object.Instantiate(
              player.m_tombstone, player.GetCenterPoint() + Vector3.up * 1.25f, player.transform.rotation);

      additionalTombstone.transform.localScale -= new Vector3(0.5f, 0.5f, 0.5f);

      return additionalTombstone;
    }

    static void SetupAdditionalTombstone(TombStone tombstone) {
      PlayerProfile profile = Game.instance.GetPlayerProfile();
      tombstone.m_nview.m_zdo.Set(ZDOVars.s_owner, profile.GetPlayerID());
      tombstone.m_nview.m_zdo.Set(ZDOVars.s_ownerName, profile.GetName());
      tombstone.m_nview.m_zdo.Set(IsAdditionalTombstoneField, true);
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(Player.CreateTombStone))]
    static void CreateTombStonePostfix(Player __instance) {
      __instance.GetInventory().m_height = rows;
      __instance.GetInventory().m_width = columns;
    }


  }
}
