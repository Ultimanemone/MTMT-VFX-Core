using System;
using System.Collections.Generic;
using BMEffects_Remaster.Util;
using UnityEngine;

namespace BM_EffectUpdate
{
    // Token: 0x0200003E RID: 62
    internal class BM_LMDChecker : MonoBehaviour
	{
		// Token: 0x060000C5 RID: 197 RVA: 0x000076A8 File Offset: 0x000058A8
		public void AddDict(LaserMissileDefence las)
		{
			int hashCode = las.GetHashCode();
			if (!this.dic.ContainsKey(hashCode))
			{
				Debug.Log("AddLMD");
				this.dic.Add(hashCode, new LMD_Container(las));
			}
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x000076E8 File Offset: 0x000058E8
		public void AllClear()
		{
			foreach (int key in new List<int>(this.dic.Keys))
			{
				LMD_Container lmd_Container = this.dic[key];
				lmd_Container.wlobj.GetComponent<SM_WaveLaser>().TgtScale = 0f;
				Object.Destroy(lmd_Container.wlobj, 5f);
				this.dic.Remove(key);
			}
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x0000777C File Offset: 0x0000597C
		public void Start()
		{
			this.creator = BM_EffectCreator.Creator;
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x0000778C File Offset: 0x0000598C
		public void Update()
		{
			foreach (int key in new List<int>(this.dic.Keys))
			{
				try
				{
					LMD_Container lmd_Container = this.dic[key];
					LaserMissileDefence obj = lmd_Container.obj;
					if (lmd_Container.root == null || obj == null)
					{
						lmd_Container.wlobj.GetComponent<SM_WaveLaser>().TgtScale = 0f;
						Object.Destroy(lmd_Container.wlobj, 5f);
						this.dic.Remove(key);
					}
					else
					{
						LineRenderer lineRenderer = obj.CWBeam.LineRenderer;
						SM_WaveLaser component = lmd_Container.wlobj.GetComponent<SM_WaveLaser>();
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
								component.transform.position = obj.GameWorldPosition;
								Vector3 vector = lineRenderer.GetPosition(1);
								vector = StaticMaths.LocalToGlobal(vector, lineRenderer.transform.position, lineRenderer.transform.rotation);
								component.transform.forward = vector - component.transform.position;
								component.Length = Vector3.Distance(component.transform.position, vector);
								component.SetColor(lineRenderer.startColor);
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
					LMD_Container lmd_Container2 = this.dic[key];
					lmd_Container2.wlobj.GetComponent<SM_WaveLaser>().TgtScale = 0f;
					Object.Destroy(lmd_Container2.wlobj, 5f);
					this.dic.Remove(key);
				}
			}
		}

		// Token: 0x04000113 RID: 275
		private Dictionary<int, LMD_Container> dic = new Dictionary<int, LMD_Container>();

		// Token: 0x04000114 RID: 276
		private BM_EffectCreator creator;
	}
}
