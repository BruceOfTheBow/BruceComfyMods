namespace PassWard;

using BepInEx.Configuration;

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

  public static void BindConfig(ConfigFile config) {
    IsModEnabled =
        config.Bind(
            "_Global",
            "isModEnabled",
            true,
            "Globally enable or disable this mod.");

    EnterPasswordKey =
        config.Bind(
          "Hotkeys",
          "enterPasswordShortcut",
          new KeyboardShortcut(KeyCode.P, KeyCode.LeftShift),
          "Enter password into a passworded ward OR assign password to own ward.");

    RemovePasswordKey =
        config.Bind(
          "Hotkeys",
          "removePasswordKey",
          new KeyboardShortcut(KeyCode.R, KeyCode.LeftShift),
          "Removes password on player's own passworded ward.");

    WardHoverTextUserListSeparator =
        config.Bind(
            "HoverText",
            "userListSeparator",
            UserListSeperator.Newline,
            "Separator to use between player names for a ward's hover-text.");
  }
}
