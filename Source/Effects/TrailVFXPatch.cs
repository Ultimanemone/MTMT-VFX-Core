using BrilliantSkies.Ftd.Game.Pools;
using HarmonyLib;
using UnityEngine;

#if false
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

    [HarmonyPatch(typeof(PooledCramProjectile), "ActivateHere")]
    public class CramTrailPatch
    {
        private static void Postfix(PooledCramProjectile __instance)
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
        private static void Postfix(ref PooledPlasmaProjectile __result)
        {
            // FtD plasma uses a LineRenderer instead of a trail...
            LineRenderer trail = __result.gameObject.GetComponentInChildren<LineRenderer>();
            if (trail != null)
            {
                Object.Destroy(trail);
                //trail.startColor = Color.clear;
                //trail.endColor = Color.clear;
            }
        }
    }
}
#endif