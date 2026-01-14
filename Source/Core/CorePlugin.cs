using BrilliantSkies.Modding;
using HarmonyLib;
using System;

namespace MTMTVFX.Core
{
    public class CorePlugin : GamePlugin_PostLoad
    {
        public string name { get { return ModInfo.ModName; } }
        public Version version { get { return ModInfo.Version; } }

        public void OnLoad()
        {
            Util.InitConfig();
            ModInfo.CheckVersion();
            new Harmony("MTMT_VFX_CORE").PatchAll();
        }

        public void OnSave() { }

        public bool AfterAllPluginsLoaded() => true;
    }
}