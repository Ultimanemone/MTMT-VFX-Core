using System;
using UnityEngine;

// Token: 0x02000026 RID: 38
public class SM_UVScroll : MonoBehaviour
{
	// Token: 0x06000079 RID: 121 RVA: 0x00004B24 File Offset: 0x00002D24
	private void Start()
	{
		Renderer component = base.GetComponent<Renderer>();
		this.mat = component.material;
	}

	// Token: 0x0600007A RID: 122 RVA: 0x00004B44 File Offset: 0x00002D44
	private void Update()
	{
		if (this.mat != null)
		{
			this.mat.SetFloat("_time", this.time);
			this.time += Time.deltaTime * this.spd;
		}
	}

	// Token: 0x0400008F RID: 143
	public float time;

	// Token: 0x04000090 RID: 144
	public float spd = 1f;

	// Token: 0x04000091 RID: 145
	private Material mat;
}
