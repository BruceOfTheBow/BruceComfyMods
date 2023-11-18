namespace Hygge {
  public static class ComfortPanelManager {
    public static ComfortPanel ComfortPanel { get; private set; }
    private static Piece lastPiece = null;
    public static void ToggleOn(Piece piece) {
      if (!ComfortPanel?.Panel) {
        if (!InstantiateComfortPanel(piece)) {
          ZLog.LogWarning("Failed to instantiate comfort panel.");
          return;
        }
      }
      
      ComfortPanel.Panel.SetActive(true);
    }

    public static void ToggleOff() {
      if (!ComfortPanel?.Panel) {
        return;
      }

      ComfortPanel.Panel.SetActive(false);
    }

    public static void Update(Piece piece) {
      ComfortPanel.Update(piece);
    }

    public static void DestroyPanel() {
      if (!ComfortPanel?.Panel) {
        return;
      }

      ComfortPanel.Panel.SetActive(false);
      UnityEngine.Object.Destroy(ComfortPanel.Panel);
      ComfortPanel = null;
    }

    private static bool InstantiateComfortPanel(Piece piece) {
      ComfortPanel = ComfortPanel.CreateComfortPanel();

      if (ComfortPanel == null) {
        return false;
      }

      ComfortPanel.Update(piece);
     
      return true;
    }
  }
}
