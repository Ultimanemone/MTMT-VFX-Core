using BrilliantSkies.Core.ChangeControl;
using BrilliantSkies.Core.Logger;
using BrilliantSkies.Core.Unity;
using BrilliantSkies.Modding;
using BrilliantSkies.Modding.Types;
using HarmonyLib;
using System;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System.Linq;
using System.IO;
using Newtonsoft.Json.Linq;

namespace MTMTVFX.Core
{
    public class CorePlugin : GamePlugin_PostLoad
    {
        public string name { get { return "MTMT_VFXCore"; } }
        public Version version { get { return new Version(CorePlugin.ver); } }
        public static string guid = Util.GetAssetbundleGUID("bmeffects_*.assetbundle");
        public static JObject config = Util.GetConfig();
        public static string ver = "1.0.0";

        public void OnLoad()
        {
            new Harmony("MTMT_VFX_CORE").PatchAll();
            ModProblems.AddModProblem($"{name} v{ver} active!", Assembly.GetExecutingAssembly().Location, string.Empty, false);
        }

        public void OnSave() { }

        public bool AfterAllPluginsLoaded() => true;
    }
}