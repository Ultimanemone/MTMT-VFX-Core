using System;
using UnityEngine;

namespace BM_EffectUpdate
{
	// Token: 0x02000036 RID: 54
	internal class BM_PacCreator : MonoBehaviour
	{
		// Token: 0x060000B0 RID: 176 RVA: 0x00006BDF File Offset: 0x00004DDF
		public void Start()
		{
			this.creator = BM_EffectCreator.Creator;
			this.pac = base.GetComponent<ParticleCannonEffect>();
			this.Damage = this.pac.Range0Damage;
			this.bInit = false;
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00006C10 File Offset: 0x00004E10
		public void LateUpdate()
		{
			if (!this.bInit)
			{
				this.bInit = true;
				this.pac._lineRenderer.widthMultiplier = 0f;
				this.pac._secondaryEffectRenderer.widthMultiplier = 0f;
				this.pac._lineRenderer.enabled = false;
				this.pac._secondaryEffectRenderer.enabled = false;
				GameObject gameObject = this.creator.CreateLaser(BM_LaserEffectName.PacEffect);
				SM_PacEffect component = gameObject.GetComponent<SM_PacEffect>();
				float num = this.Damage / 50000f;
				gameObject.transform.localScale = Vector3.one * Mathf.Lerp(0.025f, 1.5f, num);
				gameObject.transform.position = base.transform.position;
				gameObject.transform.rotation = base.transform.rotation;
				Vector3[] array = new Vector3[this.pac._lineRenderer.positionCount];
				this.pac._lineRenderer.GetPositions(array);
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = StaticMaths.LocalToGlobal(array[i], base.transform.position, base.transform.rotation);
				}
				component.LinePos = array;
			}
		}

		// Token: 0x040000F6 RID: 246
		private bool bInit;

		// Token: 0x040000F7 RID: 247
		private ParticleCannonEffect pac;

		// Token: 0x040000F8 RID: 248
		public float Damage;

		// Token: 0x040000F9 RID: 249
		private BM_EffectCreator creator;
	}
}
