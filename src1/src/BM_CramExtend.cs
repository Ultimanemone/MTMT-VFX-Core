using System;
using ProtecTechTools;
using UnityEngine;

namespace BM_EffectUpdate
{
	// Token: 0x02000041 RID: 65
	internal class BM_CramExtend : ExtendedClass<CannonFiringPiece>
	{
		// Token: 0x060000CF RID: 207 RVA: 0x00007D7C File Offset: 0x00005F7C
		[ModifyMethod(1, "", "")]
		public void WeaponFire(FiredMunitionReturn FMR)
		{
			if (FMR.GetFired() && !base.this_.Node.BombChuteAttached)
			{
				float num = 1f;
				BM_MuzzleFlashName type;
				float radius;
				float num2;
				if ((double)base.this_.Node.Stats.ShellDiameter < 0.7)
				{
					type = BM_MuzzleFlashName.Tiny;
					radius = 5f;
					num2 = 2f;
				}
				else if ((double)base.this_.Node.Stats.ShellDiameter < 0.9)
				{
					type = BM_MuzzleFlashName.Small;
					radius = 10f;
					num2 = 4f;
				}
				else if ((double)base.this_.Node.Stats.ShellDiameter < 1.1)
				{
					type = BM_MuzzleFlashName.Medium;
					radius = 15f;
					num2 = 7f;
				}
				else if ((double)base.this_.Node.Stats.ShellDiameter < 1.3)
				{
					type = BM_MuzzleFlashName.Large;
					radius = 20f;
					num2 = 10f;
				}
				else if ((double)base.this_.Node.Stats.ShellDiameter < 1.5)
				{
					type = BM_MuzzleFlashName.Largest;
					radius = 30f;
					num2 = 15f;
				}
				else if ((double)base.this_.Node.Stats.ShellDiameter < 1.8)
				{
					type = BM_MuzzleFlashName.Huge;
					radius = 40f;
					num2 = 20f;
				}
				else
				{
					type = BM_MuzzleFlashName.Mammoth;
					radius = 50f;
					num2 = 30f;
				}
				GameObject gameObject = BM_EffectCreator.Creator.CreateMuzzle(type);
				if (gameObject != null)
				{
					gameObject.transform.localScale = Vector3.one * num;
					gameObject.transform.position = base.this_.GetFirePoint(0f);
					gameObject.transform.forward = base.this_.GetFireDirection();
				}
				Vector3 vector = base.this_.GetFirePoint(0f);
				vector += base.this_.GetFireDirection() * num2;
				BM_EffectCreator.Creator.CreateImpactSplash(vector, radius);
			}
		}
	}
}
