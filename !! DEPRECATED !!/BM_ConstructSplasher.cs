using System;
using System.Collections.Generic;
using BMEffects_Remaster.Util;
using UnityEngine;

// Token: 0x02000003 RID: 3
public class BM_ConstructSplasher : MonoBehaviour
{
	// Token: 0x06000002 RID: 2 RVA: 0x0000206A File Offset: 0x0000026A
	public void Start()
	{
		this.nCounter = 0;
		this.nAdd = 100;
		this.bInit = false;
	}

	// Token: 0x06000003 RID: 3 RVA: 0x00002084 File Offset: 0x00000284
	public void Init()
	{
		this.PosBuf = new float[this.c.ArrayBasics.AliveAndDead.Count];
		Debug.Log("SetConstSP:" + this.c.ArrayBasics.AliveAndDead.Count);
		for (int i = 0; i < this.PosBuf.Length; i++)
		{
			this.PosBuf[i] = this.c.ArrayBasics.AliveAndDead[i].GameWorldPosition.y;
		}
	}

	// Token: 0x06000004 RID: 4 RVA: 0x00002118 File Offset: 0x00000318
	public void Update()
	{
		if (!BM_ConstructSplasher.bCreate)
		{
			return;
		}
		if (!this.bInit)
		{
			this.Init();
			this.bInit = true;
		}
		Vector3 velocity = this.c.Velocity;
		Vector3 angularVelocity = this.c.GetAngularVelocity();
		float magnitude = velocity.magnitude;
		float magnitude2 = angularVelocity.magnitude;
		try
		{
			for (int i = this.nCounter; i < this.nCounter + this.nAdd; i++)
			{
				int num = i % this.c.ArrayBasics.AliveAndDead.Count;
				Block block = this.c.ArrayBasics.AliveAndDead[num];
				if (!block.isAlive)
				{
					this.PosBuf[num] = block.GameWorldPosition.y;
				}
				else
				{
					float num2 = block.GameWorldPosition.y - this.PosBuf[num];
					if (this.PosBuf[num] < 0f && this.PosBuf[num] + num2 > 0f)
					{
						this.CreateSP(block.GameWorldPosition, num2);
					}
					if (this.PosBuf[num] > 0f && this.PosBuf[num] + num2 < 0f)
					{
						this.CreateSP(block.GameWorldPosition, num2);
					}
					this.PosBuf[num] = block.GameWorldPosition.y;
				}
			}
			this.AddCounter(this.nAdd);
			if (this.CreateList.Count != 0)
			{
				List<stSplash> list = new List<stSplash>();
				for (int j = 0; j < this.CreateList.Count; j++)
				{
					bool flag = false;
					for (int k = 0; k < list.Count; k++)
					{
						if (Vector2.Distance(this.CreateList[j], list[k].pos) < 25f)
						{
							list[k].scale += 0.025f;
							k = list.Count;
							flag = true;
						}
					}
					if (!flag)
					{
						list.Add(new stSplash(this.CreateList[j]));
					}
				}
				for (int l = 0; l < list.Count; l++)
				{
					float num3 = list[l].scale * 0.1f;
					GameObject gameObject = BM_EffectCreator.Creator.CreateExplosion(BM_ExplosionsName.HugeSplash);
					if (gameObject != null)
					{
						gameObject.transform.position = new Vector3(list[l].pos.x, 0f, list[l].pos.y);
						gameObject.transform.localScale = new Vector3(num3, num3 * 2f, num3);
						gameObject.transform.rotation = Quaternion.identity;
					}
				}
				this.CreateList.Clear();
			}
		}
		catch (Exception)
		{
			this.Init();
		}
	}

	// Token: 0x06000005 RID: 5 RVA: 0x00002418 File Offset: 0x00000618
	public void AddCounter(int add)
	{
		this.nCounter += add;
		if (this.nCounter >= this.c.ArrayBasics.AliveAndDead.Count)
		{
			this.nCounter -= this.c.ArrayBasics.AliveAndDead.Count;
		}
	}

	// Token: 0x06000006 RID: 6 RVA: 0x00002474 File Offset: 0x00000674
	public void CreateSP(Vector3 pos, float yspd)
	{
		yspd = Math.Abs(yspd);
		if (yspd < 0.1f || Mathf.Abs(pos.y) > 10f)
		{
			return;
		}
		this.CreateList.Add(new Vector2(pos.x, pos.z));
	}

	// Token: 0x04000003 RID: 3
	public static bool bCreate;

	// Token: 0x04000004 RID: 4
	private int nCounter;

	// Token: 0x04000005 RID: 5
	private int nAdd;

	// Token: 0x04000006 RID: 6
	public AllConstruct c;

	// Token: 0x04000007 RID: 7
	private bool bInit;

	// Token: 0x04000008 RID: 8
	private float[] PosBuf;

	// Token: 0x04000009 RID: 9
	private int SilentNum;

	// Token: 0x0400000A RID: 10
	private List<Vector2> CreateList = new List<Vector2>();
}
