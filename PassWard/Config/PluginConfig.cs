namespace PassWard;

using BepInEx.Configuration;

using ComfyLib;

using UnityEngine;

public static class PluginConfig {
  public static ConfigEntry<bool> IsModEnabled { get; private set; }

  public static ConfigEntry<KeyboardShortcut> EnterPasswordKey { get; private set; }
  public static ConfigEntry<KeyboardShortcut> RemovePasswordKey { get; private set; }

  public enum UserListSeperator {
    Newline,
    Comma
  }

  public static ConfigEntry<UserListSeperator> WardHoverTextUserListSeparator { get; private set; }

  public enum UserListSorting {
    Unsorted,
    Alphabetically,
    ReverseAlphabetically
  }

  public static ConfigEntry<UserListSorting> WardHoverTextUserListSorting { get; private set; }

  public static void BindConfig(ConfigFile config) {
    IsModEnabled =
        config.BindInOrder(
            "_Global",
            "isModEnabled",
            true,
            "Globally enable or disable this mod.");

    EnterPasswordKey =
        config.BindInOrder(
          "Hotkeys",
          "enterPasswordShortcut",
          new KeyboardShortcut(KeyCode.P, KeyCode.LeftShift),
          "Enter password into a passworded ward OR assign password to own ward.");

    RemovePasswordKey =
        config.BindInOrder(
          "Hotkeys",
          "removePasswordKey",
          new KeyboardShortcut(KeyCode.R, KeyCode.LeftShift),
          "Removes password on player's own passworded ward.");

    WardHoverTextUserListSeparator =
        config.BindInOrder(
            "HoverText",
            "userListSeparator",
            UserListSeperator.Newline,
            "Separator to use between player names for a ward's hover-text.");

    WardHoverTextUserListSorting =
        config.BindInOrder(
            "HoverText",
            "userListSorting",
            UserListSorting.Unsorted,
            "What sorting (if any) to use for player names for a ward's hover-text.");

    WardHoverTextUserListSorting.OnSettingChanged(WardManager.ClearCachedPermittedPlayerNames);
  }
}
