using System;
using UnityEngine;

// Token: 0x02000006 RID: 6
public class ColorChange : MonoBehaviour
{
	// Token: 0x06000014 RID: 20 RVA: 0x00002BC6 File Offset: 0x00000DC6
	private void Start()
	{
		base.GetComponent<Renderer>().material.SetColor("_TintColor", this.Col);
	}

	// Token: 0x06000015 RID: 21 RVA: 0x00002BE3 File Offset: 0x00000DE3
	private void Update()
	{
	}

	// Token: 0x04000022 RID: 34
	public Color Col;
}
