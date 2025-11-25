using System;
using ProtecTechTools;
using UnityEngine;

namespace BM_EffectUpdate
{
	// Token: 0x02000045 RID: 69
	internal class BM_LaserCombinerExtend : ExtendedClass<LaserCombiner>
	{
		// Token: 0x060000D9 RID: 217 RVA: 0x0000825C File Offset: 0x0000645C
		[ModifyMethod(1, "", "")]
		public void WeaponStart()
		{
			this.bFire = false;
			this.RldMax = 0.25f;
			this.NowTime = Random.Range(0f, this.RldMax);
			this.root = base.this_.MainConstruct.GameObject.gameObject;
			this.wlobj = Object.Instantiate<GameObject>(BM_EffectUpdate.GetAsset("WaveLaser"), BM_EffectUpdate.updater.transform);
			this.wlobj.GetComponent<SM_WaveLaser>().NowScale = 0f;
			this.wlobj.GetComponent<SM_WaveLaser>().TgtScale = 0f;
			this.wlobj.transform.localScale = Vector3.zero;
		}

		// Token: 0x060000DA RID: 218 RVA: 0x0000830C File Offset: 0x0000650C
		[ModifyMethod(1, "", "")]
		public void FixedUpdate_Fire()
		{
			LineRenderer lineRenderer = base.this_.CWBeam.LineRenderer;
			if (this.wlobj == null)
			{
				return;
			}
			SM_WaveLaser component = this.wlobj.GetComponent<SM_WaveLaser>();
			if (component == null)
			{
				return;
			}
			try
			{
				if (base.this_.isAlive && base.this_.CWBeam.gameObject.activeSelf && !base.this_.pulseBeam.gameObject.activeSelf)
				{
					component.TgtScale = base.this_.CWBeam.laserWidth;
				}
				else
				{
					component.TgtScale = 0f;
				}
				if (base.this_.isAlive && base.this_.canFire)
				{
					int num = base.this_.Node.allOptics.Count + 2;
					if (lineRenderer.positionCount >= num - 1)
					{
						component.transform.position = base.this_.GetFirePoint(0f);
						Vector3 vector = lineRenderer.GetPosition(num - 1);
						vector = StaticMaths.LocalToGlobal(vector, lineRenderer.transform.position, lineRenderer.transform.rotation);
						component.transform.forward = vector - component.transform.position;
						component.Length = Vector3.Distance(component.transform.position, vector);
					}
					component.SetColor(lineRenderer.startColor);
					((Light)BM_EffectUpdater.GetField("cwLight", base.this_)).enabled = false;
					((ParticleSystem.MainModule)BM_EffectUpdater.GetField("cwFireParticle", base.this_)).startColor = Color.black;
					((LensFlare)BM_EffectUpdater.GetField("cwLensEffect", base.this_)).enabled = false;
				}
			}
			catch (ArgumentException)
			{
				component.TgtScale = 0f;
			}
			lineRenderer.enabled = false;
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00008508 File Offset: 0x00006708
		[ModifyMethod(1, "", "")]
		public void StateChanged(IBlockStateChange change)
		{
			if (change.IsLostToConstructOrConstructLost)
			{
				Object.Destroy(this.wlobj);
			}
		}

		// Token: 0x0400011C RID: 284
		public GameObject root;

		// Token: 0x0400011D RID: 285
		public bool bFire;

		// Token: 0x0400011E RID: 286
		public float NowTime;

		// Token: 0x0400011F RID: 287
		public float RldMax;

		// Token: 0x04000120 RID: 288
		public GameObject wlobj;
	}
}
