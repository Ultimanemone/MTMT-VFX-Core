using System;
using System.Collections.Generic;
using UnityEngine;

namespace BM_EffectUpdate
{
	// Token: 0x02000038 RID: 56
	internal class BM_SImpleLaserChecker : MonoBehaviour
	{
		// Token: 0x060000B4 RID: 180 RVA: 0x00006DB4 File Offset: 0x00004FB4
		public void AddDict(Laser las)
		{
			int hashCode = las.GetHashCode();
			if (!this.dic.ContainsKey(hashCode))
			{
				Debug.Log("AddSimplaLaser");
				this.dic.Add(hashCode, new SL_Container(las));
			}
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00006DF2 File Offset: 0x00004FF2
		public void Start()
		{
			this.creator = BM_EffectCreator.Creator;
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00006E00 File Offset: 0x00005000
		public void Update()
		{
			foreach (int key in new List<int>(this.dic.Keys))
			{
				try
				{
					SL_Container sl_Container = this.dic[key];
					Laser obj = sl_Container.obj;
					if (sl_Container.root == null || obj == null)
					{
						this.dic.Remove(key);
					}
					else
					{
						CarriedObjectReference carriedObjectReference = (CarriedObjectReference)BM_EffectUpdater.GetField("beam", obj);
						sl_Container.bFire = carriedObjectReference.ActiveNow;
						if (sl_Container.bFire)
						{
							sl_Container.NowTime -= Time.deltaTime;
							if (sl_Container.NowTime < 0f)
							{
								sl_Container.NowTime = sl_Container.RldMax;
								GameObject gameObject = BM_EffectCreator.Creator.CreateLaser(BM_LaserEffectName.LaserFlash);
								gameObject.transform.position = obj.GetFirePoint(-0.5f);
								gameObject.transform.localScale = Vector3.one * 0.1f;
								gameObject.transform.forward = obj.GetFireDirection();
								gameObject.GetComponent<SM_LaserFlash>().Parent = null;
								gameObject.GetComponent<SM_LaserFlash>().SetColor(new Color(0.1f, 0.25f, 0.1f));
								gameObject.GetComponent<SM_LaserFlash>().Play();
							}
						}
					}
				}
				catch (Exception)
				{
					this.dic.Remove(key);
				}
			}
		}

		// Token: 0x040000FF RID: 255
		private Dictionary<int, SL_Container> dic = new Dictionary<int, SL_Container>();

		// Token: 0x04000100 RID: 256
		private BM_EffectCreator creator;
	}
}
