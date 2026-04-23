using MTMTVFX.Core;
using BrilliantSkies.Core;
using BrilliantSkies.Effects.GunSounds;
using BrilliantSkies.Effects.SoundSystem;
using HarmonyLib;
using System;
using System.Reflection;
using MTMTVFX.UI;
using BrilliantSkies.PlayerProfiles;


namespace MTMTVFX.Effects
{
    [HarmonyPatch(typeof(AdvCannonFiringPiece))]
    public class APSVFXPatch
    {
        private struct FireState
        {
            public FiredMunitionReturn FMR;
            public ShellModel shell;
            public float railDraw;
        }

        // Grab shell data before firing
        [HarmonyPatch("WeaponFire")]
        [HarmonyPrefix]
        private static void GetStateBefore(AdvCannonFiringPiece __instance, FiredMunitionReturn FMR, out FireState __state)
        {
            SettingsConfig config = ProfileManager.Instance.GetModule<SettingsConfig>();
            if ((!config.E_MUZZLE && !config.E_RAILGUN) || config.IS_DEGRADED)
            {
                __state = new FireState
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
            __state = new FireState
            {
                FMR = FMR,
                shell = shell,
                railDraw = railDraw
            };
        }

        // Stops the gun from playing the game's VFX
        // doesnt work, only patch at game start and we need a runtime patch for this
        //[HarmonyPatch("WeaponFire")]
        //[HarmonyTranspiler]
        //private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        //{
        //    SettingsConfig config = ProfileManager.Instance.GetModule<SettingsConfig>();
        //    if (!config.E_MUZZLE || config.IS_DEGRADED)
        //    {
        //        Utils.LogError<APSVFXPatch>("no vfx", BrilliantSkies.Core.Logger.LogOptions.PopupDev);
        //        foreach (var instruction in instructions)
        //            yield return instruction;

        //        yield break;
        //    }

        //    foreach (CodeInstruction code in instructions)
        //    {
        //        Utils.LogError<APSVFXPatch>("yes vfx", BrilliantSkies.Core.Logger.LogOptions.PopupDev);
        //        if (code.opcode == OpCodes.Callvirt &&
        //            code.operand is MethodInfo method &&
        //            method.Name == "get_NormalisedVolumeOfPropellant")
        //        {
        //            yield return new CodeInstruction(OpCodes.Pop);
        //            yield return new CodeInstruction(OpCodes.Ldc_R4, 0f);
        //        }
        //        else
        //        {
        //            yield return code;
        //        }
        //    }
        //}

        // Check if shell was fired, since the method still runs if it didn't
        // Render our VFX if it did
        [HarmonyPatch("WeaponFire")]
        [HarmonyPostfix]
        private static void CheckAndFire(AdvCannonFiringPiece __instance, FireState __state)
        {
            try
            {
                SettingsConfig config = ProfileManager.Instance.GetModule<SettingsConfig>();
                if (config.IS_DEGRADED) return;

                if (config.E_MUZZLE || config.E_RAILGUN)
                {
                    bool fired = (bool)AccessTools.Field(typeof(FiredMunitionReturn), "_fired").GetValue(__state.FMR);
                    if (!fired) { return; } // Gun didn't fire
                }

                if (config.E_MUZZLE)
                {
                    ShellModel firedShell = __state.shell;
                    if (firedShell == null) return; // Couldn't find the shell

                    bool gpUsed = firedShell.PartsAndMesh.AllParts.Exists(x => x.Name == "Gunpowder casing"); // gunpowder used?
                    if (gpUsed)
                    {
                        float gauge = __instance.BarrelSystem.ShellDiameter;
                        MuzzleFlash muzzleType = Enums.GetMuzzleEnum(gauge);
                        // Core._config.LogInfo<APSVFXOverride>($"shell fire: {gauge} with {type.ToString()}");

                        if (muzzleType != MuzzleFlash.none)
                        {
                            MainThreadDispatcher.Enqueue(() =>
                            {
                                VFXManager.Create(muzzleType, __instance.GetFirePoint(0f), __instance.GetFireDirection());
                            });
                        }
                    }
                }

                if (config.E_RAILGUN)
                {
                    RailgunName railType = RailgunName.none;
                    if (__state.railDraw < 5000) return;
                    else if (__state.railDraw < 15000) railType = RailgunName.muzzlerail_small;
                    else if (__state.railDraw < 50000) railType = RailgunName.muzzlerail_medium;
                    else railType = RailgunName.muzzlerail_big;

                    MainThreadDispatcher.Enqueue(() =>
                    {
                        VFXManager.Create(railType, __instance.GetFirePoint(0f), __instance.GetFireDirection());
                    });
                }
            }
            catch (Exception e)
            {
                Utils.LogError<APSVFXPatch>(e.Message, BrilliantSkies.Core.Logger.LogOptions.Popup);
            }
        }

        [HarmonyPatch("Flash", new Type[] { typeof(bool) })]
        [HarmonyPrefix]
        private static bool CancelDefaultRailgunVFX(AdvCannonFiringPiece __instance, bool localSource, ref SoundEventRegulator ____firingSoundRegulator)
        {
            SettingsConfig config = ProfileManager.Instance.GetModule<SettingsConfig>();
            if (!config.E_RAILGUN || config.IS_DEGRADED) return true;

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

        [HarmonyPatch("Vent")]
        [HarmonyPrefix]
        private static bool RemoveVentVFX(AdvCannonFiringPiece __instance)
        {
            SettingsConfig config = ProfileManager.Instance.GetModule<SettingsConfig>();
            if (config.E_MUZZLE) return false;
            return true;
        }
    }


    [HarmonyPatch(typeof(ShellModel_Propellant), "get_NormalisedVolumeOfPropellant")]
    public class MuzzleFlashPatch
    {
        private static void Postfix(ref float __result)
        {
            SettingsConfig config = ProfileManager.Instance.GetModule<SettingsConfig>();
            if (config.E_MUZZLE && !config.IS_DEGRADED)
            {
                // enabled and not degraded
                __result = 0f;
            }
        }
    }

    // Not sure if its needed but visually its not needed
    //[HarmonyPatch(typeof(AdvCannonFiringPiece), "LateVisuals")]
    //public class APSVFXRemoveLateVisuals
    //{
    //    private static bool Prefix(AdvCannonFiringPiece __instance, ref ParticleSystem ____particleSmoke, ref ITicker ____barrelColorTicker)
    //    {
    //        if (!Core._config.E_MUZZLE) return true;

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
}