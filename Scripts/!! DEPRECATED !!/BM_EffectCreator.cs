using BM_EffectUpdate;
using BrilliantSkies.Core.Logger;
using BrilliantSkies.Modding.Types;
using BrilliantSkies.Modding;
using System;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using BM_EffectUpdate.EffectCode;

namespace BM_EffectUpdate.EffectCode
{
    internal class BM_EffectCreator
    {
        public static BM_EffectCreator Instance = new BM_EffectCreator();
        private static int MuzzleMax = 32;
        private static int ExplosionsMax = 32;
        private static int LaserMax = 32;

        public GameObject EffectRoot;
        public Dictionary<string, GameObject> Muzzle;
        public Dictionary<string, GameObject> Explosions;
        public Dictionary<string, GameObject> Lasers;
        public Queue<GameObject> RenderedMuzzle;
        public Queue<GameObject> RenderedExplosions;
        public Queue<GameObject> RenderedLasers;
        public static bool bExExplosion;

        private BM_EffectCreator()
        {

            Debug.Log("BM_EffectCreator_Init");
            EffectRoot = new GameObject("EffectRoot");
            EffectRoot.transform.position = Vector3.zero;
            EffectRoot.transform.rotation = Quaternion.identity;
            EffectRoot.transform.localScale = Vector3.one;
            Debug.Log("MuzzleFlash_Init");
            Muzzle = new Dictionary<string, GameObject>()
            {
                { "MuzzleFlashTinyTiny", BM_EffectUpdate.GetAsset("MuzzleFlashTinyTiny") },
                { "MuzzleFlashTiny", BM_EffectUpdate.GetAsset("MuzzleFlashTiny") },
                { "MuzzleFlashSmall", BM_EffectUpdate.GetAsset("MuzzleFlashSmall") },
                { "MuzzleFlashMedium", BM_EffectUpdate.GetAsset("MuzzleFlashMedium") },
                { "MuzzleFlashLarge", BM_EffectUpdate.GetAsset("MuzzleFlashLarge") },
                { "MuzzleFlashLargest", BM_EffectUpdate.GetAsset("MuzzleFlashLargest") },
                { "MuzzleFlashHuge", BM_EffectUpdate.GetAsset("MuzzleFlashHuge") },
                { "MuzzleFlashMammoth", BM_EffectUpdate.GetAsset("MuzzleFlashMammoth") },
                { "MuzzleFlashRailGun_Small", BM_EffectUpdate.GetAsset("MuzzleFlashRailGun_Small") },
                { "MuzzleFlashRailGun_Medium", BM_EffectUpdate.GetAsset("MuzzleFlashRailGun_Medium") },
                { "MuzzleFlashRailGun_Big", BM_EffectUpdate.GetAsset("MuzzleFlashRailGun_Big") }
            };
            Debug.Log("MuzzleFlash_InitEnd");

            Debug.Log("Explosion_Init");
            Explosions = new Dictionary<string, GameObject>()
            {
                { "TinyBom", BM_EffectUpdate.GetAsset("TinyBom") },
                { "NormalBom", BM_EffectUpdate.GetAsset("NormalBom") },
                { "MediumBom", BM_EffectUpdate.GetAsset("MediumBom") },
                { "LargeBom", BM_EffectUpdate.GetAsset("LargeBom") },
                { "HugeBom", BM_EffectUpdate.GetAsset("HugeBom") },
                { "Splash", BM_EffectUpdate.GetAsset("Splash") },
                { "LargeSplash", BM_EffectUpdate.GetAsset("LargeSplash") },
                { "HugeSplash", BM_EffectUpdate.GetAsset("HugeSplash") },
                { "LargeSplash_Pure", BM_EffectUpdate.GetAsset("LargeSplash_Pure") },
                { "SplashBase", BM_EffectUpdate.GetAsset("SplashBase") },
                { "DistShockWave", BM_EffectUpdate.GetAsset("DistShockWave") },
                { "AtomicBom", BM_EffectUpdate.GetAsset("AtomicBom") }
            };
            Debug.Log("Explosion_InitEnd");

            Debug.Log("Lasr_Init");
            Lasers = new Dictionary<string, GameObject>()
            {
                { "PulseLaser", BM_EffectUpdate.GetAsset("PulseLaser") },
                { "LaserFlash", BM_EffectUpdate.GetAsset("LaserFlash") },
                { "PacEffect", BM_EffectUpdate.GetAsset("PacEffect") },
                { "PulseHit", BM_EffectUpdate.GetAsset("PulseHit") },
                { "ShockWave", BM_EffectUpdate.GetAsset("ShockWave") }
            };
            Debug.Log("Lasr_InitEnd");
        }

        
        internal GameObject CreateExplosion(object effecttype_sp)
        {
            throw new NotImplementedException();
        }

        public void AddToQueue(GameObject obj, Queue<GameObject> queue)
        {
            int count = int.MaxValue;
            if (queue == RenderedMuzzle)
            {
                count = MuzzleMax;
            }
            else if (queue == RenderedLasers)
            {
                count = ExplosionsMax;
            }
            else if (queue == RenderedLasers)
            {
                count = LaserMax;
            }
            else
            {
                if (obj != null) { UnityEngine.Object.Destroy(obj); }
            }

            queue.Enqueue(obj);

            bool flag = queue.Count > count;
            while (flag)
            {
                if (queue.Peek() == null) queue.Dequeue();
            }
        }

        /// <summary>
        /// Create muzzle flash
        /// </summary>
        /// <param name="type"></param>
        /// <param name="transform"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public GameObject CreateMuzzle(BM_MuzzleFlashName type, Transform parent = null)
        {
            GameObject obj = UnityEngine.Object.Instantiate<GameObject>(Muzzle[type.ToString()]);
            bool flag = parent != null;
            if (flag) obj.transform.SetParent(parent, false);

            bool flag1 = obj.transform.Find("Thunder_1") != null;
            if (flag1)
            {
                GameObject[] children = obj.GetChildren(false);
                for (int i = 0; i < children.Length; i++)
                {
                    children[i].GetComponent<SingleThunder>().SetThunder();
                }
            }

            obj.GetComponent<SM_AutoKill>().Init();

            return obj;
        }

        /// <summary>
        /// Create explosion
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public GameObject CreateExplosion(BM_ExplosionsName type)
        {
            return CreateExplosion(type, Vector3.zero);
        }

        /// <summary>
        /// Create explosion
        /// </summary>
        /// <param name="type"></param>
        /// <param name="pos"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public GameObject CreateExplosion(BM_ExplosionsName type, Vector3 pos, Transform parent = null)
        {
            if (type == BM_ExplosionsName.DistShockWave && !bExExplosion)
            {
                return null;
            }

            GameObject obj = UnityEngine.Object.Instantiate<GameObject>(Muzzle[type.ToString()]);
            bool flag = parent != null;
            if (flag) obj.transform.SetParent(parent, false);

            obj.transform.position = pos;
            //obj.transform.rotation = ;

            obj.GetComponent<SM_AutoKill>().Init();

            float num = -1f;
            switch (type)
            {
                case BM_ExplosionsName.NormalBom:
                    num = 4f;
                    break;
                case BM_ExplosionsName.MediumBom:
                    num = 8f;
                    break;
                case BM_ExplosionsName.LargeBom:
                    num = 10f;
                    break;
                case BM_ExplosionsName.HugeBom:
                    num = 15f;
                    break;
            }
            if (num > 0f) CreateImpactSplash(pos, num);

            return obj;
        }

        /// <summary>
        /// Create laser beam
        /// </summary>
        /// <param name="type"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public GameObject CreateLaser(BM_LaserEffectName type, Transform parent = null)
        {
            GameObject obj = UnityEngine.Object.Instantiate<GameObject>(Muzzle[type.ToString()]);
            bool flag = parent != null;
            if (flag) obj.transform.SetParent(parent, false);
            obj.GetComponent<SM_AutoKill>().Init();
            //gameObject.obj.parent = BMEffects_Remaster.updater.transform;

            return obj;
        }

        /// <summary>
        /// Create impact splash
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public bool CreateImpactSplash(Vector3 pos, float radius)
        {
            if (pos.y < radius && pos.y > 0f)
            {
                float num = 1f - pos.y / radius;
                num = 1f - Mathf.Pow(1f - num, 4f);
                GameObject gameObject = Instance.CreateExplosion(BM_ExplosionsName.SplashBase, pos);
                if (gameObject)
                {
                    pos.y = 1f;
                    gameObject.transform.position = pos;
                    gameObject.transform.rotation = Quaternion.identity;
                    gameObject.transform.localScale = Vector3.one * num * (radius / 30f);
                }
            }
            return false;
        }
    }
}
