using BrilliantSkies.Ftd.Game.Pools;
using BrilliantSkies.PlayerProfiles;
using HarmonyLib;
using MTMTVFX.Core;
using MTMTVFX.UI;
using UnityEngine;

namespace MTMTVFX.Projectiles
{
    [HarmonyPatch(typeof(AdvPooledProjectile))]
    public class APSTrailPatch
    {
        [HarmonyPatch("ActivateHere")]
        [HarmonyPostfix]
        private static void Override(AdvPooledProjectile __instance)
        {
            TrailRenderer trail = __instance.gameObject.GetComponent<TrailRenderer>();
            SettingsConfig config = ProfileManager.Instance.GetModule<SettingsConfig>();
            if (config.E_APS_TRAIL) return;
            else trail.emitting = false;
        }

        [HarmonyPatch("Deactivate")]
        [HarmonyPostfix]
        private static void CloneTrail(AdvPooledProjectile __instance)
        {
            TrailRenderer tr = __instance.GetComponent<TrailRenderer>();
            TrailRenderer clone = VFXManager.Create(Trail.aps, Vector3.zero, Vector3.zero).GetComponent<TrailRenderer>();
            clone.time = tr.time;
            clone.startWidth = tr.startWidth;
            clone.endWidth = tr.endWidth;
            clone.material = tr.material;
            clone.colorGradient = tr.colorGradient;

            var positions = new Vector3[tr.positionCount];
            tr.GetPositions(positions);
            //clone.positionCount = positions.Length;
            clone.SetPositions(positions);

            clone.emitting = false;

            Object.Destroy(clone.gameObject, clone.time);
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