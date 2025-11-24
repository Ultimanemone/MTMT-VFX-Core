using BrilliantSkies.Core.ChangeControl;
using BrilliantSkies.Core.Logger;
using BrilliantSkies.Core.Unity;
using BrilliantSkies.Modding;
using BrilliantSkies.Modding.Types;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace BM_EffectUpdate
{
    // Token: 0x0200002B RID: 43
    public class BM_EffectUpdate : GamePlugin_PostLoad
    {
        //public static BM_EffectUpdater updater = null;
        public const string guid = "26d4fb8e-c3bb-4860-b2c1-7530c30199d5";
        public static AssetBundle assetBundle;
        public static Dictionary<string, GameObject> Assets = new Dictionary<string, GameObject>();
        public static Shader ShieldShader;  
        public static Material ShieldMat;
        public static GameObject WaveLaser;
        public static string ver = "1.3.0";


        // Token: 0x0600008A RID: 138 RVA: 0x00005374 File Offset: 0x00003574
        public void OnLoad()
        {
            //Debug.Log("BMEU_ver" + BMEffects_Remaster.ver);
            //Debug.Log("LoadAssetBundle...!");
            //if (Application.platform == 2)
            //{
            //    BMEffects_Remaster.assetBundle = AssetBundle.LoadFromFile(Path.Combine(StaticPaths.GetModFolderForMod("BMEffects_Remaster/Bundle/"), "StandaloneWindows64/sm1"));
            //}
            //else if (Application.platform == 1)
            //{
            //    BMEffects_Remaster.assetBundle = AssetBundle.LoadFromFile(Path.Combine(StaticPaths.GetModFolderForMod("BMEffects_Remaster/Bundle/"), "StandaloneOSXUniversal/sm1"));
            //}
            //else if (Application.platform == 13)
            //{
            //    BMEffects_Remaster.assetBundle = AssetBundle.LoadFromFile(Path.Combine(StaticPaths.GetModFolderForMod("BMEffects_Remaster/Bundle/"), "StandaloneLinuxUniversal/sm1"));
            //}
            //Debug.Log("LoadData:" + BMEffects_Remaster.assetBundle);
            //BMEffects_Remaster.updater = new GameObject().AddComponent<BM_EffectUpdater>();
            //BMEffects_Remaster.updater.transform.position = Vector3.zero;
            //BMEffects_Remaster.updater.transform.localScale = Vector3.one;
            //BMEffects_Remaster.updater.name = "BMEffects_Remaster";
            //foreach (GameObject gameObject in BMEffects_Remaster.assetBundle.LoadAllAssets<GameObject>())
            //{
            //    BMEffects_Remaster.Assets.Add(gameObject.name, gameObject);
            //}

            //BMEffects_Remaster.ShieldShader = BMEffects_Remaster.assetBundle.LoadAsset<Shader>("BM_Shield");
            //BMEffects_Remaster.ShieldMat = new Material(BMEffects_Remaster.ShieldShader);
            //BMEffects_Remaster.ShieldMat.SetTexture("_NormalTex", BMEffects_Remaster.assetBundle.LoadAsset<Texture>("honeycomb"));
            //BMEffects_Remaster.ShieldMat.SetTextureScale("_NormalTex", Vector2.one * 16f);
            //BMEffects_Remaster.ShieldMat.SetFloat("_Distortion", 0.1f);
            //BMEffects_Remaster.ShieldMat.SetTexture("_NoizeTex", BMEffects_Remaster.assetBundle.LoadAsset<Texture>("noize"));
            //BMEffects_Remaster.ShieldMat.SetTextureScale("_NoizeTex", Vector2.one * 10f);

            new Harmony("BMEffects_Remaster_Plugin").PatchAll();
            ModProblems.AddModProblem($"{name} v{ver} active!", Assembly.GetExecutingAssembly().Location, string.Empty, false);
        }

        // Token: 0x0600008B RID: 139 RVA: 0x00002BE3 File Offset: 0x00000DE3
        public void OnSave()
        {
        }

        public bool AfterAllPluginsLoaded()
        {
            return true;
        }

        // Token: 0x17000002 RID: 2
        // (get) Token: 0x0600008C RID: 140 RVA: 0x0000554D File Offset: 0x0000374D
        public string name
        {
            get
            {
                return "BMEffects_Remaster";
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
            AssetBundleDefinition? bundle = Configured.i.AssetBundles.Find(new Guid(BM_EffectUpdate.guid));

            if (bundle == null!)
            {
                AdvLogger.Log(LogPriority.Error, $"Bundle (guid: {BM_EffectUpdate.guid}) not found!", LogOptions.Popup);
                ModProblems.AddModProblem($"BUNDLE NOT FOUND!", Assembly.GetExecutingAssembly().Location, string.Empty, true);
            }

            GameObject result = null;
            bundle.Loader.GetThing(name, out result);

            return result;
        }
    }
}