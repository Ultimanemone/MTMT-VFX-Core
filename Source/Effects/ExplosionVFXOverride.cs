using BrilliantSkies.Effects.Explosions;
using BrilliantSkies.Effects.SoundSystem;
using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using BrilliantSkies.Core;
using BrilliantSkies.Core.Help;
using BrilliantSkies.Core.Constants;
using BrilliantSkies.Core.Logger;
using BrilliantSkies.Core.Widgets;
using MTMTVFX.Core;

namespace MTMTVFX.Effects
{
    [HarmonyPatch(typeof(ExplosionVisualiser), "MakeExplosion")]
    public class ExplosionVFXOverride
    {
        private static bool Prefix(ExplosionVisualiser __instance, float size, Vector3 gameWorldPosition, IAudioClip sound = null, bool pushToClient = true)
        {
            bool isUnitTesting = Get.IsUnitTesting;
            if (!isUnitTesting)
            {
                if (size == 0f || float.IsInfinity(size) || float.IsNaN(size))
                {
                    AdvLogger.LogError(string.Format("Explosion of size {0} requested in {1}. Not possible.", size, "MakeExplosion"), LogOptions._AlertDevInGame);
                }
                else
                {
                    float scaler = 1f;
                    ExplosionName explosionSize = ExplosionName.expl_tiny;
                    if (size < 1)
                    {
                        goto B1;
                    }
                    else if (size < (float)ExplosionName.expl_tiny)
                    {
                    }
                    else if (size < (float)ExplosionName.expl_small)
                    {
                        explosionSize = ExplosionName.expl_small;
                        scaler = 6f;
                    }
                    else if (size < (float)ExplosionName.expl_medium)
                    {
                        explosionSize = ExplosionName.expl_medium;
                        scaler = 20f;
                    }
                    else if (size < (float)ExplosionName.expl_big)
                    {
                        explosionSize = ExplosionName.expl_big;
                        scaler = 60f;
                    }
                    else
                    {
                        explosionSize = ExplosionName.expl_huge;
                        scaler = 120f;
                    }

                    MainThreadDispatcher.Enqueue(() =>
                    {
                        PatchedSpawn(explosionSize, gameWorldPosition, size / scaler);
                    });

                    if (Net.IsServer && pushToClient)
                    {
                        BrilliantSkies.Core.Networking.Coms.AddRpc(new BrilliantSkies.Core.Networking.RpcRequest(delegate (NetInfrastructure.INetworkIdentity n)
                        {
                            BrilliantSkies.Effects.EffectRcps.Explosion(n, gameWorldPosition, size);
                        }));
                    }
                }
            }
            B1:
            return false;
        }

        public static void PatchedSpawn(ExplosionName explosionName, Vector3 pos, float scaler)
        {
            GameObject obj = VFXManager.Create<ExplosionName>(explosionName.ToString(), pos, Vector3.zero);
            if (scaler != 1f && obj != null)
            {
                obj.transform.localScale = new Vector3(scaler, scaler, scaler);
            }
        }
    }
}

