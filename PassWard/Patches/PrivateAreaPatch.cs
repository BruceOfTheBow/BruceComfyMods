namespace PassWard;

using System.Collections.Generic;
using System.Text;

using HarmonyLib;

using static PluginConfig;

[HarmonyPatch(typeof(PrivateArea))]
static class PrivateAreaPatch {
  [HarmonyPrefix]
  [HarmonyPatch(nameof(PrivateArea.AddUserList))]
  static bool AddUserList(PrivateArea __instance, StringBuilder text) {
    if (IsModEnabled.Value) {
      AddUserListUsingSeparator(__instance, text);
      return false;
    }

    return true;
  }

  public static void AddUserListUsingSeparator(PrivateArea privateArea, StringBuilder text) {
    List<string> playerNames = WardManager.GetCachedPermittedPlayerNames(privateArea.m_nview.m_zdo);
    int playerCount = playerNames.Count;

    text.Append("\n$piece_guardstone_additional: ");

    if (playerCount < 1) {
      return;
    }

    string separator =
        WardHoverTextUserListSeparator.Value == UserListSeperator.Newline
            ? "\n"
            : ", ";

    text.Append(playerNames[0]);

    for (int i = 1; i < playerCount; i++) {
      text
          .Append(separator)
          .Append(playerNames[i]);
    }
  }

  [HarmonyPostfix]
  [HarmonyPatch(nameof(PrivateArea.AddUserList))]
  static void AddUserListPostfix(PrivateArea __instance, StringBuilder text) {
    if (IsModEnabled.Value) {
      WardManager.AddPasswordHoverText(__instance, text);
    }
  }
}
