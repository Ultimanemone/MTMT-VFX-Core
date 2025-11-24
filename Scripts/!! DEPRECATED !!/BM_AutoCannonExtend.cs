using System;
using ProtecTechTools;
using UnityEngine;

namespace BM_EffectUpdate
{
	// Token: 0x02000042 RID: 66
	internal class BM_AutoCannonExtend : ExtendedClass<AutoCannon>
	{
		// Token: 0x060000D1 RID: 209 RVA: 0x00007FA0 File Offset: 0x000061A0
		[ModifyMethod(1, "", "")]
		public Transform DispenseOneShell(Transform ThePreviousReturnValue)
		{
			GameObject gameObject = BM_EffectCreator.Creator.CreateMuzzle(BM_MuzzleFlashName.TinyTiny);
			if (gameObject != null)
			{
				gameObject.transform.localScale = Vector3.one;
				gameObject.transform.position = base.this_.GetFirePoint(0f);
				gameObject.transform.forward = base.this_.GetFireDirection();
			}
			if (base.this_.muzzleFlasher)
			{
				base.this_.muzzleFlasher.Deactivate();
			}
			return ThePreviousReturnValue;
		}
	}
}
