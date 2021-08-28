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
using System;
using Object = UnityEngine.Object;

namespace PlantIt
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    internal class PlantIt : BaseUnityPlugin
    {
        public const string PluginGUID = "com.jotunn.PlantIt";
        public const string PluginName = "PlantIt";
        public const string PluginVersion = "0.1.2";
        private AssetBundle assetplanter;
        private AssetBundle plants;
        private AssetBundle plants2;
        private AssetBundle plants3;
        private AssetBundle chairs;
        private AssetBundle misc;
        private AssetBundle pottedsucculents;
        private AssetBundle plants4;


        private void Awake()
        {
           // SetupPlacementHooks();
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

            //stumpchairs
            stump1();
            stump2();
            stump3();
            stump4();
            stump5();
            stump6();
            stump7();

            //logbenches
            log1();
            log2();
            log3();
            log4();
            log5();

            stumptable();

            //misc
            GrassBlockS();
            GrassBlockM();
            GrassBlockL();
            DirtBlockS();
            DirtBlockM();
            DirtBlockL();
            BlackPineTree();
          //  GrassBlockSTest();
          //  GrassBlockSTest2();

            //pottedsucculents
            S1();
            S2();
            S3();
            S4();
            S5();
            S6();
            S7();
            S8();
            S9();
            S10();
            S11();
            S12();
            S13();
            S14();
            S15();

            //plants4

            //pots
            Pot1();
            Pot2();
            Pot3();
            Pot4();
            Pot5();
            Pot6();
            Pot7();
            Pot8();
            Pot9();
            Pot10();
            Pot11();
            Pot12();
            Pot13();
            Pot14();

            //vines
            Vines3();
            Vines4();
            Vines5();
            Vines6();
            Vines7();
            Vines8();
            Vines9();
            Vines10();
            Vines11();
            Vines12();
            VinesVanilla();
            VinesVanillaBig();
            VinesVanillaStretched();

        }


     //   private void SetupPlacementHooks()
     //   {
     //       On.Player.UpdatePlacementGhost += OnUpdatePlacementGhost;
     //   }

     //   private void OnUpdatePlacementGhost(On.Player.orig_UpdatePlacementGhost orig, Player self, bool flashGuardStone)
      //  {
      //      orig(self, flashGuardStone);
      //      if (self && self.m_placementMarkerInstance && self.m_buildPieces.name == "_PlantitPieceTable")
      //      {
     //           Object.Destroy(self.m_placementMarkerInstance);
     //       }
     //   }



        private void LoadAssets()
        {
            assetplanter = AssetUtils.LoadAssetBundleFromResources("shovel", typeof(PlantIt).Assembly);
            plants = AssetUtils.LoadAssetBundleFromResources("custompiece_plantsetfixedcolliders", typeof(PlantIt).Assembly);
            plants2 = AssetUtils.LoadAssetBundleFromResources("custompiece_plantset2", typeof(PlantIt).Assembly);
            plants3 = AssetUtils.LoadAssetBundleFromResources("plantset3", typeof(PlantIt).Assembly);
            chairs = AssetUtils.LoadAssetBundleFromResources("stumpsandlogs", typeof(PlantIt).Assembly);
            misc = AssetUtils.LoadAssetBundleFromResources("misc", typeof(PlantIt).Assembly);
            pottedsucculents =  AssetUtils.LoadAssetBundleFromResources("pottedsucculents", typeof(PlantIt).Assembly);
            plants4 = AssetUtils.LoadAssetBundleFromResources("plantset4", typeof(PlantIt).Assembly);

        }

        //assetplanter

        // private void LoadTable()
        // {
        //     PieceManager.Instance.AddPieceTable(assetplanter.LoadAsset<GameObject>("_PlantitPieceTable"));
        //     LoadShovel();

        //  }

      //  private void LoadTable()
      //  {
      //      GameObject tablePrefab = assetplanter.LoadAsset<GameObject>("_PlantitPieceTable");
       //     CustomPieceTable CPT = new CustomPieceTable(tablePrefab);
        //    PieceManager.Instance.AddPieceTable(CPT);

       //     LoadShovel();
      //  }

        private void LoadTable()
        {
            // Add a custom piece table with custom categories
            var table_prefab = assetplanter.LoadAsset<GameObject>("_PlantitPieceTable");
            CustomPieceTable plant_table = new CustomPieceTable(table_prefab,
                new PieceTableConfig
                {
                    CanRemovePieces = true,
                    UseCategories = false,
                    UseCustomCategories = true,
                    CustomCategories = new string[]
                    {
                        "Misc.", "Hanging", "No Pot", "Potted"
                    }
                }
            );
            PieceManager.Instance.AddPieceTable(plant_table);

            LoadShovel();
        }


        private void LoadShovel()
        {
            var shovelfab = assetplanter.LoadAsset<GameObject>("$PlumgaPlantItShovel");
            var shovel = new CustomItem(shovelfab, fixReference: true,
                new ItemConfig
                {
                    Amount = 1,
                    CraftingStation = "piece_workbench",
                    RepairStation = "piece_workbench",
                    MinStationLevel = 1,
                    Requirements = new[]
                    {
                        new RequirementConfig { Item = "Wood", Amount = 1, AmountPerLevel = 1}
                    }
                });
        ItemManager.Instance.AddItem(shovel);

        }



        //plantset4

        //pots

        private void Pot1() //done
        {
            var p1fab = plants4.LoadAsset<GameObject>("custompiece_fancypot");
            var p1 = new CustomPiece(p1fab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Misc.",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(p1);
        }

        private void Pot2() //done
        {
            var p2fab = plants4.LoadAsset<GameObject>("custompiece_fancypotL");
            var p2 = new CustomPiece(p2fab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Misc.",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(p2);
        }


        private void Pot3() //done
        {
            var p3fab = plants4.LoadAsset<GameObject>("custompiece_hangingpot_large");
            var p3 = new CustomPiece(p3fab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Misc.",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(p3);
        }


        private void Pot4() //done
        {
            var p4fab = plants4.LoadAsset<GameObject>("custompiece_hangingpot_small");
            var p4 = new CustomPiece(p4fab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Misc.",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(p4);
        }


        private void Pot5() //done
        {
            var p5fab = plants4.LoadAsset<GameObject>("custompiece_TCpot1");
            var p5 = new CustomPiece(p5fab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Misc.",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(p5);
        }


        private void Pot6() //done
        {
            var p6fab = plants4.LoadAsset<GameObject>("custompiece_TCpot1L");
            var p6 = new CustomPiece(p6fab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Misc.",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(p6);
        }


        private void Pot7() //done
        {
            var p7fab = plants4.LoadAsset<GameObject>("custompiece_TCpot2");
            var p7 = new CustomPiece(p7fab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Misc.",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(p7);
        }


        private void Pot8() //done
        {
            var p8fab = plants4.LoadAsset<GameObject>("custompiece_TCpot2L");
            var p8 = new CustomPiece(p8fab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Misc.",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(p8);
        }


        private void Pot9() //done
        {
            var p9fab = plants4.LoadAsset<GameObject>("custompiece_TCpot3");
            var p9 = new CustomPiece(p9fab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Misc.",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(p9);
        }


        private void Pot10() //done
        {
            var p10fab = plants4.LoadAsset<GameObject>("custompiece_TCpot3L");
            var p10 = new CustomPiece(p10fab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Misc.",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(p10);
        }


        private void Pot11() //done
        {
            var p11fab = plants4.LoadAsset<GameObject>("custompiece_TCpotrectangle");
            var p11 = new CustomPiece(p11fab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Misc.",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(p11);
        }


        private void Pot12() //done
        {
            var p12fab = plants4.LoadAsset<GameObject>("custompiece_TCpotrectangleL");
            var p12 = new CustomPiece(p12fab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Misc.",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(p12);
        }


        private void Pot13() //done
        {
            var p13fab = plants4.LoadAsset<GameObject>("custompiece_TCpotsquare");
            var p13 = new CustomPiece(p13fab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Misc.",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(p13);
        }


        private void Pot14() //done
        {
            var p14fab = plants4.LoadAsset<GameObject>("custompiece_TCpotsquareL");
            var p14 = new CustomPiece(p14fab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Misc.",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(p14);
        }



        //vines

        private void Vines3() //done
        {
            var v3fab = plants4.LoadAsset<GameObject>("custompiece_vines3");
            var v3 = new CustomPiece(v3fab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Hanging",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(v3);
        }

        private void Vines4() //done
        {
            var v4fab = plants4.LoadAsset<GameObject>("custompiece_vines4");
            var v4 = new CustomPiece(v4fab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Hanging",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(v4);
        }

        private void Vines5() //done
        {
            var v5fab = plants4.LoadAsset<GameObject>("custompiece_vines5");
            var v5 = new CustomPiece(v5fab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Hanging",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(v5);
        }

        private void Vines6() //done
        {
            var v6fab = plants4.LoadAsset<GameObject>("custompiece_vines6");
            var v6 = new CustomPiece(v6fab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Hanging",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(v6);
        }

        private void Vines7() //done
        {
            var v7fab = plants4.LoadAsset<GameObject>("custompiece_vines7");
            var v7 = new CustomPiece(v7fab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Hanging",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(v7);
        }

        private void Vines8() //done
        {
            var v8fab = plants4.LoadAsset<GameObject>("custompiece_vines8");
            var v8 = new CustomPiece(v8fab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "No Pot",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(v8);
        }

        private void Vines9() //done
        {
            var v9fab = plants4.LoadAsset<GameObject>("custompiece_vines9");
            var v9 = new CustomPiece(v9fab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "No Pot",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(v9);
        }

        private void Vines10() //done
        {
            var v10fab = plants4.LoadAsset<GameObject>("custompiece_vines10");
            var v10 = new CustomPiece(v10fab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Hanging",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(v10);
        }

        private void Vines11() //done
        {
            var v11fab = plants4.LoadAsset<GameObject>("custompiece_vines11");
            var v11 = new CustomPiece(v11fab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Hanging",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(v11);
        }

        private void Vines12() //done
        {
            var v12fab = plants4.LoadAsset<GameObject>("custompiece_vines12");
            var v12 = new CustomPiece(v12fab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Hanging",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(v12);
        }


        private void VinesVanilla() //done
        {
            var vvfab = plants4.LoadAsset<GameObject>("custompiece_vinesvanilla");
            var vv = new CustomPiece(vvfab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Hanging",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(vv);
        }

        private void VinesVanillaBig() //done
        {
            var vvbfab = plants4.LoadAsset<GameObject>("custompiece_vinesvanillabig");
            var vvb = new CustomPiece(vvbfab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Hanging",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(vvb);
        }

        private void VinesVanillaStretched() //done
        {
            var vvsfab = plants4.LoadAsset<GameObject>("custompiece_vinesvanillastretched");
            var vvs = new CustomPiece(vvsfab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Hanging",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(vvs);
        }




        //plants


        private void Bird_of_Paradise_Plant() //done
        {
            var Bird_of_Paradise_Thing = plants.LoadAsset<GameObject>("custompiece_birdofparadise");
            var Bird_of_Paradise = new CustomPiece(Bird_of_Paradise_Thing,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Potted", 
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
                    Category = "Potted", 
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
                    Category = "Potted", 
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
                    Category = "Potted", 
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
                    Category = "Potted", 
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
                    Category = "Potted", 
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
                    Category = "Potted", 
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
                    Category = "Potted", 
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
                    Category = "No Pot", 
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
                    Category = "No Pot", 
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
            var Cactus1Big_Thing = plants.LoadAsset<GameObject>("custompiece_cactus1.2");
            var Cactus1Big = new CustomPiece(Cactus1Big_Thing,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "No Pot", 
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
                    Category = "No Pot", 
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
                    Category = "No Pot", 
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
                    Category = "No Pot", 
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
                    Category = "No Pot", 
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
                    Category = "No Pot", 
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
                    Category = "No Pot", 
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
                    Category = "No Pot", 
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
                    Category = "No Pot", 
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
                    Category = "No Pot", 
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
                    Category = "No Pot", 
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
                    Category = "No Pot", 
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
                    Category = "No Pot", 
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
                    Category = "No Pot", 
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
                    Category = "No Pot", 
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
                    Category = "Potted", 
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
                    Category = "Potted", 
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
                    Category = "Hanging", 
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
                    Category = "Hanging", 
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
                    Category = "Potted", 
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
                    Category = "Potted", 
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
                    Category = "No Pot", 
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
                    Category = "Potted", 
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
                    Category = "Hanging", 
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
                    Category = "Hanging", 
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
                    Category = "No Pot", 
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
                    Category = "No Pot", 
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
                    Category = "Potted", 
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
                    Category = "Potted", 
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
                    Category = "Potted", 
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
                    Category = "Potted", 
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
                    Category = "Potted", 
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
                    Category = "No Pot", 
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
                    Category = "Potted", 
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
                    Category = "No Pot", 
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
                    Category = "No Pot", 
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
                    Category = "Potted", 
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(Weirdflowers);
        }



        //stumpchairs
        private void stump1()
        {
            var stump1fab = chairs.LoadAsset<GameObject>("$custompiece_stump1");

            var stump1 = new CustomPiece(stump1fab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Misc.", 
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(stump1);
        }

        private void stump2()
        {
            var stump2fab = chairs.LoadAsset<GameObject>("$custompiece_stump2");

            var stump2 = new CustomPiece(stump2fab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Misc.", 
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(stump2);
        }

        private void stump3()
        {
            var stump3fab = chairs.LoadAsset<GameObject>("$custompiece_stump3");

            var stump3 = new CustomPiece(stump3fab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Misc.", 
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(stump3);
        }

        private void stump4()
        {
            var stump4fab = chairs.LoadAsset<GameObject>("$custompiece_stump4");

            var stump4 = new CustomPiece(stump4fab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Misc.", 
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(stump4);
        }

        private void stump5()
        {
            var stump5fab = chairs.LoadAsset<GameObject>("$custompiece_stump5");

            var stump5 = new CustomPiece(stump5fab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Misc.", 
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(stump5);
        }

        private void stump6()
        {
            var stump6fab = chairs.LoadAsset<GameObject>("$custompiece_stump6");

            var stump6 = new CustomPiece(stump6fab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Misc.", 
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(stump6);
        }

        private void stump7()
        {
            var stump7fab = chairs.LoadAsset<GameObject>("$custompiece_stump7");

            var stump7 = new CustomPiece(stump7fab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Misc.", 
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(stump7);
        }


        //logbenches
        private void log1()
        {
            var logfab = chairs.LoadAsset<GameObject>("$custompiece_log1");

            var log = new CustomPiece(logfab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Misc.", 
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 5, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(log);
        }

        private void log2()
        {
            var log2fab = chairs.LoadAsset<GameObject>("$custompiece_log2");

            var log2 = new CustomPiece(log2fab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Misc.", 
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 5, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(log2);
        }

        private void log3()
        {
            var log3fab = chairs.LoadAsset<GameObject>("$custompiece_log3");

            var log3 = new CustomPiece(log3fab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Misc.", 
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 5, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(log3);
        }

        private void log4()
        {
            var log4fab = chairs.LoadAsset<GameObject>("$custompiece_log4");

            var log4 = new CustomPiece(log4fab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Misc.", 
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 5, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(log4);
        }

        private void log5()
        {
            var log5fab = chairs.LoadAsset<GameObject>("$custompiece_log5");

            var log5 = new CustomPiece(log5fab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Misc.", 
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 5, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(log5);
        }

        //stumptable
        private void stumptable()
        {
            var stfab = chairs.LoadAsset<GameObject>("$custompiece_stumptable");

            var st = new CustomPiece(stfab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Misc.", 
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 10, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(st);
        }



        //misc
        private void GrassBlockS()
        {
            var gbsfab = misc.LoadAsset<GameObject>("$custompiece_grassblock_small");

            var gbs = new CustomPiece(gbsfab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Misc.", 
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(gbs);
        }

        private void GrassBlockM()
        {
            var gbmfab = misc.LoadAsset<GameObject>("$custompiece_grassblock_medium");

            var gbm = new CustomPiece(gbmfab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Misc.",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 5, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(gbm);
        }

        private void GrassBlockL()
        {
            var gblfab = misc.LoadAsset<GameObject>("$custompiece_grassblock_large");

            var gbl = new CustomPiece(gblfab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Misc.",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 10, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(gbl);
        }


        private void DirtBlockS()
        {
            var dbsfab = misc.LoadAsset<GameObject>("$custompiece_dirtblock_small");

            var dbs = new CustomPiece(dbsfab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Misc.", 
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(dbs);
        }

        private void DirtBlockM()
        {
            var dbmfab = misc.LoadAsset<GameObject>("$custompiece_dirtblock_medium");

            var dbm = new CustomPiece(dbmfab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Misc.",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 5, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(dbm);
        }

        private void DirtBlockL()
        {
            var dblfab = misc.LoadAsset<GameObject>("$custompiece_dirtblock_large");

            var dbl = new CustomPiece(dblfab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Misc.",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 10, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(dbl);
        }



        private void BlackPineTree()
        {
            var bptfab = misc.LoadAsset<GameObject>("$custompiece_blackpine");

            var bpt = new CustomPiece(bptfab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "No Pot", 
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 10, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(bpt);
        }


        private void GrassBlockSTest()
        {
            var gbstfab = misc.LoadAsset<GameObject>("$custompiece_grassblock_small_test");

            var gbst = new CustomPiece(gbstfab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Misc.",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(gbst);
        }

        private void GrassBlockSTest2()
        {
            var gbst2fab = misc.LoadAsset<GameObject>("$custompiece_grassblock_small_test2");

            var gbst2 = new CustomPiece(gbst2fab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Misc.",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(gbst2);
        }


        //pottedsucculents


        private void S1()
        {
            var S1fab = pottedsucculents.LoadAsset<GameObject>("$custompiece_s1_roundcactuspinkflowers");

            var S1 = new CustomPiece(S1fab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Potted",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(S1);
        }

        private void S2()
        {
            var S2fab = pottedsucculents.LoadAsset<GameObject>("$custompiece_s2_tallcactusredflower");

            var S2 = new CustomPiece(S2fab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Potted",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(S2);
        }


        private void S3()
        {
            var S3fab = pottedsucculents.LoadAsset<GameObject>("$custompiece_s3_pottedpricklypear");

            var S3 = new CustomPiece(S3fab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Potted",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(S3);
        }


        private void S4()
        {
            var S4fab = pottedsucculents.LoadAsset<GameObject>("$custompiece_s4_snakeplant");

            var S4 = new CustomPiece(S4fab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Potted",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(S4);
        }


        private void S5()
        {
            var S5fab = pottedsucculents.LoadAsset<GameObject>("$custompiece_s5_aloe");

            var S5 = new CustomPiece(S5fab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Potted",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(S5);
        }


        private void S6()
        {
            var S6fab = pottedsucculents.LoadAsset<GameObject>("$custompiece_s6_pachy");

            var S6 = new CustomPiece(S6fab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Potted",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(S6);
        }


        private void S7()
        {
            var S7fab = pottedsucculents.LoadAsset<GameObject>("$custompiece_s7_calisunset");

            var S7 = new CustomPiece(S7fab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Potted",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(S7);
        }


        private void S8()
        {
            var S8fab = pottedsucculents.LoadAsset<GameObject>("$custompiece_s8_towercactus");

            var S8 = new CustomPiece(S8fab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Potted",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(S8);
        }


        private void S9()
        {
            var S9fab = pottedsucculents.LoadAsset<GameObject>("$custompiece_s9_whiteflowers");

            var S9 = new CustomPiece(S9fab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Potted",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(S9);
        }


        private void S10()
        {
            var S10fab = pottedsucculents.LoadAsset<GameObject>("$custompiece_s10_fatboi");

            var S10 = new CustomPiece(S10fab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Potted",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(S10);
        }


        private void S11()
        {
            var S11fab = pottedsucculents.LoadAsset<GameObject>("$custompiece_s11_cLehmannii");

            var S11 = new CustomPiece(S11fab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Potted",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(S11);
        }


        private void S12()
        {
            var S12fab = pottedsucculents.LoadAsset<GameObject>("$custompiece_s12_splitrocks");

            var S12 = new CustomPiece(S12fab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Potted",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(S12);
        }


        private void S13()
        {
            var S13fab = pottedsucculents.LoadAsset<GameObject>("$custompiece_s13_roundcactusyellowflowers");

            var S13 = new CustomPiece(S13fab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Potted",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(S13);
        }


        private void S14()
        {
            var S14fab = pottedsucculents.LoadAsset<GameObject>("$custompiece_s14_roseaeonium");

            var S14 = new CustomPiece(S14fab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Potted",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(S14);
        }


        private void S15()
        {
            var S15fab = pottedsucculents.LoadAsset<GameObject>("$custompiece_s15_euphorbia");

            var S15 = new CustomPiece(S15fab,
                new PieceConfig
                {
                    PieceTable = "_PlantitPieceTable",
                    Category = "Potted",
                    AllowedInDungeons = false,
                    Requirements = new[]
                    {
                        new RequirementConfig {Item = "Wood", Amount = 2, Recover = true}
                    }
                });
            PieceManager.Instance.AddPiece(S15);
        }




        // to here 

    }
}