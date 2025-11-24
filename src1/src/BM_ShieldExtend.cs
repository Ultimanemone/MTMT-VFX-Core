using System;
using ProtecTechTools;

namespace BM_EffectUpdate
{
	// Token: 0x02000047 RID: 71
	internal class BM_ShieldExtend : ExtendedClass<ShieldProjector>
	{
		// Token: 0x060000E1 RID: 225 RVA: 0x00008748 File Offset: 0x00006948
		[ModifyMethod(1, "", "")]
		public void BlockStart()
		{
			if (base.this_.ShieldClass.MeshRenderer1.material.shader.name == "FTD/ColouredShield")
			{
				base.this_.ShieldClass.MeshRenderer1.material = BM_EffectUpdate.ShieldMat;
				base.this_.ShieldClass.MeshRenderer2.material = BM_EffectUpdate.ShieldMat;
			}
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x000087B4 File Offset: 0x000069B4
		[ModifyMethod(1, "", "")]
		public void IdealUse(IPowerRequestRecurring request)
		{
			try
			{
				if (base.this_.ShieldClass != null && base.this_.ShieldClass.MeshRenderer1 != null)
				{
					base.this_.ShieldClass.MeshRenderer1.enabled = BM_ShieldExtend.bVisible;
					base.this_.ShieldClass.MeshRenderer2.enabled = BM_ShieldExtend.bVisible;
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x04000126 RID: 294
		public static bool bVisible = true;
	}
}
