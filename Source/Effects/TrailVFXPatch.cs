using BrilliantSkies.Ftd.Game.Pools;
using HarmonyLib;
using UnityEngine;

namespace MTMTVFX.Effects
{
    [HarmonyPatch(typeof(AdvPooledProjectile), "ActivateHere")]
    public class APSTrailPatch
    {
        private static void Postfix(AdvPooledProjectile __instance)
        {
            TrailRenderer trail = __instance.gameObject.GetComponent<TrailRenderer>();
            if (trail != null)
            {
                Object.Destroy(trail);
            }
        }
    }

    [HarmonyPatch(typeof(CramProjectilePool), "ActivateHere")]
    public class CramTrailPatch
    {
        private static void Postfix(CramProjectilePool __instance)
        {
            TrailRenderer trail = __instance.gameObject.GetComponent<TrailRenderer>();
            if (trail != null)
            {
                Object.Destroy(trail);
            }
        }
    }

    [HarmonyPatch(typeof(PlasmaProjectilePool), "ActivateHere")]
    public class PlasmaTrailPatch
    {
        private static void Postfix(PlasmaProjectilePool __instance)
        {
            TrailRenderer trail = __instance.gameObject.GetComponent<TrailRenderer>();
            if (trail != null)
            {
                Object.Destroy(trail);
            }
        }
    }
}
