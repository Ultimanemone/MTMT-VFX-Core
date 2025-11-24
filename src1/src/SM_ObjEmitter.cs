using System;
using UnityEngine;

// Token: 0x0200001C RID: 28
public class SM_ObjEmitter : MonoBehaviour
{
	// Token: 0x0600005A RID: 90 RVA: 0x00002BE3 File Offset: 0x00000DE3
	private void Start()
	{
	}

	// Token: 0x0600005B RID: 91 RVA: 0x000045C0 File Offset: 0x000027C0
	private void Update()
	{
		this.EmitTime -= Time.deltaTime;
		if (!this.bEmit && this.EmitTime < 0f)
		{
			this.bEmit = true;
			for (int i = 0; i < this.EmitNum; i++)
			{
				Object.Instantiate<GameObject>(this.EmitObj, base.transform.position, base.transform.rotation).transform.localScale = base.transform.lossyScale;
			}
		}
	}

	// Token: 0x04000076 RID: 118
	public GameObject EmitObj;

	// Token: 0x04000077 RID: 119
	public float EmitTime = 1f;

	// Token: 0x04000078 RID: 120
	public int EmitNum = 1;

	// Token: 0x04000079 RID: 121
	private bool bEmit;
}
