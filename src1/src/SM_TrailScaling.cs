using System;
using UnityEngine;

// Token: 0x02000025 RID: 37
public class SM_TrailScaling : MonoBehaviour
{
	// Token: 0x06000076 RID: 118 RVA: 0x00004A9C File Offset: 0x00002C9C
	private void Start()
	{
		this.tr = base.GetComponent<TrailRenderer>();
		this.tr.startWidth = base.transform.localScale.x;
		this.tr.endWidth = base.transform.localScale.x;
	}

	// Token: 0x06000077 RID: 119 RVA: 0x00004AEB File Offset: 0x00002CEB
	private void Update()
	{
		this.tr.startWidth = base.transform.localScale.x;
		this.tr.endWidth = base.transform.localScale.x;
	}

	// Token: 0x0400008E RID: 142
	private TrailRenderer tr;
}
