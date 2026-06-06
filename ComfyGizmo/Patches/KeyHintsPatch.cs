namespace ComfyGizmo;

using System;
using System.Collections.Generic;

using HarmonyLib;

using TMPro;

using UnityEngine;

using static PluginConfig;

// Adds ComfyGizmo control hints to Valheim's build key-hint bar. Valheim keeps
// separate keyboard and gamepad hint groups (only one shows at a time):
//   * Gamepad hints are a single TextMeshProUGUI each (button glyph inline).
//   * Keyboard hints are a row of [label Text] + [key_bkg Image -> Key text],
//     where key_bkg is the framed key box.
// We clone the matching vanilla element for each so our hints look native, and the
// game's own group toggling makes them adapt to the active input device.
[HarmonyPatch(typeof(KeyHints))]
static class KeyHintsPatch {
  static readonly List<TextMeshProUGUI> _gamepadRows = [];

  static GameObject _keyboardRow;
  static TextMeshProUGUI _keyboardLabel;
  static TextMeshProUGUI _keyboardKey;

  static bool _loggedError;

  [HarmonyPostfix]
  [HarmonyPatch(nameof(KeyHints.UpdateHints))]
  static void UpdateHintsPostfix(KeyHints __instance) {
    try {
      if (!ShowKeyHints.Value) {
        SetGamepadRowsActive(active: false);

        if (_keyboardRow != null && _keyboardRow.activeSelf) {
          _keyboardRow.SetActive(false);
        }

        return;
      }

      // Our rows live inside the build hint groups, so they hide with m_buildHints.
      if (__instance.m_buildHints == null
          || !__instance.m_buildHints.activeSelf
          || __instance.m_buildRotateKey == null) {
        return;
      }

      EnsureRows(__instance);
      RefreshText();
    } catch (Exception ex) {
      if (!_loggedError) {
        _loggedError = true;
        Debug.LogWarning($"ComfyGizmo: failed to set up key hints: {ex}");
      }
    }
  }

  static void EnsureRows(KeyHints keyHints) {
    // Recreate if never built or if the previous rows were destroyed (scene reload).
    if (_gamepadRows.Count > 0 && _gamepadRows[0] != null) {
      return;
    }

    _gamepadRows.Clear();
    _keyboardRow = null;
    _keyboardLabel = null;
    _keyboardKey = null;

    Transform buildHints = keyHints.m_buildHints.transform;
    TextMeshProUGUI gamepadTemplate = keyHints.m_buildRotateKey;
    Transform gamepadGroup = FindDirectChildContaining(buildHints, gamepadTemplate.transform);
    Transform keyboardGroup = FindOtherHintGroup(buildHints, gamepadGroup);

    // Gamepad: change-axis toggle + reset, cloned from a single-text glyph hint.
    Transform gamepadRowParent = gamepadTemplate.transform.parent;
    CreateGamepadRow(gamepadTemplate, gamepadRowParent);
    CreateGamepadRow(gamepadTemplate, gamepadRowParent);

    // Keyboard: reset, cloned from a full framed row (label + key box).
    Transform keyboardRowTemplate = keyboardGroup != null ? FindKeyboardRowTemplate(keyboardGroup) : null;

    if (keyboardRowTemplate != null) {
      CreateKeyboardRow(keyboardRowTemplate, keyboardGroup);
    }
  }

  static void CreateGamepadRow(TextMeshProUGUI template, Transform parent) {
    if (template == null || parent == null) {
      return;
    }

    GameObject row = UnityEngine.Object.Instantiate(template.gameObject, parent, false);
    row.name = "ComfyGizmoKeyHint";
    StripLocalizers(row);

    if (row.TryGetComponent(out TextMeshProUGUI text)) {
      _gamepadRows.Add(text);
    }
  }

  static void CreateKeyboardRow(Transform template, Transform parent) {
    GameObject row = UnityEngine.Object.Instantiate(template.gameObject, parent, false);
    row.name = "ComfyGizmoKeyHint";
    StripLocalizers(row);

    // A keyboard row has one direct-child label and one nested key (inside key_bkg).
    foreach (TextMeshProUGUI text in row.GetComponentsInChildren<TextMeshProUGUI>(includeInactive: true)) {
      if (text.transform.parent == row.transform) {
        _keyboardLabel = text;
      } else {
        _keyboardKey = text;
      }
    }

    _keyboardRow = row;
  }

  // A clean keyboard hint row: exactly one label + one framed key (skips multi-key
  // rows like Snap/Copy and the icon-based rotate row).
  static Transform FindKeyboardRowTemplate(Transform keyboardGroup) {
    for (int i = 0; i < keyboardGroup.childCount; i++) {
      Transform row = keyboardGroup.GetChild(i);
      TextMeshProUGUI[] texts = row.GetComponentsInChildren<TextMeshProUGUI>(includeInactive: true);

      if (texts.Length != 2) {
        continue;
      }

      bool hasLabel = false;
      bool hasKey = false;

      foreach (TextMeshProUGUI text in texts) {
        if (text.transform.parent == row) {
          hasLabel = true;
        } else {
          hasKey = true;
        }
      }

      if (hasLabel && hasKey) {
        return row;
      }
    }

    return null;
  }

  static void StripLocalizers(GameObject root) {
    // We localize and set the text ourselves; drop vanilla localizers so they can't overwrite us.
    foreach (Localize localize in root.GetComponentsInChildren<Localize>(includeInactive: true)) {
      UnityEngine.Object.Destroy(localize);
    }
  }

  static void RefreshText() {
    string glyph = JoyGlyph(JoystickGizmoButton.Value);
    string hold = GizmoLocalization.Translate(GizmoLocalization.HoldHintToken);

    if (_gamepadRows.Count > 0) {
      SetText(_gamepadRows[0], $"{GizmoLocalization.Translate(GizmoLocalization.ChangeAxisHintToken)}  {glyph}");
    }

    if (_gamepadRows.Count > 1) {
      SetText(_gamepadRows[1], $"{GizmoLocalization.Translate(GizmoLocalization.ResetHintToken)}  {hold} {glyph}");
    }

    if (_keyboardLabel != null) {
      SetText(_keyboardLabel, GizmoLocalization.Translate(GizmoLocalization.ResetHintToken));
    }

    if (_keyboardKey != null) {
      SetText(_keyboardKey, KeyboardResetKey());
    }

    if (_keyboardRow != null && !_keyboardRow.activeSelf) {
      _keyboardRow.SetActive(true);
    }
  }

  static void SetText(TextMeshProUGUI element, string text) {
    if (element == null) {
      return;
    }

    if (element.text != text) {
      element.text = text;
    }

    if (!element.gameObject.activeSelf) {
      element.gameObject.SetActive(true);
    }
  }

  static void SetGamepadRowsActive(bool active) {
    foreach (TextMeshProUGUI row in _gamepadRows) {
      if (row != null && row.gameObject.activeSelf != active) {
        row.gameObject.SetActive(active);
      }
    }
  }

  static string KeyboardResetKey() {
    KeyCode key = ResetRotationKey.Value.MainKey;

    if (key == KeyCode.None) {
      key = ResetAllRotationKey.Value.MainKey;
    }

    return key == KeyCode.None ? "-" : key.ToString();
  }

  // Native key/glyph for a gamepad button: GetBoundKeyString returns a controller
  // sprite (Xbox/PlayStation aware), falling back to a readable label if missing.
  static string JoyGlyph(string buttonName) {
    try {
      if (!string.IsNullOrWhiteSpace(buttonName) && ZInput.instance != null) {
        string glyph = ZInput.instance.GetBoundKeyString(buttonName, emptyStringOnMissing: true);

        if (!string.IsNullOrEmpty(glyph)) {
          return glyph;
        }
      }
    } catch {
      // Fall back to a readable label.
    }

    return FriendlyJoyName(buttonName);
  }

  static string FriendlyJoyName(string joyButton) {
    return joyButton switch {
      "JoyButtonA" => "A",
      "JoyButtonB" => "B",
      "JoyButtonX" => "X",
      "JoyButtonY" => "Y",
      "JoyRStick" => "R3",
      "JoyLStick" => "L3",
      "JoyLBumper" => "LB",
      "JoyRBumper" => "RB",
      "JoyLTrigger" => "LT",
      "JoyRTrigger" => "RT",
      _ => string.IsNullOrEmpty(joyButton) ? "-" : joyButton.Replace("Joy", ""),
    };
  }

  static Transform FindDirectChildContaining(Transform root, Transform descendant) {
    for (int i = 0; i < root.childCount; i++) {
      Transform child = root.GetChild(i);

      if (descendant.IsChildOf(child)) {
        return child;
      }
    }

    return null;
  }

  // The keyboard hint group is the other direct child of m_buildHints that holds text.
  static Transform FindOtherHintGroup(Transform root, Transform gamepadGroup) {
    Transform fallback = null;

    for (int i = 0; i < root.childCount; i++) {
      Transform child = root.GetChild(i);

      if (child == gamepadGroup || child.GetComponentInChildren<TextMeshProUGUI>(includeInactive: true) == null) {
        continue;
      }

      string name = child.name.ToLowerInvariant();

      if (name.Contains("key") || name.Contains("board") || name.Contains("mouse")) {
        return child;
      }

      fallback ??= child;
    }

    return fallback;
  }
}
