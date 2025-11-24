using System;
using UnityEngine;

// Token: 0x02000022 RID: 34
public class SM_RndTex : MonoBehaviour
{
	// Token: 0x0600006C RID: 108 RVA: 0x00004930 File Offset: 0x00002B30
	private void Start()
	{
		int num = Random.Range(0, this.Separate / 2);
		int num2 = Random.Range(0, this.Separate / 2);
		float num3 = 1f / ((float)this.Separate / 2f);
		base.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2((float)num * num3, (float)num2 * num3));
	}

	// Token: 0x0600006D RID: 109 RVA: 0x00002BE3 File Offset: 0x00000DE3
	private void Update()
	{
	}

	// Token: 0x04000088 RID: 136
	public int Separate = 4;
}
