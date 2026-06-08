using BrilliantSkies.PlayerProfiles;
using HarmonyLib;
using MTMTVFX.Core;
using MTMTVFX.UI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MTMTVFX.Effects.Muzzle
{
    [HarmonyPatch(typeof(FlamerMuzzleEffect))]
    public class FlamerVFXPatch
    {
        [HarmonyPatch(MethodType.Constructor)]
        [HarmonyPatch(new Type[] { typeof(Vector3), typeof(Quaternion) })]
        [HarmonyPostfix]
        private static void CancelFlamerVFX(FlamerMuzzleEffect __instance, ParticleSystem ____bigFlame, ParticleSystem ____smallFlame, ParticleSystem ____sparks)
        {
            FlamerEmitterBase.AddEmitter(__instance, ____bigFlame, ____smallFlame, ____sparks);
        }

        [HarmonyPatch("ChangeColor")]
        [HarmonyPostfix]
        private static void RecolorFlame(FlamerMuzzleEffect __instance, Color newColor)
        {
            FlamerEmitterBase.Recolor(__instance, newColor);
        }

    }

    public static class FlamerEmitterBase
    {
        private struct Emitter
        {
            public ParticleSystem big;
            public ParticleSystem small;
            public ParticleSystem sparks;

            public readonly void Set(float bigRate, float smallRate, float sparksRate)
            {
                var bigEmission = big.emission;
                bigEmission.rateOverTime = bigRate;
                var smallEmission = small.emission;
                smallEmission.rateOverTime = smallRate;
                var sparksEmission = sparks.emission;
                sparksEmission.rateOverTime = sparksRate;
            }

            public readonly void Stop()
            {
                Set(0f, 0f, 0f);
            }
        }

        private static float baseBig;
        private static float baseSmall;
        private static float baseSparks;
        private static bool enabled;
        private static readonly Dictionary<FlamerMuzzleEffect, Emitter> emitters = new Dictionary<FlamerMuzzleEffect, Emitter>();

        public static void AddEmitter(FlamerMuzzleEffect instance, ParticleSystem big, ParticleSystem small, ParticleSystem sparks)
        {
            SettingsConfig config = Utils.GetConfig();

            if (emitters.Count == 0)
            {
                baseBig = big.emission.rateOverTime.constant;
                baseSmall = small.emission.rateOverTime.constant;
                baseSparks = sparks.emission.rateOverTime.constant;
                enabled = config.E_FLAMER;
            }

            Emitter emitter = new Emitter { big = big, small = small, sparks = sparks };
            if (config.E_FLAMER)
            {
                emitter.Stop();
            }

            emitters.Add(instance, emitter);
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
                emitter.Value.Set(bigRate, smallRate, sparksRate);
            }
        }

        public static void Recolor(FlamerMuzzleEffect _instance, Color newColor)
        {

        }
    }
}