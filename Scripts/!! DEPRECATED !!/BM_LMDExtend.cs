using System;
using ProtecTechTools;
using UnityEngine;

namespace BM_EffectUpdate
{
	// Token: 0x02000046 RID: 70
	internal class BM_LMDExtend : ExtendedClass<LaserMissileDefence>
	{
		// Token: 0x060000DD RID: 221 RVA: 0x00008528 File Offset: 0x00006728
		[ModifyMethod(1, "", "")]
		public void ComponentStart()
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

		// Token: 0x060000DE RID: 222 RVA: 0x000085D8 File Offset: 0x000067D8
		[ModifyMethod(1, "", "")]
		public void FixedFire()
		{
			if (this.wlobj == null)
			{
				return;
			}
			LineRenderer lineRenderer = base.this_.CWBeam.LineRenderer;
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
				if (base.this_.isAlive)
				{
					component.transform.position = base.this_.GameWorldPosition;
					Vector3 vector = lineRenderer.GetPosition(1);
					vector = StaticMaths.LocalToGlobal(vector, lineRenderer.transform.position, lineRenderer.transform.rotation);
					component.transform.forward = vector - component.transform.position;
					component.Length = Vector3.Distance(component.transform.position, vector);
					component.SetColor(lineRenderer.startColor);
				}
			}
			catch (ArgumentException)
			{
				component.TgtScale = 0f;
			}
			lineRenderer.enabled = false;
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00008728 File Offset: 0x00006928
		[ModifyMethod(1, "", "")]
		public void StateChanged(IBlockStateChange change)
		{
			if (change.IsLostToConstructOrConstructLost)
			{
				Object.Destroy(this.wlobj);
			}
		}

		// Token: 0x04000121 RID: 289
		public GameObject root;

		// Token: 0x04000122 RID: 290
		public bool bFire;

		// Token: 0x04000123 RID: 291
		public float NowTime;

		// Token: 0x04000124 RID: 292
		public float RldMax;

		// Token: 0x04000125 RID: 293
		public GameObject wlobj;
	}
}
