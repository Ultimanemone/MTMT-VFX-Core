using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MTMTVFX.Core
{
    public class VFXRegistry
    {
        public IReadOnlyDictionary<string, GameObject> assetList => registry;
        private static Dictionary<string, GameObject> registry = new Dictionary<string, GameObject>();
        private static bool _init = false;
        
        public static void Init()
        {
            if (!_init)
            {
                registry = CorePlugin.GetDefaultAssets();
                _init = true;
            }
        }

        /// <summary>
        /// Register one VFX
        /// </summary>
        /// <param name="type"></param>
        /// <param name="prefab"></param>
        public static void Register(string type, GameObject prefab)
        {
            registry[type] = prefab;
        }

        /// <summary>
        /// Register many VFX
        /// </summary>
        /// <param name="assets"></param>
        public static void Register(Dictionary<string, GameObject> assets)
        {
            foreach (KeyValuePair<string, GameObject> asset in assets)
            {
                Register(asset.Key, asset.Value);
            }
        }

        /// <summary>
        /// Get a VFX from the registry
        /// </summary>
        /// <param name="type"></param>
        /// <param name="effect"></param>
        /// <returns></returns>
        public static bool TryGetEffect(string type, out GameObject effect)
        {
            return registry.TryGetValue(type, out effect);
        }
    }
}
