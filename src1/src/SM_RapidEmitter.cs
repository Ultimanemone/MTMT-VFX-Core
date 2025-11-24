using System;
using UnityEngine;

// Token: 0x0200001E RID: 30
public class SM_RapidEmitter : MonoBehaviour
{
	// Token: 0x06000060 RID: 96 RVA: 0x0000470D File Offset: 0x0000290D
	private void Start()
	{
		this.DefPos = base.transform.localPosition;
	}

	// Token: 0x06000061 RID: 97 RVA: 0x00004720 File Offset: 0x00002920
	private void Update()
	{
		if (this.AnmTime > 1f)
		{
			if (!this.bLocal)
			{
				Object.Destroy(base.gameObject);
				return;
			}
		}
		else
		{
			this.AnmTime += Time.deltaTime * this.AnmSpd;
			this.Rld -= Time.deltaTime + Random.value * this.Rand * Time.deltaTime;
			if (this.Rld < 0f && base.transform.parent != null)
			{
				while (this.Rld < 0f)
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.obj, base.transform.position, base.transform.rotation);
					gameObject.transform.localScale = base.transform.lossyScale;
					if (this.bLocal)
					{
						gameObject.transform.parent = base.transform;
					}
					this.Rld += this.RldTime;
				}
			}
		}
	}

	// Token: 0x0400007E RID: 126
	public GameObject obj;

	// Token: 0x0400007F RID: 127
	public float RldTime = 0.1f;

	// Token: 0x04000080 RID: 128
	private float Rld;

	// Token: 0x04000081 RID: 129
	public float AnmTime;

	// Token: 0x04000082 RID: 130
	public float AnmSpd = 1f;

	// Token: 0x04000083 RID: 131
	private Vector3 DefPos;

	// Token: 0x04000084 RID: 132
	public bool bLocal;

	// Token: 0x04000085 RID: 133
	public float Rand;
}
