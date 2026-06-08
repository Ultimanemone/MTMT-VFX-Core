using BrilliantSkies.PlayerProfiles;
using BrilliantSkies.Ui.Consoles;
using BrilliantSkies.Ui.Consoles.Examples;
using HarmonyLib;
using MTMTVFX.Core;
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
            __result.AllScreens.Add(new SettingsTab(__result, Utils.GetConfig()));
        }
    }
}
