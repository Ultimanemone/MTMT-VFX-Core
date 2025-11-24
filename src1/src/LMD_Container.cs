using System;
using UnityEngine;

namespace BM_EffectUpdate
{
	// Token: 0x0200003D RID: 61
	internal class LMD_Container
	{
		// Token: 0x060000C4 RID: 196 RVA: 0x000075EC File Offset: 0x000057EC
		public LMD_Container(LaserMissileDefence _obj)
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
