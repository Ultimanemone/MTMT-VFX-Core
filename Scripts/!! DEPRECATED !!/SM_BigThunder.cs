using System;
using UnityEngine;

// Token: 0x02000010 RID: 16
public class SM_BigThunder : MonoBehaviour
{
	// Token: 0x06000034 RID: 52 RVA: 0x00003758 File Offset: 0x00001958
	private void Start()
	{
		this.Line = base.GetComponent<LineRenderer>();
		float num = Random.value * 2f * 3.1415927f;
		this.Line.SetPosition(0, new Vector3(Mathf.Cos(num) * this.radius, -(num * this.radius / 4f), Mathf.Sin(num) * this.radius));
	}

	// Token: 0x06000035 RID: 53 RVA: 0x00002BE3 File Offset: 0x00000DE3
	private void Update()
	{
	}

	// Token: 0x0400003F RID: 63
	private LineRenderer Line;

	// Token: 0x04000040 RID: 64
	public float radius = 10f;
}
