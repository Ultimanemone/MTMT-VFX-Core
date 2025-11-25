using BrilliantSkies.Core.Types;
using System;
using UnityEngine;


public class BM_Trail : MonoBehaviour
{
	public void Start()
	{
		this.TrailL = null;
		this.TrailR = null;
		this.ResetCounter = 0f;
		this.nCounter = 0;
		this.nAdd = 100;
		this.MinX = 65535;
		this.MaxX = -65535;
		this.MinXZ = 65535;
		this.MaxXZ = 65535;
		this.g = new Gradient();
		this.gck = new GradientColorKey[2];
		this.gck[0].color = Color.white;
		this.gck[0].time = 0f;
		this.gck[1].color = Color.white;
		this.gck[1].time = 1f;
		this.gak = new GradientAlphaKey[3];
		this.gak[0].alpha = 0f;
		this.gak[0].time = 0f;
		this.gak[1].alpha = 0.1f;
		this.gak[1].time = 0.5f;
		this.gak[2].alpha = 0f;
		this.gak[2].time = 1f;
		this.g.SetKeys(this.gck, this.gak);
		this.TL = null;
		this.TR = null;
	}

	public void SetTrailParam(TrailRenderer tr)
	{
		tr.material = BM_Trail.mat;
		tr.startColor = new Color(1f, 1f, 1f, 1f);
		tr.endColor = new Color(1f, 1f, 1f, 1f);
		tr.colorGradient = this.g;
		tr.autodestruct = true;
		tr.shadowCastingMode = 0;
		tr.receiveShadows = false;
		tr.time = this.TrailTime;
	}

	public void Init()
	{
		this.TrailL = new GameObject("TrailL");
		this.TrailL.transform.parent = this.c.GameObject.SubconstructableParent;
		TrailRenderer trailRenderer = this.TrailL.AddComponent<TrailRenderer>();
		this.TrailR = new GameObject("TrailR");
		this.TrailR.transform.parent = this.c.GameObject.SubconstructableParent;
		TrailRenderer trailRenderer2 = this.TrailR.AddComponent<TrailRenderer>();
		this.SetTrailParam(trailRenderer);
		this.SetTrailParam(trailRenderer2);
		this.TL = trailRenderer;
		this.TR = trailRenderer2;
	}

	public void Update()
	{
		Vector3 velocity = this.c.Velocity;
		Vector3 angularVelocity = this.c.GetAngularVelocity();
		if (velocity.magnitude + angularVelocity.magnitude * 20f < 150f || this.c.FastPosition.y < 0f || this.c.FastPosition.y > 1800f)
		{
			if (this.TrailL == null)
			{
				return;
			}
			this.ResetCounter += Time.deltaTime;
			if (this.ResetCounter > this.ResetTime)
			{
				this.ResetCounter = 0f;
				this.TrailL.transform.parent = null;
				this.TrailR.transform.parent = null;
				this.TrailL = null;
				this.TrailR = null;
				return;
			}
		}
		else
		{
			this.ResetCounter = 0f;
			if (this.TrailL == null)
			{
				this.Init();
			}
		}
		Vector3i zero = Vector3i.zero;
		Vector3i zero2 = Vector3i.zero;
		for (int i = this.nCounter; i < this.nCounter + this.nAdd; i++)
		{
			int num = i % this.c.AllBasics.AliveAndDead.Count;
			Block block = this.c.AllBasics.AliveAndDead[num];
			if (block.IsAlive)
			{
				Vector3i localPosition = block.LocalPosition;
				if (localPosition.x <= this.MinX && (localPosition.x < this.MinX || localPosition.z < this.MinXZ))
				{
					this.MinXZ = localPosition.z;
					this.MinX = localPosition.x;
					this.nNowTrailL = num;
				}
				if (localPosition.x >= this.MaxX && (localPosition.x > this.MaxX || localPosition.z < this.MinXZ))
				{
					this.MaxXZ = localPosition.z;
					this.MaxX = localPosition.x;
					this.nNowTrailR = num;
				}
			}
		}
		if (this.nNowTrailL != -1)
		{
			this.TrailL.transform.localPosition = this.c.AllBasics.AliveAndDead.Blocks[this.nNowTrailL].LocalPosition;
		}
		if (this.nNowTrailR != -1)
		{
			this.TrailR.transform.localPosition = this.c.AllBasics.AliveAndDead.Blocks[this.nNowTrailR].LocalPosition;
		}
		this.TR.enabled = BM_Trail.bCreate;
		this.TL.enabled = BM_Trail.bCreate;
		this.nCounter += this.nAdd;
		if (this.nCounter >= this.c.AllBasics.AliveAndDead.Count)
		{
			this.nCounter -= this.c.AllBasics.AliveAndDead.Count;
			if (!this.c.AllBasics.AliveAndDead.Blocks[this.nNowTrailL].IsAlive)
			{
				this.nNowTrailL = -1;
				this.MinX = 65535;
				this.MinXZ = 65535;
			}
			if (!this.c.AllBasics.AliveAndDead.Blocks[this.nNowTrailR].IsAlive)
			{
				this.nNowTrailR = -1;
				this.MaxX = -65535;
				this.MaxXZ = 65535;
			}
		}
	}

	// Token: 0x0400000D RID: 13
	private int nCounter;

	// Token: 0x0400000E RID: 14
	private int nAdd;

	// Token: 0x0400000F RID: 15
	private int nNowTrailL;

	// Token: 0x04000010 RID: 16
	private int nNowTrailR;

	// Token: 0x04000011 RID: 17
	private int MinX;

	// Token: 0x04000012 RID: 18
	private int MaxX;

	// Token: 0x04000013 RID: 19
	private int MinXZ;

	// Token: 0x04000014 RID: 20
	private int MaxXZ;

	// Token: 0x04000015 RID: 21
	public AllConstruct c;

	// Token: 0x04000016 RID: 22
	private float ResetCounter;

	// Token: 0x04000017 RID: 23
	private float ResetTime = 1f;

	// Token: 0x04000018 RID: 24
	private float TrailTime = 5f;

	// Token: 0x04000019 RID: 25
	private GameObject TrailL;

	// Token: 0x0400001A RID: 26
	private GameObject TrailR;

	// Token: 0x0400001B RID: 27
	private TrailRenderer TL;

	// Token: 0x0400001C RID: 28
	private TrailRenderer TR;

	// Token: 0x0400001D RID: 29
	public static Material mat;

	// Token: 0x0400001E RID: 30
	public static bool bCreate;

	// Token: 0x0400001F RID: 31
	private Gradient g;

	// Token: 0x04000020 RID: 32
	private GradientColorKey[] gck;

	// Token: 0x04000021 RID: 33
	private GradientAlphaKey[] gak;
}
