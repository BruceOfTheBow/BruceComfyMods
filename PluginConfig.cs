using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx.Configuration;

using UnityEngine;

namespace ComfyQuickSlots {
    public class PluginConfig {
        public static ConfigEntry<bool> IsModEnabled = default!;

        public static ConfigEntry<bool> EnableQuickslots = default!;

        // QuickSlots
        public static ConfigEntry<KeyCode> QuickSlot1 = default!;
        public static ConfigEntry<KeyCode> QuickSlot2 = default!;
        public static ConfigEntry<KeyCode> QuickSlot3 = default!;

        // Quick Slots Hotkeybar Positioning
        public static ConfigEntry<TextAnchor> QuickSlotsAnchor;
        public static ConfigEntry<Vector2> QuickSlotsPosition;

        // Logging
        public static ConfigEntry<string> LogFilesPath = default!;


        public static void BindConfig(ConfigFile config) {
            IsModEnabled = config.Bind("_Global", "isModEnabled", true, "Globally enable or disable this mod.");

            // QuickSlots
            QuickSlot1 = config.Bind("QuickSlotBinding", "quickSlot1Use", KeyCode.Z, "Hot key for item use in quick slot 1");
            QuickSlot2 = config.Bind("QuickSlotBinding", "quickSlot2Use", KeyCode.V, "Hot key for item use in quick slot 2");
            QuickSlot3 = config.Bind("QuickSlotBinding", "quickSlot3Use", KeyCode.B, "Hot key for item use in quick slot 3");

            //Quick Slot Enable
            EnableQuickslots= config.Bind("QuickSlotToggles", "quickSlotEnable", true, "Enables or disables quickslots.");

            // Quick Slots Hotkeybar Positioning
            QuickSlotsAnchor = config.Bind("QuickSlotsAnchor", "quickSlotsAnchor", TextAnchor.LowerLeft, "The point on the HUD to anchor the Quick Slots bar. Changing this also changes the pivot of the Quick Slots to that corner.");
            QuickSlotsPosition = config.Bind("QuickSlotsOffset", "quickSlotsOffset", new Vector2(216, 150), "The position offset from the Quick Slots Anchor at which to place the Quick Slots.");

            // Logging
            LogFilesPath = config.Bind("Logging", "logFilesPath", "ItemsOnDeath/", "Path to where logging of items on death are saved.");
        }
    }

    public static class KeyCodeUtils {
        public static readonly Dictionary<KeyCode, string> KeyCodeToShortText = new Dictionary<KeyCode, string>() {
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
          {KeyCode.KeypadEquals, "kp ="},
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
}
