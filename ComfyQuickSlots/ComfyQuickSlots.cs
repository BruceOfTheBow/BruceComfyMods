namespace ComfyQuickSlots;

using System;
using System.Globalization;
using System.Reflection;

using BepInEx;
using BepInEx.Logging;

using HarmonyLib;

using static PluginConfig;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public sealed class ComfyQuickSlots : BaseUnityPlugin {
  public const string PluginGuid = "com.bruce.valheim.comfyquickslots";
  public const string PluginName = "ComfyQuickSlots";
  public const string PluginVersion = "1.10.0";

  static ManualLogSource _logger;

  void Awake() {
    _logger = Logger;
    BindConfig(Config);

    Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), harmonyInstanceId: PluginGuid);
  }

  public static void LogInfo(object obj) {
    _logger.LogInfo($"[{DateTime.Now.ToString(DateTimeFormatInfo.InvariantInfo)}] {obj}");
  }

  void Update() {
    Player player = Player.m_localPlayer;

    if (!player || !EnableQuickslots.Value || !player.TakeInput()) {
      return;
    }

    ItemDrop.ItemData item = null;

    if (ZInput.GetKeyDown(QuickSlot1.Value)) {
      item = player.GetInventory().GetItemAt(QuickSlotsManager.QuickSlot1.x, QuickSlotsManager.QuickSlot1.y);
    }
    if (ZInput.GetKeyDown(QuickSlot2.Value)) {
      item = player.GetInventory().GetItemAt(QuickSlotsManager.QuickSlot2.x, QuickSlotsManager.QuickSlot2.y);
    }
    if (ZInput.GetKeyDown(QuickSlot3.Value)) {
      item = player.GetInventory().GetItemAt(QuickSlotsManager.QuickSlot3.x, QuickSlotsManager.QuickSlot3.y);

    }

    if (item == null || player.IsEquipActionQueued(item)) {
      return;
    }

    player.UseItem(null, item, false);
  }
}
