using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

using static ComfyQuickSlots.ComfyQuickSlots;

namespace ComfyQuickSlots.Patches {
  [HarmonyPatch(typeof(FejdStartup))]
  public class FejdStartupPatcher {
    [HarmonyPrefix]
    [HarmonyPatch(nameof(FejdStartup.SetupCharacterPreview))]
    public static void FejdStartupSetupCharacterPreviewPrefix(FejdStartup __instance) {
      initialEquippedArmor = new List<ItemDrop.ItemData>();
      firstLoad = false;
    }
  }
}
