using System;
using UnityEngine;

// Token: 0x0200001A RID: 26
public class SM_LeapMove : MonoBehaviour
{
	// Token: 0x06000054 RID: 84 RVA: 0x000044C8 File Offset: 0x000026C8
	private void Start()
	{
		this.DefPos = base.transform.localPosition;
	}

	// Token: 0x06000055 RID: 85 RVA: 0x000044DC File Offset: 0x000026DC
	private void Update()
	{
		this.time += Time.deltaTime / this.AnmTime;
		base.transform.localPosition = Vector3.Lerp(this.DefPos, this.TgtPos, this.Curve.Evaluate(this.time));
	}

	// Token: 0x0400006F RID: 111
	public Vector3 TgtPos;

	// Token: 0x04000070 RID: 112
	public float AnmTime = 1f;

	// Token: 0x04000071 RID: 113
	private float time;

	// Token: 0x04000072 RID: 114
	public AnimationCurve Curve;

	// Token: 0x04000073 RID: 115
	private Vector3 DefPos;
}
