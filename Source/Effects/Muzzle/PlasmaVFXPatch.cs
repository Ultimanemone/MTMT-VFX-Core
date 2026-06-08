using BrilliantSkies.PlayerProfiles;
using HarmonyLib;
using MTMTVFX.Core;
using MTMTVFX.UI;

namespace MTMTVFX.Effects.Muzzle
{

    [HarmonyPatch(typeof(PlasmaMuzzleEffect), "Play")]
    public class PlasmaVFXPatch
    {
        private static bool Prefix()
        {
            SettingsConfig config = Utils.GetConfig();
            if (config.E_PLASMA && !config.IS_DEGRADED) return false;
            return true;
        }
    }
}
