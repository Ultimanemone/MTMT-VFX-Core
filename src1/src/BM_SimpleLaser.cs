using System;
using UnityEngine;

namespace BM_EffectUpdate
{
	// Token: 0x0200003A RID: 58
	internal class BM_SimpleLaser : MonoBehaviour
	{
		// Token: 0x060000BB RID: 187 RVA: 0x0000707A File Offset: 0x0000527A
		public void Start()
		{
			this.las = base.GetComponent<Laser>();
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00007088 File Offset: 0x00005288
		public void Update()
		{
			if (((CarriedObjectReference)BM_EffectUpdater.GetField("beam", this.las)).ActiveNow)
			{
				this.time += Time.deltaTime;
				if (this.time > this.Rld)
				{
					GameObject gameObject = BM_EffectCreator.Creator.CreateLaser(BM_LaserEffectName.LaserFlash);
					gameObject.transform.position = this.las.GetFirePoint(0f);
					gameObject.transform.localScale = Vector3.one * 0.1f;
					gameObject.transform.forward = this.las.GetFireDirection();
					gameObject.GetComponent<SM_LaserFlash>().Parent = null;
					gameObject.GetComponent<SM_LaserFlash>().SetColor(new Color(0f, 1f, 0f));
					gameObject.GetComponent<SM_LaserFlash>().Play();
					this.time = 0f;
				}
			}
		}

		// Token: 0x04000102 RID: 258
		public float Rld = 0.1f;

		// Token: 0x04000103 RID: 259
		public float time;

		// Token: 0x04000104 RID: 260
		private Laser las;
	}
}
