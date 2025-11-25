using System;
using UnityEngine;

// Token: 0x02000011 RID: 17
public class SM_ChargeTrail : MonoBehaviour
{
	// Token: 0x06000037 RID: 55 RVA: 0x000037D0 File Offset: 0x000019D0
	private void Start()
	{
		float num = ((double)Random.value > 0.5) ? 0f : 0.5f;
		float num2 = ((double)Random.value > 0.5) ? 0f : 0.5f;
		base.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(0.5f, 0.5f));
		base.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(num, num2));
		this.DefPos = base.transform.position;
	}

	// Token: 0x06000038 RID: 56 RVA: 0x0000386A File Offset: 0x00001A6A
	private void Update()
	{
		this.time += Time.deltaTime * this.AnmSpd;
		base.transform.position = Vector3.Lerp(this.DefPos, this.TgtPos, this.time);
	}

	// Token: 0x04000041 RID: 65
	public Vector3 TgtPos;

	// Token: 0x04000042 RID: 66
	private Vector3 DefPos;

	// Token: 0x04000043 RID: 67
	public float time;

	// Token: 0x04000044 RID: 68
	public float AnmSpd = 1f;
}
