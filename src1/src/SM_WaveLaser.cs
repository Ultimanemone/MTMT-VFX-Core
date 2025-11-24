using System;
using UnityEngine;

namespace BM_EffectUpdate
{
	// Token: 0x02000050 RID: 80
	public class SM_WaveLaser : MonoBehaviour
	{
		// Token: 0x06000101 RID: 257 RVA: 0x00009DA4 File Offset: 0x00007FA4
		public void Start()
		{
			this.width = 0.5f;
			this.creator = BM_EffectCreator.Creator;
			this.Line1 = base.transform.FindChild("Line1").gameObject;
			this.Line2 = base.transform.FindChild("Line2").gameObject;
			base.transform.localScale = Vector3.zero;
			this.Init();
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00009E13 File Offset: 0x00008013
		public void Init()
		{
			this.time = 0f;
			this.LerpSpd = 0.5f;
			base.transform.localScale = Vector3.zero;
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00009E3C File Offset: 0x0000803C
		private void LateUpdate()
		{
			this.NowScale = Mathf.Lerp(this.NowScale, this.TgtScale, this.LerpSpd);
			base.transform.localScale = Vector3.one * this.NowScale;
			if (this.Line1.GetComponent<MeshFilter>().mesh != null && this.Line2.GetComponent<MeshFilter>().mesh != null)
			{
				this.MeshControll(this.Line1.GetComponent<MeshFilter>().mesh, false);
				this.MeshControll(this.Line2.GetComponent<MeshFilter>().mesh, true);
				this.Line1.GetComponent<MeshRenderer>().material.SetTextureScale("_MainTex", new Vector2(this.Length * 0.1f * 0.5f, 1f));
				this.Line1.GetComponent<MeshRenderer>().material.SetTextureScale("_NoizeTex", new Vector2(this.Length * 1.82f * 0.1f * 0.5f, 1f));
				this.Line2.GetComponent<MeshRenderer>().material.SetTextureScale("_MainTex", new Vector2(this.Length * 0.1f * 0.5f, 1f));
				this.Line2.GetComponent<MeshRenderer>().material.SetTextureScale("_NoizeTex", new Vector2(this.Length * 1.82f * 0.1f * 0.5f, 1f));
			}
			if (this.NowScale < 0.1f)
			{
				return;
			}
			float spd = base.transform.localScale.x;
			spd = Mathf.Lerp(5f, 1f, Mathf.Min(1f, base.transform.localScale.x / 1.27f));
			this.Line1.GetComponent<SM_UVScroll>().spd = spd;
			this.Line2.GetComponent<SM_UVScroll>().spd = spd;
			float num = this.time;
			float value = Random.value;
			float num2 = 3f;
			this.Line1.transform.localScale = new Vector3(num2, num2, 1f);
			this.Line2.transform.localScale = new Vector3(num2, num2, 1f);
			this.time += Time.deltaTime;
			if (this.NowScale > 0.05f && this.Rld < 0f && this.Length > 1f)
			{
				this.Rld = this.RldTime;
				GameObject gameObject = this.creator.CreateLaser(BM_LaserEffectName.LaserFlash);
				gameObject.transform.position = base.transform.position;
				gameObject.transform.localScale = Vector3.one * this.NowScale * this.width;
				gameObject.transform.rotation = base.transform.rotation;
				gameObject.GetComponent<SM_LaserFlash>().Parent = base.transform;
				gameObject.GetComponent<SM_LaserFlash>().SetColor(this.MyColor);
				gameObject.GetComponent<SM_LaserFlash>().Play();
			}
			this.Rld -= Time.deltaTime;
			this.Line1.GetComponent<MeshRenderer>().material.SetColor("_TintColor", this.MyColor);
			this.Line2.GetComponent<MeshRenderer>().material.SetColor("_TintColor", this.MyColor);
		}

		// Token: 0x06000104 RID: 260 RVA: 0x0000A1AF File Offset: 0x000083AF
		public void SetColor(Color col)
		{
			this.MyColor = col;
		}

		// Token: 0x06000105 RID: 261 RVA: 0x0000A1B8 File Offset: 0x000083B8
		public void MeshControll(Mesh mesh, bool bRot90)
		{
			if (mesh && mesh.vertexCount == 4)
			{
				Vector3[] vertices = mesh.vertices;
				vertices[0].x = 1f * this.width;
				vertices[0].y = 0f;
				vertices[0].z = 0f;
				vertices[1].x = -1f * this.width;
				vertices[1].y = 0f;
				vertices[1].z = 0f;
				vertices[2].x = 1f * this.width;
				vertices[2].y = 0f;
				vertices[2].z = this.Length / base.transform.localScale.x;
				vertices[3].x = -1f * this.width;
				vertices[3].y = 0f;
				vertices[3].z = this.Length / base.transform.localScale.x;
				if (bRot90)
				{
					for (int i = 0; i < 4; i++)
					{
						vertices[i].y = vertices[i].x;
						vertices[i].x = 0f;
					}
				}
				mesh.vertices = vertices;
				mesh.RecalculateBounds();
			}
		}

		// Token: 0x04000152 RID: 338
		private GameObject Line1;

		// Token: 0x04000153 RID: 339
		private GameObject Line2;

		// Token: 0x04000154 RID: 340
		public float Length;

		// Token: 0x04000155 RID: 341
		public float time = 1f;

		// Token: 0x04000156 RID: 342
		public Color MyColor = Color.red;

		// Token: 0x04000157 RID: 343
		public float NowScale;

		// Token: 0x04000158 RID: 344
		public float TgtScale;

		// Token: 0x04000159 RID: 345
		public float LerpSpd = 0.5f;

		// Token: 0x0400015A RID: 346
		private float RldTime = 0.1f;

		// Token: 0x0400015B RID: 347
		private float Rld;

		// Token: 0x0400015C RID: 348
		private float width;

		// Token: 0x0400015D RID: 349
		public GameObject FlashPrfb;

		// Token: 0x0400015E RID: 350
		private BM_EffectCreator creator;
	}
}
