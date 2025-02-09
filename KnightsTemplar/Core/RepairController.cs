namespace KnightsTemplar;

public static class RepairController {
  public static readonly string BlackForge = "$piece_blackforge";

  public static bool CanRepairItem(Player player, ItemDrop.ItemData item) {
    if (!player || item == null || !IsKnightItem(item.m_shared.m_name)) {
      return false;
    }

    if (player.NoCostCheat()) {
      return true;
    }

    CraftingStation craftingStation = player.GetCurrentCraftingStation();

    if (!craftingStation || craftingStation.m_name != BlackForge || craftingStation.GetLevel() < 2) {
      return false;
    }

    return true;
  }

  public static readonly string ShieldKnight = "$item_shield_knight";
  public static readonly string SwordIronFire = "$item_sword_fire";

  public static bool IsKnightItem(string itemName) {
    return itemName == ShieldKnight || itemName == SwordIronFire;
  }
}
