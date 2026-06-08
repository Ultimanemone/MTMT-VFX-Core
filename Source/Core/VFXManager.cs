using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using MTMTVFX.UI;
using BrilliantSkies.PlayerProfiles;
using BrilliantSkies.Ftd.Game.Pools;
using MTMTVFX.Core.Pooling;
using MTMTVFX.Core.AssetManagement;

namespace MTMTVFX.Core
{
    public class VFXManager
    {
        public static VFXManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new VFXManager();
                return _instance;
            }
        }

        private static VFXManager _instance;
        private bool _initialized = false;

        private static Dictionary<MuzzleFlashType, VFXPool> _muzzleFlashPools;
        private static Dictionary<RailgunMuzzleType, VFXPool> _railgunPools;
        private static Dictionary<ExplosionType, VFXPool> _explosionPools;
        private static Dictionary<BeamName, VFXPool> _beamPools;
        public static Dictionary<ConventionalLaser, GameObject> laserBeams;
        public static VFXPool apsDefaultTrailPool;

        public static IReadOnlyDictionary<MuzzleFlashType, VFXPool> MuzzleFlashPools => _muzzleFlashPools;
        public static IReadOnlyDictionary<RailgunMuzzleType, VFXPool> RailgunPools => _railgunPools;
        public static IReadOnlyDictionary<ExplosionType, VFXPool> ExplosionPools => _explosionPools;
        public static IReadOnlyDictionary<BeamName, VFXPool> BeamPools => _beamPools;

        private static GameObject _vfxRoot;

        private VFXManager() { }

        // lazy init
        public void Init()
        {
            if (_initialized) return;
            else _initialized = true;
            Utils.LogInfo<VFXManager>("Initializing...");

            AssetRegistry.Init();

            _vfxRoot = new GameObject("MTMT VFX Root");
            SettingsConfig config = Utils.GetConfig();

            GameObject muzzleFlashRoot = new GameObject("APS Muzzle Flash Root");
            muzzleFlashRoot.transform.SetParent(_vfxRoot.transform);
            _muzzleFlashPools = InitPool<MuzzleFlashType>(muzzleFlashRoot.transform, config.COUNT_MUZZLE);

            GameObject railgunRoot = new GameObject("Railgun FX Root");
            railgunRoot.transform.SetParent(_vfxRoot.transform);
            _railgunPools = InitPool<RailgunMuzzleType>(railgunRoot.transform, config.COUNT_RAILGUN);

            GameObject explosionRoot = new GameObject("ExplosionType Root");
            explosionRoot.transform.SetParent(_vfxRoot.transform);
            _explosionPools = InitPool<ExplosionType>(explosionRoot.transform, config.COUNT_EXPL);

            GameObject beamRoot = new GameObject("Beam Root");
            beamRoot.transform.SetParent(_vfxRoot.transform);
            _beamPools = new Dictionary<BeamName, VFXPool>();

            VFXPool pulsePool = InitPool(BeamName.laser_pulse, beamRoot.transform, config.COUNT_PULSE);
            if (pulsePool != null) _beamPools[BeamName.laser_pulse] = pulsePool;

            VFXPool pacPool = InitPool(BeamName.pac_beam, beamRoot.transform, config.COUNT_PULSE);
            if (pacPool != null) _beamPools[BeamName.pac_beam] = pacPool;

            // not worth pooling ts
            laserBeams = new Dictionary<ConventionalLaser, GameObject>();

            GameObject apsTrailRoot = new GameObject("APS default trail Root");
            GameObject apsTrailObj = new GameObject("APS default trail ghost");
            apsTrailObj.AddComponent<LineRenderer>();
            apsDefaultTrailPool = new VFXPool(apsTrailObj, "", TrailType.aps, 100, apsTrailRoot.transform, KillType.trail);
            UnityEngine.Object.Destroy(apsTrailObj);
        }

        private Dictionary<T, VFXPool> InitPool<T>(Transform root, int count) where T : Enum
        {
            Dictionary<T, VFXPool> pool = new Dictionary<T, VFXPool>();
            foreach (T val in Enum.GetValues(typeof(T)))
            {
                if (val.ToString() == "none") continue;

                if (AssetRegistry.TryGetAsset(val.ToString(), out GameObject obj, out string modName))
                {
                    pool[val] = new VFXPool(obj, modName, val, count, root);
                }
                else Utils.LogError<VFXManager>($"Asset not found: {val}", BrilliantSkies.Core.Logger.LogOptions.PopupDev);
            }
            return pool;
        }

        private VFXPool InitPool(Enum type, Transform root, int count, GameObject obj = null)
        {
            if (obj == null)
            {
                if (AssetRegistry.TryGetAsset(type.ToString(), out obj, out string modName))
                {
                    return new VFXPool(obj, modName, type, count, root);
                }
                else
                {
                    Utils.LogError<VFXManager>($"Asset not found: {type}", BrilliantSkies.Core.Logger.LogOptions.PopupDev);
                    return null;
                }
            }
            else
            {
                return new VFXPool(obj, "", type, count, root);
            }
        }

        public void OnConfigUpdatePool<T>() where T : Enum
        {
            if (!_initialized) Init();

            IDictionary poolDict;
            if (typeof(T) == typeof(MuzzleFlashType))
                poolDict = _muzzleFlashPools;
            else if (typeof(T) == typeof(RailgunMuzzleType))
                poolDict = _railgunPools;
            else if (typeof(T) == typeof(ExplosionType))
                poolDict = _explosionPools;
            else if (typeof(T) == typeof(BeamName))
                poolDict = _beamPools;
            else return;

            foreach (DictionaryEntry entry in poolDict)
            {
                ((VFXPool)entry.Value).OnConfigUpdate();
            }
        }

        public void OnConfigUpdatePool(Enum type)
        {
            if (!_initialized) Init();

            VFXPool pool;

            if (type.GetType() == typeof(MuzzleFlashType) && (MuzzleFlashType)type != MuzzleFlashType.none)
                pool = _muzzleFlashPools[(MuzzleFlashType)type];
            else if (type.GetType() == typeof(RailgunMuzzleType) && (RailgunMuzzleType)type != RailgunMuzzleType.none)
                pool = _railgunPools[(RailgunMuzzleType)type];
            else if (type.GetType() == typeof(ExplosionType) && (ExplosionType)type != ExplosionType.none)
                pool = _explosionPools[(ExplosionType)type];
            else if (type.GetType() == typeof(BeamName) && (BeamName)type != BeamName.none)
                pool = _beamPools[(BeamName)type];
            else return;

            pool.OnConfigUpdate();
        }

        public void OnConfigUpdateAllPool()
        {
            List<IDictionary> all = new List<IDictionary>() { _muzzleFlashPools, _railgunPools, _explosionPools, _beamPools };
            foreach (var dict in all)
            {
                foreach (DictionaryEntry entry in dict)
                {
                    ((VFXPool)entry.Value).OnConfigUpdate();
                }
            }
        }

        /// <summary>
        /// Create VFX by name, requires at least one live particle
        /// </summary>
        /// <param name="type">The type of the object</param>
        /// <param name="pos">Where the object should be placed</param>
        /// <param name="forward">The direction the object should point at</param>
        /// <param name="size">The size of the object</param>
        /// <returns></returns>
        public static GameObject Create(Enum type, Vector3 pos, Vector3 forward)
        {
            Instance.Init();
            if (type.GetType() == typeof(TrailType))
            {
                VFXPool pool;
                if ((TrailType)type == TrailType.aps)
                    pool = apsDefaultTrailPool;
                else return null;

                pool.TryGet(pos, forward, out GameObject obj);
                Utils.LogInfo<VFXManager>($"Effect {type} got from _pool!");
                return obj;
            }
            else
            {
                IDictionary pool;

                if (type.GetType() == typeof(MuzzleFlashType) && (MuzzleFlashType)type != MuzzleFlashType.none)
                    pool = _muzzleFlashPools;
                else if (type.GetType() == typeof(RailgunMuzzleType) && (RailgunMuzzleType)type != RailgunMuzzleType.none)
                    pool = _railgunPools;
                else if (type.GetType() == typeof(ExplosionType) && (ExplosionType)type != ExplosionType.none)
                    pool = _explosionPools;
                else if (type.GetType() == typeof(BeamName) && (BeamName)type != BeamName.none)
                    pool = _beamPools;
                else return null;

                ((VFXPool)pool[type]).TryGet(pos, forward, out GameObject obj);
                Utils.LogInfo<VFXManager>($"Effect {type} got from _pool!");
                return obj;
            }
        }

        public static GameObject InstantiateCopy(Enum type, Vector3 pos, Vector3 forward)
        {
            Instance.Init();
            AssetRegistry.assetList.TryGetValue(type.ToString(), out AssetContainer container);

            GameObject obj = UnityEngine.Object.Instantiate(container.prefab);
            obj.transform.localPosition = pos;
            obj.transform.forward = forward;
            Utils.AddScript(obj, type, container.source);

            Utils.LogInfo<VFXManager>($"Effect {type} instantiated!");
            return obj;
        }
    }
}
