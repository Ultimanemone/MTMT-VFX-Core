using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using BrilliantSkies.Environments;
using static BrilliantSkies.FromTheDepths.Planets.Map.ForceButtonPanel;
using UnityEngine.Windows;


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

        public static Dictionary<string, VFXPool> muzzleFlashPools { get; private set; }
        public static Dictionary<string, VFXPool> railgunPools { get; private set; }
        public static Dictionary<string, VFXPool> explosionPools { get; private set; }
        public static Dictionary<string, VFXPool> beamPools { get; private set; }
        public static Dictionary<string, VFXPool> pacPool { get; private set; }
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
                muzzleFlashPools = InitPool<MuzzleFlashName>(muzzleFlashRoot.transform, Util.COUNT_MUZZLE);
            }

            if (Util.E_RAILGUN)
            {
                GameObject railgunRoot = new GameObject("Railgun FX Root");
                railgunRoot.transform.SetParent(_vfxRoot.transform);
                railgunPools = InitPool<RailgunName>(railgunRoot.transform, Util.COUNT_RAILGUN);
            }

            if (Util.E_EXPL)
            {
                GameObject explosionRoot = new GameObject("Explosion Root");
                explosionRoot.transform.SetParent(_vfxRoot.transform);
                explosionPools = InitPool<ExplosionName>(explosionRoot.transform, Util.COUNT_EXPL);
            }

            if (Util.E_PULSE)
            {
                GameObject beamRoot = new GameObject("Beam Root");
                beamRoot.transform.SetParent(_vfxRoot.transform);
                beamPools = InitPool<BeamName>(beamRoot.transform, Util.COUNT_PULSE);
            }
        }

        private Dictionary<string, VFXPool> InitPool<T>(Transform root, int count) where T : Enum
        {
            Dictionary<string, VFXPool> pool = new Dictionary<string, VFXPool>();
            foreach (T val in Enum.GetValues(typeof(T)))
            {
                if (val.ToString() == "none") continue;
                GameObject obj = null;
                AssetRegistry.TryGetAsset(val.ToString(), out obj);
                pool[val.ToString()] = new VFXPool(obj, count, root);
            }

            Util.LogInfo<Loader>("Initialized!");
            return pool;
        }

        /// <summary>
        /// Create VFX by name, requires at least one live particle
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pos"></param>
        /// <param name="forward"></param>
        /// <returns></returns>
        public static GameObject Create<T>(string name, Vector3 pos, Vector3 forward) where T : struct, Enum
        {
            Instance.Init();
            GameObject obj = null;
            Dictionary<string, VFXPool> pool;

            if (typeof(T) == typeof(MuzzleFlashName)) pool = muzzleFlashPools;
            else if (typeof(T) == typeof(RailgunName)) pool = railgunPools;
            else if (typeof(T) == typeof(ExplosionName)) pool = explosionPools;
            else if (typeof(T) == typeof(BeamName)) pool = beamPools;
            else return null;
            
            if (Enum.TryParse(name, out T result))
            {
                pool[name].TryGet(pos, forward, out obj);

                AssetRegistry.assetList.TryGetValue(name, out AssetContainer container);
                AddScriptConditional(obj, name, container.source);
            }
            else
            {
                return obj;
            }

            Util.LogInfo<VFXManager>($"Effect {name} got from pool!");

            return obj;
        }

        public static GameObject InstantiateCopy<T>(string name, Vector3 pos, Vector3 forward) where T : struct, Enum
        {
            Instance.Init();
            GameObject obj = null;
            if (Enum.TryParse(name, out T result))
            {
                AssetRegistry.assetList.TryGetValue(name, out AssetContainer container);

                obj = UnityEngine.Object.Instantiate(container.prefab);
                obj.transform.localPosition = pos;
                obj.transform.forward = forward;

                AddScriptConditional(obj, name, container.source);
            }

            Util.LogInfo<VFXManager>($"Effect {name} instantiated!");
            return obj;
        }

        /// <summary>
        /// Run or add some custom things to your VFX
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="objName"></param>
        /// <param name="modName"></param>
        private static void AddScriptConditional(GameObject obj, string objName, string modName)
        {
        }
    }
}
