using BepInEx;

using HarmonyLib;

using System.Reflection;

using static Abacus.Patches.InventoryGuiPatch;
using static Abacus.PluginConfig;

namespace Abacus {
  [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
  public class Abacus : BaseUnityPlugin {
    public const string PluginGuid = "bruce.valheim.brucecomfymods.abacus";
    public const string PluginName = "Abacus";
    public const string PluginVersion = "1.0.0";

    Harmony _harmony;

    public void Awake() {
      BindConfig(Config);

      _harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), harmonyInstanceId: PluginGuid);
    }

    public void OnDestroy() {
      _harmony?.UnpatchSelf();
    }
  }
}