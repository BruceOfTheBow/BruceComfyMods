namespace RadialRebind;

using UnityEngine;

using static PluginConfig;
using static RadialRebind;

public sealed class BindManager {
  public const string OpenRadialKeybindName = "OpenRadial";

  public static void Rebind(string buttonName, KeyCode keyCode, ZInput zInput) {
    if (zInput == null || !zInput.m_buttons.TryGetValue(buttonName, out ZInput.ButtonDef buttonDef)) {
      return;
    }

    buttonDef.Rebind($"<Keyboard>/{keyCode.ToString()}");
  }

  public static void RebindOpenRadial(ZInput zInput) {
    Rebind(OpenRadialKeybindName, OpenRadialShortcut.Value, zInput);
  }
}
