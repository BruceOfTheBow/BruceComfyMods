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

namespace PlantIt
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    internal class PlantIt : BaseUnityPlugin
    {
        public const string PluginGUID = "com.jotunn.PlantIt";
        public const string PluginName = "PlantIt";
        public const string PluginVersion = "0.0.5";
        private AssetBundle assetplanter;
        private AssetBundle plants;
        private AssetBundle plants2;
        private AssetBundle plants3;


        private void Awake()
        {

            LoadAssets();
            LoadTable();
            Bird_of_Paradise_Plant();
            Bonsai_Plant();
            Coffee_Plant_Plant();
            Fiddle_Leaf_Plant();
            Little_House_Plant_Plant();
            Orchid_Plant();
            Spiky_Plant_Plant();
            Umbrella_Palm_Plant();
            Barrel_Cactus_Plant();
            Cactus1_Plant();
            Cactus1Big_Plant();
            Cactus2_Plant();
            Desert_Lily_Plant();
            Desert_Marigold_Plant();
            Everglades_Palm_Plant();
            Hibiscus_Flower_Plant();
            Ivory_Cane_Palm_Plant();
            Lady_Palm_Plant();
            Prickly_Pear_Plant();
            Venus_Flytrap_Plant();
            //plants2
            Aeonium_Plant();
            Aloe_Plant();
            Bromeliad_Plant();
            Ivy1_Plant();
            Ivy1Big_Plant();
            Rubber_Fig_Plant();
            Tall_House_Plant_Plant();
            Vines1_Plant();
            Vines1Big_Plant();
            //plants3
            Aeoniums_Plant();
            Bamboo_Plant();
            Cattail_Plant();
            Coolpot_Plant();
            Hangingplant_Plant();
            Hangingplantbig_Plant();
            Kelp_Plant();
            Kelpbig_Plant();
            Miscplant1_Plant();
            Miscplant2_Plant();
            Orchid2_Plant();
            Pottedcactus1_Plant();
            Pottedcactus2_Plant();
            Rosevine_Plant();
            Smallpottedplant_Plant();
            Tallflowerbush_Plant();
            Vines2_Plant();
            Weirdflowers_Plant();


        }


        private void LoadAssets()
        {
            assetplanter = AssetUtils.LoadAssetBundleFromResources("shovel", typeof(PlantIt).Assembly);
            plants = AssetUtils.LoadAssetBundleFromResources("custompiece_plantsetfixedcolliders", typeof(PlantIt).Assembly);
            plants2 = AssetUtils.LoadAssetBundleFromResources("custompiece_plantset2", typeof(PlantIt).Assembly);
            plants3 = AssetUtils.LoadAssetBundleFromResources("plantset3", typeof(PlantIt).Assembly);

        }

        //assetplanter

        private void LoadTable()
        {
            PieceManager.Instance.AddPieceTable(assetplanter.LoadAsset<GameObject>("_PlantitPieceTable"));
            LoadShovel();

        }

        private void LoadShovel()
        {
            var shovelfab = assetplanter.LoadAsset<GameObject>("Shovel");
            var shovel = new CustomItem(shovelfab, fixReference: true,
                new ItemConfig
                {
                    Amount = 1,
                    CraftingStation = "piece_workbench",
                    RepairStation = "piece_workbench",
                    MinStationLevel = 1,
                    Requirements = new[]
                    {
                        new RequirementConfig { Item = "Wood", Amount = 1}
                    }
                });
            ItemManager.Instance.AddItem(shovel);

        }

        //plants

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


        private void Bonsai_Plant() //done
        {
            var Bonsai_Thing = plants.LoadAsset<GameObject>("custompiece_bonsai");
                                                                                          
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

        private void Coffee_Plant_Plant() //done
        {
            var Coffee_Plant_Thing = plants.LoadAsset<GameObject>("custompiece_coffeeplant"); 
            var Coffee_Plant = new CustomPiece(Coffee_Plant_Thing,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(Coffee_Plant);
        }

        private void Fiddle_Leaf_Plant() //done
        {
            var Fiddle_Leaf_Thing = plants.LoadAsset<GameObject>("custompiece_fiddleleaf");
            var Fiddle_Leaf = new CustomPiece(Fiddle_Leaf_Thing,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(Fiddle_Leaf);
        }

        private void Little_House_Plant_Plant() //done
        {
            var Little_House_Plant_Thing = plants.LoadAsset<GameObject>("custompiece_littlehouseplant");
            var Little_House_Plant = new CustomPiece(Little_House_Plant_Thing,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(Little_House_Plant);
        }

        private void Orchid_Plant() //done
        {
            var Orchid_Thing = plants.LoadAsset<GameObject>("custompiece_orchid");
            var Orchid = new CustomPiece(Orchid_Thing,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(Orchid);
        }

        private void Spiky_Plant_Plant() //done
        {
            var Spiky_Plant_Thing = plants.LoadAsset<GameObject>("custompiece_spikyplant");
            var Spiky_Plant = new CustomPiece(Spiky_Plant_Thing,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(Spiky_Plant);
        }

        private void Umbrella_Palm_Plant() //done
        {
            var Umbrella_Palm_Thing = plants.LoadAsset<GameObject>("custompiece_umbrellapalm");
            var Umbrella_Palm = new CustomPiece(Umbrella_Palm_Thing,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(Umbrella_Palm);
        }


        private void Barrel_Cactus_Plant() //done
        {
            var Barrel_Cactus_Thing = plants.LoadAsset<GameObject>("custompiece_barrelcactus");
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

        private void Cactus1_Plant() //done
        {
            var Cactus1_Thing = plants.LoadAsset<GameObject>("custompiece_cactus1");
            var Cactus1 = new CustomPiece(Cactus1_Thing,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(Cactus1);
        }

        private void Cactus1Big_Plant() //done
        {
            var Cactus1Big_Thing = plants2.LoadAsset<GameObject>("custompiece_cactus1.2");
            var Cactus1Big = new CustomPiece(Cactus1Big_Thing,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(Cactus1Big);
        }


        private void Cactus2_Plant() //done
        {
            var Cactus2_Thing = plants.LoadAsset<GameObject>("custompiece_cactus2");
            var Cactus2 = new CustomPiece(Cactus2_Thing,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(Cactus2);
        }

        private void Desert_Lily_Plant() //done
        {
            var Desert_Lily_Thing = plants.LoadAsset<GameObject>("custompiece_desertlily");
            var Desert_Lily = new CustomPiece(Desert_Lily_Thing,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(Desert_Lily);
        }

        private void Desert_Marigold_Plant() //done
        {
            var Desert_Marigold_Thing = plants.LoadAsset<GameObject>("custompiece_desertmarigold");
            var Desert_Marigold = new CustomPiece(Desert_Marigold_Thing,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(Desert_Marigold);
        }

        private void Everglades_Palm_Plant() //done
        {
            var Everglades_Palm_Thing = plants.LoadAsset<GameObject>("custompiece_evergladespalm");
            var Everglades_Palm = new CustomPiece(Everglades_Palm_Thing,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(Everglades_Palm);
        }

        private void Hibiscus_Flower_Plant() //done
        {
            var Hibiscus_Flower_Thing = plants.LoadAsset<GameObject>("custompiece_hibiscusflower");
            var Hibiscus_Flower = new CustomPiece(Hibiscus_Flower_Thing,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(Hibiscus_Flower);
        }

        private void Ivory_Cane_Palm_Plant() //done
        {
            var Ivory_Cane_Palm_Thing = plants.LoadAsset<GameObject>("custompiece_ivorycanepalm");
            var Ivory_Cane_Palm = new CustomPiece(Ivory_Cane_Palm_Thing,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(Ivory_Cane_Palm);
        }

        private void Lady_Palm_Plant() //done
        {
            var Lady_Palm_Thing = plants.LoadAsset<GameObject>("custompiece_ladypalm");
            var Lady_Palm = new CustomPiece(Lady_Palm_Thing,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(Lady_Palm);
        }

        private void Prickly_Pear_Plant() //done
        {
            var Prickly_Pear_Thing = plants.LoadAsset<GameObject>("custompiece_pricklypear");
            var Prickly_Pear = new CustomPiece(Prickly_Pear_Thing,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(Prickly_Pear);
        }

        private void Venus_Flytrap_Plant() //done
        {
            var Venus_Flytrap_Thing = plants.LoadAsset<GameObject>("custompiece_venusflytrap");
            var Venus_Flytrap = new CustomPiece(Venus_Flytrap_Thing,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(Venus_Flytrap);
        }

        //plants2

        private void Aeonium_Plant() //done
        {
            var Aeonium_Thing = plants2.LoadAsset<GameObject>("custompiece_aeonium");
            var Aeonium = new CustomPiece(Aeonium_Thing,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(Aeonium);
        }

        private void Aloe_Plant() //done
        {
            var Aloe_Thing = plants2.LoadAsset<GameObject>("custompiece_aloe");
            var Aloe = new CustomPiece(Aloe_Thing,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(Aloe);
        }

        private void Bromeliad_Plant() //done
        {
            var Bromeliad_Thing = plants2.LoadAsset<GameObject>("custompiece_bromeliad");
            var Bromeliad = new CustomPiece(Bromeliad_Thing,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(Bromeliad);
        }

       
        private void Ivy1_Plant() //done
        {
            var Ivy1_Thing = plants2.LoadAsset<GameObject>("custompiece_ivy1");
            var Ivy1 = new CustomPiece(Ivy1_Thing,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(Ivy1);
        }

        private void Ivy1Big_Plant() //done
        {
            var Ivy1Big_Thing = plants2.LoadAsset<GameObject>("custompiece_ivy1.2");
            var Ivy1Big = new CustomPiece(Ivy1Big_Thing,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(Ivy1Big);
        }

        private void Rubber_Fig_Plant() //done
        {
            var Rubber_Fig_Thing = plants2.LoadAsset<GameObject>("custompiece_rubberfig");
            var Rubber_Fig = new CustomPiece(Rubber_Fig_Thing,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(Rubber_Fig);
        }

        private void Tall_House_Plant_Plant() //done
        {
            var Tall_House_Plant_Thing = plants2.LoadAsset<GameObject>("custompiece_tallhouseplant");
                                                                                 
            var Tall_House_Plant = new CustomPiece(Tall_House_Plant_Thing,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(Tall_House_Plant);
        }

        private void Vines1_Plant() //done
        {
            var Vines1_Thing = plants2.LoadAsset<GameObject>("custompiece_vines1.2");
                                                                                
            var Vines1 = new CustomPiece(Vines1_Thing,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(Vines1);
        }

        private void Vines1Big_Plant() //done
        {
            var Vines1Big_Thing = plants2.LoadAsset<GameObject>("custompiece_vines1");
                                                                             
            var Vines1Big = new CustomPiece(Vines1Big_Thing,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(Vines1Big);
        }


        //plants3
        private void Aeoniums_Plant() //done
        {
            var Aeoniums_Thing = plants3.LoadAsset<GameObject>("custompiece_aeoniums");
            var Aeoniums = new CustomPiece(Aeoniums_Thing,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(Aeoniums);
        }

        private void Bamboo_Plant() //done
        {
            var Bamboo_Thing = plants3.LoadAsset<GameObject>("custompiece_bamboo");
            var Bamboo = new CustomPiece(Bamboo_Thing,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(Bamboo);
        }


        private void Cattail_Plant() //done
        {
            var Cattail_Thing = plants3.LoadAsset<GameObject>("custompiece_cattail");
            var Cattail = new CustomPiece(Cattail_Thing,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(Cattail);
        }


        private void Coolpot_Plant() //done
        {
            var Coolpot_Thing = plants3.LoadAsset<GameObject>("custompiece_coolpot");
            var Coolpot = new CustomPiece(Coolpot_Thing,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(Coolpot);
        }


        private void Hangingplant_Plant() //done
        {
            var Hangingplant_Thing = plants3.LoadAsset<GameObject>("custompiece_hangingplant");
            var Hangingplant = new CustomPiece(Hangingplant_Thing,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(Hangingplant);
        }

        private void Hangingplantbig_Plant() //done
        {
            var Hangingplantbig_Thing = plants3.LoadAsset<GameObject>("custompiece_hangingplant1.2");
            var Hangingplantbig = new CustomPiece(Hangingplantbig_Thing,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(Hangingplantbig);
        }




        private void Kelp_Plant() //done
        {
            var Kelp_Thing = plants3.LoadAsset<GameObject>("custompiece_kelp");
            var Kelp = new CustomPiece(Kelp_Thing,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(Kelp);
        }

        private void Kelpbig_Plant() //done
        {
            var Kelpbig_Thing = plants3.LoadAsset<GameObject>("custompiece_kelpbig");
            var Kelpbig = new CustomPiece(Kelpbig_Thing,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(Kelpbig);
        }




        private void Miscplant1_Plant() //done
        {
            var Miscplant1_Thing = plants3.LoadAsset<GameObject>("custompiece_miscplant1");
            var Miscplant1 = new CustomPiece(Miscplant1_Thing,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(Miscplant1);
        }


        private void Miscplant2_Plant() //done
        {
            var Miscplant2_Thing = plants3.LoadAsset<GameObject>("custompiece_miscplant2");
            var Miscplant2 = new CustomPiece(Miscplant2_Thing,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(Miscplant2);
        }


        private void Orchid2_Plant() //done
        {
            var Orchid2_Thing = plants3.LoadAsset<GameObject>("custompiece_orchid2");
            var Orchid2 = new CustomPiece(Orchid2_Thing,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(Orchid2);
        }


        private void Pottedcactus1_Plant() //done
        {
            var Pottedcactus1_Thing = plants3.LoadAsset<GameObject>("custompiece_pottedcactus1");
            var Pottedcactus1 = new CustomPiece(Pottedcactus1_Thing,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(Pottedcactus1);
        }


        private void Pottedcactus2_Plant() //done
        {
            var Pottedcactus2_Thing = plants3.LoadAsset<GameObject>("custompiece_pottedcactus2");
            var Pottedcactus2 = new CustomPiece(Pottedcactus2_Thing,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(Pottedcactus2);
        }


        private void Rosevine_Plant() //done
        {
            var Rosevine_Thing = plants3.LoadAsset<GameObject>("custompiece_rosevine");
            var Rosevine = new CustomPiece(Rosevine_Thing,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(Rosevine);
        }


        private void Smallpottedplant_Plant() //done
        {
            var Smallpottedplant_Thing = plants3.LoadAsset<GameObject>("custompiece_smallpottedplant");
            var Smallpottedplant = new CustomPiece(Smallpottedplant_Thing,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(Smallpottedplant);
        }


        private void Tallflowerbush_Plant() //done
        {
            var Tallflowerbush_Thing = plants3.LoadAsset<GameObject>("custompiece_tallflowerbush");
            var Tallflowerbush = new CustomPiece(Tallflowerbush_Thing,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(Tallflowerbush);
        }


        private void Vines2_Plant() //done
        {
            var Vines2_Thing = plants3.LoadAsset<GameObject>("custompiece_vines2");
            var Vines2 = new CustomPiece(Vines2_Thing,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(Vines2);
        }


        private void Weirdflowers_Plant() //done
        {
            var Weirdflowers_Thing = plants3.LoadAsset<GameObject>("custompiece_weirdflowers");
            var Weirdflowers = new CustomPiece(Weirdflowers_Thing,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(Weirdflowers);
        }




        // to here 

    }
}