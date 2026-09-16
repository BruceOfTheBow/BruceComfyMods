namespace WeightlessCoins;

using static PluginConfig;
using static Version;

public sealed class WeightManager {
  public static int CoinsItemHashCode = "$item_coins".GetStableHashCode();

  public static bool IsCoins(ItemDrop.ItemData itemData) {
    if (itemData == null) {
      return false;
    }

    return itemData.m_shared.m_name.GetStableHashCode() == CoinsItemHashCode;
  }

  public static void SetWeight(ItemDrop.ItemData itemData) {
    itemData.m_shared.m_weight *= CoinWeightScale.Value;
  }

  public static void UpdateCoinsWeight(Inventory inventory) {
    if (inventory == null || StoreGui.instance == null) {
      return;
    }
    
    foreach (ItemDrop.ItemData itemData in inventory.m_inventory) {
      if (!IsCoins(itemData)) {
        continue;
      }

      itemData.m_shared.m_weight *= CoinWeightScale.Value;
    }

    inventory.UpdateTotalWeight();
  }
}
