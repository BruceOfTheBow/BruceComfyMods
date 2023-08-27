using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComfyQuickSlots {
    public class InventoryLogger {
        static readonly string InventoryCsvHeaderRow =
            string.Join(
                ",",
                "name",
                "crafterId",
                "crafterName",
                "gridpos.x",
                "gridpos.y",
                "quality",
                "stack",
                "variant"
                );
        public static void LogInventoryToFile(Inventory inventory, string filename) {
            ComfyQuickSlots.log($"Logging {inventory.m_inventory.Count} inventory items to file: {filename}");
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
                item.m_variant
                );
        }

        private static string EscapeCsvField(string ValueToEscape) {
            if (ValueToEscape.Contains(",")) {
                return "\"" + ValueToEscape + "\"";
            } else {
                return ValueToEscape;
            }
        }
    }
}
