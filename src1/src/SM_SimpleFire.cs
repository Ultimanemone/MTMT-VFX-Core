using System;
using UnityEngine;

// Token: 0x02000023 RID: 35
public class SM_SimpleFire : MonoBehaviour
{
	// Token: 0x0600006F RID: 111 RVA: 0x000049A0 File Offset: 0x00002BA0
	private void Start()
	{
		base.transform.Translate(0f, 1f, 0f);
		base.transform.Rotate(Random.Range(-45f, 45f), 0f, 0f);
	}

	// Token: 0x06000070 RID: 112 RVA: 0x00002BE3 File Offset: 0x00000DE3
	private void Update()
	{
	}
}
