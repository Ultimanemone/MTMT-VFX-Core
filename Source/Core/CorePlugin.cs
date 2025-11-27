using BrilliantSkies.Core.ChangeControl;
using BrilliantSkies.Core.Logger;
using BrilliantSkies.Core.Unity;
using BrilliantSkies.Modding;
using BrilliantSkies.Modding.Types;
using HarmonyLib;
using System;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System.Linq;
using System.IO;

namespace MTMTVFX.Core
{
    public class CorePlugin : GamePlugin_PostLoad
    {
        public string name { get { return "MTMT_VFXCore"; } }
        public Version version { get { return new Version(CorePlugin.ver); } }

        public const string guid = "a2c90d0a-ce1a-4788-a691-8085c5c202ab"; // temporary guid solution
        private static Dictionary<string, GameObject> assets;
        private static List<Guid> loadedBundles;
        public static string ver = "1.0.0";

        public void OnLoad()
        {
            assets = new Dictionary<string, GameObject>();
            loadedBundles = new List<Guid>();
            new Harmony("MTMT_VFX_CORE").PatchAll();
            ModProblems.AddModProblem($"{name} v{ver} active!", Assembly.GetExecutingAssembly().Location, string.Empty, false);
        }

        public void OnSave() { }

        public bool AfterAllPluginsLoaded() => true;

        public static Dictionary<string, GameObject> GetDefaultAssets()
        {
            return GetAllAssets(new Guid(guid));
        }

        /// <summary>
        /// Load all VFX assets from your mod
        /// </summary>
        /// <param name="guid"></param>
        public static void LoadAllAssetsExternal(Guid guid)
        {
            VFXRegistry.Register(GetAllAssets(guid));
        }

        private static Dictionary<string, GameObject> GetAllAssets(Guid guid)
        {
            if (loadedBundles.Contains(guid))
            {
                Util.LogError<CorePlugin>($"AssetBundle [{guid}] already loaded!");
                return assets;
            }

            AssetBundleDefinition? bundle = Configured.i.AssetBundles.Find(guid);

            bool flag = bundle == null;
            if (flag)
            {
                Util.LogError<CorePlugin>($"AssetBundle [{guid}] not found...", LogOptions.Popup);
                return null;
            }

            string[] assetNames;
            bundle.Loader.GetAllAssetNames(out assetNames);

            string[] prefabNames = assetNames
                .Where(name => name.EndsWith(".prefab", System.StringComparison.OrdinalIgnoreCase))
                .Select(name => Path.GetFileNameWithoutExtension(name))
                .ToArray();

            if (prefabNames.Length < 1)
            {
                Util.LogError<CorePlugin>($"AssetBundle [{guid}] is empty!", LogOptions.Popup);
                return null;
            }

            loadedBundles.Add(guid);
            Dictionary<string, GameObject> assetsReturn = new Dictionary<string, GameObject>();

            foreach (string name in prefabNames)
            {
                GameObject asset = new GameObject();
                bool flag1 = GetAsset(name, bundle, out asset);
                if (flag1)
                {
                    assetsReturn.Add(name, asset);
                    Util.LogInfo<CorePlugin>($"Asset [{name}] loaded!");
                }
                else
                {
                    Util.LogError<CorePlugin>($"Asset [{name}] not found!");
                }
            }

            return assetsReturn;
        }

        private static bool GetAsset(string name, AssetBundleDefinition assetBundleDef, out GameObject asset)
        {
            if (assetBundleDef == null)
            {
                asset = null;
                return false;
            }

            return assetBundleDef.Loader.GetThing(name, out asset);
        }

        private static bool GetAsset(string name, Guid guid, out GameObject asset)
        {
            AssetBundleDefinition? bundle = Configured.i.AssetBundles.Find(guid);

            bool flag = bundle == null;
            if (flag)
            {
                Util.LogError<CorePlugin>($"AssetBundle {guid} not found", LogOptions.Popup);
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