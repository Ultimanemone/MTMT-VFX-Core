using MTMTVFX.Core;
using BrilliantSkies.Core;
using BrilliantSkies.Effects.GunSounds;
using BrilliantSkies.Effects.SoundSystem;
using System;
using HarmonyLib;
using UnityEngine;
using BrilliantSkies.Core.Widgets;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;


namespace MTMTVFX.Effects.Muzzle
{
    [HarmonyPatch(typeof(AdvCannonFiringPiece), "Vent")]
    public class APSVFXRemoveVent
    {
        private static bool Prefix(AdvCannonFiringPiece __instance)
        {
            Util.LogInfo<APSVFXRemoveVent>("effect cancelled");
            return false;
        }
    }


    [HarmonyPatch(typeof(AdvCannonFiringPiece), "LateVisuals")]
    public class APSVFXRemove
    {
        private static bool Prefix(AdvCannonFiringPiece __instance, ref ParticleSystem ____particleSmoke, ref ITicker ____barrelColorTicker)
        {
            //Util.LogInfo<APSVFXRemove>("cancelling vfx");
            //var field = AccessTools.Field(typeof(AdvCannonFiringPiece), "_particleSmoke");
            //ParticleSystem _particleSmoke = (ParticleSystem)field.GetValue(__instance);
            //var field1 = AccessTools.Field(typeof(AdvCannonFiringPiece), "_barrelColorTicker");
            //ITicker _barrelColorTicker = (ITicker)field1.GetValue(__instance);

            var _particleSmoke = ____particleSmoke;
            var _barrelColorTicker = ____barrelColorTicker;

            ///////////////////////////////////////// BASE CODE
            bool flag = _particleSmoke != null && _particleSmoke.isPlaying;
            if (flag)
            {
                bool flag2 = __instance.SmokePlayingTime.Since > 2f;
                if (flag2)
                {
                    _particleSmoke.Stop();
                }
            }
            bool flag3 = _barrelColorTicker.CheckAndReset();
            if (flag3)
            {
                ICannonBarrelSystem barrelSystem = __instance.BarrelSystem;
                if (barrelSystem != null)
                {
                    barrelSystem.CheckBarrelColors();
                }
            }
            ICannonBarrelSystem barrelSystem2 = __instance.BarrelSystem;
            if (barrelSystem2 != null)
            {
                barrelSystem2.RedrawBarrels();
            }
            bool flag4 = __instance.GetFireDirection() != Vector3.zero;
            if (flag4)
            {
                __instance.BarrelSystem.AimDirection = __instance.GetFireDirection();
            }
            else
            {
                __instance.BarrelSystem.AimDirection = __instance.GameWorldForwards;
            }
            __instance.BarrelSystem.LateUpdate(__instance.GameWorldUp);

            Util.LogInfo<APSVFXRemove>("cancelling vfx");

            // this is the base vfx disabled
            //bool flag5 = __instance.particleMagnetGlow != null;
            //if (flag5)
            //{
            //    bool flag6 = __instance.Node.RailgunCharging > 0f && __instance.Node.nRailgunFixtures > 0;
            //    if (flag6)
            //    {
            //        ParticleSystem.EmissionModule emission = __instance.particleMagnetGlow.emission;
            //        ParticleSystem.MainModule main = __instance.particleMagnetGlow.main;
            //        emission.enabled = true;
            //        main.startSpeed = (__instance.Node.RailgunCapacity / (float)__instance.Node.nRailgunFixtures + 0.25f) / 2f;
            //        main.startSize = 2f * __instance.BarrelSystem.GetMultiBarrelModifiedDiameter();
            //    }
            //    else
            //    {
            //        ParticleSystem.EmissionModule e = __instance.particleMagnetGlow.emission;
            //        e.enabled = false;
            //    }
            //}
            ///////////////////////////////////////// BASE CODE
            

            return false;
        }
    }

    [HarmonyPatch(typeof(AdvCannonFiringPiece), "WeaponFire")]
    public class APSVFXOverride
    {
        private class State
        {
            public FiredMunitionReturn FMR;
            public ShellModel shell;
        }

        private static void Prefix(AdvCannonFiringPiece __instance, FiredMunitionReturn FMR, out State __state)
        {
            __state = new State
            {
                FMR = FMR,
                shell = __instance.Node.ShellRacks.PeekNextShell()
            };
        }

        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> Transpile(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);

            foreach (var line in codes)
            {
                if (line.opcode == OpCodes.Callvirt && line.operand is MethodInfo method && method.Name == "get_NormalisedVolumeOfPropellant")
                {
                    yield return new CodeInstruction(OpCodes.Ldc_R4, 0f);
                }
                else
                {
                    yield return line;
                }
            }
        }

        private static void Postfix(AdvCannonFiringPiece __instance, State __state)
        {
            try
            {
                bool fired = (bool)AccessTools.Field(typeof(FiredMunitionReturn), "_fired").GetValue(__state.FMR);
                if (!fired) { return; }

                ShellModel nextShell = __state.shell;
                int gpCount = 0;
                int rcCount = 0;
                float num3 = 0f;
                if (nextShell == null) return;
                foreach (ShellModule shellModule in nextShell.PartsAndMesh.AllParts)
                {
                    if (gpCount > 0 && rcCount > 0) break;
                    if (shellModule.Name == "Gunpowder casing")
                    {
                        gpCount++;
                    }
                    else if (shellModule.Name == "Railgun casing")
                    {
                        rcCount++;
                    }
                }
                BM_MuzzleFlashName type = BM_MuzzleFlashName.none;
                float radius;
                float gauge = __instance.BarrelSystem.ShellDiameter;
                if (gauge < 0.0601f)
                {
                    type = BM_MuzzleFlashName.muzzleflashtinytiny;
                    radius = 0f;
                }
                else if (gauge < 0.12f)
                {
                    type = BM_MuzzleFlashName.muzzleflashtiny;
                    radius = 5f;
                    num3 = 2f;
                }
                else if (gauge < 0.236f)
                {
                    type = BM_MuzzleFlashName.muzzleflashsmall;
                    radius = 10f;
                    num3 = 4f;
                }
                else if (gauge < 0.348f)
                {
                    type = BM_MuzzleFlashName.muzzleflashmedium;
                    radius = 15f;
                    num3 = 7f;
                }
                else if (gauge < 0.402f)
                {
                    type = BM_MuzzleFlashName.muzzleflashlarge;
                    radius = 15f;
                    num3 = 10f;
                }
                else
                {
                    type = BM_MuzzleFlashName.muzzleflashlargest;
                    radius = 20f;
                    num3 = 15f;
                }

                if (gpCount > 0 && type != BM_MuzzleFlashName.none)
                {
                    //GameObject gameObject = VFXManager.Instance.Create(type.ToString(), __instance.GetFirePoint(0f), __instance.GetFireDirection());
                }

                var RailgunDraw = AccessTools.Method(typeof(AdvCannonFiringPiece), "RailgunDraw");
                bool flag1 = (float)RailgunDraw.Invoke(__instance, new object[] { null }) > 0f;
                if (flag1)
                {
                    BM_MuzzleFlashName type2 = BM_MuzzleFlashName.muzzleflashrail_small;
                    if (rcCount > 0)
                    {
                        if (gauge < 0.12)
                        {
                            type2 = BM_MuzzleFlashName.muzzleflashrail_small;
                        }
                        else if (gauge < 0.38)
                        {
                            type2 = BM_MuzzleFlashName.muzzleflashrail_medium;
                        }
                        else
                        {
                            type2 = BM_MuzzleFlashName.muzzleflashrail_big;
                        }
                    }
                    //GameObject gameObject2 = VFXManager.Instance.Create(type2.ToString(), __instance.GetFirePoint(0f), __instance.GetFireDirection());
                }
                //Vector3 vector = __instance.GetFirePoint(0f);
                //vector += __instance.GetFireDirection() * num3;
                //VFXManager.Instance.CreateImpactSplash(vector, radius);
            }
            catch
            {
            }

            ///////////////////////////////////////// BASE CODE
            //GunSoundSystem.PlaySound(__instance.GameWorldPosition, __instance.BarrelSystem.ShellDiameter, ____firingSoundRegulator, localSource);
            //bool isClient = Net.IsClient;
            //if (isClient)
            //{
            //    ShellModel nextShell = __instance.Node.ShellRacks.GetNextShell(true);
            //    CannonBarrelSystem nextBarrelReady = __instance.BarrelSystem.GetNextBarrelReady();
            //    bool flag2 = nextShell != null;
            //    if (flag2)
            //    {
            //        __instance.BarrelSystem.Fire(nextShell.Propellant.CooldownTime, nextShell.Propellant.NormalisedVolumeOfPropellant, nextBarrelReady, __instance.Data.DisableBarrelReciprocation);
            //    }
            //    else
            //    {
            //        __instance.BarrelSystem.Fire(20f, ShellConstants.ModuleVolume(__instance.BarrelSystem.ShellDiameter) * 4f, nextBarrelReady, __instance.Data.DisableBarrelReciprocation);
            //    }
            //}
            ///////////////////////////////////////// BASE CODE
        }
    }

    [HarmonyPatch(typeof(AdvCannonFiringPiece), "Flash")]
    public class APSFlashRemove
    {
        private static bool Prefix(AdvCannonFiringPiece __instance, bool localSource, ref SoundEventRegulator ____firingSoundRegulator)
        {
            GunSoundSystem.PlaySound(__instance.GameWorldPosition, __instance.BarrelSystem.ShellDiameter, ____firingSoundRegulator, localSource);
            //bool flag = this.RailgunDraw(null) > 0f;
            //if (flag)
            //{
            //    this.particleRailgunFire.main.startSize = 3f * this.BarrelSystem.GetMultiBarrelModifiedDiameter();
            //    this.particleRailgunFire.Emit(3);
            //}
            bool isClient = Net.IsClient;
            if (isClient)
            {
                ShellModel nextShell = __instance.Node.ShellRacks.GetNextShell(true);
                CannonBarrelSystem nextBarrelReady = __instance.BarrelSystem.GetNextBarrelReady();
                bool flag2 = nextShell != null;
                //if (flag2)
                //{
                //    __instance.BarrelSystem.Fire(nextShell.Propellant.CooldownTime, 0f, nextBarrelReady, __instance.Data.DisableBarrelReciprocation);
                //}
                //else
                //{
                //    __instance.BarrelSystem.Fire(20f, 0f, nextBarrelReady, __instance.Data.DisableBarrelReciprocation);
                //}
            }

            return false;
        }
    }
}