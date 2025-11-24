using System;
using System.Collections.Generic;
using UnityEngine;

namespace BM_EffectUpdate
{
	// Token: 0x0200003C RID: 60
	internal class BM_LaserCombinerChecker : MonoBehaviour
	{
		// Token: 0x060000BF RID: 191 RVA: 0x0000723C File Offset: 0x0000543C
		public void AddDict(LaserCombiner las)
		{
			int hashCode = las.GetHashCode();
			if (!this.dic.ContainsKey(hashCode))
			{
				this.dic.Add(hashCode, new LC_Container(las));
			}
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00007270 File Offset: 0x00005470
		public void Start()
		{
			this.creator = BM_EffectCreator.Creator;
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00007280 File Offset: 0x00005480
		public void AllClear()
		{
			foreach (int key in new List<int>(this.dic.Keys))
			{
				LC_Container lc_Container = this.dic[key];
				lc_Container.wlobj.GetComponent<SM_WaveLaser>().TgtScale = 0f;
				Object.Destroy(lc_Container.wlobj, 5f);
				this.dic.Remove(key);
			}
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00007314 File Offset: 0x00005514
		public void Update()
		{
			foreach (int key in new List<int>(this.dic.Keys))
			{
				try
				{
					LC_Container lc_Container = this.dic[key];
					LaserCombiner obj = lc_Container.obj;
					if (lc_Container.root == null || obj == null)
					{
						lc_Container.wlobj.GetComponent<SM_WaveLaser>().TgtScale = 0f;
						Object.Destroy(lc_Container.wlobj, 5f);
						this.dic.Remove(key);
					}
					else
					{
						LineRenderer lineRenderer = obj.CWBeam.LineRenderer;
						SM_WaveLaser component = lc_Container.wlobj.GetComponent<SM_WaveLaser>();
						try
						{
							if (obj.isAlive && obj.CWBeam.gameObject.activeSelf && !obj.pulseBeam.gameObject.activeSelf)
							{
								component.TgtScale = obj.CWBeam.laserWidth;
							}
							else
							{
								component.TgtScale = 0f;
							}
							if (obj.isAlive)
							{
								int num = obj.Node.allOptics.Count + 2;
								if (lineRenderer.positionCount >= num - 1)
								{
									component.transform.position = obj.GetFirePoint(0f);
									Vector3 vector = lineRenderer.GetPosition(num - 1);
									vector = StaticMaths.LocalToGlobal(vector, lineRenderer.transform.position, lineRenderer.transform.rotation);
									component.transform.forward = vector - component.transform.position;
									component.Length = Vector3.Distance(component.transform.position, vector);
								}
								component.SetColor(lineRenderer.startColor);
								((Light)BM_EffectUpdater.GetField("cwLight", obj)).enabled = false;
								((ParticleSystem.MainModule)BM_EffectUpdater.GetField("cwFireParticle", obj)).startColor = Color.black;
								((LensFlare)BM_EffectUpdater.GetField("cwLensEffect", obj)).enabled = false;
							}
						}
						catch (ArgumentException)
						{
							component.TgtScale = 0f;
						}
						lineRenderer.enabled = false;
					}
				}
				catch (Exception)
				{
					LC_Container lc_Container2 = this.dic[key];
					lc_Container2.wlobj.GetComponent<SM_WaveLaser>().TgtScale = 0f;
					Object.Destroy(lc_Container2.wlobj, 5f);
					this.dic.Remove(key);
				}
			}
		}

		// Token: 0x0400010B RID: 267
		private Dictionary<int, LC_Container> dic = new Dictionary<int, LC_Container>();

		// Token: 0x0400010C RID: 268
		private BM_EffectCreator creator;
	}
}
