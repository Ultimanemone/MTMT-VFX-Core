using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;
using MTMTVFX.Core;
using BrilliantSkies.GridCasts;

namespace MTMTVFX.Effects
{
    [HarmonyPatch(typeof(ParticleCannonEffect), "RenderAndRun")]
    public class PACVFXPatch
    {
        private static void Prefix(ParticleCannonEffect __instance)
        {
            if (!Core.Util.E_PAC) return;

            // We keep the main obj since it is used for damage checks
            GameObject[] children = __instance.gameObject.GetChildren();

            foreach (GameObject child in children)
            {
                //if (child.name != "SecondaryEffect") child.SetActive(false);
                child.SetActive(false);
            }
        }
    }

    [HarmonyPatch(typeof(ParticleCannonEffect), "ApplyDamage")]
    public class PACVFXPatch2
    {
        private static void Postfix(ParticleCannonEffect __instance, Vector3[] worldPositions)
        {
            if (__instance.HasHit) return;
            MainThreadDispatcher.Enqueue(() =>
            {
                GameObject obj = VFXManager.Create(BeamName.pac_beam, worldPositions[0], __instance.transform.forward);
                PacPatchMod.PacMethod(worldPositions, obj, __instance.Range0Damage, __instance.ParticleType, __instance.m_BaseColor);
            });
        }
    }

    [HarmonyPatch(typeof(ParticleCannonEffect), "TerminateAtPoint")]
    public class PACVFXPatch3
    {
        private static void Prefix(ParticleCannonEffect __instance, LineRenderer ____lineRenderer, Vector3 gameWorldPosition, int indexOfTermination)
        {
            Vector3[] worldPositions = new Vector3[indexOfTermination];

            for (int i = 0; i < indexOfTermination - 1; i++)
            {
                Vector3 localPos = ____lineRenderer.GetPosition(i);
                worldPositions[i] = ____lineRenderer.transform.TransformPoint(localPos);
            }
            worldPositions[indexOfTermination - 1] = gameWorldPosition;

            MainThreadDispatcher.Enqueue(() =>
            {
                GameObject obj = VFXManager.Create(BeamName.pac_beam, worldPositions[0], __instance.transform.forward);
                PacPatchMod.PacMethod(worldPositions, obj, __instance.Range0Damage, __instance.ParticleType, __instance.m_BaseColor);
            });
        }
    }

    public class PacPatchMod
    {
        /// <summary>
        /// Dummy method, patch this to get coordinates array <paramref name="pointArray"/> of the PAC beam 
        /// </summary>
        /// <param name="pointArray">This is the array of every point on the PAC beam</param>
        public static void PacMethod(Vector3[] pointArray, GameObject pacBeam, float damage, ParticleType type, Color color) { }
    }
}
