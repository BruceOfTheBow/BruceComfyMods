namespace ComfyGizmo;

using System.Collections.Generic;

using HarmonyLib;

// ComfyGizmo localization. Tokens are registered through Valheim's own Localization
// system so everything is translatable like any other addon. The game loads its
// languages before BepInEx plugins, so we register lazily (and re-register on any
// SetupLanguage call) instead of relying on a one-shot postfix. English is the
// fallback; existing translations from the game or other mods are never overwritten.
public static class GizmoLocalization {
  public const string ChangeAxisHintToken = "comfygizmo_keyhint_changeaxis";
  public const string ResetHintToken = "comfygizmo_keyhint_reset";
  public const string HoldHintToken = "comfygizmo_keyhint_hold";
  public const string PitchMessageToken = "comfygizmo_message_pitch";
  public const string RollMessageToken = "comfygizmo_message_roll";
  public const string ResetMessageToken = "comfygizmo_message_reset";

  const string DefaultLanguage = "English";

  static readonly Dictionary<string, Dictionary<string, string>> Translations = new() {
    [ChangeAxisHintToken] = new() {
      [DefaultLanguage] = "Change axis",
      ["French"] = "Changer d'axe",
    },
    [ResetHintToken] = new() {
      [DefaultLanguage] = "Reset orientation",
      ["French"] = "Réinitialiser l'orientation",
    },
    [HoldHintToken] = new() {
      [DefaultLanguage] = "Hold",
      ["French"] = "Maintenir",
    },
    [PitchMessageToken] = new() {
      [DefaultLanguage] = "Gizmo: X axis (up/down)",
      ["French"] = "Gizmo : axe X (haut/bas)",
    },
    [RollMessageToken] = new() {
      [DefaultLanguage] = "Gizmo: Z axis (up/down)",
      ["French"] = "Gizmo : axe Z (haut/bas)",
    },
    [ResetMessageToken] = new() {
      [DefaultLanguage] = "Gizmo rotation reset",
      ["French"] = "Rotation du gizmo réinitialisée",
    },
  };

  static string _registeredLanguage;

  // Resolves a token to localized text. Never throws: if Valheim's localization is
  // unavailable or rejects our registration at runtime, we fall back to our own
  // table so hints and messages always show readable text.
  public static string Translate(string token) {
    try {
      EnsureRegistered();

      if (Localization.instance != null) {
        string localized = Localization.instance.Localize($"${token}");

        // A missing token comes back as "$token" or "[token]"; use our table instead.
        if (!string.IsNullOrEmpty(localized) && localized[0] != '$' && localized[0] != '[') {
          return localized;
        }
      }
    } catch {
      // Fall through to the built-in table.
    }

    return Fallback(token);
  }

  static string Fallback(string token) {
    string language = DefaultLanguage;

    try {
      string selected = Localization.instance?.GetSelectedLanguage();

      if (!string.IsNullOrEmpty(selected)) {
        language = selected;
      }
    } catch {
      // Use the default language.
    }

    if (Translations.TryGetValue(token, out Dictionary<string, string> byLanguage)) {
      if (byLanguage.TryGetValue(language, out string word)) {
        return word;
      }

      if (byLanguage.TryGetValue(DefaultLanguage, out string fallback)) {
        return fallback;
      }
    }

    return token;
  }

  static void EnsureRegistered() {
    Localization localization = Localization.instance;

    if (localization == null) {
      return;
    }

    string language = localization.GetSelectedLanguage();

    if (string.IsNullOrEmpty(language) || language == _registeredLanguage) {
      return;
    }

    // Mark done up front so a failing AddWord can't retry every frame.
    _registeredLanguage = language;

    foreach (KeyValuePair<string, Dictionary<string, string>> entry in Translations) {
      try {
        // Let the game or translation mods win if they already define this token.
        if (localization.m_translations.ContainsKey(entry.Key)) {
          continue;
        }

        if (!entry.Value.TryGetValue(language, out string word)) {
          word = entry.Value[DefaultLanguage];
        }

        localization.AddWord(entry.Key, word);
      } catch {
        // AddWord may be inaccessible at runtime; Translate falls back to our table.
      }
    }
  }

  // A language (re)load rebuilds the translation table, so force re-registration.
  [HarmonyPatch(typeof(Localization))]
  static class LocalizationPatch {
    [HarmonyPostfix]
    [HarmonyPatch(nameof(Localization.SetupLanguage))]
    static void SetupLanguagePostfix() {
      _registeredLanguage = null;
    }
  }
}
