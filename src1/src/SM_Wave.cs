using System;
using UnityEngine;

// Token: 0x02000027 RID: 39
public class SM_Wave : MonoBehaviour
{
	// Token: 0x0600007C RID: 124 RVA: 0x00004B98 File Offset: 0x00002D98
	private void Start()
	{
		this.delay_work = this.delay;
		this.SetWaveVertics();
		Renderer component = base.GetComponent<Renderer>();
		this.mat = component.material;
		this.DefColor = this.mat.GetColor("_TintColor");
		base.transform.Rotate(0f, Random.value * 360f, 0f);
	}

	// Token: 0x0600007D RID: 125 RVA: 0x00004C00 File Offset: 0x00002E00
	private void Update()
	{
		Renderer component = base.GetComponent<Renderer>();
		this.delay_work -= Time.deltaTime;
		if (this.delay_work < 0f)
		{
			this.LocalTime += Time.deltaTime / this.AnmTime;
			component.enabled = true;
		}
		if (this.AutoKill && this.LocalTime > 1f)
		{
			Object.Destroy(base.gameObject);
		}
		this.SetWaveVertics();
		base.transform.Rotate(0f, this.RotateSpd.Evaluate(this.LocalTime) * Time.deltaTime, 0f);
		Color defColor = this.DefColor;
		defColor.a = 0f;
		this.mat.SetColor("_TintColor", Color.Lerp(defColor, this.DefColor, this.Alpha.Evaluate(this.LocalTime)));
	}

	// Token: 0x0600007E RID: 126 RVA: 0x00004CE8 File Offset: 0x00002EE8
	private void SetWaveVertics()
	{
		Mesh mesh = base.GetComponent<MeshFilter>().mesh;
		Vector3[] vertices = mesh.vertices;
		for (int i = 0; i < vertices.Length; i += 2)
		{
			float num = (float)i / (float)vertices.Length * 4f * 3.1415927f;
			vertices[i].x = Mathf.Cos(num) * Mathf.Lerp(0f, this.OutSize.Evaluate(this.LocalTime), this.InSize.Evaluate(this.LocalTime));
			vertices[i].y = 0f;
			vertices[i].z = Mathf.Sin(num) * Mathf.Lerp(0f, this.OutSize.Evaluate(this.LocalTime), this.InSize.Evaluate(this.LocalTime));
			vertices[i + 1].x = Mathf.Cos(num) * this.OutSize.Evaluate(this.LocalTime);
			vertices[i + 1].y = this.Height.Evaluate(this.LocalTime);
			vertices[i + 1].z = Mathf.Sin(num) * this.OutSize.Evaluate(this.LocalTime);
		}
		mesh.vertices = vertices;
	}

	// Token: 0x04000092 RID: 146
	public AnimationCurve InSize;

	// Token: 0x04000093 RID: 147
	public AnimationCurve OutSize;

	// Token: 0x04000094 RID: 148
	public AnimationCurve Height;

	// Token: 0x04000095 RID: 149
	public AnimationCurve Alpha;

	// Token: 0x04000096 RID: 150
	public AnimationCurve RotateSpd;

	// Token: 0x04000097 RID: 151
	public float LocalTime;

	// Token: 0x04000098 RID: 152
	public float AnmTime = 1f;

	// Token: 0x04000099 RID: 153
	public bool AutoKill;

	// Token: 0x0400009A RID: 154
	private Color DefColor;

	// Token: 0x0400009B RID: 155
	private Material mat;

	// Token: 0x0400009C RID: 156
	public float delay;

	// Token: 0x0400009D RID: 157
	private float delay_work;
}
