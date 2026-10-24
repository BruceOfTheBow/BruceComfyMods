namespace ComfyGizmo;

using BepInEx;
using BepInEx.Configuration;

using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public sealed class HammerTableManager {
  static readonly string _searsCatalogGUID = "redseiko.valheim.searscatalog";
  static readonly string _columnConfigSection = "BuildHud.Panel";
  static readonly string _columnConfigKey = "buildHudPanelColumns";
  static readonly int _defaultColumnCount = 15;

  static BaseUnityPlugin _searsCatalog = null;
  static ConfigEntry<int> _searsCatalogColumnsConfigEntry;

  static readonly PieceSelectionCache<Piece> _pieceCache =
      new(piece => piece ? GetPieceIdentifier(piece) : null);
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

    Piece targetPiece = player.GetHoveringPiece();

    if (!HasCachedPiece(targetPiece)) {
      return;
    }

    SetSelectedPiece(player, targetPiece);
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
    return piece && _pieceCache.TryGet(piece, out _);
  }

  public static void SetSelectedPiece(Player player, Piece piece) {
    if (!player || !piece || !player.m_buildPieces) {
      return;
    }

    if (_pieceCache.TryGet(piece, out Piece selectedPiece)) {
      _targetSelection = player.SetSelectedPiece(selectedPiece);
    }
  }

  public static bool IsHammerTableCached() {
    return _cachedAvailablePieceCount != -1;
  }

  public static void CacheHammerTable(Player player) {
    PieceTable hammerPieceTable = player.m_buildPieces;
    _pieceCache.Rebuild(hammerPieceTable.m_availablePieces);
    _cachedAvailablePieceCount = _pieceCache.Count;
  }

  public static bool IsHammerTableChanged(Player player) {
    if (!player || !player.m_buildPieces || player.m_buildPieces.m_availablePieces == null) {
      return false;
    }
    return !_pieceCache.Matches(player.m_buildPieces.m_availablePieces);
  }
  private static string GetPieceIdentifier(Piece piece) {
    return Utils.GetPrefabName(piece.gameObject);
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

    if (_searsCatalog.Config.TryGetEntry(
            new ConfigDefinition(_columnConfigSection, _columnConfigKey), out ConfigEntry<int> columns)) {
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

    Dictionary<string, BaseUnityPlugin> plugins =
        loadedPlugins
            .Where(plugin => plugin.Info.Metadata.GUID == _searsCatalogGUID)
            .ToDictionary(plugin => plugin.Info.Metadata.GUID);

    if (plugins.TryGetValue(_searsCatalogGUID, out BaseUnityPlugin plugin)) {
      _searsCatalog = plugin;
    }
  }

  private static IEnumerable<BaseUnityPlugin> GetLoadedPlugins() {
    return
        BepInEx.Bootstrap.Chainloader.PluginInfos
            .Where(x => x.Value != null && x.Value.Instance != null)
            .Select(x => x.Value.Instance);
  }
}
