using MTMTVFX.Core;
using BrilliantSkies.Core;
using BrilliantSkies.Effects.GunSounds;
using BrilliantSkies.Effects.SoundSystem;
using System;
using HarmonyLib;
using UnityEngine;


namespace MTMTVFX.Effects.Muzzle
{
    [HarmonyPatch(typeof(AdvCannonFiringPiece), "Flash", new Type[] { typeof(bool) })]
    internal class BM_AdvExtend
    {
        private static bool Prefix(AdvCannonFiringPiece __instance, bool localSource, ref SoundEventRegulator ____firingSoundRegulator)
        {
            try
            {
                int gpCount = 0;
                int rcCount = 0;
                float num3 = 0f;
                ShellModel nextShell = __instance.Node.ShellRacks.GetNextShell(false);
                foreach (ShellModule shellModule in nextShell.PartsAndMesh.AllParts)
                {
                    if (shellModule.Name == "Gunpowder casing")
                    {
                        gpCount++;
                    }
                    if (shellModule.Name == "Railgun casing")
                    {
                        rcCount++;
                    }
                }
                BM_MuzzleFlashName type;
                float radius;
                if (__instance.BarrelSystem.ShellDiameter < 0.0601f)
                {
                    type = BM_MuzzleFlashName.TinyTiny;
                    radius = 0f;
                }
                else if (__instance.BarrelSystem.ShellDiameter < 0.12f)
                {
                    type = BM_MuzzleFlashName.Tiny;
                    radius = 5f;
                    num3 = 2f;
                }
                else if (__instance.BarrelSystem.ShellDiameter < 0.236f)
                {
                    type = BM_MuzzleFlashName.Small;
                    radius = 10f;
                    num3 = 4f;
                }
                else if (__instance.BarrelSystem.ShellDiameter < 0.348f)
                {
                    type = BM_MuzzleFlashName.Medium;
                    radius = 15f;
                    num3 = 7f;
                }
                else if (__instance.BarrelSystem.ShellDiameter < 0.402f)
                {
                    type = BM_MuzzleFlashName.Large;
                    radius = 15f;
                    num3 = 10f;
                }
                else
                {
                    type = BM_MuzzleFlashName.Largest;
                    radius = 20f;
                    num3 = 15f;
                }
                if (gpCount > 0)
                {
                    GameObject gameObject = VFXManager.Instance.Create(type.ToString(), __instance.GetFirePoint(0f), __instance.GetFireDirection());
                }

                var RailgunDraw = AccessTools.Method(typeof(AdvCannonFiringPiece), "RailgunDraw");
                bool flag = (float)RailgunDraw.Invoke(__instance, new object[] { null }) > 0f;
                if (flag)
                {
                    BM_MuzzleFlashName type2 = BM_MuzzleFlashName.Rail_Small;
                    if (rcCount > 0)
                    {
                        if (__instance.BarrelSystem.ShellDiameter < 0.12)
                        {
                            type2 = BM_MuzzleFlashName.Rail_Small;
                        }
                        else if (__instance.BarrelSystem.ShellDiameter < 0.38)
                        {
                            type2 = BM_MuzzleFlashName.Rail_Medium;
                        }
                        else
                        {
                            type2 = BM_MuzzleFlashName.Rail_Big;
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
                    __instance.BarrelSystem.Fire(nextShell.Propellant.CooldownTime, nextShell.Propellant.NormalisedVolumeOfPropellant, nextBarrelReady, __instance.Data.DisableBarrelReciprocation);
                }
                else
                {
                    __instance.BarrelSystem.Fire(20f, ShellConstants.ModuleVolume(__instance.BarrelSystem.ShellDiameter) * 4f, nextBarrelReady, __instance.Data.DisableBarrelReciprocation);
                }
            }
            /////////////////////////////////////// BASE CODE
            
            return false;
        }
    }
}
