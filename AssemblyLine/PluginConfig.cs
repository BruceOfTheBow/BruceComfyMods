using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx.Configuration;

using UnityEngine;

namespace AssemblyLine {
  public class PluginConfig {
    public static ConfigEntry<bool> IsModEnabled = default!;
    public static ConfigEntry<KeyboardShortcut> AmountChangeModifier { get; private set; }
    public static ConfigEntry<KeyboardShortcut> MaxAmountChangeModifier { get; private set; }

    public static ConfigEntry<bool> RoundToStackSize = default!;

    public static void BindConfig(ConfigFile config) {
      IsModEnabled = config.Bind("_Global", "isModEnabled", true, "Globally enable or disable this mod.");

      AmountChangeModifier =
          config.Bind(
              "Modifiers",
              "amountChangeModifier",
              new KeyboardShortcut(KeyCode.LeftShift),
              "Keyboard shortcut to increment or decrement craft amount by 10.");

      MaxAmountChangeModifier =
          config.Bind(
              "Modifiers",
              "maxAmountChangeModifier",
              new KeyboardShortcut(KeyCode.LeftControl),
              "Keyboard shortcut to increment or decrement craft amount to maximum or minimum.");

      RoundToStackSize = config.Bind("Toggles", "roundToStackSize", false, "Changes first increment with account change modifier from 1 to 11 to 1 to 10 so that even stacks may be made.");
    }
  }
}