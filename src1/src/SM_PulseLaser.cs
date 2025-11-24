using System;
using UnityEngine;

// Token: 0x02000029 RID: 41
public class SM_PulseLaser : MonoBehaviour
{
	// Token: 0x06000083 RID: 131 RVA: 0x00004EBA File Offset: 0x000030BA
	public void Start()
	{
		this.maxtime = 1f;
		this.Init();
	}

	// Token: 0x06000084 RID: 132 RVA: 0x00004ECD File Offset: 0x000030CD
	public void Init()
	{
		this.time = this.maxtime;
		this.bStart = false;
	}

	// Token: 0x06000085 RID: 133 RVA: 0x00004EE4 File Offset: 0x000030E4
	private void Update()
	{
		if (base.GetComponent<SM_AutoKill>().bCreate)
		{
			return;
		}
		if (!this.bStart)
		{
			this.Line1 = base.transform.FindChild("Line1").gameObject;
			this.Line2 = base.transform.FindChild("Line2").gameObject;
			this.MeshControll(this.Line1.GetComponent<MeshFilter>().mesh, false);
			this.MeshControll(this.Line2.GetComponent<MeshFilter>().mesh, true);
			this.time = this.maxtime;
			this.bStart = true;
			this.Line1.GetComponent<SM_UVScroll>().time = 0f;
			this.Line2.GetComponent<SM_UVScroll>().time = 0f;
			this.SetColor(this.col);
		}
		float spd = base.transform.localScale.x;
		spd = Mathf.Lerp(5f, 1f, Mathf.Min(1f, base.transform.localScale.x * 4f / 1.27f));
		this.Line1.GetComponent<SM_UVScroll>().spd = spd;
		this.Line2.GetComponent<SM_UVScroll>().spd = spd;
		this.Line1.GetComponent<MeshRenderer>().material.SetTextureScale("_MainTex", new Vector2(this.Length * 0.1f * 0.5f, 1f));
		this.Line1.GetComponent<MeshRenderer>().material.SetTextureScale("_NoizeTex", new Vector2(this.Length * 1.82f * 0.1f * 0.5f, 1f));
		this.Line2.GetComponent<MeshRenderer>().material.SetTextureScale("_MainTex", new Vector2(this.Length * 0.1f * 0.5f, 1f));
		this.Line2.GetComponent<MeshRenderer>().material.SetTextureScale("_NoizeTex", new Vector2(this.Length * 1.82f * 0.1f * 0.5f, 1f));
		float num = 1f - this.time / this.maxtime;
		float num2 = this.Anm.Evaluate(num) * (1f - Mathf.Pow(1f - (1f - num), 4f)) * 10f * 0.8f;
		this.Line1.transform.localScale = new Vector3(num2, num2, 1f);
		this.Line2.transform.localScale = new Vector3(num2, num2, 1f);
		this.time -= Time.deltaTime;
		if (this.time < 0f)
		{
			this.time = 0f;
		}
	}

	// Token: 0x06000086 RID: 134 RVA: 0x000051B1 File Offset: 0x000033B1
	public void SetColor(Color col)
	{
		this.Line1.GetComponent<MeshRenderer>().material.SetColor("_TintColor", col);
		this.Line2.GetComponent<MeshRenderer>().material.SetColor("_TintColor", col);
	}

	// Token: 0x06000087 RID: 135 RVA: 0x000051EC File Offset: 0x000033EC
	public void MeshControll(Mesh mesh, bool bRot90)
	{
		Vector3[] vertices = mesh.vertices;
		vertices[0].x = 1f;
		vertices[0].y = 0f;
		vertices[0].z = 0f;
		vertices[1].x = -1f;
		vertices[1].y = 0f;
		vertices[1].z = 0f;
		vertices[2].x = 1f;
		vertices[2].y = 0f;
		vertices[2].z = this.Length / base.transform.localScale.x;
		vertices[3].x = -1f;
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

	// Token: 0x040000A1 RID: 161
	private GameObject Line1;

	// Token: 0x040000A2 RID: 162
	private GameObject Line2;

	// Token: 0x040000A3 RID: 163
	public float Length;

	// Token: 0x040000A4 RID: 164
	public AnimationCurve Anm;

	// Token: 0x040000A5 RID: 165
	public float maxtime;

	// Token: 0x040000A6 RID: 166
	public float time = 1f;

	// Token: 0x040000A7 RID: 167
	private bool bStart;

	// Token: 0x040000A8 RID: 168
	public Color col;
}
