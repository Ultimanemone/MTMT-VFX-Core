using System;
using ProtecTechTools;
using UnityEngine;

namespace BM_EffectUpdate
{
	// Token: 0x02000043 RID: 67
	internal class BM_SimpleCannonExtend : ExtendedClass<Cannon>
	{
		// Token: 0x060000D3 RID: 211 RVA: 0x00008030 File Offset: 0x00006230
		[ModifyMethod(1, "", "")]
		public void WeaponFire(FiredMunitionReturn FMR)
		{
			if (FMR.GetFired())
			{
				GameObject gameObject = BM_EffectCreator.Creator.CreateMuzzle(BM_MuzzleFlashName.TinyTiny);
				if (gameObject != null)
				{
					gameObject.transform.localScale = Vector3.one;
					gameObject.transform.position = base.this_.GetFirePoint(0f);
					gameObject.transform.forward = base.this_.GetFireDirection();
				}
			}
		}
	}
}
