using System;
using System.Collections.Generic;
using BM_EffectUpdate.EffectCode;
using UnityEngine;

// Token: 0x0200000D RID: 13
public class SM_AutoKill : MonoBehaviour
{
	// Token: 0x06000029 RID: 41 RVA: 0x00003477 File Offset: 0x00001677
	private void Start()
	{
		this.nowInit = true;
		base.gameObject.SetActive(false);
		this.bCreate = false;
	}

	// Token: 0x0600002A RID: 42 RVA: 0x00003494 File Offset: 0x00001694
	public void Init()
	{
		this.nowInit = true;
		this.NowTime = 0f;
		if (base.GetComponent<ParticleSystem>())
		{
			base.GetComponent<ParticleSystem>().time = 0f;
			base.GetComponent<ParticleSystem>().Play();
		}
		base.gameObject.SetActive(true);
		SM_LightController[] componentsInChildren = base.GetComponentsInChildren<SM_LightController>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Init();
		}
	}

	// Token: 0x0600002B RID: 43 RVA: 0x00003504 File Offset: 0x00001704
	private void LateUpdate()
	{
		this.nowInit = false;
		this.deltaTime = Time.deltaTime;
		this.NowTime += this.deltaTime;
		if (this.KillTime < this.NowTime)
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x04000030 RID: 48
	public float KillTime = 5f;

	// Token: 0x04000031 RID: 49
	public List<GameObject> pool;

	// Token: 0x04000032 RID: 50
	public float NowTime;

	// Token: 0x04000033 RID: 51
	public float deltaTime;

	// Token: 0x04000034 RID: 52
	public bool nowInit = true;

	// Token: 0x04000035 RID: 53
	public bool bCreate = true;
}
