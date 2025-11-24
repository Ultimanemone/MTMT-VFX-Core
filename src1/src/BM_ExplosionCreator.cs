using System;
using UnityEngine;

namespace BM_EffectUpdate
{
	// Token: 0x0200002C RID: 44
	internal class BM_ExplosionCreator : MonoBehaviour
	{
		// Token: 0x06000091 RID: 145 RVA: 0x0000559A File Offset: 0x0000379A
		public void Start()
		{
			this.creator = BM_EffectCreator.Creator;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x000055A8 File Offset: 0x000037A8
		public void LateUpdate()
		{
			Explosion component = base.gameObject.GetComponent<Explosion>();
			if (!component.isActive)
			{
				return;
			}
			ExplosionSpec explosionSpec = component.explosionSpec;
			float num = 1f;
			float num2 = 1f;
			float num3 = 1f;
			BM_ExplosionsName type = BM_ExplosionsName.TinyBom;
			BM_ExplosionsName type2 = BM_ExplosionsName.TinySplash;
			if (explosionSpec.hotMetalDrops == 10)
			{
				type = BM_ExplosionsName.TinyBom;
				type2 = BM_ExplosionsName.TinySplash;
				num2 = 0.1f;
				num3 = 5f;
			}
			else if (explosionSpec.hotMetalDrops == 15)
			{
				type = BM_ExplosionsName.NormalBom;
				type2 = BM_ExplosionsName.TinySplash;
				num2 = 0.125f;
				num3 = 10f;
			}
			else if (explosionSpec.hotMetalDrops == 20)
			{
				if (explosionSpec.afterSmoke == 5)
				{
					type = BM_ExplosionsName.MediumBom;
					type2 = BM_ExplosionsName.LargeSplash;
					num = 0.25f;
					num3 = 15f;
				}
				else if (explosionSpec.afterSmoke == 15)
				{
					type = BM_ExplosionsName.LargeBom;
					type2 = BM_ExplosionsName.LargeSplash;
					num = 0.3f;
					num2 = 0.5f;
					num3 = 20f;
				}
				else if (explosionSpec.afterSmoke == 50)
				{
					type = BM_ExplosionsName.HugeBom;
					type2 = BM_ExplosionsName.HugeSplash;
					num = 0.3f;
					num2 = 1f;
					num3 = 25f;
				}
			}
			GameObject gameObject = this.creator.CreateExplosion(type, base.gameObject.transform.position);
			if (gameObject != null)
			{
				gameObject.transform.position = base.gameObject.transform.position;
				gameObject.transform.rotation = base.gameObject.transform.rotation;
				gameObject.transform.localScale = new Vector3(num, num, num);
			}
			if (base.gameObject.transform.position.y < 0f && base.gameObject.transform.position.y > -num3)
			{
				gameObject = this.creator.CreateExplosion(type2);
				if (gameObject != null)
				{
					gameObject.transform.position = new Vector3(base.gameObject.transform.position.x, 0f, base.gameObject.transform.position.z);
					num2 *= 0.25f;
					gameObject.transform.localScale = new Vector3(num2, num2 * 2f, num2);
					gameObject.transform.rotation = Quaternion.identity;
				}
			}
			component.Deactivate();
		}

		// Token: 0x040000B0 RID: 176
		private BM_EffectCreator creator;
	}
}
