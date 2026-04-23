using BrilliantSkies.Core.Logger;
using BrilliantSkies.Effects.Explosions;
using BrilliantSkies.Effects.SoundSystem;
using BrilliantSkies.PlayerProfiles;
using HarmonyLib;
using MTMTVFX.Core;
using MTMTVFX.UI;
using UnityEngine;

namespace MTMTVFX.Effects
{
    [HarmonyPatch(typeof(ExplosionVisualiser), "MakeExplosion")]
    public class ExplosionVFXPatch
    {
        private static void Prefix(ExplosionVisualiser __instance, float size, Vector3 gameWorldPosition, IAudioClip sound = null, bool pushToClient = true)
        {
            SettingsConfig config = ProfileManager.Instance.GetModule<SettingsConfig>();
            if (config.IS_DEGRADED) return;

            if (size < 1f || float.IsInfinity(size) || float.IsNaN(size))
            {
                AdvLogger.LogError(string.Format("Explosion of size {0} requested in {1}. Not possible.", size, "MakeExplosion"), LogOptions._AlertDevInGame);
            }
            else
            {
                float scaler = 1f;
                Explosion explosionSize = Explosion.expl_tiny;

                if (size < (float)Explosion.expl_tiny)
                {
                    goto spawn;
                }
                else if (size < (float)Explosion.expl_small)
                {
                    explosionSize = Explosion.expl_small;
                    scaler = 6f;
                }
                else if (size < (float)Explosion.expl_medium)
                {
                    explosionSize = Explosion.expl_medium;
                    scaler = 20f;
                }
                else if (size < (float)Explosion.expl_big)
                {
                    explosionSize = Explosion.expl_big;
                    scaler = 60f;
                }
                else
                {
                    explosionSize = Explosion.expl_huge;
                    scaler = 120f;
                }

                spawn:
                MainThreadDispatcher.Enqueue(() =>
                {
                    PatchedSpawn(explosionSize, gameWorldPosition, size / scaler);
                });
            }
        }

        public static void PatchedSpawn(Explosion explosionName, Vector3 pos, float scaler)
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

