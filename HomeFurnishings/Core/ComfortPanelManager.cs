using System.Security.Policy;
using UnityEngine;
using UnityEngine.UI;

using static HomeFurnishings.HomeFurnishings;

namespace HomeFurnishings {
  public static class ComfortPanelManager {
    public static ComfortPanel ComfortPanel { get; private set; }

    public static void ToggleOn(Piece piece) {
      if (!ComfortPanel?.Panel) {
        if (!InstantiateComfortPanel()) {
          ZLog.LogWarning("Failed to instantiate comfort panel.");
          return;
        }
      }

      ComfortPanel.Update(piece);
      ComfortPanel.Panel.SetActive(true);
    }

    public static void ToggleOff() {
      if (!ComfortPanel?.Panel) {
        return;
      }

      ComfortPanel.Panel.SetActive(false);
    }

    private static bool InstantiateComfortPanel() {
      Transform selectedInfo = FindSelectedInfo();

      if (selectedInfo == null) {
        ZLog.LogWarning("Failed to find 'SelectedInfo' transform in build hud.");
        return false;
      }

      ComfortPanel = new(selectedInfo);

      RectTransform bkg2 = FindBkg();

      ComfortPanel.Panel.RectTransform()
        .SetAnchorMin(new(0f, 0f))
        .SetAnchorMax(new(1, 1f))
        .SetPivot(new(0.5f, 0.5f))
        .SetPosition(new(bkg2.anchoredPosition.x + bkg2.rect.width/2 + bkg2.rect.width/4 + 5f, bkg2.anchoredPosition.y))
        .SetImage(new Color(0, 0, 0, 0.5f));

      ComfortPanel.Panel.RectTransform().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, bkg2.rect.width/2);
      ComfortPanel.Panel.RectTransform().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, bkg2.rect.height);

      ComfortPanel.Panel.SetActive(false);

      return true;
    }

    private static RectTransform FindBkg() {
      Transform selectedInfo = Hud.instance.m_buildHud.transform.Find("SelectedInfo");
      if (selectedInfo == null) {
        ZLog.LogWarning("SelectedInfo transform not found building comfort panel.");
        return null;
      }

      Transform bkg2 = selectedInfo.Find("Bkg2");

      if (bkg2 == null) {
        ZLog.LogWarning("Bkg2 not found building comfort panel.");
        return null;
      }

      return (RectTransform)bkg2;
    }

    private static Transform FindSelectedInfo() {
      return Hud.instance.m_buildHud.transform.Find("SelectedInfo");
    }
  }
}
