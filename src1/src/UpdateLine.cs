using System;
using UnityEngine;

// Token: 0x02000028 RID: 40
public class UpdateLine : MonoBehaviour
{
	// Token: 0x06000080 RID: 128 RVA: 0x00004E44 File Offset: 0x00003044
	private void Start()
	{
		this.Line = base.GetComponent<LineRenderer>();
		this.Line.SetWidth(0f, 0f);
	}

	// Token: 0x06000081 RID: 129 RVA: 0x00004E68 File Offset: 0x00003068
	private void Update()
	{
		this.Line.SetWidth(this.BaseS * base.transform.localScale.x, this.BaseE * base.transform.localScale.x);
		this.Line.enabled = true;
	}

	// Token: 0x0400009E RID: 158
	private LineRenderer Line;

	// Token: 0x0400009F RID: 159
	public float BaseS;

	// Token: 0x040000A0 RID: 160
	public float BaseE;
}
