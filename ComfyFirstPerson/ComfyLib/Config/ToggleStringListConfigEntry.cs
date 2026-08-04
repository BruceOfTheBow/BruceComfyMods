namespace ComfyLib;

using System;
using System.Collections.Generic;

using BepInEx.Configuration;

using UnityEngine;

public sealed class ToggleStringListConfigEntry {
  public ConfigEntry<string> BaseConfigEntry { get; }
  public event EventHandler<string[]> SettingChanged;

  public ToggleStringListConfigEntry(
      ConfigFile config,
      string section,
      string key,
      string defaultValue,
      string description) {
    BaseConfigEntry = config.BindInOrder(section, key, defaultValue, description, Drawer);
    BaseConfigEntry.SettingChanged += OnBaseSettingChanged;
  }

  void OnBaseSettingChanged(object sender, EventArgs eventArgs) {
    SettingChanged?.Invoke(this, ToggledStringValues());
  }

  public static readonly char[] ValueSeparator = [','];
  public static readonly char[] ToggleSeperator = ['='];

  readonly List<string> _valuesCache = [];
  string _valueText = string.Empty;

  public string[] ToggledStringValues() {
    _valuesCache.Clear();
    string[] values = BaseConfigEntry.Value.Split(ValueSeparator, StringSplitOptions.RemoveEmptyEntries);

    for (int i = 0, count = values.Length; i < count; i++) {
      string[] parts = values[i].Split(ToggleSeperator, 2, StringSplitOptions.RemoveEmptyEntries);

      if (parts.Length >= 2 && parts[1] == "1") {
        _valuesCache.Add(parts[0]);
      }
    }

    return [.. _valuesCache];
  }

  public void Drawer(ConfigEntryBase configEntry) {
    GUILayout.BeginVertical(GUILayout.ExpandWidth(true));

    _valuesCache.Clear();

    _valuesCache.AddRange(
        configEntry.BoxedValue.ToString().Split(ValueSeparator, StringSplitOptions.RemoveEmptyEntries));

    GUILayout.BeginHorizontal();
    bool toggleOnClicked = GUILayout.Button("Toggle On", GUILayout.ExpandWidth(true));
    bool toggleOffClicked = GUILayout.Button("Toggle Off", GUILayout.ExpandWidth(true));
    GUILayout.EndHorizontal();

    bool hasChanged = false;
    int removeIndex = -1;

    for (int i = 0, count = _valuesCache.Count; i < count; i++) {
      string[] parts = _valuesCache[i].Split(ToggleSeperator, 2, StringSplitOptions.RemoveEmptyEntries);
      bool isToggled = parts.Length >= 2 && parts[1] == "1";

      GUILayout.BeginHorizontal();

      bool result = GUILayout.Toggle(isToggled, parts[0], GUILayout.ExpandWidth(true));

      if (GUILayout.Button("\u2212", GUILayout.MinWidth(40f), GUILayout.ExpandWidth(false))) {
        removeIndex = i;
      }

      GUILayout.EndHorizontal();

      if (toggleOnClicked) {
        result = true;
      } else if (toggleOffClicked) {
        result = false;
      }

      if (result != isToggled) {
        hasChanged = true;
        _valuesCache[i] = parts[0] + (result ? "=1" : "=0");
      }
    }

    GUILayout.BeginHorizontal();

    _valueText = GUILayout.TextField(_valueText, GUILayout.ExpandWidth(true));
    GUILayout.Space(3f);

    if (GUILayout.Button("\u002B", GUILayout.MinWidth(40f), GUILayout.ExpandWidth(false))
        && !string.IsNullOrWhiteSpace(_valueText)
        && _valueText.IndexOf('=') < 0) {
      _valuesCache.Add(_valueText + "=1");
      _valueText = string.Empty;
      hasChanged = true;
    }

    GUILayout.EndHorizontal();

    GUILayout.EndVertical();

    if (removeIndex >= 0) {
      _valuesCache.RemoveAt(removeIndex);
      hasChanged = true;
    }

    if (hasChanged) {
      configEntry.BoxedValue = string.Join(",", _valuesCache);
    }
  }
}
