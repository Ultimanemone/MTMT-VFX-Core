using BrilliantSkies.Ftd.Game.Pools;
using HarmonyLib;
using UnityEngine;

namespace MTMTVFX.Projectiles
{
    [HarmonyPatch(typeof(PooledCramProjectile), "ActivateHere")]
    public class CramTrailPatch
    {
        private static void Postfix(PooledCramProjectile __instance)
        {
            TrailRenderer trail = __instance.gameObject.GetComponent<TrailRenderer>();
            if (trail != null)
            {
                trail.gameObject.SetActive(false);
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
                trail.gameObject.SetActive(false);
                //trail.startColor = Color.clear;
                //trail.endColor = Color.clear;
            }
        }
    }
}