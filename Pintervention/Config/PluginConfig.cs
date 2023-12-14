using BepInEx.Configuration;
using UnityEngine;

namespace Pintervention {
  public class PluginConfig {
    public static ConfigEntry<bool> IsModEnabled { get; private set; }
    public static ConfigEntry<KeyboardShortcut> DisplayFilterPanel { get; private set; }
    public static ConfigEntry<Vector2> PlayerPinFilterSizeDelta { get; private set; }
    public static ConfigEntry<Vector2> PlayerPinFilterPosition { get; private set; }

    public static void BindConfig(ConfigFile config) {
      IsModEnabled = config.Bind("_Global", "isModEnabled", true, "Globally enable or disable this mod.");

      DisplayFilterPanel =
          config.Bind(
              "Hotkeys",
              "displayFilterPanelShortcut",
              new KeyboardShortcut(KeyCode.F, KeyCode.LeftControl),
              "Keyboard shortcut for the displaying filter panel with minimap open.");

      PlayerPinFilterSizeDelta =
        config.Bind(
            "PlayerPinFilter.Panel",
            "playerListPanelSizeDelta",
            new Vector2(400f, 400f),
            "The value for the PlayerPinFilter.Panel sizeDelta (width/height in pixels).");

      PlayerPinFilterPosition =
          config.Bind(
              "PlayerPinFilter.Panel",
              "pinFilterPanelPanelPosition",
              new Vector2(-25f, 0f),
              "The value for the PlayerPinFilter.Panel position (relative to pivot/anchors).");
    }
  }
}