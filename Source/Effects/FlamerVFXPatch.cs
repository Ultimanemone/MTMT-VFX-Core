using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MTMTVFX.Effects
{
    [HarmonyPatch(typeof(FlamerMuzzleEffect))]
    [HarmonyPatch(MethodType.Constructor)]
    [HarmonyPatch(new Type[] { typeof(Vector3), typeof(Quaternion) })]
    public class FlamerVFXPatch
    {
        [HarmonyPostfix]
        private static void CancelFlamerVFX(ParticleSystem.EmissionModule ____bigFlameEmission, ParticleSystem.EmissionModule ____smallFlameEmission, ParticleSystem.EmissionModule ____sparksEmission)
        {
            ____bigFlameEmission.rateOverTime = 0f;
            ____smallFlameEmission.rateOverTime = 0f;
            ____sparksEmission.rateOverTime = 0f;
        }
    }
}