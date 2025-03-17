namespace PassWard;

using System;
using System.Collections.Generic;
using System.Text;

using UnityEngine;

using static PluginConfig;

public static class WardManager {
  public const int GuardStoneHash = -1024209535;  // "guard_stone".GetStableHashCode();
  public const int PasswordHash = 1595635768;     // "passward.password".GetStableHashCode();

  public static void OptPlayerIn(PrivateArea privateArea) {
    if (!privateArea || !Player.m_localPlayer) {
      PassWard.LogError("Error opting-in after password success.");
      return;
    }

    long playerId = Player.m_localPlayer.GetPlayerID();

    if (privateArea.IsPermitted(playerId)) {
      ShowMessage("Already opted-in.");
      return;
    }

    privateArea.AddPermitted(playerId, Player.m_localPlayer.GetPlayerName());
    ShowMessage("Password accepted.");

    if (Player.m_localPlayer.TryGetComponent(out Talker talker)) {
      talker.Say(Talker.Type.Normal, $"[PassWard] {privateArea.GetCreatorName()}'s Ward: password accepted.");
    }
  }

  public static void AddPasswordHoverText(PrivateArea privateArea, StringBuilder hoverText) {
    bool hasPassword = HasPassword(privateArea);

    if (privateArea.m_piece.IsCreator()) {
      if (hasPassword) {
        hoverText
            .Append("\n<size=18>[<color=yellow>")
            .Append(RemovePasswordKey.Value)
            .Append("</color>] Remove password.</size>");
      }

      hoverText
          .Append("\n<size=18>[<color=yellow>")
          .Append(EnterPasswordKey.Value)
          .Append("</color>] ")
          .Append(hasPassword ? "Change password.</size>" : "Add password.</size>");
    } else {
      if (hasPassword) {
        hoverText
            .Append("\n\n<size=18><color=green>Password enabled</color>\n[<color=yellow>")
            .Append(EnterPasswordKey.Value)
            .Append("</color>] Enter password.</size>");
      } else {
        hoverText.Append("\n\n<size=18><color=red>No password</color></size>");
      }
    }
  }

  public static void EnterPassword(PrivateArea privateArea) {
    bool hasPassword = HasPassword(privateArea);

    if (privateArea.m_piece.IsCreator()) {
      WardPasswordReceiver passwordReceiver = new WardPasswordReceiver(privateArea);
      TextInput.instance.RequestText(passwordReceiver, hasPassword ? "Change password" : "Set password", 32);
      return;
    }
    
    if (!hasPassword) {
      ShowMessage("No password assigned to this ward. Cannot opt-in using password.");
      return;
    }

    TryPasswordReceiver tryPassword = new TryPasswordReceiver(privateArea);
    TextInput.instance.RequestText(tryPassword, "Enter password", 32);
  }

  public static void RemovePassword(PrivateArea privateArea) {
    if (!privateArea.m_piece.IsCreator()) {
      ShowMessage("You do not own this ward. Cannot remove password.");
      return;
    }

    if (!HasPassword(privateArea)) {
      ShowMessage($"No password on this ward to remove.");
      return;
    }

    privateArea.m_nview.m_zdo.Set(PasswordHash, -1);

    ShowMessage($"Password removed.");
  }

  public static bool HasPassword(PrivateArea privateArea) {
    return privateArea.m_nview.m_zdo.GetInt(PasswordHash, out int password) && password != -1;
  }

  public static void ShowMessage(string message) {
    if (MessageHud.instance) {
      MessageHud.instance.ShowMessage(MessageHud.MessageType.TopLeft, message);
    }
  }

  static readonly List<string> _cachedPlayerNames = new(capacity: 100);

  static ZDOID _lastZDOID = ZDOID.None;
  static uint _lastDataRevision = uint.MaxValue;

  public static void ClearCachedPermittedPlayerNames() {
    _lastZDOID = ZDOID.None;
    _lastDataRevision = uint.MaxValue;
    _cachedPlayerNames.Clear();
  }

  public static List<string> GetCachedPermittedPlayerNames(ZDO zdo) {
    if (_lastZDOID == zdo.m_uid && _lastDataRevision == zdo.DataRevision) {
      return _cachedPlayerNames;
    }

    _lastZDOID = zdo.m_uid;
    _lastDataRevision = zdo.DataRevision;
    _cachedPlayerNames.Clear();

    int permittedCount = Mathf.Max(zdo.GetInt(ZDOVars.s_permitted, 0), 0);

    CachePuIdNameHashCodes(permittedCount);

    for (int i = 0; i < permittedCount; i++) {
      if (zdo.GetLong(_cachedPuIdHashCodes[i], 0L) == 0L) {
        continue;
      }

      string playerName = zdo.GetString(_cachedPuNameHashCodes[i], string.Empty);

      if (playerName.Length > 0) {
        _cachedPlayerNames.Add(playerName);
      }
    }

    UserListSorting sorting = WardHoverTextUserListSorting.Value;

    if (sorting == UserListSorting.Alphabetically) {
      _cachedPlayerNames.Sort(StringComparer.OrdinalIgnoreCase);
    } else if (sorting == UserListSorting.ReverseAlphabetically) {
      _cachedPlayerNames.Sort(StringComparer.OrdinalIgnoreCase);
      _cachedPlayerNames.Reverse();
    }

    return _cachedPlayerNames;
  }

  static int[] _cachedPuIdHashCodes = [];
  static int[] _cachedPuNameHashCodes = [];

  static void CachePuIdNameHashCodes(int capacity) {
    int count = Mathf.Min(_cachedPuIdHashCodes.Length, _cachedPuNameHashCodes.Length);

    if (capacity <= 0 || capacity < count) {
      return;
    }

    Array.Resize(ref _cachedPuIdHashCodes, capacity);
    Array.Resize(ref _cachedPuNameHashCodes, capacity);

    for (int i = count; i < capacity; i++) {
      _cachedPuIdHashCodes[i] = $"pu_id{i}".GetStableHashCode();
      _cachedPuNameHashCodes[i] = $"pu_name{i}".GetStableHashCode();
    }
  }
}
