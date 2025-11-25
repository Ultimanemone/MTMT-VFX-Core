using System;
using ProtecTechTools;
using UnityEngine;

namespace BM_EffectUpdate
{
	// Token: 0x02000049 RID: 73
	internal class BM_PropellerExtend : ExtendedClass<Propeller>
	{
		// Token: 0x060000EA RID: 234 RVA: 0x00008DA8 File Offset: 0x00006FA8
		public void InitParticle()
		{
			for (int i = 0; i < 2; i++)
			{
				this.SmokeObj[i] = Object.Instantiate<GameObject>(BM_EffectUpdate.GetAsset("MissileSmoke"));
				ParticleSystem component = this.SmokeObj[i].transform.FindChild("MissileBase").GetComponent<ParticleSystem>();
				ParticleSystem component2 = this.SmokeObj[i].transform.FindChild("SmokeBody").GetComponent<ParticleSystem>();
				ParticleSystem component3 = this.SmokeObj[i].transform.FindChild("SmokeBody_NoFire").GetComponent<ParticleSystem>();
				this.sys2W[i] = this.SmokeObj[i].transform.FindChild("WaterBody").GetComponent<ParticleSystem>();
				ParticleSystem.EmissionModule emission = component2.emission;
				this.emitW[i] = this.sys2W[i].emission;
				ParticleSystem.EmissionModule emission2 = component3.emission;
				this.sys2W[i].startSpeed *= 0.5f;
				this.sys2W[i].startLifetime *= 0.5f;
				this.sys2W[i].startSize *= 0.5f;
				component.emissionRate = 0f;
				emission.rateOverDistance = 0f;
				emission2.rateOverDistance = 0f;
				this.emitW[i].rateOverDistance = 0f;
				this.sys2W[i].sizeOverLifetime.enabled = false;
				this.SmokeObj[i].transform.position = base.this_.GameWorldPosition;
				this.SmokeObj[i].transform.forward = base.this_.GameWorldForwards;
			}
			this.sys2W[1].startLifetime *= 10f;
			this.sys2W[1].startSize *= 3f;
			this.sys2W[1].startSpeed *= 0.25f;
			Color startColor = this.sys2W[1].startColor;
			startColor.a *= 0.5f;
			this.sys2W[1].startColor = startColor;
			this.bInit = true;
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00008FDF File Offset: 0x000071DF
		[ModifyMethod(1, "", "")]
		public void BlockStart()
		{
			this.InitParticle();
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00008FE8 File Offset: 0x000071E8
		[ModifyMethod(1, "", "")]
		public void RunPropulsion()
		{
			((PropellerEffect)BM_EffectUpdater.GetField("EffectSystem", base.this_)).gameObject.SetActive(false);
			try
			{
				if (!this.bInit)
				{
					this.InitParticle();
				}
				StandardPropulsioModule standardPropulsioModule = (StandardPropulsioModule)BM_EffectUpdater.GetField("Propulsion", base.this_);
				float num = base.this_.GameWorldPosition.y;
				if (num < 0f && BM_PropellerExtend.bCreate)
				{
					float num2 = 1f - Mathf.Abs(Vector3.Dot(base.this_.GameWorldForwards, Vector3.up));
					num = -num;
					float num3 = 1f - Mathf.Min(1f, num / 30f);
					float num4 = 1f - Mathf.Min(1f, num / 10f);
					this.emitW[0].rateOverTime = 1f * standardPropulsioModule.Request.LastDrive * num2 * num3;
					this.emitW[0].rateOverDistance = 1f * standardPropulsioModule.Request.LastDrive * num2 * num3;
					this.emitW[1].rateOverTime = 1f * standardPropulsioModule.Request.LastDrive * num2 * num4 * 2f;
					this.emitW[1].rateOverDistance = 1f * standardPropulsioModule.Request.LastDrive * num2 * num4 * 2f;
				}
				else
				{
					this.emitW[0].rateOverTime = 0f;
					this.emitW[0].rateOverDistance = 0f;
					this.emitW[1].rateOverTime = 0f;
					this.emitW[1].rateOverDistance = 0f;
				}
				this.SmokeObj[0].transform.position = base.this_.GameWorldPosition;
				this.SmokeObj[0].transform.forward = base.this_.GameWorldForwards * (float)((standardPropulsioModule.Request.LastDrive > 0f) ? 1 : -1);
				Vector3 gameWorldPosition = base.this_.GameWorldPosition;
				gameWorldPosition.y = 0f;
				this.SmokeObj[1].transform.position = base.this_.GameWorldPosition;
				this.SmokeObj[1].transform.position = gameWorldPosition;
				this.SmokeObj[1].transform.forward = base.this_.GameWorldForwards * (float)((standardPropulsioModule.Request.LastDrive > 0f) ? 1 : -1);
			}
			catch (Exception)
			{
				Debug.Log("BMEU_PropellerError try InitParticle()");
				this.InitParticle();
			}
		}

		// Token: 0x060000ED RID: 237 RVA: 0x000092E8 File Offset: 0x000074E8
		[ModifyMethod(1, "", "")]
		public void StateChanged(IBlockStateChange change)
		{
			if (change.IsLostToConstructOrConstructLost)
			{
				Object.Destroy(this.SmokeObj[0]);
				Object.Destroy(this.SmokeObj[1]);
			}
			if (change.IsRepaired)
			{
				this.InitParticle();
			}
		}

		// Token: 0x0400012B RID: 299
		private GameObject[] SmokeObj = new GameObject[2];

		// Token: 0x0400012C RID: 300
		private ParticleSystem[] sys2W = new ParticleSystem[2];

		// Token: 0x0400012D RID: 301
		private ParticleSystem.EmissionModule[] emitW = new ParticleSystem.EmissionModule[2];

		// Token: 0x0400012E RID: 302
		public static bool bCreate;

		// Token: 0x0400012F RID: 303
		private bool bInit;
	}
}
