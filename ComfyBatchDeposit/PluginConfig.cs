using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx.Configuration;

using UnityEngine;

namespace ComfyBatchDeposit {
  public class PluginConfig {
    public static ConfigEntry<bool> IsModEnabled = default!;
    public static ConfigEntry<bool> SortOnDump = default!;
    public static ConfigEntry<KeyCode> Modifier = default!;

    public static void BindConfig(ConfigFile config) {
      IsModEnabled = config.Bind("_Global", "isModEnabled", true, "Globally enable or disable this mod.");
      SortOnDump = config.Bind("Behavior", "sortOnDump", false, "Runs sort function immediately after dump function is completely when dump button is pressed.");
      Modifier = config.Bind("DepositAllModifier", "depositAllModifier", KeyCode.LeftAlt, "Hot key modifier for deposit all on left click.");
    }
  }
}
