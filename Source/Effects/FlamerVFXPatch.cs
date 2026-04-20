using BrilliantSkies.PlayerProfiles;
using HarmonyLib;
using MTMTVFX.UI;
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
            FlamerEmitterBase.AddEmitter(____bigFlameEmission, ____smallFlameEmission, ____sparksEmission);
        }
    }

    public static class FlamerEmitterBase
    {
        private struct Emitter
        {
            public ParticleSystem.EmissionModule big;
            public ParticleSystem.EmissionModule small;
            public ParticleSystem.EmissionModule sparks;
        }

        private static float baseBig;
        private static float baseSmall;
        private static float baseSparks;
        private static bool enabled;
        private static List<Emitter> emitters = new List<Emitter>();

        public static void AddEmitter(ParticleSystem.EmissionModule big, ParticleSystem.EmissionModule small, ParticleSystem.EmissionModule sparks)
        {
            SettingsConfig config = ProfileManager.Instance.GetModule<SettingsConfig>();

            if (emitters.Count == 0)
            {
                baseBig = big.rateOverTime.constant;
                baseSmall = small.rateOverTime.constant;
                baseSparks = sparks.rateOverTime.constant;
                enabled = config.E_FLAMER;
            }

            if (!config.E_FLAMER)
            {
                big.rateOverTime = 0f;
                small.rateOverTime = 0f;
                sparks.rateOverTime = 0f;
            }
            emitters.Add(new Emitter { big = big, small = small, sparks = sparks });
        }

        public static void ToggleVFX(bool enabled)
        {
            if (enabled == FlamerEmitterBase.enabled) return;
            FlamerEmitterBase.enabled = enabled;

            float bigRate = enabled ? 0f : baseBig;
            float smallRate = enabled ? 0f : baseSmall;
            float sparksRate = enabled ? 0f : baseSparks;

            foreach (var emitter in emitters)
            {
                var bigEmission = emitter.big;
                bigEmission.rateOverTime = bigRate;
                var smallEmission = emitter.small;
                smallEmission.rateOverTime = smallRate;
                var sparksEmission = emitter.sparks;
                sparksEmission.rateOverTime = sparksRate;
            }
        }
    }
}