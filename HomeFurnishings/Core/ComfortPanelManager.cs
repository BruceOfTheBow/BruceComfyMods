using System.Security.Policy;
using UnityEngine;
using UnityEngine.UI;

using static HomeFurnishings.HomeFurnishings;

namespace HomeFurnishings {
  public static class ComfortPanelManager {
    public static ComfortPanel ComfortPanel { get; private set; }
    private static Piece lastPiece = null;
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
      ComfortPanel = ComfortPanel.CreateComfortPanel();

      if (ComfortPanel == null) {
        return false;
      }
     
      return true;
    }


  }
}
