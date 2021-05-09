// JotunnModStub
// a Valheim mod skeleton using Jötunn
// 
// File:    JotunnModStub.cs
// Project: JotunnModStub

using BepInEx;
using UnityEngine;
using BepInEx.Configuration;
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

        private void Awake()
        {
           

        }

    }
}