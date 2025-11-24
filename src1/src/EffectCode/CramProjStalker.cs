using System;
using UnityEngine;

namespace BM_EffectUpdate.EffectCode
{
	// Token: 0x02000052 RID: 82
	internal class CramProjStalker : MonoBehaviour
	{
		// Token: 0x0600010A RID: 266 RVA: 0x0000A468 File Offset: 0x00008668
		public void Start()
		{
			this.bExplode = false;
		}

		// Token: 0x0600010B RID: 267 RVA: 0x0000A474 File Offset: 0x00008674
		public void Update()
		{
			if (!this.proj.gameObject.activeSelf)
			{
				if (this.bExplode)
				{
					this.bExplode = false;
					float explosiveDamage = this.proj._pState.ExplosiveDamage;
					float explosiveRadius = this.proj._pState.ExplosiveRadius;
					if (explosiveRadius > 20f)
					{
						BM_ExplosionsName type = BM_ExplosionsName.DistShockWave;
						GameObject gameObject = BM_EffectCreator.Creator.CreateExplosion(type);
						gameObject.transform.localScale = Vector3.one * explosiveRadius * 2f;
						gameObject.transform.localPosition = this.proj.myTransform.position;
						return;
					}
				}
			}
			else
			{
				this.bExplode = true;
			}
		}

		// Token: 0x04000161 RID: 353
		public PooledCramProjectile proj;

		// Token: 0x04000162 RID: 354
		public bool bExplode;
	}
}
