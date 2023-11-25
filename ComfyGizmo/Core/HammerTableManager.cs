using BepInEx;
using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using UnityEngine;
using static ComfyGizmo.ComfyGizmo;

namespace ComfyGizmo {
  public class HammerTableManager {
    static readonly string _searsCatalogGUID = "redseiko.valheim.searscatalog";
    static readonly string _columnConfigSection = "BuildHud.Panel";
    static readonly string _columnConfigKey = "buildHudPanelColumns";
    static readonly int _defaultColumnCount = 15;

    static BaseUnityPlugin _searsCatalog = null;
    static ConfigEntry<int> _searsCatalogColumnsConfigEntry;

    static Dictionary<string, Vector2Int> _pieceLocations = new();
    static int _cachedAvailablePieceCount = -1;


    static bool _targetSelection = false;

    public static void Initialize() {
      if (!IsSearsCatalogEnabled()) {
        return;
      }

      FindSearsCatalogPlugin();
    }

    public static void SelectTargetPiece(Player player) {
      if (IsHammerTableChanged(player) || !IsHammerTableCached()) {
        CacheHammerTable(player);
      }

      if (!HasCachedPiece(player.GetHoveringPiece())) {
        return;
      }

      _targetSelection = true;
      SetSelectedPiece(player, player.GetHoveringPiece());
    }

    public static bool IsTargetSelected() {
      return _targetSelection;
    }

    public static int GetColumnCount() {
      if (!IsSearsCatalogEnabled()) {
        return _defaultColumnCount;
      }

      return GetSearsCatalogColumnCount();
    }

    public static bool HasCachedPiece(Piece piece) {
      return _pieceLocations.ContainsKey(GetPieceIdentifier(piece));
    }

    public static void SetSelectedPiece(Player player, Piece piece) {
      Vector2Int pieceLocation = _pieceLocations[GetPieceIdentifier(piece)];
      Piece.PieceCategory previousCategory = player.m_buildPieces.m_selectedCategory;

      player.m_buildPieces.m_selectedCategory = (Piece.PieceCategory)pieceLocation.x;
      player.SetSelectedPiece(new Vector2Int(pieceLocation.y % HammerTableManager.GetColumnCount(), pieceLocation.y / HammerTableManager.GetColumnCount()));
      player.SetupPlacementGhost();

      if (previousCategory != player.m_buildPieces.m_selectedCategory) {
        Hud.instance.UpdatePieceList(player, new Vector2Int(pieceLocation.y % 15, pieceLocation.y / 15), (Piece.PieceCategory)pieceLocation.x, true);
      }
    }

    public static bool IsHammerTableCached() {
      return _cachedAvailablePieceCount != -1;
    }

    public static void CacheHammerTable(Player player) {
      PieceTable hammerPieceTable = player.m_buildPieces;
      _cachedAvailablePieceCount = 0;
      _pieceLocations = new();

      for (int i = 0; i < hammerPieceTable.m_availablePieces.Count; i++) {
        List<Piece> categoryPieces = hammerPieceTable.m_availablePieces[i];

        for (int j = 0; j < categoryPieces.Count; j++) {
          if (_pieceLocations.ContainsKey(GetPieceIdentifier(categoryPieces[j]))) {
            continue;
          }

          _pieceLocations.Add(GetPieceIdentifier(categoryPieces[j]), new Vector2Int(i, j));
          _cachedAvailablePieceCount++;
        }
      }
    }

    public static bool IsHammerTableChanged(Player player) {
      if (!player || !player.m_buildPieces || player.m_buildPieces.m_availablePieces == null) {
        return false;
      }
      int currentPieceCount = 0;

      for (int i = 0; i < player.m_buildPieces.m_availablePieces.Count; i++) {
        currentPieceCount += player.m_buildPieces.m_availablePieces[i].Count;
      }

      if (currentPieceCount == _cachedAvailablePieceCount) {
        return false;
      }

      return true;
    }
    private static string GetPieceIdentifier(Piece piece) {
      return piece.m_name + piece.m_description;
    }

    public static bool IsSearsCatalogEnabled() {
      FindSearsCatalogPlugin();

      if (!_searsCatalog) {
        return false;
      }

      return true;
    }

    public static int GetSearsCatalogColumnCount() {
      if (_searsCatalogColumnsConfigEntry != null) {
        return _searsCatalogColumnsConfigEntry.Value;
      }

      if (_searsCatalog.Config.TryGetEntry(new ConfigDefinition(_columnConfigSection, _columnConfigKey), out ConfigEntry<int> columns)) {
        _searsCatalogColumnsConfigEntry = columns;
        return columns.Value;
      }

      return _defaultColumnCount;
    }

    private static void FindSearsCatalogPlugin() {
      IEnumerable<BaseUnityPlugin> loadedPlugins = GetLoadedPlugins();

      if (loadedPlugins == null) {
        return;
      }

      Dictionary<string, BaseUnityPlugin> plugins
          = loadedPlugins
              .Where(plugin => plugin.Info.Metadata.GUID == _searsCatalogGUID)
              .ToDictionary(plugin => plugin.Info.Metadata.GUID);

      if (plugins.TryGetValue(_searsCatalogGUID, out BaseUnityPlugin plugin)) {
        _searsCatalog = plugin;
      }

      return;
    }

    private static IEnumerable<BaseUnityPlugin> GetLoadedPlugins() {
      return BepInEx.Bootstrap.Chainloader.PluginInfos
                    .Where(x => x.Value != null && x.Value.Instance != null)
                    .Select(x => x.Value.Instance);
    }
  }
}
