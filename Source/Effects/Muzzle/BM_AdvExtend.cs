using MTMTVFX.Core;
using BrilliantSkies.Core;
using BrilliantSkies.Effects.GunSounds;
using BrilliantSkies.Effects.SoundSystem;
using System;
using HarmonyLib;
using UnityEngine;


namespace MTMTVFX.Effects.Muzzle
{
    [HarmonyPatch(typeof(AdvCannonFiringPiece), "WeaponFire")]
    internal class BM_AdvExtend
    {
        public class State
        {
            public FiredMunitionReturn FMR;
            public ShellModel shell;
        }

        public static void Prefix(AdvCannonFiringPiece __instance, FiredMunitionReturn FMR, out State __state)
        {
            __state = new State
            {
                FMR = FMR,
                shell = __instance.Node.ShellRacks.PeekNextShell()
            };
        }

        public static void Postfix(AdvCannonFiringPiece __instance, State __state)
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
                    GameObject gameObject = VFXManager.Instance.Create(type.ToString(), __instance.GetFirePoint(0f), __instance.GetFireDirection());
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
                    GameObject gameObject2 = VFXManager.Instance.Create(type2.ToString(), __instance.GetFirePoint(0f), __instance.GetFireDirection());
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
}
