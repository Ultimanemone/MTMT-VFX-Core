using System;
using System.Collections.Generic;
using System.Reflection;
using BM_EffectUpdate.EffectCode;
using BrilliantSkies.FromTheDepths.Game.UserInterfaces;
using ProtecTechTools;
using UnityEngine;
using UnityEngine.PostProcessing;

namespace BM_EffectUpdate
{
	// Token: 0x0200002D RID: 45
	public class BM_EffectUpdater : MonoBehaviour
	{
		// Token: 0x06000094 RID: 148 RVA: 0x000057E0 File Offset: 0x000039E0
		public void Start()
		{
			Debug.Log("BMEU_Start");
			this.frameCount = 0;
			this.prevTime = 0f;
			this.gui = new BMEU_Gui();
			this.bInit = false;
			Object.DontDestroyOnLoad(base.gameObject);
			BM_EffectCreator.Creator = new BM_EffectCreator();
			this.LightObject = new GameObject("LightObject");
			this.LightObject.transform.parent = base.transform;
			this.DirectionalLight = this.LightObject.AddComponent<Light>();
			this.DirectionalLight.type = 1;
			Debug.Log("BMEU_Start-end");
		}

		// Token: 0x06000095 RID: 149 RVA: 0x0000587D File Offset: 0x00003A7D
		public static object GetField(string name, object obj)
		{
			return Access.GetFieldAlongHierarchy(obj.GetType(), name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy).GetValue(obj);
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00005894 File Offset: 0x00003A94
		public void Update()
		{
			this.frameCount++;
			float num = Time.realtimeSinceStartup - this.prevTime;
			if (num >= 0.5f)
			{
				this.fps = (float)this.frameCount / num;
				this.frameCount = 0;
				this.prevTime = Time.realtimeSinceStartup;
			}
		}

		// Token: 0x06000097 RID: 151 RVA: 0x000058E5 File Offset: 0x00003AE5
		public void DisableFancyFancyMF(GameObject mf)
		{
			ParticleSystem[] components = mf.GetComponents<ParticleSystem>();
			components[0].GetComponent<Renderer>().enabled = false;
			components[1].GetComponent<Renderer>().enabled = false;
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00005908 File Offset: 0x00003B08
		public void OnGUI()
		{
			if (this.gui.bShowFPS)
			{
				GUI.Label(new Rect(10f, 10f, 120f, 40f), "FPS:" + this.fps);
			}
			GUI.Label(new Rect(10f, 100f, 1024f, 100f), this.DbgMessage);
		}

		// Token: 0x06000099 RID: 153 RVA: 0x0000597C File Offset: 0x00003B7C
		public void LateUpdate()
		{
			if (Input.GetKeyDown(283))
			{
				GuiDisplayer singleton = GuiDisplayer.GetSingleton();
				if (!singleton.IsAnyGUIShowing())
				{
					singleton.AddGuiToStack(this.gui);
					this.bGui = !this.bGui;
				}
			}
			foreach (AllConstruct allConstruct in StaticConstructablesManager.constructables)
			{
				if (allConstruct.IsMain)
				{
					if (!allConstruct.GameObject.gameObject.GetComponent<BM_Trail>())
					{
						allConstruct.GameObject.gameObject.AddComponent<BM_Trail>().c = allConstruct;
					}
					if (!allConstruct.GameObject.gameObject.GetComponent<BM_ConstructSplasher>())
					{
						allConstruct.GameObject.gameObject.AddComponent<BM_ConstructSplasher>().c = allConstruct;
					}
				}
			}
			Camera foregroundCamera = CameraManager.GetSingleton().foregroundCamera;
			PostProcessingBehaviour component = foregroundCamera.GetComponent<PostProcessingBehaviour>();
			DepthOfFieldModel.Settings settings = component.profile.depthOfField.settings;
			float num = 1000f;
			Transform myTransform = CameraManager.GetSingleton().myTransform;
			GridCastReturn gridCastReturn = GridCasting.GridCastAllConstructables(new GridCastReturn(myTransform.position, myTransform.forward, 1000f, 1, false));
			if (gridCastReturn.HitSomething)
			{
				gridCastReturn.FirstHit.BlockHit.GetConstructableOrSubConstructable();
				num = Vector3.Distance(myTransform.transform.position, gridCastReturn.FirstHit.InPointGlobal);
			}
			settings.focusDistance = Mathf.Lerp(settings.focusDistance, num, 0.25f);
			component.profile.depthOfField.settings = settings;
			RenderSettings.ambientMode = 1;
			RenderSettings.ambientSkyColor = DirectionalSun.Light.color;
			RenderSettings.ambientGroundColor = DirectionalSun.Light.color * 0.5f;
			RenderSettings.ambientEquatorColor = new Color(0.3f, 0.3f, 0.3f);
			StaticInstantiables.missileThrusterParticles.Get();
			if (!this.bInit)
			{
				Ocean instance = Ocean.Instance;
				this.bInit = true;
				Debug.Log("BMEU_Init");
				CameraManager.GetSingleton().foregroundCamera.allowHDR = true;
				foregroundCamera.GetComponent<PostProcessingBehaviour>().profile = (PostProcessingProfile)BM_EffectUpdate.assetBundle.LoadAsset("BM_PPP");
				for (int i = 0; i < StaticPools.flash.poolSize; i++)
				{
					StaticPools.flash.poolArray[i].gameObject.AddComponent<BM_ExplosionCreator>();
				}
				for (int j = 0; j < StaticPools.LaserPulsePool.poolSize; j++)
				{
					LaserPulseRender obj = StaticPools.LaserPulsePool.poolArray[j];
					((Light)BM_EffectUpdater.GetField("_light", obj)).enabled = false;
					((LensFlare)BM_EffectUpdater.GetField("_lensEffect", obj)).enabled = false;
					((ParticleSystem.MainModule)BM_EffectUpdater.GetField("_fireParticle", obj)).startLifetime = 0f;
					StaticPools.LaserPulsePool.poolArray[j].gameObject.AddComponent<BM_PulseLaserCreator>();
				}
				for (int k = 0; k < StaticPools.splash.poolSize; k++)
				{
					StaticPools.splash.poolArray[k].gameObject.AddComponent<BM_SplashCreator>();
				}
				for (int l = 0; l < StaticPools.distort.poolSize; l++)
				{
					StaticPools.distort.poolArray[l].GetComponent<MeshRenderer>().enabled = false;
				}
				for (int m = 0; m < StaticPools.advprojectile.poolSize; m++)
				{
					AdvPooledProjectile proj = StaticPools.advprojectile.poolArray[m];
					StaticPools.advprojectile.gameObject.AddComponent<AdvProjStalker>().proj = proj;
				}
				for (int n = 0; n < StaticPools.cramProjectile.poolSize; n++)
				{
					PooledCramProjectile proj2 = StaticPools.cramProjectile.poolArray[n];
					StaticPools.cramProjectile.gameObject.AddComponent<CramProjStalker>().proj = proj2;
				}
				StaticInstantiables.customMissile.Get().AddComponent<SM_CancelTrail>();
				StaticInstantiables.customMissile.Get().AddComponent<SM_CreateMissileSmoke>();
				StaticInstantiables.stickyFlare.Get().AddComponent<SM_StickyFlare>();
				StaticInstantiables.ParticleCannonParticle.Get().AddComponent<BM_PacCreator>();
				StaticInstantiables.MF_Huge.Get().SetActive(false);
				StaticInstantiables.MF_Huge_HD.Get().SetActive(false);
				StaticInstantiables.MF_Large.Get().SetActive(false);
				StaticInstantiables.MF_Large_HD.Get().SetActive(false);
				StaticInstantiables.MF_Medium.Get().SetActive(false);
				StaticInstantiables.MF_Medium_HD.Get().SetActive(false);
				StaticInstantiables.MF_Small.Get().SetActive(false);
				StaticInstantiables.MF_Small_HD.Get().SetActive(false);
				StaticInstantiables.MF_Huge_L.Get().GetComponent<ExplosionsLightCurves>().GetComponent<Light>().enabled = false;
				StaticInstantiables.MF_Large_L.Get().GetComponent<ExplosionsLightCurves>().GetComponent<Light>().enabled = false;
				StaticInstantiables.MF_Small_L.Get().GetComponent<ExplosionsLightCurves>().GetComponent<Light>().enabled = false;
				StaticInstantiables.MF_Medium_L.Get().GetComponent<ExplosionsLightCurves>().GetComponent<Light>().enabled = false;
				this.gui.UpdateEffects();
				BM_Trail.mat = (Material)BM_EffectUpdate.assetBundle.LoadAsset("FireLoop 1");
				BM_Trail.mat.SetColor("_TintColor", Color.white);
				Debug.Log("BMEU_Init-end");
			}
			BM_Trail.mat.SetFloat("_time", BM_Trail.mat.GetFloat("_time") + Time.deltaTime * 1f);
			this.DirectionalLight.transform.forward = DirectionalSun.SunPosition;
			this.DirectionalLight.color = DirectionalSun.Light.color;
			this.DirectionalLight.intensity = DirectionalSun.Light.intensity;
			if (Input.GetKeyDown(122))
			{
				this.allname = "";
				foreach (GameObject obj2 in Array.FindAll<GameObject>(Object.FindObjectsOfType<GameObject>(), (GameObject item) => item.transform.parent == null))
				{
					this.AllGameObject(obj2, 0);
				}
				Debug.Log(this.allname);
			}
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00005FA4 File Offset: 0x000041A4
		public void AllGameObject(GameObject obj, int layer)
		{
			string text = "";
			for (int i = 0; i < layer; i++)
			{
				text += "-";
			}
			this.allname = this.allname + text + obj.name;
			this.allname += "\n";
			for (int j = 0; j < obj.transform.childCount; j++)
			{
				this.AllGameObject(obj.transform.GetChild(j).gameObject, layer + 1);
			}
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00006030 File Offset: 0x00004230
		public void FindBlocks(GameObject obj, int layer)
		{
			string text = "";
			for (int i = 0; i < layer; i++)
			{
				text += "-";
			}
			this.allname = this.allname + text + obj.name;
			this.allname += "\n";
			AllConstructGameObject component = obj.GetComponent<AllConstructGameObject>();
			if (component != null)
			{
				foreach (Block block in component.AllConstruct.iBlocks.AliveAndDead.Blocks)
				{
					this.allname = this.allname + text + block.Name;
					this.allname += "\n";
				}
				this.allname += "\n";
			}
			for (int j = 0; j < obj.transform.childCount; j++)
			{
				this.FindBlocks(obj.transform.GetChild(j).gameObject, layer + 1);
			}
		}

		// Token: 0x040000B1 RID: 177
		private Light DirectionalLight;

		// Token: 0x040000B2 RID: 178
		private Dictionary<int, BM_Selectable<float>> FirlingDic = new Dictionary<int, BM_Selectable<float>>();

		// Token: 0x040000B3 RID: 179
		private Dictionary<int, GameObject> WaveLaserDic = new Dictionary<int, GameObject>();

		// Token: 0x040000B4 RID: 180
		private Dictionary<int, GameObject> HashObject = new Dictionary<int, GameObject>();

		// Token: 0x040000B5 RID: 181
		private bool bGui;

		// Token: 0x040000B6 RID: 182
		private bool bInit;

		// Token: 0x040000B7 RID: 183
		private BMEU_Gui gui;

		// Token: 0x040000B8 RID: 184
		private static bool bUseBMEU = true;

		// Token: 0x040000B9 RID: 185
		private float fps;

		// Token: 0x040000BA RID: 186
		private int frameCount;

		// Token: 0x040000BB RID: 187
		private float prevTime;

		// Token: 0x040000BC RID: 188
		public GameObject LightObject;

		// Token: 0x040000BD RID: 189
		public string DbgMessage = "";

		// Token: 0x040000BE RID: 190
		public string allname = "";
	}
}
