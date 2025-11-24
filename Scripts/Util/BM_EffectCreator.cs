using System;
using BM_EffectUpdate.EffectCode;
using UnityEngine;

namespace BM_EffectUpdate
{
    // Token: 0x02000031 RID: 49
    internal class BM_EffectCreator
    {
        // Token: 0x0600009E RID: 158 RVA: 0x000061B0 File Offset: 0x000043B0
        public BM_EffectCreator()
        {
            Debug.Log("BM_EffectCreator_Init");
            this.EffectRoot = new GameObject("EffectRoot");
            this.EffectRoot.transform.position = Vector3.zero;
            this.EffectRoot.transform.rotation = Quaternion.identity;
            this.EffectRoot.transform.localScale = Vector3.one;
            Debug.Log("MuzzleFlash_Init");
            Debug.Log("IndexSize:" + this.MuzzleIndex.Length);
            this.Muzzle = new GameObject[11];
            this.Muzzle[0] = BM_EffectUpdate.GetAsset("MuzzleFlashTinyTiny");
            this.Muzzle[1] = BM_EffectUpdate.GetAsset("MuzzleFlashTiny");
            this.Muzzle[2] = BM_EffectUpdate.GetAsset("MuzzleFlashSmall");
            this.Muzzle[3] = BM_EffectUpdate.GetAsset("MuzzleFlashMedium");
            this.Muzzle[4] = BM_EffectUpdate.GetAsset("MuzzleFlashLarge");
            this.Muzzle[5] = BM_EffectUpdate.GetAsset("MuzzleFlashLargest");
            this.Muzzle[6] = BM_EffectUpdate.GetAsset("MuzzleFlashHuge");
            this.Muzzle[7] = BM_EffectUpdate.GetAsset("MuzzleFlashMammoth");
            this.Muzzle[8] = BM_EffectUpdate.GetAsset("MuzzleFlashRailGun_Small");
            this.Muzzle[9] = BM_EffectUpdate.GetAsset("MuzzleFlashRailGun_Medium");
            this.Muzzle[10] = BM_EffectUpdate.GetAsset("MuzzleFlashRailGun_Big");
            this.MuzzlePool = new GameObject[11][];
            for (int i = 0; i < 11; i++)
            {
                this.MuzzlePool[i] = new GameObject[BM_EffectCreator.MuzzleMax];
                for (int j = 0; j < BM_EffectCreator.MuzzleMax; j++)
                {
                    this.MuzzlePool[i][j] = UnityEngine.Object.Instantiate<GameObject>(this.Muzzle[i]);//, BM_EffectUpdate.updater.transform);
                }
            }
            Debug.Log("MuzzleFlash_InitEnd");
            Debug.Log("Explosion_Init");
            this.Explosions = new GameObject[12];
            this.Explosions[0] = BM_EffectUpdate.GetAsset("TinyBom");
            this.Explosions[1] = BM_EffectUpdate.GetAsset("NormalBom");
            this.Explosions[2] = BM_EffectUpdate.GetAsset("MediumBom");
            this.Explosions[3] = BM_EffectUpdate.GetAsset("LargeBom");
            this.Explosions[4] = BM_EffectUpdate.GetAsset("HugeBom");
            this.Explosions[5] = BM_EffectUpdate.GetAsset("Splash");
            this.Explosions[6] = BM_EffectUpdate.GetAsset("LargeSplash");
            this.Explosions[7] = BM_EffectUpdate.GetAsset("HugeSplash");
            this.Explosions[8] = BM_EffectUpdate.GetAsset("LargeSplash_Pure");
            this.Explosions[9] = BM_EffectUpdate.GetAsset("SplashBase");
            this.Explosions[10] = BM_EffectUpdate.GetAsset("DistShockWave");
            this.Explosions[11] = BM_EffectUpdate.GetAsset("AtomicBom");
            this.ExplosionPool = new GameObject[12][];
            for (int k = 0; k < 12; k++)
            {
                this.ExplosionPool[k] = new GameObject[BM_EffectCreator.ExplosionMax];
                for (int l = 0; l < BM_EffectCreator.ExplosionMax; l++)
                {
                    this.ExplosionPool[k][l] = UnityEngine.Object.Instantiate<GameObject>(this.Explosions[k]);//, BM_EffectUpdate.updater.transform);
                }
            }
            Debug.Log("Explosion_InitEnd");
            Debug.Log("Lasr_Init");
            this.Lasers = new GameObject[5];
            this.Lasers[0] = BM_EffectUpdate.GetAsset("PulseLaser");
            this.Lasers[2] = BM_EffectUpdate.GetAsset("LaserFlash");
            this.Lasers[1] = BM_EffectUpdate.GetAsset("PacEffect");
            this.Lasers[3] = BM_EffectUpdate.GetAsset("PulseHit");
            this.Lasers[4] = BM_EffectUpdate.GetAsset("ShockWave");
            this.LaserPool = new GameObject[5][];
            for (int m = 0; m < 5; m++)
            {
                this.LaserPool[m] = new GameObject[BM_EffectCreator.LaserMax];
                for (int n = 0; n < BM_EffectCreator.LaserMax; n++)
                {
                    this.LaserPool[m][n] = UnityEngine.Object.Instantiate<GameObject>(this.Lasers[m]);// BM_EffectUpdate.updater.transform);
                }
            }
            Debug.Log("Lasr_InitEnd");
        }

        // Token: 0x0600009F RID: 159 RVA: 0x000065DE File Offset: 0x000047DE
        internal GameObject CreateExplosion(object effecttype_sp)
        {
            throw new NotImplementedException();
        }

        // Token: 0x060000A0 RID: 160 RVA: 0x000065E8 File Offset: 0x000047E8
        public GameObject CreateMuzzle(BM_MuzzleFlashName type)
        {
            GameObject gameObject = null;
            if (!this.MuzzlePool[(int)type][this.MuzzleIndex[(int)type]].activeSelf)
            {
                gameObject = this.MuzzlePool[(int)type][this.MuzzleIndex[(int)type]];
                gameObject.GetComponent<SM_AutoKill>().Init();
                Transform transform = gameObject.transform.FindChild("Thunder_1");
                if (transform != null)
                {
                    GameObject[] children = transform.GetChildren(false);
                    for (int i = 0; i < children.Length; i++)
                    {
                        children[i].GetComponent<SingleThunder>().SetThunder();
                    }
                }
            }
            this.MuzzleIndex[(int)type] = (this.MuzzleIndex[(int)type] + 1) % BM_EffectCreator.MuzzleMax;
            return gameObject;
        }

        // Token: 0x060000A1 RID: 161 RVA: 0x00006682 File Offset: 0x00004882
        public GameObject CreateExplosion(BM_ExplosionsName type)
        {
            return this.CreateExplosion(type, Vector3.zero);
        }

        // Token: 0x060000A2 RID: 162 RVA: 0x00006690 File Offset: 0x00004890
        public GameObject CreateExplosion(BM_ExplosionsName type, Vector3 pos)
        {
            if (type == BM_ExplosionsName.DistShockWave && !BM_EffectCreator.bExExplosion)
            {
                return null;
            }
            GameObject gameObject = null;
            if (!this.ExplosionPool[(int)type][this.ExplosionIndex[(int)type]].activeSelf)
            {
                gameObject = this.ExplosionPool[(int)type][this.ExplosionIndex[(int)type]];
                gameObject.GetComponent<SM_AutoKill>().Init();
            }
            this.ExplosionIndex[(int)type] = (this.ExplosionIndex[(int)type] + 1) % BM_EffectCreator.ExplosionMax;
            float num = 0f;
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
            if (num > 0f)
            {
                this.CreateImpactSplash(pos, num);
            }
            return gameObject;
        }

        // Token: 0x060000A3 RID: 163 RVA: 0x0000674C File Offset: 0x0000494C
        public GameObject CreateLaser(BM_LaserEffectName type)
        {
            GameObject gameObject = null;
            if (!this.LaserPool[(int)type][this.LaserIndex[(int)type]].activeSelf)
            {
                gameObject = this.LaserPool[(int)type][this.LaserIndex[(int)type]];
                gameObject.GetComponent<SM_AutoKill>().Init();
                gameObject.transform.position = Vector3.zero;
                gameObject.transform.rotation = Quaternion.identity;
                gameObject.transform.localScale = Vector3.one;
                // gameObject.transform.parent = BM_EffectUpdate.updater.transform;
            }
            this.LaserIndex[(int)type] = (this.LaserIndex[(int)type] + 1) % BM_EffectCreator.LaserMax;
            return gameObject;
        }

        // Token: 0x060000A4 RID: 164 RVA: 0x000067F0 File Offset: 0x000049F0
        public bool CreateImpactSplash(Vector3 pos, float radius)
        {
            if (pos.y < radius && pos.y > 0f)
            {
                float num = 1f - pos.y / radius;
                num = 1f - Mathf.Pow(1f - num, 4f);
                GameObject gameObject = BM_EffectCreator.Creator.CreateExplosion(BM_ExplosionsName.SplashBase, pos);
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

        // Token: 0x040000E1 RID: 225
        public GameObject EffectRoot;

        // Token: 0x040000E2 RID: 226
        public GameObject[] Muzzle;

        // Token: 0x040000E3 RID: 227
        public GameObject[] Explosions;

        // Token: 0x040000E4 RID: 228
        public GameObject[] Lasers;

        // Token: 0x040000E5 RID: 229
        private static int MuzzleMax = 32;

        // Token: 0x040000E6 RID: 230
        private static int ExplosionMax = 32;

        // Token: 0x040000E7 RID: 231
        private static int LaserMax = 32;

        // Token: 0x040000E8 RID: 232
        public static bool bExExplosion;

        // Token: 0x040000E9 RID: 233
        public static BM_EffectCreator Creator;

        // Token: 0x040000EA RID: 234
        private int[] MuzzleIndex = new int[11];

        // Token: 0x040000EB RID: 235
        private GameObject[][] MuzzlePool;

        // Token: 0x040000EC RID: 236
        private int[] ExplosionIndex = new int[12];

        // Token: 0x040000ED RID: 237
        private GameObject[][] ExplosionPool;

        // Token: 0x040000EE RID: 238
        private int[] LaserIndex = new int[5];

        // Token: 0x040000EF RID: 239
        private GameObject[][] LaserPool;
    }
}
