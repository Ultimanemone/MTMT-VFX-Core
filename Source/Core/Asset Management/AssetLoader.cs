using BrilliantSkies.Core.Logger;
using BrilliantSkies.Modding;
using BrilliantSkies.Modding.Types;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

namespace MTMTVFX.Core.AssetManagement
{
    public class AssetLoader
    {
        private static Dictionary<string, GameObject> assets;
        private static Guid loadedBundle;

        /// <summary>
        /// Load default assets
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, GameObject> GetDefault()
        {
            return new Dictionary<string, GameObject>();
        }

        /// <summary>
        /// Load all assets from the provided guid of the assetbundle
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static Dictionary<string, GameObject> GetAllAssets(Guid guid)
        {
            if (guid == loadedBundle)
            {
                Utils.LogError<CorePlugin>($"AssetBundle [{guid}] already loaded!");
                return assets;
            }

            AssetBundleDefinition? bundle = Configured.i.AssetBundles.Find(guid);

            bool flag = bundle == null;
            if (flag)
            {
                Utils.LogError<CorePlugin>($"AssetBundle [{guid}] not found...", LogOptions.Popup);
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
                Utils.LogError<CorePlugin>($"AssetBundle [{guid}] is empty!", LogOptions.Popup);
                return null;
            }

            loadedBundle = guid;
            Dictionary<string, GameObject> assetsReturn = new Dictionary<string, GameObject>();

            foreach (string name in prefabNames)
            {
                if (!assetsReturn.ContainsKey(name))
                {
                    bool flag1 = GetAsset(name, bundle, out GameObject asset);
                    if (flag1)
                    {
                        assetsReturn.Add(name, asset);
                        Utils.LogInfo<CorePlugin>($"Asset [{name}] loaded!");
                    }
                    else
                    {
                        Utils.LogError<CorePlugin>($"Asset [{name}] not found!");
                    }
                }
                else
                {
                    Utils.LogError<CorePlugin>($"Duplicate asset [{name}]");
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
                Utils.LogError<CorePlugin>($"AssetBundle {guid} not found", LogOptions.Popup);
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
