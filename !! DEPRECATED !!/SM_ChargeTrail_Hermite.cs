using System;
using UnityEngine;

// Token: 0x02000012 RID: 18
public class SM_ChargeTrail_Hermite : MonoBehaviour
{
	// Token: 0x0600003A RID: 58 RVA: 0x000038BC File Offset: 0x00001ABC
	private void Start()
	{
		float num = ((double)Random.value > 0.5) ? 0f : 0.5f;
		float num2 = ((double)Random.value > 0.5) ? 0f : 0.5f;
		base.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(0.5f, 0.5f));
		base.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(num, num2));
		this.DefPos = base.transform.position;
		float num3 = 16f;
		this.VecS = new Vector3((float)Random.Range(-1, 1), (float)Random.Range(0, -1), (float)Random.Range(-1, 1)) * num3;
		this.VecE = new Vector3((float)Random.Range(-1, 1), (float)Random.Range(-1, -2), (float)Random.Range(-1, 1)) * num3;
	}

	// Token: 0x0600003B RID: 59 RVA: 0x000039B0 File Offset: 0x00001BB0
	private Vector3 HermiteLerp(Vector3 s, Vector3 e, Vector3 svec, Vector3 evec, float t)
	{
		return (t - 1f) * (t - 1f) * (2f * t + 1f) * s + t * t * (3f - 2f * t) * e + (1f - t * t) * t * svec + (t - 1f) * (t * t) * evec;
	}

	// Token: 0x0600003C RID: 60 RVA: 0x00003A38 File Offset: 0x00001C38
	private void Update()
	{
		this.time += Time.deltaTime * this.AnmSpd;
		base.transform.position = this.HermiteLerp(this.DefPos, this.DefPos + this.TgtPos, this.VecS, this.VecE, Mathf.Min(1f, this.time));
	}

	// Token: 0x04000045 RID: 69
	public Vector3 TgtPos;

	// Token: 0x04000046 RID: 70
	private Vector3 DefPos;

	// Token: 0x04000047 RID: 71
	private Vector3 VecS;

	// Token: 0x04000048 RID: 72
	private Vector3 VecE;

	// Token: 0x04000049 RID: 73
	public float time;

	// Token: 0x0400004A RID: 74
	public float AnmSpd = 1f;
}
