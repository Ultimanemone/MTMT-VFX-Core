using BrilliantSkies.Core.ChangeControl;
using BrilliantSkies.Core.Logger;
using BrilliantSkies.Core.Unity;
using BrilliantSkies.Modding;
using BrilliantSkies.Modding.Types;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace MTMTVFX.Core
{
    // Token: 0x0200002B RID: 43
    public class CorePlugin : GamePlugin_PostLoad
    {
        public string name { get { return "BMEffects_Remaster"; } }
        public Version version { get { return new Version(CorePlugin.ver); } }

        //public static BM_EffectUpdater updater = null;
        public const string guid = "26d4fb8e-c3bb-4860-b2c1-7530c30199d5";
        public static Dictionary<string, GameObject> Assets = new Dictionary<string, GameObject>();
        public static string ver = "1.0.0";

        private CorePlugin() { }

        public void OnLoad()
        {
            new Harmony("MTMT_VFX_CORE").PatchAll();
            ModProblems.AddModProblem($"{name} v{ver} active!", Assembly.GetExecutingAssembly().Location, string.Empty, false);
        }

        public void OnSave() { }

        public bool AfterAllPluginsLoaded() => true;

        public static Dictionary<string, GameObject> GetAllAssets()
        {
            AssetBundleDefinition? bundle = Configured.i.AssetBundles.Find(new Guid(guid));

            bool flag = bundle == null;
            if (flag)
            {
                AdvLogger.LogError($"MTMTVFX : AssetBundle {guid} not found", LogOptions.Popup);
                return null;
            }

            string[] assetNames;
            bundle.Loader.GetAllAssetNames(out assetNames);
            Dictionary<string, GameObject> assets = new Dictionary<string, GameObject>();
            foreach (string name in assetNames)
            {
                GameObject asset = new GameObject();
                bool flag1 = GetAsset(name, bundle, out asset);
                if (flag1)
                {
                    assets.Add(name, asset);
                }
                else
                {
                    AdvLogger.LogError($"MTMTVFX : Asset {name} not found??", LogOptions.Popup);
                    break;
                }
            }

            return assets;
        }

        public static bool GetAsset(string name, AssetBundleDefinition assetBundleDef, out GameObject asset)
        {
            if (assetBundleDef == null)
            {
                asset = null;
                return false;
            }

            return assetBundleDef.Loader.GetThing(name, out asset);
        }

        public static bool GetAsset(string name, out GameObject asset)
        {
            AssetBundleDefinition? bundle = Configured.i.AssetBundles.Find(new Guid(guid));

            bool flag = bundle == null;
            if (flag)
            {
                AdvLogger.LogError($"MTMTVFX : AssetBundle {guid} not found", LogOptions.Popup);
                asset = null;
                return false;
            }
            else
            {
                return bundle.Loader.GetThing(name, out asset);
            }
        }
    }
}