using System;
using ProtecTechTools;
using UnityEngine;

namespace BM_EffectUpdate
{
	// Token: 0x0200003F RID: 63
	internal class BM_NukeExtend : ExtendedClass<AprilFirst>
	{
		// Token: 0x060000CA RID: 202 RVA: 0x000079EC File Offset: 0x00005BEC
		[ModifyMethod(1, "", "")]
		public void Detonate()
		{
			if (!this.bDetonate)
			{
				this.bDetonate = true;
				GameObject gameObject = GameObject.Find("Detonator-MushroomCloud(Clone)");
				if (gameObject)
				{
					GameObject gameObject2 = BM_EffectCreator.Creator.CreateExplosion(BM_ExplosionsName.AtomicBom);
					gameObject2.transform.localScale = Vector3.one * 100f;
					gameObject2.transform.localPosition = gameObject.transform.position;
					Object.Destroy(gameObject);
				}
			}
		}

		// Token: 0x04000115 RID: 277
		private bool bDetonate;
	}
}
