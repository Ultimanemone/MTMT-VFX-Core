using System;
using BM_EffectUpdate;
using UnityEngine;

namespace BMEffects_Remaster.Util
{
    // Token: 0x0200003B RID: 59
    internal class LC_Container
    {
        // Token: 0x060000BE RID: 190 RVA: 0x00007180 File Offset: 0x00005380
        public LC_Container(LaserCombiner _obj)
        {
            obj = _obj;
            bFire = false;
            RldMax = 0.25f;
            NowTime = UnityEngine.Random.Range(0f, RldMax);
            root = obj.MainConstruct.GameObject.gameObject;
            wlobj = Object.Instantiate<GameObject>(BM_EffectUpdate.GetAsset("WaveLaser"), BM_EffectUpdate.updater.transform);
            wlobj.GetComponent<SM_WaveLaser>().NowScale = 0f;
            wlobj.GetComponent<SM_WaveLaser>().TgtScale = 0f;
            wlobj.transform.localScale = Vector3.zero;
        }

        public LaserCombiner obj;
        public GameObject root;
        public bool bFire;
        public float NowTime;
        public float RldMax;
        public GameObject wlobj;
    }
}
