using BrilliantSkies.Blocks.Lasers.Weaponry;
using BrilliantSkies.Core.Pooling;
using BrilliantSkies.Effects.Pools.Lasers;
using HarmonyLib;
using MTMTVFX.Core;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace MTMTVFX.Effects
{
    [HarmonyPatch(typeof(ConventionalLaser), "EnableOrDisableEffects")]
    public class LaserVFXPatchContinuous
    {
        private static bool Prefix(ConventionalLaser  __instance, bool newState)
        {
            if (!Core.Util.E_CONTINUOUS) return true;

            if (newState) return false;
            return true;
        }
    }

    [HarmonyPatch(typeof(ShortRangeLaser), "DrawBeam")]
    public class LaserVFXPatchContinuous2
    {
        private static void Prefix(ShortRangeLaser __instance, Vector3 exitPoint, Vector3 direction, Vector3 hitPoint, Color ____continuousColor)
        {
            if (!Core.Util.E_CONTINUOUS) return;

            LaserPatchMod.PatchContLaser(__instance, exitPoint, direction, hitPoint, ____continuousColor);
        }
    }

    [HarmonyPatch(typeof(LaserCombiner), "DrawBeam")]
    public class LaserVFXPatchContinuous3
    {
        private static void Prefix(LaserCombiner __instance, Vector3 exitPoint, Vector3 direction, Vector3 hitPoint, Color ____continuousColor)
        {
            if (!Core.Util.E_CONTINUOUS) return;

            LaserPatchMod.PatchContLaser(__instance, exitPoint, direction, hitPoint, ____continuousColor);
        }
    }

    [HarmonyPatch(typeof(ConventionalLaser), "WeaponStart")]
    public class LaserVFXPatchContinuous4
    {
        private static void Postfix(ConventionalLaser __instance)
        {
            if (!Core.Util.E_CONTINUOUS) return;

            MainThreadDispatcher.Enqueue(() =>
            {
                GameObject obj = VFXManager.InstantiateCopy<SpecialName>(SpecialName.laser_cont.ToString(), Vector3.zero, Vector3.zero);
                LaserPatchMod.laserBeams.Add(__instance, obj);
            });
        }
    }

    [HarmonyPatch(typeof(ConventionalLaser), "FixedUpdate_Fire")]
    public class LaserVFXPatchPulse2
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            if (!Core.Util.E_PULSE) return instructions;

            var codes = new List<CodeInstruction>(instructions);

            var emitMethod = AccessTools.Method(
                typeof(ParticleSystem),
                "Emit",
                new[] { typeof(ParticleSystem.EmitParams), typeof(int) }
            );

            for (int i = 0; i < codes.Count; i++)
            {
                // Match the Emit call
                if (codes[i].Calls(emitMethod))
                {
                    // Remove:
                    //  ldarg.0
                    //  ldfld pulseFirePS
                    //  ldloc.s V_26
                    //  ldc.i4.1
                    //  callvirt Emit
                    codes.RemoveRange(i - 4, 5);
                    i -= 5;
                }
            }

            return codes;
        }
    }

    [HarmonyPatch(typeof(LaserPulsePool), "ActivateHere")]
    public class LaserVFXPatchPulse
    {
        private static bool Prefix(LaserPulsePool __instance, LaserPulseSpecification spec, ref LaserPulseRender __result, PoolIndex ___Indexor)
        {
            if (!Core.Util.E_PULSE) return true;

            __result = __instance.PoolArray[___Indexor.CycleIndex()];
            MainThreadDispatcher.Enqueue(() =>
            {
                GameObject obj = VFXManager.Create<BeamName>(BeamName.laser_pulse.ToString(), spec.StartPosition, spec.StartPosition - spec.EndPosition);
                LaserPatchMod.PulseMethod(spec, obj);
            });
            return false;
        }
    }

    /// <summary>
    /// Helper class for patching lasers
    /// </summary>
    public class LaserPatchMod
    {
        public static Dictionary<ConventionalLaser, GameObject> laserBeams = new Dictionary<ConventionalLaser, GameObject>();

        public static void PulseMethod(LaserPulseSpecification spec, GameObject obj)
        {
            // dummy method, patch this to grab the parameters
        }

        public static void ContMethod(Vector3 start, Vector3 end, Vector3 direction, float width, Color color, GameObject obj)
        {
            // dummy method, patch this to grab the parameters
        }

        public static void PatchContLaser(ConventionalLaser __instance, Vector3 exitPoint, Vector3 direction, Vector3 hitPoint, Color ____continuousColor)
        {
            try
            {
                if (__instance != null)
                {
                    MethodInfo getWidth = AccessTools.Method(typeof(ConventionalLaser), "GetWidth");
                    if (getWidth != null)
                    {
                        float width = (float)getWidth.Invoke(__instance, new object[] { });
                        GameObject beam = laserBeams[__instance];
                        if (beam != null)
                        {
                            ContMethod(exitPoint, hitPoint, direction, width, ____continuousColor, beam);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Core.Util.LogError<LaserPatchMod>(e.Message);
            }
        }

        public static void UpdateContBeams()
        {
            if (laserBeams.Count < 1) { return; }
            List<ConventionalLaser> keysToRemove = new List<ConventionalLaser>();

            foreach (KeyValuePair<ConventionalLaser, GameObject> kvp in laserBeams)
            {
                if (kvp.Key == null || kvp.Key.IsDeleted)
                {
                    if (kvp.Value != null) UnityEngine.Object.Destroy(kvp.Value);

                    keysToRemove.Add(kvp.Key);
                }
            }

            foreach (var key in keysToRemove)
            {
                laserBeams.Remove(key);
            }
        }
    }
}

