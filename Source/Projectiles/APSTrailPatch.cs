using BrilliantSkies.Ftd.Game.Pools;
using BrilliantSkies.PlayerProfiles;
using HarmonyLib;
using MTMTVFX.Core;
using MTMTVFX.Internal;
using MTMTVFX.UI;
using System;
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
                var tr = __instance.GetComponent<TrailRenderer>();
                if (tr == null) return;

                // Capture BEFORE it gets cleared
                int count = tr.positionCount;
                if (count == 0) return;

                var positions = new Vector3[count];
                tr.GetPositions(positions);
                Array.Reverse(positions);
                var mat = tr.material;

                float time = tr.time;

                Utils.LogInfo<APSTrailPatch>("cloning trail");

                // Create a new GameObject with a LineRenderer instead of TrailRenderer
                GameObject cloneObj = VFXManager.Create(Trail.aps, Vector3.zero, Vector3.zero);
                LineRenderer lineRenderer = cloneObj.GetComponent<LineRenderer>();

                // Copy properties to LineRenderer
                lineRenderer.widthCurve = tr.widthCurve;
                lineRenderer.widthMultiplier = tr.widthMultiplier;
                lineRenderer.startWidth = tr.startWidth;
                lineRenderer.endWidth = tr.endWidth;
                lineRenderer.textureMode = (UnityEngine.LineTextureMode)tr.textureMode;
                lineRenderer.numCapVertices = tr.numCapVertices;
                lineRenderer.numCornerVertices = tr.numCornerVertices;
                lineRenderer.material = tr.material;
                //lineRenderer.colorGradient = tr.colorGradient;

                // Set positions
                lineRenderer.positionCount = count;
                lineRenderer.SetPositions(positions);

                // Add the EffectManualKill component to return it to the pool
                cloneObj.GetComponent<TrailCloneFadeout>()?.Init(time, VFXManager.apsDefaultTrailPool);
            });
        }
    }
}