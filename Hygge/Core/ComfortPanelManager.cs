namespace Hygge;

using UnityEngine;

public static class ComfortPanelManager {
  public static ComfortPanel ComfortPanel { get; private set; }

  public static bool HasValidPanel() {
    return ComfortPanel != default && ComfortPanel.Panel;
  }

  public static void ToggleOn(Piece piece) {
    if (!HasValidPanel()) {
      if (!InstantiateComfortPanel(piece)) {
        Hygge.LogWarning("Failed to instantiate ComfortPanel.");
        return;
      }
    }
    
    ComfortPanel.Panel.SetActive(true);
  }

  public static void ToggleOff() {
    if (HasValidPanel()) {
      ComfortPanel.Panel.SetActive(false);
    }
  }

  static Piece _lastUpdatePiece = default;
  static float _lastUpdateTime = 0f;

  public static void Update(Piece piece) {
    float time = Time.time;

    if (_lastUpdatePiece == piece && (time - _lastUpdateTime < 0.5f)) {
      return;
    }

    _lastUpdatePiece = piece;
    _lastUpdateTime = time;

    ComfortPanel.Update(piece);
  }

  public static void DestroyPanel() {
    if (!HasValidPanel()) {
      return;
    }

    ComfortPanel.Panel.SetActive(false);
    UnityEngine.Object.Destroy(ComfortPanel.Panel);

    ComfortPanel = default;
  }

  static bool InstantiateComfortPanel(Piece piece) {
    ComfortPanel = ComfortPanel.CreateComfortPanel();

    if (ComfortPanel == default) {
      return false;
    }

    ComfortPanel.Update(piece);
   
    return true;
  }
}
