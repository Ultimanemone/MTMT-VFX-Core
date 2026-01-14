using MTMTVFX.Core;
using BrilliantSkies.Core;
using BrilliantSkies.Effects.GunSounds;
using BrilliantSkies.Effects.SoundSystem;
using HarmonyLib;
using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections.Generic;
using System.Linq;


namespace MTMTVFX.Effects
{
    // Code for gun venting heat after shots
    [HarmonyPatch(typeof(AdvCannonFiringPiece), "Vent")]
    public class APSVFXRemoveVent
    {
        private static bool Prefix(AdvCannonFiringPiece __instance)
        {
            if (Util.E_MUZZLE && !Util.IS_DEGRADED) return false;
            return true;
        }
    }

    // Not sure if its needed but visually its not needed
    //[HarmonyPatch(typeof(AdvCannonFiringPiece), "LateVisuals")]
    //public class APSVFXRemoveLateVisuals
    //{
    //    private static bool Prefix(AdvCannonFiringPiece __instance, ref ParticleSystem ____particleSmoke, ref ITicker ____barrelColorTicker)
    //    {
    //        if (!Core.Util.E_MUZZLE) return true;

    //        ///////////////////////////////////////// BASE CODE
    //        bool flag = ____particleSmoke != null && ____particleSmoke.isPlaying;
    //        if (flag)
    //        {
    //            bool flag2 = __instance.SmokePlayingTime.Since > 2f;
    //            if (flag2)
    //            {
    //                ____particleSmoke.Stop();
    //            }
    //        }
    //        bool flag3 = ____barrelColorTicker.CheckAndReset();
    //        if (flag3)
    //        {
    //            ICannonBarrelSystem barrelSystem = __instance.BarrelSystem;
    //            if (barrelSystem != null)
    //            {
    //                barrelSystem.CheckBarrelColors();
    //            }
    //        }
    //        ICannonBarrelSystem barrelSystem2 = __instance.BarrelSystem;
    //        if (barrelSystem2 != null)
    //        {
    //            barrelSystem2.RedrawBarrels();
    //        }
    //        bool flag4 = __instance.GetFireDirection() != Vector3.zero;
    //        if (flag4)
    //        {
    //            __instance.BarrelSystem.AimDirection = __instance.GetFireDirection();
    //        }
    //        else
    //        {
    //            __instance.BarrelSystem.AimDirection = __instance.GameWorldForwards;
    //        }
    //        __instance.BarrelSystem.LateUpdate(__instance.GameWorldUp);
    //        ///////////////////////////////////////// BASE CODE

    //        return false;
    //    }
    //}

    [HarmonyPatch(typeof(AdvCannonFiringPiece), "WeaponFire")]
    public class APSVFXPatch
    {
        private struct State
        {
            public FiredMunitionReturn FMR;
            public ShellModel shell;
            public float railDraw;
        }

        // Grab shell data before firing
        private static void Prefix(AdvCannonFiringPiece __instance, FiredMunitionReturn FMR, out State __state)
        {
            if ((!Util.E_MUZZLE && !Util.E_RAILGUN) || Util.IS_DEGRADED)
            {
                __state = new State
                {
                    FMR = null,
                    shell = null,
                    railDraw = 0
                };
                return;
            }

            MethodInfo RailgunDraw = AccessTools.Method(typeof(AdvCannonFiringPiece), "RailgunDraw");
            ShellModel shell = __instance.Node.ShellRacks.PeekNextShell();
            float railDraw = (float)RailgunDraw.Invoke(__instance, new object[] { shell });
            __state = new State
            {
                FMR = FMR,
                shell = shell,
                railDraw = railDraw
            };
        }

        // Stops the gun from playing the game's VFX
        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            if (!Util.E_MUZZLE || Util.IS_DEGRADED)
            {
                foreach (var instruction in instructions)
                    yield return instruction;

                yield break;
            }

            foreach (CodeInstruction code in instructions)
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
                if (Util.IS_DEGRADED) return;

                if (Util.E_MUZZLE)
                {
                    bool fired = (bool)AccessTools.Field(typeof(FiredMunitionReturn), "_fired").GetValue(__state.FMR);
                    if (!fired) { return; } // Gun didn't fire

                    ShellModel firedShell = __state.shell;
                    if (firedShell == null) return; // Couldn't find the shell

                    bool gpUsed = firedShell.PartsAndMesh.AllParts.Exists(x => x.Name == "Gunpowder casing");
                    if (gpUsed)
                    {
                        float gauge = __instance.BarrelSystem.ShellDiameter;
                        MuzzleFlashName muzzleType = Enums.GetMuzzleEnum(gauge);
                        // Core.Util.LogInfo<APSVFXOverride>($"shell fire: {gauge} with {type.ToString()}");

                        if (muzzleType != MuzzleFlashName.none)
                        {
                            MainThreadDispatcher.Enqueue(() =>
                            {
                                VFXManager.Create(muzzleType, __instance.GetFirePoint(0f), __instance.GetFireDirection());
                            });
                        }
                    }
                }

                if (Util.E_RAILGUN)
                {
                    bool fired = (bool)AccessTools.Field(typeof(FiredMunitionReturn), "_fired").GetValue(__state.FMR);
                    if (!fired) { return; }

                    RailgunName railType = RailgunName.none;
                    if (__state.railDraw < 5000)
                    {
                        return;
                    }
                    else if (__state.railDraw < 15000)
                    {
                        railType = RailgunName.muzzlerail_small;
                    }
                    else if (__state.railDraw < 50000)
                    {
                        railType = RailgunName.muzzlerail_medium;
                    }
                    else
                    {
                        railType = RailgunName.muzzlerail_big;
                    }

                    MainThreadDispatcher.Enqueue(() =>
                    {
                        VFXManager.Create(railType, __instance.GetFirePoint(0f), __instance.GetFireDirection());
                    });
                }
            }
            catch (Exception e)
            {
                Util.LogError<APSVFXPatch>(e.Message, BrilliantSkies.Core.Logger.LogOptions.Popup);
            }
        }
    }

    // Removes the railgun vfx
    [HarmonyPatch(typeof(AdvCannonFiringPiece), "Flash", new Type[] { typeof(bool) })]
    public class APSVFXRemoveRail
    {
        private static bool Prefix(AdvCannonFiringPiece __instance, bool localSource, ref SoundEventRegulator ____firingSoundRegulator)
        {
            if (!Util.E_RAILGUN || Util.IS_DEGRADED) return true;

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
            return false;
        }
    }
}