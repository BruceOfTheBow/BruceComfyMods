namespace ComfyLib;

using TMPro;

using UnityEngine;

public static class UIBuilder {
  public static TextMeshProUGUI CreateTMPLabel(Transform parentTransform) {
    TextMeshProUGUI label = UnityEngine.Object.Instantiate(UnifiedPopup.instance.bodyText, parentTransform);
    label.name = "Label";

    label
        .SetEnableAutoSizing(false)
        .SetRichText(true)
        .SetOverflowMode(TextOverflowModes.Overflow)
        .SetTextWrappingMode(TextWrappingModes.NoWrap)
        .SetFontSize(16f)
        .SetColor(Color.white)
        .SetText(string.Empty);

    return label;
  }

  public static TextMeshProUGUI CreateTMPHeaderLabel(Transform parentTransform) {
    TextMeshProUGUI label =
        UnityEngine.Object.Instantiate(UnifiedPopup.instance.headerText, parentTransform, worldPositionStays: false);
    label.name = "Label";

    label
        .SetEnableAutoSizing(false)
        .SetRichText(true)
        .SetOverflowMode(TextOverflowModes.Overflow)
        .SetTextWrappingMode(TextWrappingModes.NoWrap)
        .SetFontSize(32f)
        .SetText(string.Empty);

    return label;
  }
}
