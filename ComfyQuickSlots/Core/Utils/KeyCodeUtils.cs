namespace ComfyQuickSlots;

using System.Collections.Generic;
using System.Linq;

using BepInEx.Configuration;

using UnityEngine;

public static class KeyCodeUtils {
  public static readonly Dictionary<KeyCode, string> KeyCodeToShortText = new() {
    //Lower Case Letters
    {KeyCode.A, "A"},
    {KeyCode.B, "B"},
    {KeyCode.C, "C"},
    {KeyCode.D, "D"},
    {KeyCode.E, "E"},
    {KeyCode.F, "F"},
    {KeyCode.G, "G"},
    {KeyCode.H, "H"},
    {KeyCode.I, "I"},
    {KeyCode.J, "J"},
    {KeyCode.K, "K"},
    {KeyCode.L, "L"},
    {KeyCode.M, "M"},
    {KeyCode.N, "N"},
    {KeyCode.O, "O"},
    {KeyCode.P, "P"},
    {KeyCode.Q, "Q"},
    {KeyCode.R, "R"},
    {KeyCode.S, "S"},
    {KeyCode.T, "T"},
    {KeyCode.U, "U"},
    {KeyCode.V, "V"},
    {KeyCode.W, "W"},
    {KeyCode.X, "X"},
    {KeyCode.Y, "Y"},
    {KeyCode.Z, "Z"},

    //KeyPad Numbers
    {KeyCode.Keypad1, "kp1"},
    {KeyCode.Keypad2, "kp2"},
    {KeyCode.Keypad3, "kp3"},
    {KeyCode.Keypad4, "kp4"},
    {KeyCode.Keypad5, "kp5"},
    {KeyCode.Keypad6, "kp6"},
    {KeyCode.Keypad7, "kp7"},
    {KeyCode.Keypad8, "kp8"},
    {KeyCode.Keypad9, "kp9"},
    {KeyCode.Keypad0, "kp10"},

    //Other Symbols
    {KeyCode.Exclaim, "!"},
    {KeyCode.DoubleQuote, "\""},
    {KeyCode.Hash, "#"},
    {KeyCode.Dollar, "$"},
    {KeyCode.Ampersand, "&"},
    {KeyCode.Quote, "\'"},
    {KeyCode.LeftParen, "("},
    {KeyCode.RightParen, ")"},
    {KeyCode.Asterisk, "*"},
    {KeyCode.Plus, "+"},
    {KeyCode.Comma, ","},
    {KeyCode.Minus, "-"},
    {KeyCode.Period, "."},
    {KeyCode.Slash, "/"},
    {KeyCode.Colon, ":"},
    {KeyCode.Semicolon, ";"},
    {KeyCode.Less, "<"},
    {KeyCode.Equals, "="},
    {KeyCode.Greater, ">"},
    {KeyCode.Question, "?"},
    {KeyCode.At, "@"},
    {KeyCode.LeftBracket, "["},
    {KeyCode.Backslash, "\\"},
    {KeyCode.RightBracket, "]"},
    {KeyCode.Caret, "^"},
    {KeyCode.Underscore, "_"},
    {KeyCode.BackQuote, "`"},

    {KeyCode.Alpha1, "1"},
    {KeyCode.Alpha2, "2"},
    {KeyCode.Alpha3, "3"},
    {KeyCode.Alpha4, "4"},
    {KeyCode.Alpha5, "5"},
    {KeyCode.Alpha6, "6"},
    {KeyCode.Alpha7, "7"},
    {KeyCode.Alpha8, "8"},
    {KeyCode.Alpha9, "9"},
    {KeyCode.Alpha0, "0"},

    {KeyCode.KeypadPeriod, "kp ."},
    {KeyCode.KeypadDivide, "kp //"},
    {KeyCode.KeypadMultiply, "kp *"},
    {KeyCode.KeypadMinus, "kp -"},
    {KeyCode.KeypadPlus, "kp +"},
    {KeyCode.KeypadEquals, "kp ="}
  };

  public static string ToShortString(this KeyCode keyCode) {
    return KeyCodeToShortText.ContainsKey(keyCode) ? KeyCodeToShortText[keyCode] : keyCode.ToString();
  }

  public static string ToShortString(this KeyboardShortcut keyboardShortcut) {
    return string.Join(
        " + ",
        keyboardShortcut.Modifiers
            .Concat(Enumerable.Repeat(element: keyboardShortcut.MainKey, count: 1))
            .Select(ToShortString));
  }
}
