using System;
using BM_EffectUpdate.EffectCode;
using UnityEngine;

namespace BM_EffectUpdate
{
	// Token: 0x0200004F RID: 79
	internal class SM_StickyFlare : MonoBehaviour
	{
		// Token: 0x060000FE RID: 254 RVA: 0x00009C98 File Offset: 0x00007E98
		private void Start()
		{
			this.light = base.gameObject.AddComponent<Light>();
			this.light.range = 100f;
			this.light.color = new Color(1f, 0.5f, 0.25f);
			this.light.renderMode = 1;
			this.light.type = 2;
			this.light.enabled = false;
			this.flare = base.GetComponent<StickyFlare>();
			this.light.intensity = 3f;
			this.time = 3f;
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00009D30 File Offset: 0x00007F30
		private void LateUpdate()
		{
			this.time -= Time.deltaTime;
			if (this.time < 0f)
			{
				this.light.intensity *= 0.9f;
			}
			if (SM_LightController.bEnable)
			{
				this.light.enabled = (this.light.range > 0f);
				return;
			}
			this.light.enabled = false;
		}

		// Token: 0x0400014F RID: 335
		public Light light;

		// Token: 0x04000150 RID: 336
		public StickyFlare flare;

		// Token: 0x04000151 RID: 337
		private float time;
	}
}
