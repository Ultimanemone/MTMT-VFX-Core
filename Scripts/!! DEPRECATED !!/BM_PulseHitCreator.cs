using System;
using UnityEngine;

namespace BM_EffectUpdate
{
	// Token: 0x02000032 RID: 50
	internal class BM_PulseHitCreator : MonoBehaviour
	{
		// Token: 0x060000A6 RID: 166 RVA: 0x000068B8 File Offset: 0x00004AB8
		public void Start()
		{
			this.creator = BM_EffectCreator.Creator;
			BM_PulseHitCreator.hitobj = this.creator.CreateLaser(BM_LaserEffectName.LaserHit);
			BM_PulseHitCreator.hitobj.transform.parent = base.transform;
			base.GetComponent<SM_AutoKill>().Init();
		}

		// Token: 0x040000F0 RID: 240
		private static GameObject hitobj;

		// Token: 0x040000F1 RID: 241
		private BM_EffectCreator creator;
	}
}
