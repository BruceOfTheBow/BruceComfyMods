namespace ComfyBatchDeposit;

using BepInEx.Configuration;

using ComfyLib;

using UnityEngine;

public static class PluginConfig {
  public static ConfigEntry<bool> IsModEnabled { get; private set; }
  public static ConfigEntry<bool> SortOnDump { get; private set; }
  public static ConfigEntry<KeyboardShortcut> Modifier { get; private set; }

public static void BindConfig(ConfigFile config) {
    IsModEnabled = config.BindInOrder("_Global", "isModEnabled", true, "Globally enable or disable this mod.");
    SortOnDump = config.BindInOrder("Behavior", "sortOnDump", false, "Runs sort function immediately after dump function is completely when dump button is pressed.");
    Modifier = config.BindInOrder("DepositAllModifier", "depositAllModifier", new KeyboardShortcut(KeyCode.LeftAlt), "Hot key modifier for deposit all on left click.");
  }
}
