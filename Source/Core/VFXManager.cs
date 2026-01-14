using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;


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

        private static Dictionary<MuzzleFlashName, VFXPool> _muzzleFlashPools;
        private static Dictionary<RailgunName, VFXPool> _railgunPools;
        private static Dictionary<ExplosionName, VFXPool> _explosionPools;
        private static Dictionary<BeamName, VFXPool> _beamPools;

        public static IReadOnlyDictionary<MuzzleFlashName, VFXPool> MuzzleFlashPools => _muzzleFlashPools;
        public static IReadOnlyDictionary<RailgunName, VFXPool> RailgunPools => _railgunPools;
        public static IReadOnlyDictionary<ExplosionName, VFXPool> ExplosionPools => _explosionPools;
        public static IReadOnlyDictionary<BeamName, VFXPool> BeamPools => _beamPools;

        private static GameObject _vfxRoot;

        private VFXManager() { }

        // lazy ass init, idk how to do this otherwise
        public void Init()
        {
            if (_initialized) return;
            else _initialized = true;
            Util.LogInfo<VFXManager>("Initializing...");

            AssetRegistry.Init();

            _vfxRoot = new GameObject("MTMT VFX Root");

            if (Util.E_MUZZLE)
            {
                GameObject muzzleFlashRoot = new GameObject("Muzzle Flash Root");
                muzzleFlashRoot.transform.SetParent(_vfxRoot.transform);
                _muzzleFlashPools = InitPool<MuzzleFlashName>(muzzleFlashRoot.transform, Util.COUNT_MUZZLE);
            }

            if (Util.E_RAILGUN)
            {
                GameObject railgunRoot = new GameObject("Railgun FX Root");
                railgunRoot.transform.SetParent(_vfxRoot.transform);
                _railgunPools = InitPool<RailgunName>(railgunRoot.transform, Util.COUNT_RAILGUN);
            }

            if (Util.E_EXPL)
            {
                GameObject explosionRoot = new GameObject("Explosion Root");
                explosionRoot.transform.SetParent(_vfxRoot.transform);
                _explosionPools = InitPool<ExplosionName>(explosionRoot.transform, Util.COUNT_EXPL);
            }


            if (Util.E_PULSE || Util.E_PAC)
            {
                GameObject beamRoot = new GameObject("Beam Root");
                beamRoot.transform.SetParent(_vfxRoot.transform);
                _beamPools = new Dictionary<BeamName, VFXPool>();

                if (Util.E_PULSE)
                {
                    VFXPool pulsePool = InitPool(BeamName.laser_pulse, beamRoot.transform, Util.COUNT_PULSE);
                    if (pulsePool != null) _beamPools[BeamName.laser_pulse] = pulsePool;
                }

                if (Util.E_PULSE)
                {
                    VFXPool pacPool = InitPool(BeamName.pac_beam, beamRoot.transform, Util.COUNT_PULSE);
                    if (pacPool != null) _beamPools[BeamName.pac_beam] = pacPool;
                }
            }
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
                else Util.LogError<VFXManager>($"Asset not found: {val}", BrilliantSkies.Core.Logger.LogOptions.PopupDev);
            }
            return pool;
        }

        private VFXPool InitPool(Enum type, Transform root, int count)
        {
            if (AssetRegistry.TryGetAsset(type.ToString(), out GameObject obj, out string modName))
            {
                return new VFXPool(obj, modName, type, count, root);
            }
            else Util.LogError<VFXManager>($"Asset not found: {type}", BrilliantSkies.Core.Logger.LogOptions.PopupDev);

            return null;
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

            IDictionary pool;

            if (type.GetType() == typeof(MuzzleFlashName) && (MuzzleFlashName)type != MuzzleFlashName.none)
                pool = _muzzleFlashPools;
            else if (type.GetType() == typeof(RailgunName) && (RailgunName)type != RailgunName.none)
                pool = _railgunPools;
            else if (type.GetType() == typeof(ExplosionName) && (ExplosionName)type != ExplosionName.none)
                pool = _explosionPools;
            else if (type.GetType() == typeof(BeamName) && (BeamName)type != BeamName.none)
                pool = _beamPools;
            else return null;

            ((VFXPool)pool[type]).TryGet(pos, forward, out GameObject obj);

            Util.LogInfo<VFXManager>($"Effect {type} got from pool!");

            return obj;
        }

        public static GameObject InstantiateCopy(Enum type, Vector3 pos, Vector3 forward)
        {
            Instance.Init();
            AssetRegistry.assetList.TryGetValue(type.ToString(), out AssetContainer container);

            GameObject obj = UnityEngine.Object.Instantiate(container.prefab);
            obj.transform.localPosition = pos;
            obj.transform.forward = forward;
            Util.AddScript(obj, type, container.source);

            Util.LogInfo<VFXManager>($"Effect {type} instantiated!");
            return obj;
        }
    }
}
