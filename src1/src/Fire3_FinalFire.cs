using System;
using UnityEngine;

// Token: 0x02000008 RID: 8
public class Fire3_FinalFire : MonoBehaviour
{
	// Token: 0x0600001B RID: 27 RVA: 0x00002E4B File Offset: 0x0000104B
	private void Start()
	{
		this.OldScale = base.transform.localScale;
	}

	// Token: 0x0600001C RID: 28 RVA: 0x00002E60 File Offset: 0x00001060
	private void Update()
	{
		this.anmtime += Time.deltaTime * this.AnmSpd;
		Vector3 oldScale = this.OldScale;
		oldScale.x = this.AnmCurveWidth.Evaluate(this.anmtime);
		oldScale.y = this.AnmCurveHeight.Evaluate(this.anmtime);
		oldScale.z = this.AnmCurveWidth.Evaluate(this.anmtime);
		base.transform.localScale = oldScale;
	}

	// Token: 0x04000025 RID: 37
	public AnimationCurve AnmCurveWidth;

	// Token: 0x04000026 RID: 38
	public AnimationCurve AnmCurveHeight;

	// Token: 0x04000027 RID: 39
	public float AnmSpd = 1f;

	// Token: 0x04000028 RID: 40
	private float anmtime;

	// Token: 0x04000029 RID: 41
	private Vector3 OldScale;
}
