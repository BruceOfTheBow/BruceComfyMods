using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WristWatch {
  public static class ConfigFileExtensions {
    public static void OnSettingChanged<T>(this ConfigEntry<T> configEntry, Action settingChangedHandler) {
      configEntry.SettingChanged += (_, _) => settingChangedHandler();
    }

    public static void OnSettingChanged<T>(this ConfigEntry<T> configEntry, Action<T> settingChangedHandler) {
      configEntry.SettingChanged +=
          (_, eventArgs) =>
              settingChangedHandler.Invoke((T)((SettingChangedEventArgs)eventArgs).ChangedSetting.BoxedValue);
    }

    public static void OnSettingChanged<T>(
        this ConfigEntry<T> configEntry, Action<ConfigEntry<T>> settingChangedHandler) {
      configEntry.SettingChanged +=
          (_, eventArgs) =>
              settingChangedHandler.Invoke(
                  (ConfigEntry<T>)((SettingChangedEventArgs)eventArgs).ChangedSetting.BoxedValue);
    }
  }
}
