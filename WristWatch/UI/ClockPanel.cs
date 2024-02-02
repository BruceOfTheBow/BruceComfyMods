using UnityEngine;

using static WristWatch.PluginConfig;

namespace WristWatch {
  public class ClockPanel {
    static readonly int _windowId = "clockWindowId".GetStableHashCode();
    RectTransform _window;
    public ClockPanel() {
      Rect rect = new Rect(PluginConfig.ClockPosition.Value, new Vector2(1000, 100));

      _window = GUILayout.Window(_windowId,
        new Rect(rect.position, timeRect.size)
        new GUI.WindowFunction(WindowBuilder),
        ""));
    }
  }
}
