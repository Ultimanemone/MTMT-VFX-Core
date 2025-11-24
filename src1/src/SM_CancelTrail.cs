using System;
using UnityEngine;

namespace BM_EffectUpdate
{
	// Token: 0x0200004C RID: 76
	internal class SM_CancelTrail : MonoBehaviour
	{
		// Token: 0x060000F4 RID: 244 RVA: 0x000093F8 File Offset: 0x000075F8
		public void Update()
		{
			Projectile component = base.GetComponent<Projectile>();
			if (component)
			{
				component.TR._trail.startWidth = 0f;
				component.TR._trail.endWidth = 0f;
			}
		}
	}
}
