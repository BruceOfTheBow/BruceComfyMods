﻿using System;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace Pintervention {
  public static class UIBuilder {
    public static TextMeshProUGUI CreateTMPLabel(Transform parentTransform) {
      TextMeshProUGUI label =
          UnityEngine.Object.Instantiate(UnifiedPopup.instance.bodyText, parentTransform, worldPositionStays: false);

      label.name = "Label";
      label.text = string.Empty;
      label.fontSize = 16f;
      label.color = Color.white;
      label.richText = true;
      label.enableAutoSizing = false;

      return label;
    }

    public static GameObject CreateRowSpacer(Transform parentTransform) {
      GameObject spacer = new($"{parentTransform.name}.Spacer", typeof(RectTransform));
      spacer.SetParent(parentTransform);

      spacer.AddComponent<LayoutElement>()
          .SetPreferred(width: 20f);

      return spacer;
    }

    static readonly Lazy<TextGenerator> CachedTextGenerator = new();

    public static float GetPreferredWidth(this Text text) {
      return GetPreferredWidth(text, text.text);
    }

    public static float GetPreferredWidth(Text text, string content) {
      return CachedTextGenerator.Value.GetPreferredWidth(
          content, text.GetGenerationSettings(text.rectTransform.rect.size));
    }

    static readonly Dictionary<string, Sprite> _spriteCache = new();

    static readonly Color32 ColorWhite = Color.white;
    static readonly Color32 ColorClear = Color.clear;

    public static Sprite CreateRoundedCornerSprite(
        int width, int height, int radius, FilterMode filterMode = FilterMode.Bilinear) {
      string name = $"RoundedCorner-{width}w-{height}h-{radius}r";

      if (_spriteCache.TryGetValue(name, out Sprite sprite)) {
        return sprite;
      }

      Texture2D texture =
          new Texture2D(width, height)
              .SetName(name)
              .SetWrapMode(TextureWrapMode.Clamp)
              .SetFilterMode(filterMode);

      Color32[] pixels = new Color32[width * height];

      for (int y = 0; y < height; y++) {
        for (int x = 0; x < width; x++) {
          pixels[(y * width) + x] = IsCornerPixel(x, y, width, height, radius) ? ColorClear : ColorWhite;
        }
      }

      texture.SetPixels32(pixels);
      texture.Apply();

      int borderWidth;
      for (borderWidth = 0; borderWidth < width; borderWidth++) {
        if (pixels[borderWidth] == Color.white) {
          break;
        }
      }

      int borderHeight;
      for (borderHeight = 0; borderHeight < height; borderHeight++) {
        if (pixels[borderHeight * width] == Color.white) {
          break;
        }
      }

      sprite =
          Sprite.Create(
              texture,
              new(0, 0, width, height),
              new(0.5f, 0.5f),
              pixelsPerUnit: 100f,
              extrude: 0,
              SpriteMeshType.FullRect,
              new(borderWidth, borderHeight, borderWidth, borderHeight))
          .SetName(name);

      _spriteCache[name] = sprite;
      return sprite;
    }

    static bool IsCornerPixel(int x, int y, int w, int h, int rad) {
      if (rad == 0) {
        return false;
      }

      int dx = Math.Min(x, w - x);
      int dy = Math.Min(y, h - y);

      if (dx == 0 && dy == 0) {
        return true;
      }

      if (dx > rad || dy > rad) {
        return false;
      }

      dx = rad - dx;
      dy = rad - dy;

      return Math.Round(Math.Sqrt(dx * dx + dy * dy)) > rad;
    }

    public static Sprite CreateSuperellipse(int width, int height, float exponent) {
      string name = $"Superellipse-{width}w-{height}h-{exponent}e";

      if (_spriteCache.TryGetValue(name, out Sprite sprite)) {
        return sprite;
      }

      Texture2D texture =
          new Texture2D(width, height)
              .SetName(name)
              .SetWrapMode(TextureWrapMode.Clamp)
              .SetFilterMode(FilterMode.Trilinear);

      Color32[] pixels = new Color32[width * height];

      int XYToIndex(int x, int y) {
        return x + (y * width);
      }

      int mx = width / 2;
      int my = height / 2;

      float factor = 1f;
      float a = factor * (width / 2f);
      float b = factor * (height / 2f);

      for (int x = 0; x < mx; x++) {
        for (int y = 0; y < my; y++) {
          float lhs = Mathf.Pow(Mathf.Abs(x / a), exponent) + Mathf.Pow(Mathf.Abs(y / b), exponent);
          Color32 color = lhs > 1f ? Color.clear : Color.white;

          int rightx = x + mx;
          int leftx = -x + mx - 1;
          int topy = -y + my - 1;
          int bottomy = y + my;

          pixels[XYToIndex(rightx, bottomy)] = color;
          pixels[XYToIndex(rightx, topy)] = color;
          pixels[XYToIndex(leftx, topy)] = color;
          pixels[XYToIndex(leftx, bottomy)] = color;
        }
      }

      texture.SetPixels32(pixels);
      texture.Apply();

      int borderWidth;
      for (borderWidth = 0; borderWidth < width; borderWidth++) {
        if (pixels[borderWidth] == Color.white) {
          break;
        }
      }

      int borderHeight;
      for (borderHeight = 0; borderHeight < height; borderHeight++) {
        if (pixels[borderHeight * width] == Color.white) {
          break;
        }
      }

      sprite =
          Sprite.Create(
              texture,
              new(0, 0, width, height),
              new(0.5f, 0.5f),
              pixelsPerUnit: 100f,
              extrude: 0,
              SpriteMeshType.FullRect,
              new(borderWidth, borderHeight, borderWidth, borderHeight))
          .SetName(name);

      _spriteCache[name] = sprite;
      return sprite;
    }
  }
}
