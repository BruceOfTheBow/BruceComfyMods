using BepInEx;
using BepInEx.Logging;

using HarmonyLib;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using static ComfyBatchDeposit.PluginConfig;
namespace ComfyBatchDeposit {
  [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
  public class ComfyBatchDeposit : BaseUnityPlugin {
    public const string PluginGuid = "com.bruce.valheim.comfybatchdeposit";
    public const string PluginName = "ComfyBatchDeposit";
    public const string PluginVersion = "1.1.0";

    static ManualLogSource _logger;

    Harmony _harmony;

    public void Awake() {
      _harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), harmonyInstanceId: PluginGuid);
      _logger = Logger;
      BindConfig(Config);
    }

    public void OnDestroy() {
      _harmony?.UnpatchSelf();
    }

    public static void SortInventory(Inventory inventory, bool isPlayerInventory) {
      Player player = Player.m_localPlayer;
      List<ItemDrop.ItemData> items = new List<ItemDrop.ItemData>(inventory.GetAllItems());
      List<ItemDrop.ItemData> compared = new List<ItemDrop.ItemData>();
      List<string> sorted = new List<string>();

      foreach (ItemDrop.ItemData itemData in items) {
        if (sorted.Contains(itemData.m_shared.m_name + itemData.m_quality.ToString())) {
          continue;
        }
        // To support sorting of fish quality must be checked as unique. For all other stackable items this should leave the check unaffected.
        sorted.Add(itemData.m_shared.m_name + itemData.m_quality.ToString());

        List<ItemDrop.ItemData> targetItems = items
            .FindAll(data => data.m_shared.m_name == itemData.m_shared.m_name && data.m_quality == itemData.m_quality)
            .FindAll(data => !data.m_shared.m_questItem && !player.IsItemEquiped(data) && !(isPlayerInventory && data.m_gridPos.y == 0));

        foreach (ItemDrop.ItemData item in targetItems) {
          ZLog.Log($"{itemData.m_shared.m_name} of quality {itemData.m_quality} will be stacked.");
          inventory.RemoveItem(item);
        }

        if (itemData.m_shared.m_maxStackSize > 1) {
          int amount = targetItems.ConvertAll(data => data.m_stack).Sum(); // Total Amount of the item
          int stacks = amount / itemData.m_shared.m_maxStackSize; // number of stacks required
          amount -= stacks * itemData.m_shared.m_maxStackSize; // overflow amount on last stack

          if (stacks > 0) {
            for (int i = 0; i < stacks; i++) {
              ItemDrop.ItemData maxStack = itemData.Clone();
              maxStack.m_stack = itemData.m_shared.m_maxStackSize;
              compared.Add(maxStack);
            }
          }

          if (amount > 0) {
            var surplus = itemData.Clone();
            surplus.m_stack = amount;
            compared.Add(surplus);
          }
        } else {
          compared.AddRange(targetItems);
        }
      }

      compared.Sort((firstData, secondData) => string.Compare(firstData.m_shared.m_name, secondData.m_shared.m_name, StringComparison.Ordinal));

      foreach (ItemDrop.ItemData item in compared) {
        inventory.AddItem(item);
      }

      inventory.Changed();
    }
  }
}