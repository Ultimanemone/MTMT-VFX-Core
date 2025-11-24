using System;
using BMEffects_Remaster.src.Effects.Projectiles;
using BMEffects_Remaster.src.Util;
using UnityEngine;
using UnityEngine.PostProcessing;

namespace BrilliantSkies.FromTheDepths.Game.UserInterfaces
{
    // Token: 0x02000056 RID: 86
    public class BMEU_Gui : ThrowAwayGui
	{
		// Token: 0x06000119 RID: 281 RVA: 0x0000AA8C File Offset: 0x00008C8C
		public BMEU_Gui()
		{
			Debug.Log("LoadPrefs");
			this.NowPage = PlayerPrefs.GetInt("NowPage", 0);
			this.bBMEU = (PlayerPrefs.GetInt("BMEU", 1) == 1);
			this.bPointLight = (PlayerPrefs.GetInt("PointLight", 1) == 1);
			this.bBloom = (PlayerPrefs.GetInt("Bloom", 1) == 1);
			this.fBloom = PlayerPrefs.GetFloat("fBloom", 1f);
			this.bMotionBlur = (PlayerPrefs.GetInt("MotionBlur", 0) == 1);
			this.bDOF = (PlayerPrefs.GetInt("Dof", 1) == 1);
			this.bAA = (PlayerPrefs.GetInt("AA", 1) == 1);
			this.bAO = (PlayerPrefs.GetInt("AO", 1) == 1);
			this.fAO = PlayerPrefs.GetFloat("fAO", 1f);
			this.bVignette = (PlayerPrefs.GetInt("Vignette", 0) == 1);
			this.bChromaticaberration = (PlayerPrefs.GetInt("Chromaticaberration", 0) == 1);
			this.bShowFPS = (PlayerPrefs.GetInt("ShowFPS", 0) == 1);
			this.bVsync = (PlayerPrefs.GetInt("Vsync", 0) == 1);
			this.bShield = (PlayerPrefs.GetInt("Shield", 1) == 1);
			BM_ShieldExtend.bVisible = this.bShield;
			this.fDamageEffect = PlayerPrefs.GetFloat("DamageEffect", 1f);
			this.bSunShaft = (PlayerPrefs.GetInt("SunShaft", 0) == 1);
			this.bSSR = (PlayerPrefs.GetInt("SSR", 0) == 1);
			this.bVapar = (PlayerPrefs.GetInt("Vapar", 1) == 1);
			this.bScrew = (PlayerPrefs.GetInt("Screw", 1) == 1);
			this.bSplashes = (PlayerPrefs.GetInt("Splashes", 1) == 1);
			this.bExExplosion = (PlayerPrefs.GetInt("ExExplosion", 1) == 1);
			PlayerPrefs.SetInt("Vsync", this.bVsync ? 1 : 0);
			QualitySettings.vSyncCount = (this.bVsync ? 1 : 0);
			BM_ConstructSplasher.bCreate = this.bSplashes;
			BM_PropellerExtend.bCreate = this.bScrew;
			BM_Trail.bCreate = this.bVapar;
			this.UpdateEffects();
		}

		// Token: 0x0600011A RID: 282 RVA: 0x0000ACC8 File Offset: 0x00008EC8
		public void UpdateEffects()
		{
			Camera foregroundCamera = CameraManager.GetSingleton().foregroundCamera;
			PostProcessingBehaviour component = foregroundCamera.GetComponent<PostProcessingBehaviour>();
			SM_LightController.bEnable = this.bPointLight;
			foreach (Light light in SM_LightController.SMLC_LightList)
			{
				light.enabled = this.bPointLight;
			}
			component.profile.bloom.enabled = this.bBloom;
			BloomModel.Settings settings = component.profile.bloom.settings;
			settings.bloom.intensity = this.fBloom;
			component.profile.bloom.settings = settings;
			component.profile.motionBlur.enabled = this.bMotionBlur;
			component.profile.depthOfField.enabled = this.bDOF;
			component.profile.antialiasing.enabled = this.bAA;
			component.profile.screenSpaceReflection.enabled = this.bSSR;
			component.profile.vignette.enabled = this.bVignette;
			component.profile.ambientOcclusion.enabled = this.bAO;
			AmbientOcclusionModel.Settings settings2 = component.profile.ambientOcclusion.settings;
			settings2.intensity = this.fAO;
			component.profile.ambientOcclusion.settings = settings2;
			component.profile.chromaticAberration.enabled = this.bChromaticaberration;
			BM_EffectCreator.bExExplosion = this.bExExplosion;
			((BleedBehavior)foregroundCamera.GetComponent("BleedBehavior")).maxAlpha = this.fDamageEffect;
		}

		// Token: 0x0600011B RID: 283 RVA: 0x0000AE74 File Offset: 0x00009074
		public override void SetGuiSettings()
		{
			base.GuiSettings.PausesPlay = false;
		}

		// Token: 0x0600011C RID: 284 RVA: 0x0000AE84 File Offset: 0x00009084
		public override void OnGui()
		{
			GUILayout.BeginArea(new Rect(20f, 20f, 1240f, 720f), "EffectSetting GUI(エフェクトセッティングGUI) ver1.3.0 " + (this.NowPage + 1) + "/2", GUI.skin.window);
			this._scroll = GUILayout.BeginScrollView(this._scroll, new GUILayoutOption[0]);
			int nowPage = this.NowPage;
			if (nowPage != 0)
			{
				if (nowPage == 1)
				{
					if (this.bScrew != GUILayout.Toggle(this.bScrew, "ScrewEffect(スクリューの追加エフェクト)", new GUILayoutOption[0]))
					{
						this.bScrew = !this.bScrew;
						BM_PropellerExtend.bCreate = this.bScrew;
						PlayerPrefs.SetInt("Screw", this.bScrew ? 1 : 0);
					}
					if (this.bVapar != GUILayout.Toggle(this.bVapar, "VaporTrail(飛行機とかの端から出る軌跡)", new GUILayoutOption[0]))
					{
						this.bVapar = !this.bVapar;
						BM_Trail.bCreate = this.bVapar;
						PlayerPrefs.SetInt("Vapar", this.bVapar ? 1 : 0);
					}
					if (this.bSplashes != GUILayout.Toggle(this.bSplashes, "Vehicle Splashes(機体から出る水しぶき)", new GUILayoutOption[0]))
					{
						this.bSplashes = !this.bSplashes;
						BM_ConstructSplasher.bCreate = this.bSplashes;
						PlayerPrefs.SetInt("Splashes", this.bSplashes ? 1 : 0);
					}
					if (this.bExExplosion != GUILayout.Toggle(this.bExExplosion, "ExExplosion(広範囲榴弾用特殊エフェクト)", new GUILayoutOption[0]))
					{
						this.bExExplosion = !this.bExExplosion;
						PlayerPrefs.SetInt("ExExplosion", this.bExExplosion ? 1 : 0);
						BM_EffectCreator.bExExplosion = this.bExExplosion;
					}
					if (this.bShield != GUILayout.Toggle(this.bShield, "VisibleShield（シールド表示）", new GUILayoutOption[0]))
					{
						this.bShield = !this.bShield;
						PlayerPrefs.SetInt("Shield", this.bShield ? 1 : 0);
						BM_ShieldExtend.bVisible = this.bShield;
					}
					if (this.bShowFPS != GUILayout.Toggle(this.bShowFPS, "ShowFPS(FPS表示)", new GUILayoutOption[0]))
					{
						this.bShowFPS = !this.bShowFPS;
						PlayerPrefs.SetInt("ShowFPS", this.bShowFPS ? 1 : 0);
					}
					if (this.bVsync != GUILayout.Toggle(this.bVsync, "VSync(垂直同期)", new GUILayoutOption[0]))
					{
						this.bVsync = !this.bVsync;
						PlayerPrefs.SetInt("Vsync", this.bVsync ? 1 : 0);
						QualitySettings.vSyncCount = (this.bVsync ? 1 : 0);
					}
				}
			}
			else
			{
				if (this.bPointLight != GUILayout.Toggle(this.bPointLight, "PointLight(点光源の有効化)", new GUILayoutOption[0]))
				{
					this.bPointLight = !this.bPointLight;
					PlayerPrefs.SetInt("PointLight", this.bPointLight ? 1 : 0);
					this.UpdateEffects();
				}
				if (this.bAA != GUILayout.Toggle(this.bAA, "AntiAlias(アンチエイリアス)", new GUILayoutOption[0]))
				{
					this.bAA = !this.bAA;
					PlayerPrefs.SetInt("AA", this.bAA ? 1 : 0);
					this.UpdateEffects();
				}
				if (this.bAO != GUILayout.Toggle(this.bAO, "AmbientOcclusion(アンビエントオクルージョン（隅っことかに陰影がつくやつ）)", new GUILayoutOption[0]))
				{
					this.bAO = !this.bAO;
					PlayerPrefs.SetInt("AO", this.bAO ? 1 : 0);
					this.UpdateEffects();
				}
				GUILayout.Label("Intensity(強さ):" + this.fAO, new GUILayoutOption[0]);
				float num = GUILayout.HorizontalSlider(this.fAO, 0f, 10f, new GUILayoutOption[0]);
				if (this.fAO != num)
				{
					this.fAO = num;
					PlayerPrefs.SetFloat("fAO", this.fAO);
					this.UpdateEffects();
				}
				if (GUILayout.Button("Reset", new GUILayoutOption[0]))
				{
					this.fAO = 1f;
					PlayerPrefs.SetFloat("fAO", this.fAO);
					this.UpdateEffects();
				}
				if (this.bBloom != GUILayout.Toggle(this.bBloom, "LightBloom(ライトブルーム)", new GUILayoutOption[0]))
				{
					this.bBloom = !this.bBloom;
					PlayerPrefs.SetInt("Bloom", this.bBloom ? 1 : 0);
					this.UpdateEffects();
				}
				GUILayout.Label("Intensity(強さ):" + this.fBloom, new GUILayoutOption[0]);
				num = GUILayout.HorizontalSlider(this.fBloom, 0f, 10f, new GUILayoutOption[0]);
				if (this.fBloom != num)
				{
					this.fBloom = num;
					PlayerPrefs.SetFloat("fBloom", this.fBloom);
					this.UpdateEffects();
				}
				if (GUILayout.Button("Reset", new GUILayoutOption[0]))
				{
					this.fBloom = 1f;
					PlayerPrefs.SetFloat("fBloom", this.fBloom);
					this.UpdateEffects();
				}
				if (this.bMotionBlur != GUILayout.Toggle(this.bMotionBlur, "MotionBlur(モーションブラー)", new GUILayoutOption[0]))
				{
					this.bMotionBlur = !this.bMotionBlur;
					PlayerPrefs.SetInt("MotionBlur", this.bMotionBlur ? 1 : 0);
					this.UpdateEffects();
				}
				if (this.bDOF != GUILayout.Toggle(this.bDOF, "DepthOfField(被写界深度)", new GUILayoutOption[0]))
				{
					this.bDOF = !this.bDOF;
					PlayerPrefs.SetInt("Dof", this.bDOF ? 1 : 0);
					this.UpdateEffects();
				}
				if (this.bVignette != GUILayout.Toggle(this.bVignette, "Vignette(ヴィネット（古いカメラ風のアレ）)", new GUILayoutOption[0]))
				{
					this.bVignette = !this.bVignette;
					PlayerPrefs.SetInt("Vignette", this.bVignette ? 1 : 0);
					this.UpdateEffects();
				}
				if (this.bChromaticaberration != GUILayout.Toggle(this.bChromaticaberration, "ChromaticAberration(色収差（古いカメラ風のアレ２）)", new GUILayoutOption[0]))
				{
					this.bChromaticaberration = !this.bChromaticaberration;
					PlayerPrefs.SetInt("Chromaticaberration", this.bChromaticaberration ? 1 : 0);
					this.UpdateEffects();
				}
				GUILayout.Label("DamageEffect(ダメージエフェクト強さ)", new GUILayoutOption[0]);
				num = GUILayout.HorizontalSlider(this.fDamageEffect, 0f, 1f, new GUILayoutOption[0]);
				if (this.fDamageEffect != num)
				{
					this.fDamageEffect = num;
					PlayerPrefs.SetFloat("DamageEffect", this.fDamageEffect);
					this.UpdateEffects();
				}
			}
			if (GUILayout.Button("NextPage", new GUILayoutOption[]
			{
				GUILayout.Height(30f)
			}))
			{
				this.NowPage++;
				this.NowPage %= 2;
				PlayerPrefs.SetInt("NowPage", this.NowPage);
			}
			if (GUILayout.Button("Close", new GUILayoutOption[]
			{
				GUILayout.Height(30f)
			}))
			{
				GUISoundManager.GetSingleton().PlayBeep();
				base.DeactivateGui(0);
			}
			if (GuiCommon.DisplayCloseButton(1240))
			{
				GUISoundManager.GetSingleton().PlayBeep();
				base.DeactivateGui(0);
			}
			GUILayout.EndScrollView();
			GUILayout.EndArea();
		}

		// Token: 0x04000179 RID: 377
		private bool bBMEU;

		// Token: 0x0400017A RID: 378
		private bool bPointLight;

		// Token: 0x0400017B RID: 379
		private bool bBloom;

		// Token: 0x0400017C RID: 380
		private float fBloom;

		// Token: 0x0400017D RID: 381
		private bool bMotionBlur;

		// Token: 0x0400017E RID: 382
		private bool bDOF;

		// Token: 0x0400017F RID: 383
		private bool bAA;

		// Token: 0x04000180 RID: 384
		private bool bAO;

		// Token: 0x04000181 RID: 385
		private float fAO;

		// Token: 0x04000182 RID: 386
		private bool bSSR;

		// Token: 0x04000183 RID: 387
		private bool bVignette;

		// Token: 0x04000184 RID: 388
		private bool bChromaticaberration;

		// Token: 0x04000185 RID: 389
		private float fDamageEffect;

		// Token: 0x04000186 RID: 390
		private bool bSunShaft;

		// Token: 0x04000187 RID: 391
		public bool bShowFPS;

		// Token: 0x04000188 RID: 392
		public bool bVsync;

		// Token: 0x04000189 RID: 393
		public bool bShield;

		// Token: 0x0400018A RID: 394
		public bool bVapar;

		// Token: 0x0400018B RID: 395
		public bool bScrew;

		// Token: 0x0400018C RID: 396
		public bool bSplashes;

		// Token: 0x0400018D RID: 397
		public bool bExExplosion;

		// Token: 0x0400018E RID: 398
		private Vector2 _scroll = Vector2.zero;

		// Token: 0x0400018F RID: 399
		public int NowPage;
	}
}
