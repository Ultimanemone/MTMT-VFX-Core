using System;
using UnityEngine;

// Token: 0x0200001B RID: 27
public class SM_MainFire : MonoBehaviour
{
	// Token: 0x06000057 RID: 87 RVA: 0x00002BE3 File Offset: 0x00000DE3
	private void Start()
	{
	}

	// Token: 0x06000058 RID: 88 RVA: 0x00004544 File Offset: 0x00002744
	private void Update()
	{
		if (this.Delay <= 0f)
		{
			for (int i = 0; i < 8; i++)
			{
				Object.Instantiate<GameObject>(this.Obj, base.transform.position, base.transform.rotation).transform.localScale = base.transform.lossyScale;
			}
		}
		this.Delay -= Time.deltaTime;
		Object.Destroy(base.gameObject);
	}

	// Token: 0x04000074 RID: 116
	public GameObject Obj;

	// Token: 0x04000075 RID: 117
	public float Delay;
}
