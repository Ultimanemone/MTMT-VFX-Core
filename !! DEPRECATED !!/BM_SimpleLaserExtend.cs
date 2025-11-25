using System;
using ProtecTechTools;
using UnityEngine;

namespace BM_EffectUpdate
{
	// Token: 0x02000044 RID: 68
	internal class BM_SimpleLaserExtend : ExtendedClass<Laser>
	{
		// Token: 0x060000D5 RID: 213 RVA: 0x000080A4 File Offset: 0x000062A4
		[ModifyMethod(1, "", "")]
		public void WeaponStart()
		{
			this.bFire = false;
			this.RldMax = 0.1f;
			this.NowTime = Random.Range(0f, this.RldMax);
			this.root = base.this_.MainConstruct.GameObject.gameObject;
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x000080F4 File Offset: 0x000062F4
		private void FixedFire(float dt)
		{
			CarriedObjectReference carriedObjectReference = (CarriedObjectReference)BM_EffectUpdater.GetField("beam", base.this_);
			if (carriedObjectReference.ActiveNow)
			{
				this.NowTime -= Time.deltaTime;
				if (this.NowTime < 0f)
				{
					this.NowTime = this.RldMax;
					GameObject gameObject = BM_EffectCreator.Creator.CreateLaser(BM_LaserEffectName.LaserFlash);
					if (gameObject != null)
					{
						gameObject.transform.localScale = Vector3.one * 0.1f;
						gameObject.transform.localRotation = Quaternion.identity;
						gameObject.GetComponent<SM_LaserFlash>().AddPos = new Vector3(0f, 0f, 3.2f);
						gameObject.GetComponent<SM_LaserFlash>().Parent = carriedObjectReference.ObjectItself.transform;
						gameObject.GetComponent<SM_LaserFlash>().SetColor(new Color(0.1f, 0.25f, 0.1f));
						gameObject.GetComponent<SM_LaserFlash>().Play();
					}
				}
			}
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x000081F4 File Offset: 0x000063F4
		[ModifyMethod(1, "", "")]
		public void StateChanged(IBlockStateChange change)
		{
			if (change.IsAvailableToConstruct)
			{
				base.this_.MainConstruct.iScheduler.RegisterForFixedUpdate(new Action<float>(this.FixedFire));
				return;
			}
			if (change.IsLostToConstructOrConstructLost)
			{
				base.this_.MainConstruct.iScheduler.UnregisterForFixedUpdate(new Action<float>(this.FixedFire));
			}
		}

		// Token: 0x04000118 RID: 280
		public GameObject root;

		// Token: 0x04000119 RID: 281
		public bool bFire;

		// Token: 0x0400011A RID: 282
		public float NowTime;

		// Token: 0x0400011B RID: 283
		public float RldMax;
	}
}
