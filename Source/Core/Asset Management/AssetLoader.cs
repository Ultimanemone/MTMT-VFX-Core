using BrilliantSkies.Core.Logger;
using BrilliantSkies.Modding;
using BrilliantSkies.Modding.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace MTMTVFX.Core.AssetManagement
{
    public static class AssetLoader
    {
        /// <summary>
        /// Load default assets, patch this to load your assets
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, GameObject> GetDefault()
        {
            return new Dictionary<string, GameObject>();
        }

        /// <summary>
        /// Load all assets from a bundle
        /// </summary>
        /// <param name="modName">The mod providing the assets</param>
        /// <param name="guid">The guid of the bundle</param>
        /// <returns>A dictionary of prefabs keyed by their names</returns>
        public static Dictionary<string, GameObject> LoadAllAssetsFromBundle(Guid guid)
        {
            AssetBundleDefinition? bundle = Configured.i.AssetBundles.Find(guid);

            if (bundle == null)
            {
                Utils.LogError<CorePlugin>($"AssetBundle [{guid}] not found...", LogOptions.Popup);
                return default;
            }
            else
            {
                string[] assetNames;
                bundle.Loader.GetAllAssetNames(out assetNames);

                string[] prefabNames = assetNames
                    .Where(name => name.EndsWith(".prefab", System.StringComparison.OrdinalIgnoreCase))
                    .Select(name => Path.GetFileNameWithoutExtension(name))
                    .ToArray();

                if (prefabNames.Length < 1)
                {
                    Utils.LogError<CorePlugin>($"AssetBundle [{guid}] has no prefabs to load!", LogOptions.Popup);
                    return default;
                }
                else
                {
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
            }
        }

        public static void RemapAssetDictionary(Dictionary<string, string> nameMap, Dictionary<string, GameObject> dict)
        {
            foreach (var entry in nameMap)
            {
                dict[entry.Key] = dict[entry.Value];
            }
        }

        public static bool TryLoadAsset(string name, string modName, Guid guid, out GameObject asset)
        {
            AssetBundleDefinition? bundle = Configured.i.AssetBundles.Find(guid);

            if (bundle == null)
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

        private static bool GetAsset(string name, AssetBundleDefinition assetBundleDef, out GameObject asset)
        {
            if (assetBundleDef == null)
            {
                asset = null;
                return false;
            }

            return assetBundleDef.Loader.GetThing(name, out asset);
        }
    }
}
