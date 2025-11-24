using System;
using System.Collections.Generic;
using UnityEngine;

namespace BM_EffectUpdate.EffectCode
{
	// Token: 0x02000053 RID: 83
	internal class SM_LightController : MonoBehaviour
	{
		// Token: 0x0600010D RID: 269 RVA: 0x0000A524 File Offset: 0x00008724
		public void Start()
		{
			this.MyLight = base.GetComponent<Light>();
			this.BaseRange = this.MyLight.range;
			this.child = Object.Instantiate<GameObject>(BM_EffectUpdate.GetAsset("LightChild"), base.transform);
			this.WaterLight = this.child.GetComponent<Light>();
			this.WaterLight.cullingMask = 1 << LayerMask.NameToLayer("Water");
			SM_LightController.SMLC_LightList.Add(this.MyLight);
			SM_LightController.SMLC_LightList.Add(this.WaterLight);
			this.FadeSpd = 1f;
		}

		// Token: 0x0600010E RID: 270 RVA: 0x0000A5BF File Offset: 0x000087BF
		public void Init()
		{
			this.MyLight.color = this.StartColor;
			this.time = 1f;
		}

		// Token: 0x0600010F RID: 271 RVA: 0x0000A5E0 File Offset: 0x000087E0
		public void Update()
		{
			if (base.transform.parent.GetComponent<SM_AutoKill>() && base.transform.parent.GetComponent<SM_AutoKill>().nowInit)
			{
				this.Init();
			}
			this.time -= this.FadeSpd * base.transform.parent.GetComponent<SM_AutoKill>().deltaTime;
			this.MyLight.color = Color.Lerp(new Color(0f, 0f, 0f, 1f), this.StartColor, Mathf.Max(0f, this.time));
			this.MyLight.range = this.BaseRange * this.time * Mathf.Max(1f, base.transform.parent.localScale.x);
			this.WaterLight.color = this.MyLight.color;
			this.WaterLight.intensity = this.MyLight.intensity * 1f;
			this.WaterLight.range = this.MyLight.range * 2f;
		}

		// Token: 0x04000163 RID: 355
		public static bool bEnable;

		// Token: 0x04000164 RID: 356
		public static List<Light> SMLC_LightList = new List<Light>();

		// Token: 0x04000165 RID: 357
		public float FadeSpd = 1f;

		// Token: 0x04000166 RID: 358
		public Color StartColor;

		// Token: 0x04000167 RID: 359
		private float time = 1f;

		// Token: 0x04000168 RID: 360
		private Light WaterLight;

		// Token: 0x04000169 RID: 361
		private Light MyLight;

		// Token: 0x0400016A RID: 362
		private GameObject child;

		// Token: 0x0400016B RID: 363
		private float BaseRange;
	}
}
