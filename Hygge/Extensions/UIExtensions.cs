using UnityEngine;
using UnityEngine.UI;

namespace Hygge {
  public static class GameObjectExtensions {
    public static RectTransform RectTransform(this GameObject gameObject) {
      return gameObject ? gameObject.GetComponent<RectTransform>() : null;
    }


  }
  public static class RectTransformExtensions {
    public static RectTransform SetAnchorMin(this RectTransform rectTransform, Vector2 anchorMin) {
      rectTransform.anchorMin = anchorMin;
      return rectTransform;
    }

    public static RectTransform SetAnchorMax(this RectTransform rectTransform, Vector2 anchorMax) {
      rectTransform.anchorMax = anchorMax;
      return rectTransform;
    }

    public static RectTransform SetPivot(this RectTransform rectTransform, Vector2 pivot) {
      rectTransform.pivot = pivot;
      return rectTransform;
    }

    public static RectTransform SetPosition(this RectTransform rectTransform, Vector2 position) {
      rectTransform.anchoredPosition = position;
      return rectTransform;
    }

    public static RectTransform SetSizeDelta(this RectTransform rectTransform, Vector2 sizeDelta) {
      rectTransform.sizeDelta = sizeDelta;
      return rectTransform;
    }

    public static RectTransform SetImage(this RectTransform rectTransform, Color color) {
      Image newImage = rectTransform.gameObject.AddComponent<Image>();
      newImage.color = color;
      return rectTransform;
    }
  }
}
