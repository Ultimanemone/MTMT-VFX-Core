using System;
using BM_EffectUpdate.EffectCode;
using UnityEngine;

// Token: 0x02000007 RID: 7
public class SM_LaserFlash : MonoBehaviour
{
	// Token: 0x06000017 RID: 23 RVA: 0x00002BF0 File Offset: 0x00000DF0
	public void Update()
	{
		if (this.Parent)
		{
			base.transform.position = this.Parent.position + this.Parent.rotation * this.AddPos;
			base.transform.rotation = this.Parent.rotation;
		}
	}

	// Token: 0x06000018 RID: 24 RVA: 0x00002C54 File Offset: 0x00000E54
	public void SetColor(Color col)
	{
		base.transform.FindChild("Flash").GetComponent<ParticleSystem>().Stop();
		base.transform.FindChild("LaserBase").GetComponent<ParticleSystem>().Stop();
		base.transform.FindChild("Spark").GetComponent<ParticleSystem>().Stop();
		base.transform.FindChild("LaserBase").GetComponent<ParticleSystem>().startColor = col + new Color(0.5f, 0.5f, 0.5f);
		base.transform.FindChild("Flash").GetComponent<ParticleSystem>().startColor = col + new Color(0.25f, 0.25f, 0.25f);
		base.transform.FindChild("Spark").GetComponent<ParticleSystem>().startColor = col + new Color(0.25f, 0.25f, 0.25f);
		base.transform.FindChild("PL").GetComponent<SM_LightController>().StartColor = col;
		base.transform.FindChild("PL").GetComponent<SM_LightController>().Init();
	}

	// Token: 0x06000019 RID: 25 RVA: 0x00002D80 File Offset: 0x00000F80
	public void Play()
	{
		base.transform.FindChild("Flash").GetComponent<ParticleSystem>().time = 0f;
		base.transform.FindChild("LaserBase").GetComponent<ParticleSystem>().time = 0f;
		base.transform.FindChild("Spark").GetComponent<ParticleSystem>().time = 0f;
		base.transform.FindChild("Flash").GetComponent<ParticleSystem>().Play();
		base.transform.FindChild("LaserBase").GetComponent<ParticleSystem>().Play();
		base.transform.FindChild("Spark").GetComponent<ParticleSystem>().Play();
	}

	// Token: 0x04000023 RID: 35
	public Transform Parent;

	// Token: 0x04000024 RID: 36
	public Vector3 AddPos = Vector3.zero;
}
