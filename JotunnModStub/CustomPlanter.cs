// JotunnModStub
// a Valheim mod skeleton using Jötunn
// 
// File:    JotunnModStub.cs
// Project: JotunnModStub

using BepInEx;
using UnityEngine;
using BepInEx.Configuration;
using Jotunn.Configs;
using Jotunn.Managers;
using Jotunn.Entities;
using Jotunn.Utils;

namespace CustomPlanter
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    internal class CustomPlanter : BaseUnityPlugin
    {
        public const string PluginGUID = "com.jotunn.CustomPlanter";
        public const string PluginName = "CustomPlanter";
        public const string PluginVersion = "0.0.1";
        private AssetBundle assetplanter;
        private AssetBundle plants;


        private void Awake()
        {

            LoadAssets();
            LoadTable();
        }


        private void LoadAssets()
        {
            assetplanter = AssetUtils.LoadAssetBundleFromResources("plantit", typeof(CustomPlanter).Assembly);
            plants = AssetUtils.LoadAssetBundleFromResources("eightplants3", typeof(CustomPlanter).Assembly);

        }


        private void LoadTable()
        {
            PieceManager.Instance.AddPieceTable(assetplanter.LoadAsset<GameObject>("_PlantitPieceTable"));
            LoadShovel();

        }

        private void LoadShovel()
        {
            var shovelfab = assetplanter.LoadAsset<GameObject>("Planter");
            var shovel = new CustomItem(shovelfab, fixReference: true,
                new ItemConfig
                {
                    Amount = 1,
                    Requirements = new[]
                    {
                        new RequirementConfig { Item = "Wood", Amount = 1}
                    }
                });
            ItemManager.Instance.AddItem(shovel);

        }

        private void PlantMaker()
        {
            var planterthing = plants.LoadAsset<GameObject>("customitem_birdofparadise");
            var planter = new CustomPiece(planterthing,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(planter);
        }
    }
}