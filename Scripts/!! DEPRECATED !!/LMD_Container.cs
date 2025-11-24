using System;
using BM_EffectUpdate;
using UnityEngine;

namespace BMEffects_Remaster.Util
{
    // Token: 0x0200003D RID: 61
    internal class LMD_Container
    {
        // Token: 0x060000C4 RID: 196 RVA: 0x000075EC File Offset: 0x000057EC
        public LMD_Container(LaserMissileDefence _obj)
        {
            obj = _obj;
            bFire = false;
            RldMax = 0.25f;
            NowTime = Random.Range(0f, RldMax);
            root = obj.MainConstruct.GameObject.gameObject;
            wlobj = Object.Instantiate<GameObject>(BM_EffectUpdate.GetAsset("WaveLaser"), BM_EffectUpdate.updater.transform);
            wlobj.GetComponent<SM_WaveLaser>().NowScale = 0f;
            wlobj.GetComponent<SM_WaveLaser>().TgtScale = 0f;
            wlobj.transform.localScale = Vector3.zero;
        }

        // Token: 0x0400010D RID: 269
        public LaserMissileDefence obj;

        // Token: 0x0400010E RID: 270
        public GameObject root;

        // Token: 0x0400010F RID: 271
        public bool bFire;

        // Token: 0x04000110 RID: 272
        public float NowTime;

        // Token: 0x04000111 RID: 273
        public float RldMax;

        // Token: 0x04000112 RID: 274
        public GameObject wlobj;
    }
}
