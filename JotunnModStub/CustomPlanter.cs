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
            plants = AssetUtils.LoadAssetBundleFromResources("custompiece_plantset1", typeof(CustomPlanter).Assembly);

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


        /// you will care about copying basically this chunk of code right here 
        private void Bird_of_Paradise_Plant() //this is the function name you need to change it 
        {
            var Bird_of_Paradise_Thing = plants.LoadAsset<GameObject>("custompiece_birdofparadise"); //this defines the prefab name in your uniy project 
           // you need to change planterthing and planter every time you copy this 
            var Bird_of_Paradise = new CustomPiece(Bird_of_Paradise_Thing,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(Bird_of_Paradise);
        }


        private void Bonsai_Plant() //this is the function name you need to change it 
        {
            var Bonsai_Thing = plants.LoadAsset<GameObject>("custompiece_bonsai"); //this defines the prefab name in your uniy project 
                                                                                          // you need to change planterthing and planter every time you copy this 
            var Bonsai = new CustomPiece(Bonsai_Thing,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(Bonsai);
        }


        private void Barrel_Cactus_Plant() //this is the function name you need to change it 
        {
            var Barrel_Cactus_Thing = plants.LoadAsset<GameObject>("custompiece_barrelcactus"); //this defines the prefab name in your uniy project 
                                                                                          // you need to change planterthing and planter every time you copy this 
            var Barrel_Cactus = new CustomPiece(Barrel_Cactus_Thing,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(Barrel_Cactus);
        }

        // to here 

    }
}