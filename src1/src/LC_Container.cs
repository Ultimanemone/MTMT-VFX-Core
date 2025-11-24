using System;
using UnityEngine;

namespace BM_EffectUpdate
{
	// Token: 0x0200003B RID: 59
	internal class LC_Container
	{
		// Token: 0x060000BE RID: 190 RVA: 0x00007180 File Offset: 0x00005380
		public LC_Container(LaserCombiner _obj)
		{
			this.obj = _obj;
			this.bFire = false;
			this.RldMax = 0.25f;
			this.NowTime = Random.Range(0f, this.RldMax);
			this.root = this.obj.MainConstruct.GameObject.gameObject;
			this.wlobj = Object.Instantiate<GameObject>(BM_EffectUpdate.GetAsset("WaveLaser"), BM_EffectUpdate.updater.transform);
			this.wlobj.GetComponent<SM_WaveLaser>().NowScale = 0f;
			this.wlobj.GetComponent<SM_WaveLaser>().TgtScale = 0f;
			this.wlobj.transform.localScale = Vector3.zero;
		}

		// Token: 0x04000105 RID: 261
		public LaserCombiner obj;

		// Token: 0x04000106 RID: 262
		public GameObject root;

		// Token: 0x04000107 RID: 263
		public bool bFire;

		// Token: 0x04000108 RID: 264
		public float NowTime;

		// Token: 0x04000109 RID: 265
		public float RldMax;

		// Token: 0x0400010A RID: 266
		public GameObject wlobj;
	}
}
