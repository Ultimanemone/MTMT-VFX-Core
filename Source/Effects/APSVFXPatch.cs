using MTMTVFX.Core;
using BrilliantSkies.Core;
using BrilliantSkies.Core.Widgets;
using BrilliantSkies.Effects.GunSounds;
using BrilliantSkies.Effects.SoundSystem;
using HarmonyLib;
using UnityEngine;
using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections.Generic;


namespace MTMTVFX.Effects
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
    public class APSVFXRemoveLateVisuals
    {
        private static bool Prefix(AdvCannonFiringPiece __instance, ref ParticleSystem ____particleSmoke, ref ITicker ____barrelColorTicker)
        {
            if (!Core.Util.E_MUZZLE) return true;

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
            if (!Core.Util.E_MUZZLE && !Core.Util.E_RAILGUN)
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
            if (!Core.Util.E_MUZZLE)
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
                if (Core.Util.E_MUZZLE)
                {
                    bool fired = (bool)AccessTools.Field(typeof(FiredMunitionReturn), "_fired").GetValue(__state.FMR);
                    if (!fired) { return; }

                    ShellModel firedShell = __state.shell;
                    int gpCount = 0;
                    int rcCount = 0;
                    if (firedShell == null) return;
                    foreach (ShellModule shellModule in firedShell.PartsAndMesh.AllParts)
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
                    MuzzleFlashName muzzleType = Enums.GetMuzzleEnum(gauge);
                    // Core.Util.LogInfo<APSVFXOverride>($"shell fire: {gauge} with {type.ToString()}");

                    if (gpCount > 0 && muzzleType != MuzzleFlashName.none)
                    {
                        MainThreadDispatcher.Enqueue(() =>
                        {
                            VFXManager.Create<MuzzleFlashName>(muzzleType.ToString(), __instance.GetFirePoint(0f), __instance.GetFireDirection());
                        });
                    }
                }

                if (Core.Util.E_RAILGUN)
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
                        VFXManager.Create<RailgunName>(railType.ToString(), __instance.GetFirePoint(0f), __instance.GetFireDirection());
                    });
                }
            }
            catch (Exception e)
            {
                Core.Util.LogError<APSVFXPatch>(e.Message, BrilliantSkies.Core.Logger.LogOptions.Popup);
            }
        }
    }

    // Removes the railgun vfx
    [HarmonyPatch(typeof(AdvCannonFiringPiece), "Flash", new Type[] { typeof(bool) })]
    public class APSVFXRemoveRail
    {
        private static bool Prefix(AdvCannonFiringPiece __instance, bool localSource, ref SoundEventRegulator ____firingSoundRegulator)
        {
            if (!Core.Util.E_RAILGUN) return true;

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