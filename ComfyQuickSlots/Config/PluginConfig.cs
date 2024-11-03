namespace ComfyQuickSlots;

using ComfyLib;

using BepInEx.Configuration;

using UnityEngine;

public sealed class PluginConfig {
  public static ConfigEntry<bool> IsModEnabled { get; private set; }

  // QuickSlots Enable
  public static ConfigEntry<bool> EnableQuickslots { get; private set; }

  // QuickSlots
  public static ConfigEntry<KeyCode> QuickSlot1 { get; private set; }
  public static ConfigEntry<KeyCode> QuickSlot2 { get; private set; }
  public static ConfigEntry<KeyCode> QuickSlot3 { get; private set; }

  // QuickSlots HotKeyBar Positioning
  public static ConfigEntry<TextAnchor> QuickSlotsAnchor { get; private set; }
  public static ConfigEntry<Vector2> QuickSlotsPosition { get; private set; }

  // Container
  public static ConfigEntry<Vector2> ContainerInventoryGridAnchoredPosition { get; private set; }

  // Logging
  public static ConfigEntry<string> LogFilesPath { get; private set; }

  public static void BindConfig(ConfigFile config) {
    IsModEnabled =
        config.BindInOrder(
            "_Global",
            "isModEnabled",
            true,
            "Globally enable or disable this mod.");

    //QuickSlots Enable
    EnableQuickslots =
        config.BindInOrder(
            "QuickSlotToggles",
            "quickSlotEnable",
            true,
            "Enables or disables quickslots.");

    // QuickSlots
    QuickSlot1 =
        config.BindInOrder(
            "QuickSlotBinding",
            "quickSlot1Use",
            KeyCode.Z,
            "Hot key for item use in quick slot 1");

    QuickSlot1.OnSettingChanged(QuickSlotsManager.RefreshBindings);

    QuickSlot2 =
        config.BindInOrder(
            "QuickSlotBinding",
            "quickSlot2Use",
            KeyCode.V,
            "Hot key for item use in quick slot 2");

    QuickSlot2.OnSettingChanged(QuickSlotsManager.RefreshBindings);

    QuickSlot3 =
        config.BindInOrder(
            "QuickSlotBinding",
            "quickSlot3Use",
            KeyCode.B,
            "Hot key for item use in quick slot 3");

    QuickSlot3.OnSettingChanged(QuickSlotsManager.RefreshBindings);

    // QuickSlots HotKeyBar Positioning
    QuickSlotsAnchor =
        config.BindInOrder(
            "QuickSlotsAnchor",
            "quickSlotsAnchor",
            TextAnchor.LowerLeft,
            "The point on the HUD to anchor the Quick Slots bar.\n"
                + "Changing this also changes the pivot of the Quick Slots to that corner.");

    QuickSlotsPosition =
        config.BindInOrder(
            "QuickSlotsOffset",
            "quickSlotsOffset",
            new Vector2(216f, 150f),
            "The position offset from the Quick Slots Anchor at which to place the QuickSlots.");

    // Container
    ContainerInventoryGridAnchoredPosition =
        config.BindInOrder(
            "Container.InventoryGrid",
            "anchoredPosition",
            new Vector2(40f, -437f),
            "The anchoredPosition for the the Container.InventoryGrid panel.");

    // Logging
    LogFilesPath =
        config.BindInOrder(
            "Logging",
            "logFilesPath",
            "ItemsOnDeath/",
            "Path to where logging of items on death are saved.");
  }
}
