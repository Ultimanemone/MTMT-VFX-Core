using System;
using UnityEngine;

namespace BM_EffectUpdate
{
	// Token: 0x0200004E RID: 78
	public class SM_PacEffect : MonoBehaviour
	{
		// Token: 0x060000FA RID: 250 RVA: 0x000098C3 File Offset: 0x00007AC3
		public void Start()
		{
			this.creator = BM_EffectCreator.Creator;
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00002BE3 File Offset: 0x00000DE3
		public void Init()
		{
		}

		// Token: 0x060000FC RID: 252 RVA: 0x000098D0 File Offset: 0x00007AD0
		private void Update()
		{
			if (base.GetComponent<SM_AutoKill>().bCreate)
			{
				return;
			}
			if (base.GetComponent<SM_AutoKill>().nowInit)
			{
				this.ScaleUp = true;
				Transform transform = base.transform.Find("PacFlash");
				transform.GetComponent<ParticleSystem>().time = 0f;
				transform.GetComponent<ParticleSystem>().Play();
				this.MaxScale = 23f;
				this.LR = base.GetComponent<LineRenderer>();
				this.LRSecond = base.transform.FindChild("LineSecond").GetComponent<LineRenderer>();
				this.LR.enabled = true;
				this.LRSecond.enabled = true;
				Vector3[] linePos = this.LinePos;
				this.LR.positionCount = this.LinePos.Length;
				this.LR.SetPositions(linePos);
				GameObject gameObject = this.creator.CreateLaser(BM_LaserEffectName.ShockWave);
				gameObject.transform.position = linePos[0];
				gameObject.transform.forward = base.transform.forward;
				gameObject.transform.localScale = Vector3.one * 2f * base.transform.localScale.x;
				if (base.transform.localScale.x >= 0.75f)
				{
					GameObject gameObject2 = this.creator.CreateLaser(BM_LaserEffectName.ShockWave);
					gameObject2.transform.position = linePos[0] + base.transform.forward * 50f * base.transform.localScale.x;
					gameObject2.transform.forward = base.transform.forward;
					gameObject2.transform.localScale = Vector3.one * 1f * base.transform.localScale.x;
				}
				if (base.transform.localScale.x >= 1.3f)
				{
					GameObject gameObject3 = this.creator.CreateLaser(BM_LaserEffectName.ShockWave);
					gameObject3.transform.position = linePos[0] + base.transform.forward * 100f * base.transform.localScale.x;
					gameObject3.transform.forward = base.transform.forward;
					gameObject3.transform.localScale = Vector3.one * 0.5f * base.transform.localScale.x;
				}
				this.LRSecond.positionCount = this.LinePos.Length;
				this.LRSecond.SetPositions(linePos);
			}
			this.LR.widthMultiplier = this.NowWidth * base.transform.localScale.x;
			if (this.ScaleUp)
			{
				this.NowWidth += this.MaxScale / 2f;
				if (this.NowWidth > this.MaxScale)
				{
					this.ScaleUp = false;
					return;
				}
			}
			else
			{
				this.LRSecond.widthMultiplier = this.NowWidth2 * base.transform.localScale.x;
				this.NowWidth = Mathf.Lerp(this.NowWidth, 0f, this.LerpSpd * (Time.deltaTime / 0.016666668f));
				this.NowWidth2 = Mathf.Lerp(this.NowWidth2, 0f, this.LerpSpd2 * (Time.deltaTime / 0.016666668f));
			}
		}

		// Token: 0x04000140 RID: 320
		public GameObject Wave;

		// Token: 0x04000141 RID: 321
		public GameObject Thunder;

		// Token: 0x04000142 RID: 322
		public LineRenderer LR;

		// Token: 0x04000143 RID: 323
		public LineRenderer LRSecond;

		// Token: 0x04000144 RID: 324
		public float WaveLen = 500f;

		// Token: 0x04000145 RID: 325
		public float NowWidth;

		// Token: 0x04000146 RID: 326
		public float NowWidth2 = 10f;

		// Token: 0x04000147 RID: 327
		public float LerpSpd = 0.1f;

		// Token: 0x04000148 RID: 328
		public float LerpSpd2 = 0.03f;

		// Token: 0x04000149 RID: 329
		public int ThunderMax = 3;

		// Token: 0x0400014A RID: 330
		public float ThunderRnd = 10f;

		// Token: 0x0400014B RID: 331
		public float MaxScale;

		// Token: 0x0400014C RID: 332
		private bool ScaleUp = true;

		// Token: 0x0400014D RID: 333
		public Vector3[] LinePos;

		// Token: 0x0400014E RID: 334
		private BM_EffectCreator creator;
	}
}
