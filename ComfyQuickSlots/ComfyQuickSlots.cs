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
  public const string PluginVersion = "1.7.0";

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

    if (player == null || !EnableQuickslots.Value || !player.TakeInput()) {
      return;
    }

    ItemDrop.ItemData item = null;

    if (ZInput.GetKeyDown(QuickSlot3.Value)) {
      item = player.GetInventory().GetItemAt(7, 4);
    }
    if (ZInput.GetKeyDown(QuickSlot2.Value)) {
      item = player.GetInventory().GetItemAt(6, 4);
    }
    if (ZInput.GetKeyDown(QuickSlot1.Value)) {
      item = player.GetInventory().GetItemAt(5, 4);

    }

    if (item == null || player.IsEquipActionQueued(item)) {
      return;
    }

    player.UseItem(null, item, false);
  }
}
