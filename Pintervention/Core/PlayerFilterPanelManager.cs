using static Pintervention.PluginConfig;

namespace Pintervention {
  public class PlayerFilterPanelManager {
    static PlayerPinFilterPanel _filterPanel;
    public static void ToggleFilterPanel() {
      if (!_filterPanel?.Panel) {
        _filterPanel = new(Minimap.m_instance.m_largeRoot.transform);
        _filterPanel.Panel.RectTransform()
            .SetAnchorMin(new(0f, 0.5f))
            .SetAnchorMax(new(0f, 0.5f))
            .SetPivot(new(0f, 0.5f))
            .SetPosition(PlayerPinFilterPosition.Value)
            .SetSizeDelta(PlayerPinFilterSizeDelta.Value);

        PlayerPinFilterPosition.OnSettingChanged(
            position => _filterPanel?.Panel.Ref()?.RectTransform().SetPosition(position));

        PlayerPinFilterSizeDelta.OnSettingChanged(
            sizeDelta => {
              if (_filterPanel?.Panel) {
                _filterPanel.Panel.RectTransform().SetSizeDelta(sizeDelta);
              }
            });

        _filterPanel.PanelDragger.OnPanelEndDrag += (_, position) => PlayerPinFilterPosition.Value = position;
        _filterPanel.PanelResizer.OnPanelEndResize += (_, sizeDelta) => PlayerPinFilterSizeDelta.Value = sizeDelta;
        _filterPanel.Panel.SetActive(false);
      }

      bool toggleOn = !_filterPanel.Panel.activeSelf;
      _filterPanel.Panel.SetActive(!_filterPanel.Panel.activeSelf);

      if (toggleOn) {
        _filterPanel.UpdatePanel();
      }
    }

    public static void UpdatePanel() {
      if (!_filterPanel?.Panel) {
        return;
      }

      _filterPanel.UpdatePanel();
    }

    public static void UpdatePinCounts() {
      if (!_filterPanel?.Panel) {
        return;
      }

      _filterPanel.UpdatePinCounts();
    }
  }
}
