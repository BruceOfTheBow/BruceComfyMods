namespace ComfyQuickSlots;

using System.IO;

public static class InventoryLogger {
  public static readonly string InventoryCsvHeaderRow =
      string.Join(",", "name", "crafterId", "crafterName", "gridpos.x", "gridpos.y", "quality", "stack", "variant");

  public static void LogInventoryToFile(Inventory inventory, string filename) {
    using StreamWriter writer = File.CreateText(filename);
    writer.AutoFlush = true;
    writer.WriteLine(InventoryCsvHeaderRow);

    foreach (ItemDrop.ItemData item in inventory.m_inventory) {
      writer.WriteLine(ItemToCsvRow(item));
    }
  }

  static string ItemToCsvRow(ItemDrop.ItemData item) {
    return string.Join(
        ",",
        EscapeCsvField(item.m_shared.m_name),
        item.m_crafterID,
        EscapeCsvField(item.m_crafterName),
        item.m_gridPos.x,
        item.m_gridPos.y,
        item.m_quality,
        item.m_stack,
        item.m_variant);
  }

  static string EscapeCsvField(string valueToEscape) {
    if (valueToEscape.Contains(",")) {
      return "\"" + valueToEscape + "\"";
    } else {
      return valueToEscape;
    }
  }
}
