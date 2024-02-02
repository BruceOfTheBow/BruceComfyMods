using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using BepInEx;

using UnityEngine;

using Jotunn.Configs;
using Jotunn.Managers;
using Jotunn.Entities;
using Jotunn.Utils;

using HarmonyLib;

using static PlantThings.Category;

using BepInEx.Logging;

namespace PlantThings {
  [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
  [BepInDependency(Jotunn.Main.ModGuid)]
  public class PlantThings : BaseUnityPlugin {
    public const string PluginGUID = "bruce.valheim.comfymods.";
    public const string PluginName = "PlantThings";
    public const string PluginVersion = "1.1.0";

    static AssetBundle assetplanter;

    static readonly string _plantItPieceTable = "_PlantitPieceTable";

    static readonly string[] _prefabAssetBundleNames = new string[] {
      "custompiece_plantsetfixedcolliders",
      "custompiece_plantset2",
      "plantset3",
      "stumpsandlogs",
      "misc",
      "pottedsucculents",
      "plantset4"
    };

    static Dictionary<string, AssetBundle> _assetBundles = new Dictionary<string, AssetBundle>();
    static Dictionary<string, string> _assetsByBundle = new Dictionary<string, string>();

    Harmony _harmony;

    static ManualLogSource _logger;

    void Awake() {
      _logger = Logger;

      LoadAssets();
      LoadTable();
      AddPieces();

      _harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), harmonyInstanceId: PluginGUID);
    }
    void OnDestroy() {
      _harmony?.UnpatchSelf();
    }

    void LoadAssets() {
      assetplanter = AssetUtils.LoadAssetBundleFromResources("shovel", typeof(PlantThings).Assembly);

      foreach (string prefabAssetBundleName in _prefabAssetBundleNames) {
        AssetBundle bundle = AssetUtils.LoadAssetBundleFromResources(prefabAssetBundleName, typeof(PlantThings).Assembly);

        if (bundle == null) {
          LogWarning($"PlantIt: could not load asset bundle {prefabAssetBundleName}");
          continue;
        }

        _assetBundles.Add(prefabAssetBundleName, bundle);
      }
    }

    static void LoadTable() {
      // Add a custom piece table with custom categories
      GameObject tablePrefab = assetplanter.LoadAsset<GameObject>(_plantItPieceTable);
      CustomPieceTable plantTable = new CustomPieceTable(tablePrefab,
          new PieceTableConfig {
            CanRemovePieces = true,
            UseCategories = false,
            UseCustomCategories = true,
            CustomCategories = ShovelCategories
          }
      );
      PieceManager.Instance.AddPieceTable(plantTable);

      LoadShovel();
    }

    static void LoadShovel() {
      GameObject shovelfab = assetplanter.LoadAsset<GameObject>("$PlumgaPlantItShovel");
      CustomItem shovel = new CustomItem(shovelfab, fixReference: true,
          new ItemConfig {
            Amount = 1,
            CraftingStation = "piece_workbench",
            RepairStation = "piece_workbench",
            MinStationLevel = 1,
            Requirements = new[] {
              new RequirementConfig { Item = "Wood", Amount = 1, AmountPerLevel = 1}
              }
          });

      ItemManager.Instance.AddItem(shovel);
    }

    static void AddPieces() {
      foreach (KeyValuePair<string, AssetBundle> assetBundle in _assetBundles) {
        foreach (string assetName in assetBundle.Value.GetAllAssetNames()) {
          AddPiece(assetName, assetBundle.Value);
        }
      }
    }

    static void AddPiece(string assetName, AssetBundle bundle) {
      string name = GetBaseName(assetName);
      GameObject go = bundle.LoadAsset<GameObject>(name);

      if (go == null || GetCategory(name) == null) {
        LogWarning($"Piece {name} not added. GameObject not found.");
        return;
      }

      CustomPiece customPiece = new CustomPiece(go,
          false,
          new PieceConfig {
            PieceTable = _plantItPieceTable,
            Category = GetCategory(name),
            AllowedInDungeons = false,
            Requirements = new[] {
                new RequirementConfig {
                  Item = "Wood", 
                  Amount = GetAmount(name), 
                  Recover = true}
              }
          });

      customPiece.Piece.m_comfort = 0;
      customPiece.Piece.m_comfortGroup = Piece.ComfortGroup.None;

      PieceManager.Instance.AddPiece(customPiece);
    }

    static string GetBaseName(string bundleName) {
      string fileName = bundleName.Split('/').LastOrDefault();

      if (fileName == null) {
        return bundleName;
      }

      return fileName.Replace(".prefab", "");
    }

    static int GetAmount(string name) {
      if (Requirements.WoodRequired.Keys.Contains(name)) {
        return Requirements.WoodRequired[name];
      }

      return 2;
    }

    public static void Log(string message) {
      _logger.LogInfo($"{message}");
    }

    public static void LogWarning(string message) {
      _logger.LogWarning($"{message}");
    }
  }
}