using System;
using System.Reflection;
using UnityEngine;

namespace BM_EffectUpdate
{
	// Token: 0x02000033 RID: 51
	internal class BM_PulseLaserCreator : MonoBehaviour
	{
		// Token: 0x060000A9 RID: 169 RVA: 0x000068F6 File Offset: 0x00004AF6
		public void Start()
		{
			this.creator = BM_EffectCreator.Creator;
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00006904 File Offset: 0x00004B04
		public void LateUpdate()
		{
			LaserPulseRender component = base.gameObject.GetComponent<LaserPulseRender>();
			try
			{
				if (base.gameObject.activeSelf)
				{
					base.gameObject.SetActive(false);
					this.MyPulse = this.creator.CreateLaser(BM_LaserEffectName.PulseLaser);
					GameObject gameObject = this.creator.CreateLaser(BM_LaserEffectName.LaserFlash);
					gameObject.transform.parent = this.MyPulse.transform;
					LineRenderer lineRenderer = (LineRenderer)component.GetType().GetField("_lineRender", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField).GetValue(component);
					Transform transform = (Transform)component.GetType().GetField("_transform", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField).GetValue(component);
					LaserPulseSpecification laserPulseSpecification = (LaserPulseSpecification)component.GetType().GetField("_spec", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField).GetValue(component);
					this.MyPulse.transform.position = transform.position;
					this.MyPulse.transform.forward = transform.forward;
					this.MyPulse.transform.localScale = Vector3.one * Mathf.Lerp(0f, 0.5f, laserPulseSpecification.StartingWidth / 3f);
					SM_PulseLaser component2 = this.MyPulse.GetComponent<SM_PulseLaser>();
					component2.Init();
					component2.col = lineRenderer.GetComponent<Renderer>().material.GetColor("_Color");
					component2.Length = lineRenderer.GetPosition(1).z;
					gameObject.GetComponent<SM_LaserFlash>().Parent = null;
					gameObject.GetComponent<SM_LaserFlash>().SetColor(component2.col);
					gameObject.GetComponent<SM_LaserFlash>().Play();
				}
			}
			catch
			{
			}
		}

		// Token: 0x040000F2 RID: 242
		public GameObject MyPulse;

		// Token: 0x040000F3 RID: 243
		private BM_EffectCreator creator;
	}
}
