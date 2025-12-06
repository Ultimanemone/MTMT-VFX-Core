using System.Collections.Generic;
using UnityEngine;

namespace MTMTVFX.Core
{
    public struct AssetContainer
    {
        public GameObject prefab;
        public int priority;
        public string source;
    }

    public class AssetRegistry
    {
        public static IReadOnlyDictionary<string, AssetContainer> assetList => registry;
        private static Dictionary<string, AssetContainer> registry = new Dictionary<string, AssetContainer>();
        private static bool _init = false;

        private AssetRegistry() { }

        public static void Init()
        {
            if (!_init)
            {
                Register(AssetLoader.GetDefault(), -1, "MTMT_VFXCore.Default");
                _init = true;
            }
        }

        /// <summary>
        /// Register one VFX
        /// </summary>
        /// <param name="key"></param>
        /// <param name="prefab"></param>
        /// <param name="priority"></param>
        /// <param name="source"></param>
        public static void Register(string key, GameObject prefab, int priority, string source)
        {
            key.Trim().ToLowerInvariant();
            if (registry.TryGetValue(key, out AssetContainer currentAsset))
            {
                if (currentAsset.priority > priority)
                {
                    registry[key] = new AssetContainer()
                    {
                        prefab = prefab,
                        priority = priority,
                        source = source
                    };
                }
                else return;
            }

            registry[key] = new AssetContainer()
            {
                prefab = prefab,
                priority = priority,
                source = source
            };
        }

        /// <summary>
        /// Register many VFX
        /// </summary>
        /// <param name="assets"></param>
        /// <param name="priority"></param>
        /// <param name="source"></param>
        public static void Register(Dictionary<string, GameObject> assets, int priority, string source)
        {
            foreach (KeyValuePair<string, GameObject> asset in assets)
            {
                Register(asset.Key, asset.Value, priority, source);
            }
        }

        /// <summary>
        /// Get a VFX from the registry
        /// </summary>
        /// <param name="type"></param>
        /// <param name="effect"></param>
        /// <returns></returns>
        public static bool TryGetAsset(string type, out GameObject effect)
        {
            bool flag = registry.TryGetValue(type, out AssetContainer container);
            if (flag) effect = container.prefab;
            else effect = null;
            return flag;
        }
    }
}
