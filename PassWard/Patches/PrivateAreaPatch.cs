using System.Collections.Generic;
using System.Text;

using HarmonyLib;

using static PassWard.PluginConfig;

namespace PassWard.Patches {
  [HarmonyPatch(typeof(PrivateArea))]
  static class PrivateAreaPatch {
    [HarmonyPrefix]
    [HarmonyPatch(nameof(PrivateArea.AddUserList))]
    static bool AddUserList(PrivateArea __instance, StringBuilder text) {
      if (!IsModEnabled.Value) {
        return true;
      }

      List<KeyValuePair<long, string>> permittedPlayers = __instance.GetPermittedPlayers();
      text.Append("\n$piece_guardstone_additional: ");
      for (int i = 0; i < permittedPlayers.Count; i++) {
        text.Append(permittedPlayers[i].Value);
        if (i != permittedPlayers.Count - 1) {
          text.Append("\n");
        }
      }

      return false;
    }
  }
}
