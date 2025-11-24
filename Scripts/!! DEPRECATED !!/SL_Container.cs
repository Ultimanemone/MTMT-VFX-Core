using System;
using UnityEngine;

namespace BM_EffectUpdate
{
	// Token: 0x02000037 RID: 55
	internal class SL_Container
	{
		// Token: 0x060000B3 RID: 179 RVA: 0x00006D54 File Offset: 0x00004F54
		public SL_Container(Laser _obj)
		{
			this.obj = _obj;
			this.bFire = false;
			this.RldMax = 0.1f;
			this.NowTime = Random.Range(0f, this.RldMax);
			this.root = this.obj.MainConstruct.GameObject.gameObject;
		}

		// Token: 0x040000FA RID: 250
		public Laser obj;

		// Token: 0x040000FB RID: 251
		public GameObject root;

		// Token: 0x040000FC RID: 252
		public bool bFire;

		// Token: 0x040000FD RID: 253
		public float NowTime;

		// Token: 0x040000FE RID: 254
		public float RldMax;
	}
}
