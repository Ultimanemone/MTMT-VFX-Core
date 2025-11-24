using System;
using UnityEngine;

// Token: 0x0200000F RID: 15
public class SM_Beam : MonoBehaviour
{
	// Token: 0x06000030 RID: 48 RVA: 0x000035A4 File Offset: 0x000017A4
	private void Start()
	{
		this.BaseScale = base.transform.localScale;
		this.main();
		base.GetComponent<MeshRenderer>().enabled = false;
		base.transform.Rotate(0f, 0f, Random.Range(this.RotMin, this.RotMax));
		Renderer component = base.GetComponent<Renderer>();
		float num = ((double)Random.value > 0.5) ? 0f : 0.5f;
		float num2 = ((double)Random.value > 0.5) ? 0f : 0.5f;
		component.material.SetTextureScale("_MainTex", new Vector2(0.5f, 0.5f));
		component.material.SetTextureOffset("_MainTex", new Vector2(num, num2));
	}

	// Token: 0x06000031 RID: 49 RVA: 0x00003674 File Offset: 0x00001874
	private void Update()
	{
		if (this.Delay <= 0f)
		{
			base.GetComponent<MeshRenderer>().enabled = true;
			this.time += Time.deltaTime * this.AnmSpd;
			this.main();
			return;
		}
		this.Delay -= Time.deltaTime;
	}

	// Token: 0x06000032 RID: 50 RVA: 0x000036CC File Offset: 0x000018CC
	private void main()
	{
		base.transform.localScale = new Vector3(this.Width.Evaluate(this.time) * ((this.BaseScale.x + this.BaseScale.z) / 2f), 1f, this.Length.Evaluate(this.time) * this.BaseScale.y);
	}

	// Token: 0x04000037 RID: 55
	public AnimationCurve Width;

	// Token: 0x04000038 RID: 56
	public AnimationCurve Length;

	// Token: 0x04000039 RID: 57
	public float AnmSpd = 1f;

	// Token: 0x0400003A RID: 58
	public float Delay;

	// Token: 0x0400003B RID: 59
	public float RotMin;

	// Token: 0x0400003C RID: 60
	public float RotMax = 1f;

	// Token: 0x0400003D RID: 61
	private Vector3 BaseScale;

	// Token: 0x0400003E RID: 62
	private float time;
}
