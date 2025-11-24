using System;
using UnityEngine;

namespace BM_EffectUpdate.EffectCode
{
	// Token: 0x02000051 RID: 81
	internal class AdvProjStalker : MonoBehaviour
	{
		// Token: 0x06000107 RID: 263 RVA: 0x0000A368 File Offset: 0x00008568
		public void Start()
		{
			this.bExplode = false;
		}

		// Token: 0x06000108 RID: 264 RVA: 0x0000A374 File Offset: 0x00008574
		public void Update()
		{
			if (!this.proj.gameObject.activeSelf)
			{
				if ((bool)BM_EffectUpdater.GetField("_explodedAlready", this.proj) && this.bExplode)
				{
					this.bExplode = false;
					ShellModel shellModel = (ShellModel)BM_EffectUpdater.GetField("ShellModel", this.proj);
					shellModel.ExplosiveCharges.GetExplosionDamage();
					shellModel.ExplosiveCharges.GetFlakExplosionDamage();
					float num = shellModel.ExplosiveCharges.GetExplosionRadius();
					float flakExplosionRadius = shellModel.ExplosiveCharges.GetFlakExplosionRadius();
					num = Mathf.Max(num, flakExplosionRadius);
					if (num > 20f)
					{
						BM_ExplosionsName type = BM_ExplosionsName.DistShockWave;
						GameObject gameObject = BM_EffectCreator.Creator.CreateExplosion(type);
						gameObject.transform.localScale = Vector3.one * num * 2f;
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

		// Token: 0x0400015F RID: 351
		public AdvPooledProjectile proj;

		// Token: 0x04000160 RID: 352
		public bool bExplode;
	}
}
