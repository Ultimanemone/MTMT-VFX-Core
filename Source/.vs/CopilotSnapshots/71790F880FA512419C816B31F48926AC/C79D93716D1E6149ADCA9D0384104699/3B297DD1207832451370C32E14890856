using BrilliantSkies.Core.Logger;
using BrilliantSkies.Effects.Explosions;
using BrilliantSkies.Effects.SoundSystem;
using BrilliantSkies.PlayerProfiles;
using HarmonyLib;
using MTMTVFX.Core;
using MTMTVFX.UI;
using UnityEngine;

namespace MTMTVFX.Effects.Muzzle
{
    [HarmonyPatch(typeof(ExplosionVisualiser), "MakeExplosion")]
    public class ExplosionVFXPatch
    {
        private static void Prefix(ExplosionVisualiser __instance, float size, Vector3 gameWorldPosition, IAudioClip sound = null, bool pushToClient = true)
        {
            SettingsConfig config = Utils.GetConfig();
            if (config.IS_DEGRADED) return;

            if (size < 1f || float.IsInfinity(size) || float.IsNaN(size))
            {
                AdvLogger.LogError(string.Format("ExplosionType of size {0} requested in {1}. Not possible.", size, "MakeExplosion"), LogOptions._AlertDevInGame);
            }
            else
            {
                float scaler = 1f;
                ExplosionType explosionSize = ExplosionType.expl_tiny;

                if (size < (float)ExplosionType.expl_tiny)
                {
                    goto spawn;
                }
                else if (size < (float)ExplosionType.expl_small)
                {
                    explosionSize = ExplosionType.expl_small;
                    scaler = 6f;
                }
                else if (size < (float)ExplosionType.expl_medium)
                {
                    explosionSize = ExplosionType.expl_medium;
                    scaler = 20f;
                }
                else if (size < (float)ExplosionType.expl_big)
                {
                    explosionSize = ExplosionType.expl_big;
                    scaler = 60f;
                }
                else
                {
                    explosionSize = ExplosionType.expl_huge;
                    scaler = 120f;
                }

                spawn:
                MainThreadDispatcher.Enqueue(() =>
                {
                    PatchedSpawn(explosionSize, gameWorldPosition, size / scaler);
                });
            }
        }

        public static void PatchedSpawn(ExplosionType explosionName, Vector3 pos, float scaler)
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

