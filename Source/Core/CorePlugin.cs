using BrilliantSkies.Core.Timing;
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
            ModInfo.CheckVersion();
            new Harmony("MTMT_VFX_CORE").PatchAll();
            GameEvents.ProfileChange.RegWithEvent(OnStart);
        }

        public void OnStart()
        {
            GameEvents.ProfileChange.UnregWithEvent(OnStart);
            VFXManager.Init();
        }

        public void OnSave() { }

        public bool AfterAllPluginsLoaded() => true;
    }
}