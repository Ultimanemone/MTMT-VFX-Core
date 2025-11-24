using System;
using UnityEngine;

// Token: 0x02000021 RID: 33
public class SM_RndRot_Dir : MonoBehaviour
{
	// Token: 0x06000069 RID: 105 RVA: 0x000048C4 File Offset: 0x00002AC4
	private void Start()
	{
		base.transform.Rotate(Random.Range(-this.RndLen.x, this.RndLen.x), Random.Range(-this.RndLen.y, this.RndLen.y), Random.Range(-this.RndLen.z, this.RndLen.z));
	}

	// Token: 0x0600006A RID: 106 RVA: 0x00002BE3 File Offset: 0x00000DE3
	private void Update()
	{
	}

	// Token: 0x04000087 RID: 135
	public Vector3 RndLen;
}
