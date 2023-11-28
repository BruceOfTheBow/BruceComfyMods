using BepInEx.Configuration;
using System;

namespace HomieHeadcount {
  public static class ConfigExtensions {
    public static void OnSettingChanged<T>(this ConfigEntry<T> configEntry, Action settingChangedHandler) {
      configEntry.SettingChanged += (_, _) => settingChangedHandler();
    }
  }
}
