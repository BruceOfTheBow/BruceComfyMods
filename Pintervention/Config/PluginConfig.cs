namespace Pintervention;

using BepInEx.Configuration;

using ComfyLib;

using UnityEngine;

public static class PluginConfig {
  public static ConfigEntry<bool> IsModEnabled { get; private set; }
  public static ConfigEntry<bool> ReadPinsOnInteract { get; private set; }
  public static ConfigEntry<bool> ReadRevealedMapOnInteract { get; private set; }
  public static ConfigEntry<bool> WritePinsOnInteract { get; private set; }
  public static ConfigEntry<bool> WriteRevealedMapOnInteract { get; private set; }
  public static ConfigEntry<KeyboardShortcut> DisplayFilterPanel { get; private set; }
  public static ConfigEntry<Vector2> PlayerPinFilterSizeDelta { get; private set; }
  public static ConfigEntry<Vector2> PlayerPinFilterPosition { get; private set; }

  public static void BindConfig(ConfigFile config) {
    IsModEnabled =
        config.BindInOrder(
            "_Global",
            "isModEnabled",
            true,
            "Globally enable or disable this mod.");

    ReadPinsOnInteract = 
        config.BindInOrder(
            "CartographyTable", 
            "readPinsOnInteract", 
            true, 
            "Allows not taking pins when reading from cartography table.");

    ReadRevealedMapOnInteract =
        config.BindInOrder(
            "CartographyTable",
            "readRevealedMapOnInteract",
            true,
            "Allows not taking shared map data when reading from cartography table.");

    WritePinsOnInteract =
        config.BindInOrder(
            "CartographyTable",
            "writePinsOnInteract",
            true,
            "Allows not writing pins when writing to cartography table.");

    WriteRevealedMapOnInteract =
        config.BindInOrder(
            "CartographyTable",
            "writeRevealedMapOnInteract",
            true,
            "Allows not writing shared map data when writing to cartography table.");

    DisplayFilterPanel =
        config.BindInOrder(
            "Hotkeys",
            "displayFilterPanelShortcut",
            new KeyboardShortcut(KeyCode.F, KeyCode.LeftControl),
            "Keyboard shortcut for the displaying filter panel with minimap open.");

    PlayerPinFilterSizeDelta =
      config.BindInOrder(
          "PlayerPinFilter.Panel",
          "playerListPanelSizeDelta",
          new Vector2(400f, 400f),
          "The value for the PlayerPinFilter.Panel sizeDelta (width/height in pixels).");

    PlayerPinFilterPosition =
        config.BindInOrder(
            "PlayerPinFilter.Panel",
            "pinFilterPanelPanelPosition",
            new Vector2(-25f, 0f),
            "The value for the PlayerPinFilter.Panel position (relative to pivot/anchors).");
  }
}
