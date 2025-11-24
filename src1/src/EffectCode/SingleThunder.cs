using System;
using UnityEngine;

namespace BM_EffectUpdate.EffectCode
{
	// Token: 0x02000054 RID: 84
	public class SingleThunder : MonoBehaviour
	{
		// Token: 0x06000112 RID: 274 RVA: 0x0000A739 File Offset: 0x00008939
		private void Start()
		{
			this.Line = base.GetComponent<LineRenderer>();
			base.GetComponent<Renderer>().material.SetTextureOffset("_MaskTex", new Vector2(0f, Random.value));
			this.SetThunder();
		}

		// Token: 0x06000113 RID: 275 RVA: 0x0000A774 File Offset: 0x00008974
		public void SetThunder()
		{
			for (int i = 0; i < 8; i++)
			{
				float num = (float)i / 8f;
				this.PosBuf[i].x = Random.Range(-this.Width, this.Width) * num;
				this.PosBuf[i].z = Random.Range(-this.Width, this.Width) * num;
				this.PosBuf[i].y = num * this.Height;
				this.Line.SetPositions(this.PosBuf);
				this.Line.widthMultiplier = 10f;
				this.MoveVec[i] = new Vector3((float)Random.Range(-1, 1), (float)Random.Range(-1, 1), (float)Random.Range(-1, 1)) * num * this.MoveLength * 100f;
			}
		}

		// Token: 0x06000114 RID: 276 RVA: 0x0000A864 File Offset: 0x00008A64
		private void Update()
		{
			for (int i = 0; i < 8; i++)
			{
				float num = (float)(8 - i - 1) / 8f;
				Vector3[] posBuf = this.PosBuf;
				int num2 = i;
				posBuf[num2].y = posBuf[num2].y - base.transform.parent.parent.GetComponent<SM_AutoKill>().deltaTime * num * 50f;
				this.PosBuf[i] += this.MoveVec[i] * base.transform.parent.parent.GetComponent<SM_AutoKill>().deltaTime;
				this.Line.SetPositions(this.PosBuf);
			}
		}

		// Token: 0x0400016C RID: 364
		private LineRenderer Line;

		// Token: 0x0400016D RID: 365
		public float Width = 1f;

		// Token: 0x0400016E RID: 366
		public float Height = 10f;

		// Token: 0x0400016F RID: 367
		public float MoveLength = 0.1f;

		// Token: 0x04000170 RID: 368
		private Vector3[] PosBuf = new Vector3[8];

		// Token: 0x04000171 RID: 369
		private Vector3[] MoveVec = new Vector3[8];
	}
}
