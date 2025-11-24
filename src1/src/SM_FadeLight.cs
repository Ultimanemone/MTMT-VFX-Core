using System;
using UnityEngine;

// Token: 0x02000015 RID: 21
public class SM_FadeLight : MonoBehaviour
{
	// Token: 0x06000044 RID: 68 RVA: 0x00003D21 File Offset: 0x00001F21
	private void Start()
	{
		this.size = this.StartSize;
		this.myLight = base.GetComponent<Light>();
	}

	// Token: 0x06000045 RID: 69 RVA: 0x00003D3C File Offset: 0x00001F3C
	private void Update()
	{
		if (this.myLight != null)
		{
			if (this.StartTime > 0f)
			{
				this.myLight.range = 0f;
				this.StartTime -= Time.deltaTime;
			}
			else
			{
				this.myLight.range = this.size;
			}
		}
		this.size -= this.DownSpd * Time.deltaTime;
		this.size = Mathf.Max(0f, this.size);
	}

	// Token: 0x04000051 RID: 81
	public float StartTime = 0.25f;

	// Token: 0x04000052 RID: 82
	public float StartSize = 1f;

	// Token: 0x04000053 RID: 83
	public float DownSpd = 0.1f;

	// Token: 0x04000054 RID: 84
	private float size;

	// Token: 0x04000055 RID: 85
	private Light myLight;
}
