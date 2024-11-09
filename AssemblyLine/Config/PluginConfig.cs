namespace AssemblyLine;

using BepInEx.Configuration;

using ComfyLib;

using UnityEngine;

public static class PluginConfig {
  public static ConfigEntry<KeyboardShortcut> AmountChangeModifier { get; private set; }
  public static ConfigEntry<KeyboardShortcut> MaxAmountChangeModifier { get; private set; }

  public static ConfigEntry<bool> RoundToStackSize { get; private set; }

  public static void BindConfig(ConfigFile config) {
    AmountChangeModifier =
        config.BindInOrder(
            "Modifiers",
            "amountChangeModifier",
            new KeyboardShortcut(KeyCode.LeftShift),
            "Keyboard shortcut to increment or decrement craft amount by 10.");

    MaxAmountChangeModifier =
        config.BindInOrder(
            "Modifiers",
            "maxAmountChangeModifier",
            new KeyboardShortcut(KeyCode.LeftControl),
            "Keyboard shortcut to increment or decrement craft amount to maximum or minimum.");

    RoundToStackSize =
        config.BindInOrder(
            "Toggles",
            "roundToStackSize",
            false,
            "Changes first increment with account change modifier from 1 to 11 to 1 to 10 to make even stacks.");
  }
}
