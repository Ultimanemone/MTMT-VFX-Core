using System;
using UnityEngine;

namespace BM_EffectUpdate.EffectCode
{
	// Token: 0x02000055 RID: 85
	public class SM_Fadeout : MonoBehaviour
	{
		// Token: 0x06000116 RID: 278 RVA: 0x0000A96C File Offset: 0x00008B6C
		private void Start()
		{
			Renderer component = base.GetComponent<Renderer>();
			this.mat = component.material;
			this.DefColor = this.mat.GetColor("_TintColor");
			this.DefAlpha = this.DefColor.a;
			this.TgtColor = this.DefColor;
			this.TgtColor.a = this.TgtAlpha;
		}

		// Token: 0x06000117 RID: 279 RVA: 0x0000A9D0 File Offset: 0x00008BD0
		private void Update()
		{
			if (base.transform.parent.parent.GetComponent<SM_AutoKill>().nowInit)
			{
				this.mat.SetColor("_TintColor", this.DefColor);
				this.time = 0f;
			}
			this.time += base.transform.parent.parent.GetComponent<SM_AutoKill>().deltaTime * this.spd;
			this.mat.SetColor("_TintColor", Color.Lerp(this.DefColor, this.TgtColor, Mathf.Min(1f, this.time)));
		}

		// Token: 0x04000172 RID: 370
		public float time;

		// Token: 0x04000173 RID: 371
		public float spd = 1f;

		// Token: 0x04000174 RID: 372
		public float TgtAlpha;

		// Token: 0x04000175 RID: 373
		private Material mat;

		// Token: 0x04000176 RID: 374
		private Color DefColor;

		// Token: 0x04000177 RID: 375
		private Color TgtColor;

		// Token: 0x04000178 RID: 376
		private float DefAlpha;
	}
}
