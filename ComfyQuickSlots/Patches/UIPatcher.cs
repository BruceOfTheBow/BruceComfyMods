using BepInEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using static ComfyQuickSlots.ComfyQuickSlots;

namespace ValheimInventorySlots {
    [BepInPlugin(FixGuiGuid, FixGuiName, FixVersion)]
    public class FixGuiFrame : BaseUnityPlugin {
        public const string FixGuiGuid = "8F0F9F2B-E481-4BEC-B641-01228D7508F9";
        public const string FixGuiName = "MoreSlotsPatcherForComfyQuickslots";
        public const string FixVersion = "1.1.0";

        GameObject inventoryScreenObjectRoot;
        Transform inventoryScreenObject;
        Transform containerScreenObject;
        float bgYOffset = 0;
        float containerYOffset = 0;
        Coroutine screenSearch;


        void OnEnable() {
            screenSearch = StartCoroutine(screenSearchCoroutine());
        }

        void OnDisable() {
            StopCoroutine(screenSearch);
        }

        public System.Collections.IEnumerator screenSearchCoroutine() {
            while (enabled) {
                if (inventoryScreenObjectRoot == null || inventoryScreenObject == null || containerScreenObject == null) {
                    bgYOffset = 0;
                    containerYOffset = 0;
                    inventoryScreenObjectRoot = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == "Inventory_screen").Where(obj => obj.transform.position.x != 0).SingleOrDefault();
                    if (inventoryScreenObjectRoot != null) {
                        yield return new WaitForSecondsRealtime(1f);
                        inventoryScreenObject = inventoryScreenObjectRoot.transform.Find("root/Player/Bkg");
                        containerScreenObject = inventoryScreenObjectRoot.transform.Find("root/Container");
                      }

          yield return new WaitForSecondsRealtime(5f);
                } else {
                    if (bgYOffset == 0) {

                        containerYOffset = inventoryScreenObject.transform.localPosition.y * (((float)rows - 4f) * 1.8125f);
                        containerScreenObject.transform.localPosition = new Vector3(containerScreenObject.transform.localPosition.x, inventoryScreenObject.transform.localPosition.y - containerYOffset, containerScreenObject.transform.localPosition.z);
                        // Some sane default
                        float sleepSeconds = 30f;
                        yield return new WaitForSecondsRealtime(sleepSeconds);
                    }
                }
                yield return 0;
            }
        }

    }
}