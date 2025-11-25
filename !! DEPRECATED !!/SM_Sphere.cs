using System;
using UnityEngine;

// Token: 0x02000024 RID: 36
public class SM_Sphere : MonoBehaviour
{
	// Token: 0x06000072 RID: 114 RVA: 0x000049E0 File Offset: 0x00002BE0
	private void Start()
	{
		this.BaseScale = base.transform.localScale;
		this.main();
		base.GetComponent<MeshRenderer>().enabled = false;
	}

	// Token: 0x06000073 RID: 115 RVA: 0x00004A08 File Offset: 0x00002C08
	private void Update()
	{
		if (this.Delay < 0f)
		{
			base.GetComponent<MeshRenderer>().enabled = true;
			this.time += Time.deltaTime * this.AnmSpd;
			this.main();
			return;
		}
		this.Delay -= Time.deltaTime;
	}

	// Token: 0x06000074 RID: 116 RVA: 0x00004A60 File Offset: 0x00002C60
	private void main()
	{
		base.transform.localScale = this.BaseScale * this.Size.Evaluate(this.time);
	}

	// Token: 0x04000089 RID: 137
	public AnimationCurve Size;

	// Token: 0x0400008A RID: 138
	public float AnmSpd = 1f;

	// Token: 0x0400008B RID: 139
	public float Delay;

	// Token: 0x0400008C RID: 140
	private Vector3 BaseScale;

	// Token: 0x0400008D RID: 141
	private float time;
}
