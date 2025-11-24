using System;
using UnityEngine;

// Token: 0x02000019 RID: 25
public class SM_HitThunder : MonoBehaviour
{
	// Token: 0x06000050 RID: 80 RVA: 0x000041DC File Offset: 0x000023DC
	private void Start()
	{
		this.Line = base.GetComponent<LineRenderer>();
		this.SetThunder();
		base.GetComponent<Renderer>().material.SetTextureOffset("_MaskTex", new Vector2(0f, Random.value));
	}

	// Token: 0x06000051 RID: 81 RVA: 0x00004214 File Offset: 0x00002414
	private void SetThunder()
	{
		Vector3 vector = base.transform.forward;
		Vector3 vector2 = base.transform.position;
		for (int i = 0; i < 8; i++)
		{
			float num = (float)i / 8f;
			this.PosBuf[i].x = Random.Range(-this.Width, this.Width) * num;
			this.PosBuf[i].y = Random.Range(-this.Width, this.Width) * num;
			this.PosBuf[i].z = (1f - num) * this.Height;
			Vector3 vector3 = base.transform.right * Random.Range(-this.Width, this.Width) * num;
			vector3 += base.transform.up * Random.Range(-this.Width, this.Width) * num;
			this.PosBuf[i] = vector2 + vector3;
			vector2 += vector * (float)i / 8f * this.Height;
			this.MoveVec[i] = new Vector3((float)Random.Range(-1, 1), (float)Random.Range(-1, 1), (float)Random.Range(-1, 1)) * num * this.MoveLength;
			RaycastHit raycastHit;
			if (Physics.Raycast(new Ray(this.PosBuf[i], vector), ref raycastHit, (float)i / 8f * this.Height))
			{
				vector2 = raycastHit.point;
				vector = Vector3.Reflect(vector, raycastHit.normal);
				this.PosBuf[i] = raycastHit.point + raycastHit.normal * 0.1f;
				Object.Instantiate<GameObject>(this.HitObj, this.PosBuf[i], Quaternion.LookRotation(raycastHit.normal));
			}
		}
		this.Line.SetPositions(this.PosBuf);
	}

	// Token: 0x06000052 RID: 82 RVA: 0x00004428 File Offset: 0x00002628
	private void Update()
	{
		for (int i = 0; i < 8; i++)
		{
			this.PosBuf[i] += this.MoveVec[i];
			this.Line.SetPositions(this.PosBuf);
		}
	}

	// Token: 0x04000068 RID: 104
	public GameObject HitObj;

	// Token: 0x04000069 RID: 105
	private LineRenderer Line;

	// Token: 0x0400006A RID: 106
	public float Width = 1f;

	// Token: 0x0400006B RID: 107
	public float Height = 10f;

	// Token: 0x0400006C RID: 108
	public float MoveLength = 0.1f;

	// Token: 0x0400006D RID: 109
	private Vector3[] PosBuf = new Vector3[8];

	// Token: 0x0400006E RID: 110
	private Vector3[] MoveVec = new Vector3[8];
}
