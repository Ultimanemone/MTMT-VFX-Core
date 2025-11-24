using System;
using UnityEngine;

// Token: 0x02000018 RID: 24
public class SM_Fire_lv3 : MonoBehaviour
{
	// Token: 0x0600004D RID: 77 RVA: 0x00002BE3 File Offset: 0x00000DE3
	private void Start()
	{
	}

	// Token: 0x0600004E RID: 78 RVA: 0x00004088 File Offset: 0x00002288
	private void Update()
	{
		if (this.time > this.RldTime)
		{
			if (this.Num <= 0)
			{
				if (this.NextObj != null)
				{
					Object.Instantiate<GameObject>(this.NextObj, base.transform.position, base.transform.rotation).transform.localScale = base.transform.localScale;
				}
				Object.Destroy(base.gameObject);
			}
			else
			{
				this.time = 0f;
				Vector3 vector = new Vector3(Random.value - 0.5f, 0f, Random.value - 0.5f) * this.RandLength;
				vector.Scale(base.transform.localScale);
				Object.Instantiate<GameObject>(this.obj, base.transform.position + vector, base.transform.rotation).transform.localScale = base.transform.localScale;
				this.Num--;
				if (this.Num == 0)
				{
					this.RldTime *= 4f;
				}
			}
		}
		this.time += Time.deltaTime;
	}

	// Token: 0x04000062 RID: 98
	public GameObject obj;

	// Token: 0x04000063 RID: 99
	public GameObject NextObj;

	// Token: 0x04000064 RID: 100
	public float RldTime;

	// Token: 0x04000065 RID: 101
	private float time;

	// Token: 0x04000066 RID: 102
	public int Num = 5;

	// Token: 0x04000067 RID: 103
	public float RandLength = 1f;
}
