using System;
using System.IO;

using BepInEx;
using BepInEx.Logging;

using HarmonyLib;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using System.Reflection;

using static DumpsterFire.PluginConfig;

namespace DumpsterFire {
  [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
  public class DumpsterFire : BaseUnityPlugin {
    public const string PluginGuid = "bruce.valheim.comfy.dumpsterfire";
    public const string PluginName = "DumpsterFire";
    public const string PluginVersion = "1.1.0";

    public static Sprite DumpsterSprite;
    public static AudioClip FireBurnAudioClip;
    public static AudioSource InventoryAudioSource;

    private static string _dumpsterSpriteResource = "DumpsterFire.Resources.dumpster-85x64.png";
    private static string _fireAudioResource = "DumpsterFire.Resources.DumpsterFireBurn.wav";

    private static string FormatCode = "PCM";

    internal static ManualLogSource _logger;

    Harmony _harmony;

    public void Awake() {
      _logger = Logger;

      BindConfig(Config);

      DumpsterSprite = CreateDumpsterSprite();

      CreateAudioClip();

      _harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), harmonyInstanceId: PluginGuid);
    }

    public void OnDestroy() {
      _harmony?.UnpatchSelf();
    }

    private static Sprite CreateDumpsterSprite() {
      Texture2D texture = new(1, 1);

      Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(_dumpsterSpriteResource);
      byte[] data = new byte[stream.Length];

      stream.Read(data, offset: 0, count: (int)stream.Length);
      texture.LoadImage(data);
      return Sprite.Create(texture, new(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

    // Supports .wav signed 16-bit uncompressed PCM files
    private static void CreateAudioClip() {
      Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(_fireAudioResource);
      using (MemoryStream memoryStream = new MemoryStream()) {
        stream.CopyTo(memoryStream);
        byte[] data = memoryStream.GetBuffer();

        int headers = BitConverter.ToInt32(data, 16);
        UInt16 audioFormat = BitConverter.ToUInt16(data, 20);

        UInt16 channels = BitConverter.ToUInt16(data, 22);
        int sampleRate = BitConverter.ToInt32(data, 24);
        UInt16 bitDepth = BitConverter.ToUInt16(data, 34);

        int headerOffset = 16 + 4 + headers + 4;
        int dataSize = BitConverter.ToInt32(data, headerOffset);
      
        float[] floatData;
        floatData = ConvertByteArrayToAudioClip(data, headerOffset);

        FireBurnAudioClip = AudioClip.Create("DumpsterBurning", data.Length, (int)channels, sampleRate, false);
        FireBurnAudioClip.SetData(floatData, 0);
      }
    }

    private static float[] ConvertByteToFloat(byte[] array) {
      float[] floatArr = new float[array.Length / 2];

      for (int i = 0; i < floatArr.Length; i++) {
        floatArr[i] = ((float)BitConverter.ToInt16(array, i * 2)) / 32768.0f;
      }

      return floatArr;
    }

    public static void DestroyItem() {
      if (InventoryGui.m_instance == null || InventoryGui.m_instance.m_dragItem == null) {
        return;
      }

      InventoryGui __instance = InventoryGui.m_instance;
      if (__instance.m_dragAmount.Equals(__instance.m_dragItem.m_stack)) {
        Player.m_localPlayer.RemoveEquipAction(__instance.m_dragItem);
        Player.m_localPlayer.UnequipItem(__instance.m_dragItem);
        __instance.m_dragInventory.RemoveItem(__instance.m_dragItem);
      } else {
        __instance.m_dragInventory.RemoveItem(__instance.m_dragItem, __instance.m_dragAmount);
      }

      if (IsAudioEnabled.Value) {
        InventoryAudioSource.PlayOneShot(FireBurnAudioClip);
      }

      __instance.SetupDragItem(null, null, 1);
    }

    private static float[] ConvertByteArrayToAudioClip(byte[] data, int headerOffset) {
      int wavSize = BitConverter.ToInt32(data, headerOffset);
      headerOffset += sizeof(int);
      int convertedSize = wavSize / sizeof(Int16);

      float[] floatData = new float[convertedSize];

      int bytesIn = 0;
      int i = 0;
      while (i < convertedSize) {
        bytesIn = i * sizeof(Int16) + headerOffset;
        floatData[i] = (float)BitConverter.ToInt16(data, bytesIn) / Int16.MaxValue;
        ++i;
      }

      return floatData;
    }
  }
}