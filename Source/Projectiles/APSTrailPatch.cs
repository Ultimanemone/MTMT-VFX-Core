using BrilliantSkies.Ftd.Game.Pools;
using BrilliantSkies.PlayerProfiles;
using HarmonyLib;
using MTMTVFX.Core;
using MTMTVFX.Internal;
using MTMTVFX.UI;
using System.Collections;
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
            Utils.LogInfo<APSTrailPatch>("overriding trail");
            SettingsConfig config = ProfileManager.Instance.GetModule<SettingsConfig>();
            if (config.E_APS_TRAIL)
            {
                MainThreadDispatcher.Enqueue(() =>
                {
                    __instance.gameObject.GetComponent<TrailRenderer>().emitting = false;
                });
            }
            else return;
        }

        [HarmonyPatch("Deactivate")]
        [HarmonyPostfix]
        private static void CloneTrail(AdvPooledProjectile __instance)
        {
            MainThreadDispatcher.Enqueue(() =>
            {
                Utils.LogInfo<APSTrailPatch>("cloning trail");

                var tr = __instance.GetComponent<TrailRenderer>();
                if (tr == null) return;

                GameObject cloneObj = VFXManager.Create(Trail.aps, Vector3.zero, Vector3.zero);
                TrailRenderer clone = cloneObj.GetComponent<TrailRenderer>();

                // Basic properties
                clone.time = tr.time;
                clone.widthMultiplier = tr.widthMultiplier;
                clone.startWidth = tr.startWidth;
                clone.endWidth = tr.endWidth;

                // create a new material instance instead of sharing
                var sourceMat = tr.material;
                var newMat = new Material(sourceMat);

                // Copy color safely using actual shader properties
                if (sourceMat.HasProperty("_TintColor"))
                {
                    newMat.SetColor("_TintColor", sourceMat.GetColor("_TintColor"));
                }
                else if (sourceMat.HasProperty("_Color"))
                {
                    newMat.SetColor("_Color", sourceMat.GetColor("_Color"));
                }
                else if (sourceMat.HasProperty("_BaseColor"))
                {
                    newMat.SetColor("_BaseColor", sourceMat.GetColor("_BaseColor"));
                }

                clone.material = newMat;

                //clone.colorGradient = tr.colorGradient;

                // Copy positions
                int count = tr.positionCount;
                if (count > 0)
                {
                    var positions = new Vector3[count];
                    tr.GetPositions(positions);
                    clone.SetPositions(positions);
                }

                clone.emitting = false;
                cloneObj.GetComponent<EffectManualKill>().Init(clone.time, VFXManager.apsDefaultTrailPool);
            });
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
}