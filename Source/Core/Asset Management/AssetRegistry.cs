using MonoMod.Utils;
using static MTMTVFX.Core.AssetType;
using System;
using System.Collections.Generic;
using UnityEngine;
using MTMTVFX.UI;
using System.Linq;

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

    public static class AssetRegistry
    {
        public static Action LoadPacks;
        public static IReadOnlyDictionary<AssetDetail, GameObject> AssetList => _registry;
        public static IReadOnlyDictionary<string, HashSet<string>> LoadedMods => _loadedMods;
        
        private static readonly Dictionary<AssetDetail, GameObject> _registry = new Dictionary<AssetDetail, GameObject>();
        private static readonly Dictionary<string, HashSet<string>> _loadedMods = new Dictionary<string, HashSet<string>>();
        private static bool _init = false;

        public static void Init()
        {
            if (!_init)
            {
                _init = true;
                
                foreach (MuzzleFlash val in Enum.GetValues(typeof(MuzzleFlash)))
                {
                    if (val.ToString() != "none")
                        _loadedMods[val.ToString()] = new HashSet<string>();
                }
                
                foreach (Railgun val in Enum.GetValues(typeof(Railgun)))
                {
                    if (val.ToString() != "none")
                        _loadedMods[val.ToString()] = new HashSet<string>();
                }
                
                foreach (Explosion val in Enum.GetValues(typeof(Explosion)))
                {
                    if (val.ToString() != "none")
                        _loadedMods[val.ToString()] = new HashSet<string>();
                }
                
                foreach (Beam val in Enum.GetValues(typeof(Beam)))
                {
                    _loadedMods[val.ToString()] = new HashSet<string>();
                }
                
                foreach (PlasmaMuzzle val in Enum.GetValues(typeof(PlasmaMuzzle)))
                {
                    _loadedMods[val.ToString()] = new HashSet<string>();
                }
                
                foreach (Emitter val in Enum.GetValues(typeof(Emitter)))
                {
                    _loadedMods[val.ToString()] = new HashSet<string>();
                }
                
                foreach (Trail val in Enum.GetValues(typeof(Trail)))
                {
                    _loadedMods[val.ToString()] = new HashSet<string>();
                }
                
                foreach (Model val in Enum.GetValues(typeof(Model)))
                {
                    _loadedMods[val.ToString()] = new HashSet<string>();
                }

                LoadPacks?.Invoke();
            }
        }

        /// <summary>
        /// Register one VFX
        /// </summary>
        /// <param name="assetName">The name of the asset, this should match the pattern provided by MTMT VFX</param>
        /// <param name="sourceModName">The name of the mod providing the asset</param>
        public static void Register(string assetName, string sourceModName, GameObject prefab)
        {
            if (_loadedMods.TryGetValue(assetName, out var hashSet))
            {
                _registry[new AssetDetail(assetName, sourceModName)] = prefab;
                hashSet.Add(sourceModName);
            }
        }

        /// <summary>
        /// Register one VFX
        /// </summary>
        /// <param name="assetDetail">The details of the asset and mod providing it</param>
        public static void Register(AssetDetail assetDetail, GameObject prefab)
        {
            if (_loadedMods.TryGetValue(assetDetail.assetName, out var hashSet))
            {
                _registry[assetDetail] = prefab;
                hashSet.Add(assetDetail.sourceModName);
            }
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
            foreach (KeyValuePair<AssetDetail, GameObject> asset in assets)
            {
                Register(asset.Key, asset.Value);
            }
        }

        /// <summary>
        /// Get a VFX from the _registry
        /// </summary>
        /// <param name="assetName">Type of VFX</param>
        /// <param name="prefab">Name of the VFX</param>
        /// <param name="sourceModName">The name of the mod providing the asset</param>
        /// <returns></returns>
        public static bool TryGetAsset(string assetName, Enum type, out GameObject prefab, out string modName)
        {
            modName = GetModName(type);
            return _registry.TryGetValue(new AssetDetail(assetName, modName), out prefab);
        }

        public static void UpdateConfigs()
        {
            SettingsConfig config = Utils.GetConfig();
            if (!_loadedMods["muzzleflash_medium"].Contains(config.MUZZLE_MOD))
            {
                config.MUZZLE_MOD = _loadedMods["muzzleflash_medium"].FirstOrDefault();
            }
            if (!_loadedMods["muzzlerail_medium"].Contains(config.RAILGUN_MOD))
            {
                config.RAILGUN_MOD = _loadedMods["muzzlerail_medium"].FirstOrDefault();
            }
            if (!_loadedMods["expl_medium"].Contains(config.EXPL_MOD))
            {
                config.EXPL_MOD = _loadedMods["expl_medium"].FirstOrDefault();
            }
            if (!_loadedMods["laser_cont"].Contains(config.CONTINUOUS_MOD))
            {
                config.CONTINUOUS_MOD = _loadedMods["laser_cont"].FirstOrDefault();
            }
            if (!_loadedMods["laser_pulse"].Contains(config.PULSE_MOD))
            {
                config.PULSE_MOD = _loadedMods["laser_pulse"].FirstOrDefault();
            }
            if (!_loadedMods["pac_beam"].Contains(config.PAC_MOD))
            {
                config.PAC_MOD = _loadedMods["pac_beam"].FirstOrDefault();
            }
            if (!_loadedMods["plasma_medium"].Contains(config.PLASMA_MOD))
            {
                config.PLASMA_MOD = _loadedMods["plasma_medium"].FirstOrDefault();
            }
            if (!_loadedMods["flame"].Contains(config.FLAMER_MOD))
            {
                config.FLAMER_MOD = _loadedMods["flame"].FirstOrDefault();
            }
        }
    }
}
