namespace ComfyGizmo;

using System;
using System.Collections.Generic;

public sealed class PieceSelectionCache<T> {
  readonly Func<T, string> _keySelector;
  readonly Dictionary<string, T> _pieces = new(StringComparer.Ordinal);

  public PieceSelectionCache(Func<T, string> keySelector) {
    _keySelector = keySelector ?? throw new ArgumentNullException(nameof(keySelector));
  }

  public int Count => _pieces.Count;

  public void Rebuild(IEnumerable<T> pieces) {
    _pieces.Clear();

    if (pieces == null) {
      return;
    }

    foreach (T piece in pieces) {
      AddIfIdentified(piece);
    }
  }

  public bool Matches(IEnumerable<T> pieces) {
    if (pieces == null) {
      return _pieces.Count == 0;
    }

    HashSet<string> keys = new(StringComparer.Ordinal);

    foreach (T piece in pieces) {
      if (piece is null) {
        continue;
      }

      string key = GetKey(piece);

      if (!string.IsNullOrEmpty(key)) {
        keys.Add(key);
      }
    }

    if (keys.Count != _pieces.Count) {
      return false;
    }

    foreach (string key in keys) {
      if (!_pieces.ContainsKey(key)) {
        return false;
      }
    }

    return true;
  }

  public bool TryGet(T piece, out T selectedPiece) {
    if (piece is null) {
      selectedPiece = default;
      return false;
    }

    string key = GetKey(piece);

    if (!string.IsNullOrEmpty(key) && _pieces.TryGetValue(key, out selectedPiece)) {
      return true;
    }

    selectedPiece = default;
    return false;
  }

  void AddIfIdentified(T piece) {
    if (piece is null) {
      return;
    }

    string key = GetKey(piece);

    if (!string.IsNullOrEmpty(key) && !_pieces.ContainsKey(key)) {
      _pieces.Add(key, piece);
    }
  }

  string GetKey(T piece) {
    return _keySelector(piece);
  }
}
