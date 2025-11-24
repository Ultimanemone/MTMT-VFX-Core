using System;
using UnityEngine;

// Token: 0x02000020 RID: 32
public class SM_RndRot : MonoBehaviour
{
	// Token: 0x06000066 RID: 102 RVA: 0x000048B0 File Offset: 0x00002AB0
	private void Start()
	{
		base.transform.rotation = Random.rotation;
	}

	// Token: 0x06000067 RID: 103 RVA: 0x00002BE3 File Offset: 0x00000DE3
	private void Update()
	{
	}
}
