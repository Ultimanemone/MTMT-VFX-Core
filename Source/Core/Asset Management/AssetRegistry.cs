using MonoMod.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MTMTVFX.Core.AssetManagement
{
    public struct AssetDetail
    {
        public string assetName;
        public string sourceModName;

        public AssetDetail(string assetName, string sourceModName)
        {
            this.assetName = assetName;
            this.sourceModName = sourceModName;
        }
    }

    public enum AssetType
    {
        none,

        muzzle,
        railgun,
        explosion,
        pulse_laser,
        pac,
        plasma,
        flamer,
        cont_laser,

        aps_trail,
        cram_trail,
        plasma_trail,
        missile_trail,

        aps_model,
        cram_model,
        plasma_model,
        missile_model
    }

    public static class AssetRegistry
    {
        public static IReadOnlyDictionary<AssetDetail, GameObject> assetList => _registry;
        private static readonly Dictionary<AssetDetail, GameObject> _registry = new Dictionary<AssetDetail, GameObject>();
        private static bool _init = false;

        public static void Init()
        {
            if (!_init)
            {
                Register(AssetLoader.GetDefault(), "Default");
                _init = true;
            }
        }

        /// <summary>
        /// Register one VFX
        /// </summary>
        /// <param name="assetName">The name of the asset, this should match the pattern provided by MTMT VFX</param>
        /// <param name="sourceModName">The name of the mod providing the asset</param>
        public static void Register(string assetName, string sourceModName, GameObject prefab)
        {
            _registry[new AssetDetail(assetName, sourceModName)] = prefab;
        }

        /// <summary>
        /// Register one VFX
        /// </summary>
        /// <param name="assetDetail">The details of the asset and mod providing it</param>
        public static void Register(AssetDetail assetDetail, GameObject prefab)
        {
            _registry[assetDetail] = prefab;
        }

        /// <summary>
        /// Register a dictionary of VFXs
        /// </summary>
        /// <param name="assets">The dictionary of assets to register</param>
        /// <param name="sourceModName">The name of the mod providing the assets</param>
        public static void Register(Dictionary<string, GameObject> assets, string sourceModName)
        {
            foreach (KeyValuePair<string, GameObject> asset in assets)
            {
                Register(asset.Key, sourceModName, asset.Value);
            }
        }

        /// <summary>
        /// Register a dictionary of VFXs
        /// </summary>
        /// <param name="assets">The dictionary of assets to register</param>
        public static void Register(Dictionary<AssetDetail, GameObject> assets)
        {
            _registry.AddRange(assets);
        }

        /// <summary>
        /// Get a VFX from the _registry
        /// </summary>
        /// <param name="assetName">Type of VFX</param>
        /// <param name="prefab">Name of the VFX</param>
        /// <param name="sourceModName">The name of the mod providing the asset</param>
        /// <returns></returns>
        public static bool TryGetAsset(string assetName, AssetType type, out GameObject prefab, out string sourceModName)
        {
            sourceModName = Utils.GetModNameFromAssetType(type);
            return _registry.TryGetValue(new AssetDetail(assetName, sourceModName), out prefab);
        }
    }
}
