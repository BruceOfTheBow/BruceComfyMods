namespace AddAllFuel;

using UnityEngine;

using BepInEx.Configuration;

using ComfyLib;

public static class PluginConfig {
  public static ConfigEntry<bool> IsModEnabled { get; private set; }
  public static ConfigEntry<KeyCode> AddAllModifier { get; private set; }

  public static ConfigEntry<bool> ExcludeFinewood { get; private set; }
  public static ConfigEntry<string> SmelterCookableItemsToExclude { get; private set; }

  public static void BindConfig(ConfigFile config) {
    IsModEnabled =
        config.BindInOrder(
            "_Global",
            "isModEnabled",
            true,
            "Globally enable or disable this mod.");

    AddAllModifier =
        config.BindInOrder(
            "ModifierKey",
            "ModifierKey",
            KeyCode.LeftShift,
            "Modifier key to hold for using add all feature.");

    SmelterCookableItemsToExclude =
        config.BindInOrder(
            "Smelter",
            "cookableItemsToExclude",
            "$item_finewood",
            "Comma-separated list of item-names that Smelter stations will not use as cookable items.");

    SmelterCookableItemsToExclude.OnSettingChanged(OnSmelterCookableItemsToExcludeChanged);
    OnSmelterCookableItemsToExcludeChanged(SmelterCookableItemsToExclude.Value);
  }

  public static readonly char[] CommaSplitter = [','];  

  static void OnSmelterCookableItemsToExcludeChanged(string values) {
    string[] itemNames = values.Split(CommaSplitter, System.StringSplitOptions.RemoveEmptyEntries);
    SmelterManager.SetExcludeCookableItems(itemNames);
  }
}
