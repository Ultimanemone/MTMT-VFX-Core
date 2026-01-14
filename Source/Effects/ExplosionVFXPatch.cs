using BrilliantSkies.Effects.Explosions;
using BrilliantSkies.Effects.SoundSystem;
using HarmonyLib;
using UnityEngine;
using BrilliantSkies.Core.Logger;
using MTMTVFX.Core;

namespace MTMTVFX.Effects
{
    [HarmonyPatch(typeof(ExplosionVisualiser), "MakeExplosion")]
    public class ExplosionVFXPatch
    {
        private static void Prefix(ExplosionVisualiser __instance, float size, Vector3 gameWorldPosition, IAudioClip sound = null, bool pushToClient = true)
        {
            if (Core.Util.IS_DEGRADED) return;

            if (size < 1f || float.IsInfinity(size) || float.IsNaN(size))
            {
                AdvLogger.LogError(string.Format("Explosion of size {0} requested in {1}. Not possible.", size, "MakeExplosion"), LogOptions._AlertDevInGame);
            }
            else
            {
                float scaler = 1f;
                ExplosionName explosionSize = ExplosionName.expl_tiny;

                if (size < (float)ExplosionName.expl_tiny)
                {
                    goto spawn;
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

                spawn:
                MainThreadDispatcher.Enqueue(() =>
                {
                    PatchedSpawn(explosionSize, gameWorldPosition, size / scaler);
                });
            }
        }

        public static void PatchedSpawn(ExplosionName explosionName, Vector3 pos, float scaler)
        {
            GameObject obj = VFXManager.Create(explosionName, pos, Vector3.zero);
            if (scaler != 1f && obj != null)
            {
                obj.transform.localScale = new Vector3(scaler, scaler, scaler);
            }
        }
    }


    [HarmonyPatch(typeof(ExplosionVisualiser), "Spawn")]
    public class ExplosionVFXPatch2
    {
        private static bool Prefix()
        {
            return false;
        }
    }
}

