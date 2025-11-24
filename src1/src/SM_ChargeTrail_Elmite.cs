using System;
using UnityEngine;

// Token: 0x02000013 RID: 19
public class SM_ChargeTrail_Elmite : MonoBehaviour
{
	// Token: 0x0600003E RID: 62 RVA: 0x00003AB8 File Offset: 0x00001CB8
	private void Start()
	{
		float num = ((double)Random.value > 0.5) ? 0f : 0.5f;
		float num2 = ((double)Random.value > 0.5) ? 0f : 0.5f;
		base.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(0.5f, 0.5f));
		base.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(num, num2));
		this.DefPos = base.transform.position;
	}

	// Token: 0x0600003F RID: 63 RVA: 0x00003B52 File Offset: 0x00001D52
	private void Update()
	{
		this.time += Time.deltaTime * this.AnmSpd;
		base.transform.position = Vector3.Lerp(this.DefPos, this.TgtPos, this.time);
	}

	// Token: 0x0400004B RID: 75
	public Vector3 TgtPos;

	// Token: 0x0400004C RID: 76
	private Vector3 DefPos;

	// Token: 0x0400004D RID: 77
	public float time;

	// Token: 0x0400004E RID: 78
	public float AnmSpd = 1f;
}
