using System;
using UnityEngine;

// Token: 0x0200001F RID: 31
public class SM_RndPos : MonoBehaviour
{
	// Token: 0x06000063 RID: 99 RVA: 0x00004844 File Offset: 0x00002A44
	private void Start()
	{
		float num = Random.value * 2f * 3.1415927f;
		base.transform.localPosition += new Vector3(Mathf.Cos(num) * this.radius, 0f, Mathf.Sin(num) * this.radius);
	}

	// Token: 0x06000064 RID: 100 RVA: 0x00002BE3 File Offset: 0x00000DE3
	private void Update()
	{
	}

	// Token: 0x04000086 RID: 134
	public float radius = 1f;
}
