using BMEffects_Remaster;
using BMEffects_Remaster.Effects.Projectiles;
using BrilliantSkies.Core.Logger;
using BrilliantSkies.Modding.Types;
using BrilliantSkies.Modding;
using System;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace BMEffects_Remaster.Util
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
                { "MuzzleFlashTinyTiny", BMEffects_Remaster.GetAsset("MuzzleFlashTinyTiny") },
                { "MuzzleFlashTiny", BMEffects_Remaster.GetAsset("MuzzleFlashTiny") },
                { "MuzzleFlashSmall", BMEffects_Remaster.GetAsset("MuzzleFlashSmall") },
                { "MuzzleFlashMedium", BMEffects_Remaster.GetAsset("MuzzleFlashMedium") },
                { "MuzzleFlashLarge", BMEffects_Remaster.GetAsset("MuzzleFlashLarge") },
                { "MuzzleFlashLargest", BMEffects_Remaster.GetAsset("MuzzleFlashLargest") },
                { "MuzzleFlashHuge", BMEffects_Remaster.GetAsset("MuzzleFlashHuge") },
                { "MuzzleFlashMammoth", BMEffects_Remaster.GetAsset("MuzzleFlashMammoth") },
                { "MuzzleFlashRailGun_Small", BMEffects_Remaster.GetAsset("MuzzleFlashRailGun_Small") },
                { "MuzzleFlashRailGun_Medium", BMEffects_Remaster.GetAsset("MuzzleFlashRailGun_Medium") },
                { "MuzzleFlashRailGun_Big", BMEffects_Remaster.GetAsset("MuzzleFlashRailGun_Big") }
            };
            Debug.Log("MuzzleFlash_InitEnd");

            Debug.Log("Explosion_Init");
            Explosions = new Dictionary<string, GameObject>()
            {
                { "TinyBom", BMEffects_Remaster.GetAsset("TinyBom") },
                { "NormalBom", BMEffects_Remaster.GetAsset("NormalBom") },
                { "MediumBom", BMEffects_Remaster.GetAsset("MediumBom") },
                { "LargeBom", BMEffects_Remaster.GetAsset("LargeBom") },
                { "HugeBom", BMEffects_Remaster.GetAsset("HugeBom") },
                { "Splash", BMEffects_Remaster.GetAsset("Splash") },
                { "LargeSplash", BMEffects_Remaster.GetAsset("LargeSplash") },
                { "HugeSplash", BMEffects_Remaster.GetAsset("HugeSplash") },
                { "LargeSplash_Pure", BMEffects_Remaster.GetAsset("LargeSplash_Pure") },
                { "SplashBase", BMEffects_Remaster.GetAsset("SplashBase") },
                { "DistShockWave", BMEffects_Remaster.GetAsset("DistShockWave") },
                { "AtomicBom", BMEffects_Remaster.GetAsset("AtomicBom") }
            };
            Debug.Log("Explosion_InitEnd");

            Debug.Log("Lasr_Init");
            Lasers = new Dictionary<string, GameObject>()
            {
                { "PulseLaser", BMEffects_Remaster.GetAsset("PulseLaser") },
                { "LaserFlash", BMEffects_Remaster.GetAsset("LaserFlash") },
                { "PacEffect", BMEffects_Remaster.GetAsset("PacEffect") },
                { "PulseHit", BMEffects_Remaster.GetAsset("PulseHit") },
                { "ShockWave", BMEffects_Remaster.GetAsset("ShockWave") }
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
