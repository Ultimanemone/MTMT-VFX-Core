using System;
using UnityEngine;

// Token: 0x0200001D RID: 29
public class SM_PatricleControll : MonoBehaviour
{
	// Token: 0x0600005D RID: 93 RVA: 0x0000465C File Offset: 0x0000285C
	private void Start()
	{
		this.pat = base.GetComponent<ParticleSystem>();
		if (this.StartTime > 0f)
		{
			this.pat.Stop();
		}
	}

	// Token: 0x0600005E RID: 94 RVA: 0x00004684 File Offset: 0x00002884
	private void Update()
	{
		this.time += Time.deltaTime;
		if (this.StartTime > 0f && this.StartTime < this.time)
		{
			this.pat.Play();
		}
		if (this.StopTime > 0f && this.StopTime < this.time)
		{
			this.pat.Stop();
		}
	}

	// Token: 0x0400007A RID: 122
	public float StopTime = -1f;

	// Token: 0x0400007B RID: 123
	public float StartTime = -1f;

	// Token: 0x0400007C RID: 124
	private float time;

	// Token: 0x0400007D RID: 125
	private ParticleSystem pat;
}
