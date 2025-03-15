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
    List<KeyValuePair<long, string>> permittedPlayers = privateArea.GetPermittedPlayers();
    int playerCount = permittedPlayers.Count;

    text.Append("\n$piece_guardstone_additional: ");

    if (playerCount < 1) {
      return;
    }

    string separator =
        WardHoverTextUserListSeparator.Value == UserListSeperator.Newline
            ? "\n"
            : ", ";

    text.Append(permittedPlayers[0].Value);

    for (int i = 1; i < playerCount; i++) {
      text
          .Append(separator)
          .Append(permittedPlayers[i].Value);
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
