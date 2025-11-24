using System;
using System.Reflection;
using UnityEngine;

namespace BM_EffectUpdate
{
	// Token: 0x02000035 RID: 53
	internal class BM_SimpleFlashCreator : MonoBehaviour
	{
		// Token: 0x060000AD RID: 173 RVA: 0x00006ADE File Offset: 0x00004CDE
		public void Start()
		{
			this.mzl_creator = BM_EffectCreator.Creator;
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00006AEC File Offset: 0x00004CEC
		public void LateUpdate()
		{
			PooledProjectile component = base.gameObject.GetComponent<PooledProjectile>();
			enumProjectileMeshType enumProjectileMeshType = (enumProjectileMeshType)component.GetType().GetField("_meshType", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField).GetValue(component);
			float num = 1f;
			if ((enumProjectileMeshType == 4 || enumProjectileMeshType == 1) && enumProjectileMeshType == 1)
			{
				num = 0.1f;
			}
			BM_MuzzleFlashName type;
			if (enumProjectileMeshType == 3 || enumProjectileMeshType == 2)
			{
				type = BM_MuzzleFlashName.TinyTiny;
			}
			else
			{
				type = BM_MuzzleFlashName.TinyTiny;
			}
			if (component.GetFlightTime() < this.PrevTime)
			{
				GameObject gameObject = this.mzl_creator.CreateMuzzle(type);
				if (gameObject != null)
				{
					gameObject.transform.position = component.transform.position;
					gameObject.transform.rotation = base.gameObject.transform.rotation;
					gameObject.transform.localScale = Vector3.one * num;
				}
			}
			this.PrevTime = component.GetFlightTime();
		}

		// Token: 0x040000F4 RID: 244
		private float PrevTime = 65535f;

		// Token: 0x040000F5 RID: 245
		private BM_EffectCreator mzl_creator;
	}
}
