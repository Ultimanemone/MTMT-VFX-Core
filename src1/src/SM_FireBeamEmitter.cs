using System;
using UnityEngine;

// Token: 0x02000016 RID: 22
public class SM_FireBeamEmitter : MonoBehaviour
{
	// Token: 0x06000047 RID: 71 RVA: 0x00003DF1 File Offset: 0x00001FF1
	private void Start()
	{
		this.DefPos = base.transform.localPosition;
	}

	// Token: 0x06000048 RID: 72 RVA: 0x00003E04 File Offset: 0x00002004
	private void Update()
	{
		this.AnmTime += Time.deltaTime * this.AnmSpd;
		base.transform.localPosition = Vector3.Lerp(this.DefPos, Vector3.zero, Mathf.Pow(this.AnmTime, 3f));
		this.Rld -= Time.deltaTime;
		if (this.Rld < 0f && base.transform.parent != null)
		{
			this.Rld = this.RldTime;
			GameObject gameObject = Object.Instantiate<GameObject>(this.obj, base.transform.position, base.transform.rotation);
			gameObject.transform.localScale = base.transform.parent.parent.lossyScale;
			if (gameObject.GetComponent<SM_ChargeTrail_Hermite>() != null)
			{
				gameObject.GetComponent<SM_ChargeTrail_Hermite>().TgtPos = base.transform.parent.transform.position;
			}
		}
		if (this.AnmTime > 1f)
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x04000056 RID: 86
	public GameObject obj;

	// Token: 0x04000057 RID: 87
	public float RldTime = 0.1f;

	// Token: 0x04000058 RID: 88
	private float Rld;

	// Token: 0x04000059 RID: 89
	public float AnmTime;

	// Token: 0x0400005A RID: 90
	public float AnmSpd = 1f;

	// Token: 0x0400005B RID: 91
	private Vector3 DefPos;
}
