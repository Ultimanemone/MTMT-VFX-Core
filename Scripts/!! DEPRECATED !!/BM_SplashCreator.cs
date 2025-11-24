using System;
using BMEffects_Remaster.Util;
using UnityEngine;

namespace BM_EffectUpdate
{
    // Token: 0x02000039 RID: 57
    internal class BM_SplashCreator : MonoBehaviour
	{
		// Token: 0x060000B8 RID: 184 RVA: 0x00006FBB File Offset: 0x000051BB
		public void Start()
		{
			this.creator = BM_EffectCreator.Creator;
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00006FC8 File Offset: 0x000051C8
		public void LateUpdate()
		{
			Splash component = base.gameObject.GetComponent<Splash>();
			if (!component.isActive)
			{
				return;
			}
			GameObject gameObject = this.creator.CreateExplosion(BM_ExplosionsName.LargeSplash_Pure);
			if (gameObject != null)
			{
				gameObject.transform.position = new Vector3(base.gameObject.transform.position.x, 0f, base.gameObject.transform.position.z);
				gameObject.transform.rotation = Quaternion.identity;
				gameObject.transform.localScale = base.gameObject.transform.localScale * 0.01f;
			}
			component.Deactivate();
		}

		// Token: 0x04000101 RID: 257
		private BM_EffectCreator creator;
	}
}
