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
using BrilliantSkies.Blocks.Decorative.Display;


namespace MTMTVFX.Effects.Muzzle
{
    // Code for gun venting heat after shots
    //[HarmonyPatch(typeof(AdvCannonFiringPiece), "Vent")]
    public class APSVFXRemoveVent
    {
        private static bool Prefix(AdvCannonFiringPiece __instance)
        {
            return false;
        }
    }

    // Not sure if its needed but visually its not needed
    //[HarmonyPatch(typeof(AdvCannonFiringPiece), "LateVisuals")]
    public class APSVFXRemove
    {
        private static bool Prefix(AdvCannonFiringPiece __instance, ref ParticleSystem ____particleSmoke, ref ITicker ____barrelColorTicker)
        {
            ///////////////////////////////////////// BASE CODE
            bool flag = ____particleSmoke != null && ____particleSmoke.isPlaying;
            if (flag)
            {
                bool flag2 = __instance.SmokePlayingTime.Since > 2f;
                if (flag2)
                {
                    ____particleSmoke.Stop();
                }
            }
            bool flag3 = ____barrelColorTicker.CheckAndReset();
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
            ///////////////////////////////////////// BASE CODE
            
            return false;
        }
    }

    // Patch manual firing
    [HarmonyPatch(typeof(AdvCannonFiringPiece), "WeaponFire")]
    public class APSVFXOverride
    {
        private class State
        {
            public FiredMunitionReturn FMR;
            public ShellModel shell;
        }

        // Grab shell data before firing
        private static void Prefix(AdvCannonFiringPiece __instance, FiredMunitionReturn FMR, out State __state)
        {
            __state = new State
            {
                FMR = FMR,
                shell = __instance.Node.ShellRacks.PeekNextShell()
            };
        }

        // Stops the gun from playing the game's VFX
        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> Transpile(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);

            foreach (CodeInstruction code in codes)
            {
                if (code.opcode == OpCodes.Callvirt &&
                    code.operand is MethodInfo method &&
                    method.Name == "get_NormalisedVolumeOfPropellant")
                {
                    yield return new CodeInstruction(OpCodes.Pop);
                    yield return new CodeInstruction(OpCodes.Ldc_R4, 0f);
                }
                else
                {
                    yield return code;
                }
            }
        }

        // Check if shell was fired, since the method still runs if it didn't
        // Render our VFX if it did
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

                float gauge = __instance.BarrelSystem.ShellDiameter;
                MuzzleFlashName type = Enums.GetMuzzleEnum(gauge);
                float radius;

                if (gpCount > 0 && type != MuzzleFlashName.none)
                {
                    GameObject gameObject = VFXManager.Instance.Create(type.ToString(), __instance.GetFirePoint(0f), __instance.GetFireDirection());
                }

                MethodInfo RailgunDraw = AccessTools.Method(typeof(AdvCannonFiringPiece), "RailgunDraw");
                bool flag1 = (float)RailgunDraw.Invoke(__instance, new object[] { null }) > 0f;
                if (flag1)
                {
                    MuzzleFlashName type2 = MuzzleFlashName.muzzleflashrail_small;
                    if (rcCount > 0)
                    {
                        if (gauge < 0.12)
                        {
                            type2 = MuzzleFlashName.muzzleflashrail_small;
                        }
                        else if (gauge < 0.38)
                        {
                            type2 = MuzzleFlashName.muzzleflashrail_medium;
                        }
                        else
                        {
                            type2 = MuzzleFlashName.muzzleflashrail_big;
                        }
                    }
                    GameObject gameObject2 = VFXManager.Instance.Create(type2.ToString(), __instance.GetFirePoint(0f), __instance.GetFireDirection());
                }
                //Vector3 vector = __instance.GetFirePoint(0f);
                //vector += __instance.GetFireDirection() * num3;
                //VFXManager.Instance.CreateImpactSplash(vector, radius);
            }
            catch
            {
            }
        }
    }

    [HarmonyPatch(typeof(AdvCannonFiringPiece), "Flash", new Type[] { typeof(bool) })]
    internal class BM_AdvExtend
    {
        private static bool Prefix(AdvCannonFiringPiece __instance, bool localSource, ref SoundEventRegulator ____firingSoundRegulator)
        {
            try
            {
                int num = 0;
                int num2 = 0;
                float num3 = 0f;
                ShellModel nextShell = __instance.Node.ShellRacks.GetNextShell(false);
                foreach (ShellModule shellModule in nextShell.PartsAndMesh.AllParts)
                {
                    if (shellModule.Name == "Gunpowder casing")
                    {
                        num++;
                    }
                    if (shellModule.Name == "Railgun casing")
                    {
                        num2++;
                    }
                }
                
                float radius;
                float gauge = __instance.BarrelSystem.ShellDiameter;
                MuzzleFlashName type = Enums.GetMuzzleEnum(gauge);
                Util.LogInfo<APSVFXOverride>($"shell fire: {gauge} with {type.ToString()}");

                if (num > 0)
                {
                    // GameObject gameObject = VFXManager.Instance.Create(type.ToString(), __instance.GetFirePoint(0f), __instance.GetFireDirection());
                }

                var RailgunDraw = AccessTools.Method(typeof(AdvCannonFiringPiece), "RailgunDraw");
                bool flag = (float)RailgunDraw.Invoke(__instance, new object[] { null }) > 0f;
                if (flag)
                {
                    MuzzleFlashName type2 = MuzzleFlashName.muzzleflashrail_small;
                    if (num2 > 0)
                    {
                        if (__instance.BarrelSystem.ShellDiameter < 0.12)
                        {
                            type2 = MuzzleFlashName.muzzleflashrail_small;
                        }
                        else if (__instance.BarrelSystem.ShellDiameter < 0.38)
                        {
                            type2 = MuzzleFlashName.muzzleflashrail_medium;
                        }
                        else
                        {
                            type2 = MuzzleFlashName.muzzleflashrail_big;
                        }
                    }
                    // GameObject gameObject2 = VFXManager.Instance.Create(type2.ToString(), __instance.GetFirePoint(0f), __instance.GetFireDirection());
                }
                //Vector3 vector = __instance.GetFirePoint(0f);
                //vector += __instance.GetFireDirection() * num3;
                //VFXManager.Instance.CreateImpactSplash(vector, radius);
            }
            catch
            {
            }

            /////////////////////////////////////// BASE CODE
            GunSoundSystem.PlaySound(__instance.GameWorldPosition, __instance.BarrelSystem.ShellDiameter, ____firingSoundRegulator, localSource);
            bool isClient = Net.IsClient;
            if (isClient)
            {
                ShellModel nextShell = __instance.Node.ShellRacks.GetNextShell(true);
                CannonBarrelSystem nextBarrelReady = __instance.BarrelSystem.GetNextBarrelReady();
                bool flag2 = nextShell != null;
                if (flag2)
                {
                    // __instance.BarrelSystem.Fire(nextShell.Propellant.CooldownTime, nextShell.Propellant.NormalisedVolumeOfPropellant, nextBarrelReady, __instance.Data.DisableBarrelReciprocation);
                    __instance.BarrelSystem.Fire(nextShell.Propellant.CooldownTime, 0f, nextBarrelReady, __instance.Data.DisableBarrelReciprocation);
                }
                else
                {
                    __instance.BarrelSystem.Fire(20f, 0f, nextBarrelReady, __instance.Data.DisableBarrelReciprocation);
                    // __instance.BarrelSystem.Fire(20f, ShellConstants.ModuleVolume(__instance.BarrelSystem.ShellDiameter) * 4f, nextBarrelReady, __instance.Data.DisableBarrelReciprocation);
                }
            }
            /////////////////////////////////////// BASE CODE
            ///
            return false;
        }
    }
}