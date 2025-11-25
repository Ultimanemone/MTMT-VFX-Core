using System;
using System.Reflection;
using ProtecTechTools;
using UnityEngine;

namespace BM_EffectUpdate
{
	// Token: 0x0200004D RID: 77
	internal class SM_CreateMissileSmoke : MonoBehaviour
	{
		// Token: 0x060000F6 RID: 246 RVA: 0x00009440 File Offset: 0x00007640
		public void Start()
		{
			this.bInit = false;
			this.obj = Object.Instantiate<GameObject>(BM_EffectUpdate.GetAsset("MissileSmoke"));
			this.obj.transform.position = base.transform.position;
			this.obj.transform.rotation = base.transform.rotation;
			this.Fuel = 0f;
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x000094AC File Offset: 0x000076AC
		public void Update()
		{
			if (!this.bInit)
			{
				this.sys = this.obj.transform.FindChild("MissileBase").GetComponent<ParticleSystem>();
				this.sys2S = this.obj.transform.FindChild("SmokeBody").GetComponent<ParticleSystem>();
				this.sys2S_NF = this.obj.transform.FindChild("SmokeBody_NoFire").GetComponent<ParticleSystem>();
				this.sys2W = this.obj.transform.FindChild("WaterBody").GetComponent<ParticleSystem>();
				this.emitS = this.sys2S.emission;
				this.emitW = this.sys2W.emission;
				this.emitS_NF = this.sys2S_NF.emission;
				this.proj = base.GetComponent<CustomMissile>();
				if (this.proj.GetType().ToString().EndsWith("CustomMissileGauged"))
				{
					PropertyInfo property = this.proj.GetType().GetProperty("GaugeFactor", BindingFlags.Instance | BindingFlags.Public);
					this.Gauge = (float)Access.GetValue(property, this.proj);
					if (this.Gauge < 0.5f)
					{
						this.RateFactor = 5f;
						this.Gauge = 0.5f;
					}
				}
				else
				{
					this.RateFactor = 1f;
					this.Gauge = 1f;
				}
				this.bInit = true;
			}
			this.obj.transform.localScale = Vector3.one * this.Gauge * 0.5f;
			this.obj.transform.rotation = base.transform.rotation;
			this.obj.transform.position = base.transform.position - base.transform.forward * ((float)this.proj.components.Count / 2f);
			this.sys.emissionRate = 0f;
			this.emitS.rateOverDistance = 0f;
			this.emitS_NF.rateOverDistance = 0f;
			this.emitW.rateOverDistance = 0f;
			bool flag = false;
			bool flag2 = false;
			foreach (MissileComponent missileComponent in this.proj.components)
			{
				if (missileComponent.GetComponentType() == 50 || missileComponent.GetComponentType() == 52 || missileComponent.GetType().ToString().EndsWith("MissileVariableThrusterGauged"))
				{
					flag = true;
				}
				if (missileComponent.GetComponentType() == 51)
				{
					flag2 = true;
				}
			}
			bool flag3 = base.transform.position.y > 0f;
			if (this.proj.thrusters.Count > 0)
			{
				if (flag3)
				{
					if (flag && this.proj.volume > 0.01f)
					{
						if (this.proj.fuel < this.Fuel)
						{
							this.sys.emissionRate = 8f;
							this.emitS.rateOverDistance = 1f * this.RateFactor;
						}
						else
						{
							this.emitS_NF.rateOverDistance = 4f * this.RateFactor;
						}
					}
				}
				else if (flag2)
				{
					this.emitW.rateOverDistance = 4f * this.RateFactor;
				}
				this.Fuel = this.proj.fuel;
			}
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x0000984C File Offset: 0x00007A4C
		public void OnDestroy()
		{
			if (this.obj != null)
			{
				Transform transform = this.obj.transform.FindChild("MissileBase");
				if (transform)
				{
					transform.GetComponent<ParticleSystem>().emissionRate = 0f;
				}
				Object.Destroy(this.obj, 10f);
			}
		}

		// Token: 0x04000133 RID: 307
		private GameObject obj;

		// Token: 0x04000134 RID: 308
		private float Fuel;

		// Token: 0x04000135 RID: 309
		private float Gauge = 1f;

		// Token: 0x04000136 RID: 310
		private bool bInit;

		// Token: 0x04000137 RID: 311
		private CustomMissile proj;

		// Token: 0x04000138 RID: 312
		private float RateFactor = 1f;

		// Token: 0x04000139 RID: 313
		private ParticleSystem sys;

		// Token: 0x0400013A RID: 314
		private ParticleSystem sys2S;

		// Token: 0x0400013B RID: 315
		private ParticleSystem sys2S_NF;

		// Token: 0x0400013C RID: 316
		private ParticleSystem sys2W;

		// Token: 0x0400013D RID: 317
		private ParticleSystem.EmissionModule emitS;

		// Token: 0x0400013E RID: 318
		private ParticleSystem.EmissionModule emitW;

		// Token: 0x0400013F RID: 319
		private ParticleSystem.EmissionModule emitS_NF;
	}
}
