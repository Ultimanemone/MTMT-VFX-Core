using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace BM_EffectUpdate
{
	// Token: 0x0200002B RID: 43
	public class BM_EffectUpdate : FTDPlugin
	{
		// Token: 0x0600008A RID: 138 RVA: 0x00005374 File Offset: 0x00003574
		public void OnLoad()
		{
			Debug.Log("BMEU_ver" + BM_EffectUpdate.ver);
			Debug.Log("LoadAssetBundle...!");
			if (Application.platform == 2)
			{
				BM_EffectUpdate.assetBundle = AssetBundle.LoadFromFile(Path.Combine(StaticPaths.GetModFolderForMod("BM_EffectUpdate/Bundle/"), "StandaloneWindows64/sm1"));
			}
			else if (Application.platform == 1)
			{
				BM_EffectUpdate.assetBundle = AssetBundle.LoadFromFile(Path.Combine(StaticPaths.GetModFolderForMod("BM_EffectUpdate/Bundle/"), "StandaloneOSXUniversal/sm1"));
			}
			else if (Application.platform == 13)
			{
				BM_EffectUpdate.assetBundle = AssetBundle.LoadFromFile(Path.Combine(StaticPaths.GetModFolderForMod("BM_EffectUpdate/Bundle/"), "StandaloneLinuxUniversal/sm1"));
			}
			Debug.Log("LoadData:" + BM_EffectUpdate.assetBundle);
			BM_EffectUpdate.updater = new GameObject().AddComponent<BM_EffectUpdater>();
			BM_EffectUpdate.updater.transform.position = Vector3.zero;
			BM_EffectUpdate.updater.transform.localScale = Vector3.one;
			BM_EffectUpdate.updater.name = "BM_EffectUpdate";
			foreach (GameObject gameObject in BM_EffectUpdate.assetBundle.LoadAllAssets<GameObject>())
			{
				BM_EffectUpdate.Assets.Add(gameObject.name, gameObject);
			}
			BM_EffectUpdate.ShieldShader = BM_EffectUpdate.assetBundle.LoadAsset<Shader>("BM_Shield");
			BM_EffectUpdate.ShieldMat = new Material(BM_EffectUpdate.ShieldShader);
			BM_EffectUpdate.ShieldMat.SetTexture("_NormalTex", BM_EffectUpdate.assetBundle.LoadAsset<Texture>("honeycomb"));
			BM_EffectUpdate.ShieldMat.SetTextureScale("_NormalTex", Vector2.one * 16f);
			BM_EffectUpdate.ShieldMat.SetFloat("_Distortion", 0.1f);
			BM_EffectUpdate.ShieldMat.SetTexture("_NoizeTex", BM_EffectUpdate.assetBundle.LoadAsset<Texture>("noize"));
			BM_EffectUpdate.ShieldMat.SetTextureScale("_NoizeTex", Vector2.one * 10f);
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00002BE3 File Offset: 0x00000DE3
		public void OnSave()
		{
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600008C RID: 140 RVA: 0x0000554D File Offset: 0x0000374D
		public string name
		{
			get
			{
				return "BM_EffectUpdate";
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600008D RID: 141 RVA: 0x00005554 File Offset: 0x00003754
		public Version version
		{
			get
			{
				return new Version(BM_EffectUpdate.ver);
			}
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00005560 File Offset: 0x00003760
		public static GameObject GetAsset(string name)
		{
			GameObject result = null;
			BM_EffectUpdate.Assets.TryGetValue(name, out result);
			return result;
		}

		// Token: 0x040000A9 RID: 169
		public static BM_EffectUpdater updater = null;

		// Token: 0x040000AA RID: 170
		public static AssetBundle assetBundle;

		// Token: 0x040000AB RID: 171
		public static Dictionary<string, GameObject> Assets = new Dictionary<string, GameObject>();

		// Token: 0x040000AC RID: 172
		public static Shader ShieldShader;

		// Token: 0x040000AD RID: 173
		public static Material ShieldMat;

		// Token: 0x040000AE RID: 174
		public static GameObject WaveLaser;

		// Token: 0x040000AF RID: 175
		public static string ver = "1.3.0";
	}
}
