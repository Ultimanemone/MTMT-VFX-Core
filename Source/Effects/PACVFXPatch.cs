using BrilliantSkies.PlayerProfiles;
using HarmonyLib;
using MTMTVFX.Core;
using MTMTVFX.UI;
using UnityEngine;

namespace MTMTVFX.Effects
{
    [HarmonyPatch(typeof(ParticleCannonEffect))]
    public class PACVFXPatch
    {
        [HarmonyPatch("RenderAndRun")]
        [HarmonyPrefix]
        private static void CancelBaseBeam(ParticleCannonEffect __instance)
        {
            SettingsConfig config = ProfileManager.Instance.GetModule<SettingsConfig>();
            if (!config.E_PAC || config.IS_DEGRADED) return;

            // We keep the main obj since it is used for damage checks
            // GameObject[] children = __instance.gameObject.GetChildren();

            foreach (var child in __instance.gameObject.GetComponentsInChildren<Transform>())
            {
                //if (child.name != "SecondaryEffect") child.SetActive(false);
                child.gameObject.SetActive(false);
            }
        }

        [HarmonyPatch("ApplyDamage")]
        [HarmonyPostfix]
        private static void MakeBeam(ParticleCannonEffect __instance, Vector3[] worldPositions)
        {
            SettingsConfig config = ProfileManager.Instance.GetModule<SettingsConfig>();
            if (!config.E_PAC || config.IS_DEGRADED) return;

            if (__instance.HasHit) return;
            MainThreadDispatcher.Enqueue(() =>
            {
                GameObject obj = VFXManager.Create(BeamName.pac_beam, worldPositions[0], __instance.transform.forward);
                PacPatchMod.PacMethod(worldPositions, obj, __instance.Range0Damage, __instance.ParticleType, __instance.m_BaseColor);
            });
        }

        [HarmonyPatch("TerminateAtPoint")]
        [HarmonyPrefix]
        private static void UpdateBeam(ParticleCannonEffect __instance, LineRenderer ____lineRenderer, Vector3 gameWorldPosition, int indexOfTermination)
        {
            SettingsConfig config = ProfileManager.Instance.GetModule<SettingsConfig>();
            if (!config.E_PAC || config.IS_DEGRADED) return;

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
        /// <param name="pacBeam">The PAC beam object</param>
        public static void PacMethod(Vector3[] pointArray, GameObject pacBeam, float damage, ParticleType type, Color color) { }
    }
}
