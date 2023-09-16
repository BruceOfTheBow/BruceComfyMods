using HarmonyLib;

using static PlantThings.Core.Category;

namespace PlantThings.Patches {
  [HarmonyPatch(typeof(Piece))]
  static class PiecePatch {
    static string _plantThingsVerificationField = "PlantThings";
    static string _plantThingsVerificationValue = "Yes";

    [HarmonyPostfix]
    [HarmonyPatch(nameof(Piece.SetCreator))]
    static void SetCreatorPostfix(ref Piece __instance, long uid) {
      if (__instance == null
            || __instance.gameObject == null
            || !__instance.gameObject.TryGetComponent(out ZNetView zNetView)
            || zNetView.GetZDO() == null) {

        return;
      }

      if (GetCategory(__instance.gameObject.name.Replace($"(Clone)", "")) == null) {
        return;
      }

      zNetView.GetZDO().Set(_plantThingsVerificationField, _plantThingsVerificationValue);
    }
  }
}
