using System;
using UnityEngine;

namespace BM_EffectUpdate
{
	// Token: 0x0200004B RID: 75
	public class BM_GodRay : MonoBehaviour
	{
		// Token: 0x060000F1 RID: 241 RVA: 0x0000934E File Offset: 0x0000754E
		private void Start()
		{
			this.m_Material = new Material((Material)BM_EffectUpdate.assetBundle.LoadAsset("BM_GodRayMat"));
			Debug.Log("ShaderName:" + this.m_Material.shader.name);
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00009390 File Offset: 0x00007590
		private void OnRenderImage(RenderTexture src, RenderTexture dest)
		{
			this.SunPosition = this.DirLight.transform.forward * 1000f + base.transform.position;
			this.m_Material.SetVector("_SunPos", this.SunPosition);
			Graphics.Blit(src, dest, this.m_Material);
		}

		// Token: 0x04000130 RID: 304
		private Material m_Material;

		// Token: 0x04000131 RID: 305
		public Light DirLight;

		// Token: 0x04000132 RID: 306
		private Vector3 SunPosition;
	}
}
