using System;
using UnityEngine;

// Token: 0x0200000E RID: 14
public class SM_AutoRotate : MonoBehaviour
{
	// Token: 0x0600002D RID: 45 RVA: 0x00002BE3 File Offset: 0x00000DE3
	private void Start()
	{
	}

	// Token: 0x0600002E RID: 46 RVA: 0x00003571 File Offset: 0x00001771
	private void Update()
	{
		base.transform.Rotate(0f, 0f, this.RotateSpd);
	}

	// Token: 0x04000036 RID: 54
	public float RotateSpd = 1f;
}
