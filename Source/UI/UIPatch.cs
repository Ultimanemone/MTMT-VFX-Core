using BrilliantSkies.PlayerProfiles;
using BrilliantSkies.Ui.Consoles;
using BrilliantSkies.Ui.Consoles.Examples;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTMTVFX.UI
{
    [HarmonyPatch(typeof(OptionsMenuUi), "BuildInterface")]
    public class UIPatch
    {
        private static void Postfix(ref ConsoleWindow __result)
        {
            __result.AllScreens.Add(new SettingsTab(__result, ProfileManager.Instance.GetModule<SettingsConfig>()));
        }
    }
}
