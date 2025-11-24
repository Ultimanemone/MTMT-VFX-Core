using System;
using UnityEngine;

// Token: 0x02000017 RID: 23
public class SM_FireTrailEmitter : MonoBehaviour
{
	// Token: 0x0600004A RID: 74 RVA: 0x00003F3C File Offset: 0x0000213C
	private void Start()
	{
		this.DefPos = base.transform.localPosition;
	}

	// Token: 0x0600004B RID: 75 RVA: 0x00003F50 File Offset: 0x00002150
	private void Update()
	{
		this.AnmTime += Time.deltaTime * this.AnmSpd;
		base.transform.localPosition = Vector3.Lerp(this.DefPos, Vector3.zero, Mathf.Pow(this.AnmTime, 3f));
		this.Rld -= Time.deltaTime;
		if (this.Rld < 0f)
		{
			this.Rld = this.RldTime;
			Vector3 tgtPos = default(Vector3);
			tgtPos.x = (Random.value - 0.5f) * 16f;
			tgtPos.y = 16f;
			tgtPos.z = (Random.value - 0.5f) * 16f;
			Object.Instantiate<GameObject>(this.obj, new Vector3(base.transform.position.x, base.transform.position.y, base.transform.position.z), base.transform.rotation).GetComponent<SM_ChargeTrail_Hermite>().TgtPos = tgtPos;
		}
	}

	// Token: 0x0400005C RID: 92
	public GameObject obj;

	// Token: 0x0400005D RID: 93
	public float RldTime = 0.1f;

	// Token: 0x0400005E RID: 94
	private float Rld;

	// Token: 0x0400005F RID: 95
	public float AnmTime;

	// Token: 0x04000060 RID: 96
	public float AnmSpd = 1f;

	// Token: 0x04000061 RID: 97
	private Vector3 DefPos;
}
