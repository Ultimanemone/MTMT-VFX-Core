using System;
using UnityEngine;

// Token: 0x0200000C RID: 12
public class RndThunder : MonoBehaviour
{
	// Token: 0x06000026 RID: 38 RVA: 0x00002BE3 File Offset: 0x00000DE3
	private void Start()
	{
	}

	// Token: 0x06000027 RID: 39 RVA: 0x00003378 File Offset: 0x00001578
	private void Update()
	{
		if (this.time > this.RldTime)
		{
			if (this.Num <= 0)
			{
				Object.Destroy(base.gameObject);
			}
			else
			{
				this.time = 0f;
				GameObject gameObject = Object.Instantiate<GameObject>(this.obj, base.transform.position + Vector3.up * 0.25f, Quaternion.AngleAxis(Random.Range(90f, 90f), Vector3.left) * Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.forward));
				float num = Random.value * 0.25f;
				gameObject.transform.localScale = new Vector3(num, 0.5f, num);
				this.Num--;
			}
		}
		this.time += Time.deltaTime;
	}

	// Token: 0x0400002B RID: 43
	public GameObject obj;

	// Token: 0x0400002C RID: 44
	public float RldTime;

	// Token: 0x0400002D RID: 45
	private float time;

	// Token: 0x0400002E RID: 46
	public int Num = 5;

	// Token: 0x0400002F RID: 47
	public float RandLength = 1f;
}
