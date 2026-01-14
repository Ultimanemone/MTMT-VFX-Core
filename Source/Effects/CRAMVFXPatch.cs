using HarmonyLib;
using BrilliantSkies.Core.Pooling;
using BrilliantSkies.Effects.SoundSystem;
using BrilliantSkies.Modding.Types;
using BrilliantSkies.Modding;
using UnityEngine;
using MTMTVFX.Core;

namespace MTMTVFX.Effects
{
    [HarmonyPatch(typeof(CannonFiringPiece), "Flash")]
    public class CRAMVFXPatch
    {
        private static bool Prefix(CannonFiringPiece __instance, float packedPayload)
        {
            if (!Util.E_MUZZLE) return true;

            bool bombChuteAttached = __instance.Node.BombChuteAttached;
            if (!bombChuteAttached)
            {
                AudioClipDefinition randomClipByCollectionName = Configured.i.AudioCollections.GetRandomClipByCollectionName("Cram Fire");
                bool flag = randomClipByCollectionName != null;
                if (flag)
                {
                    Pooler.GetPool<AdvSoundManager>().PlaySound(new SoundRequest(randomClipByCollectionName, __instance.GameWorldPosition)
                    {
                        Priority = SoundPriority.ShouldHear,
                        Pitch = Random.Range(0.9f, 1.1f) * (120f / __instance.Node.Stats.GetMuzzleVelocityUnscaled()),
                        MinDistance = 50f,
                        Volume = 1f
                    });
                }
                __instance.contraction = 0f;
                if (Util.IS_DEGRADED)
                {
                    __instance.flasher.nominalScale = 0.75f * __instance.Node.Stats.BarrelDiameter * __instance.Node.Stats.FlashEffect;
                    __instance.flasher.nominalLengthScale = 1f * __instance.Node.Stats.BarrelDiameter;
                    __instance.flasher.flashTime = 0.05f;
                    __instance.flasher.FireSeveral(Mathf.RoundToInt(8f * __instance.Node.Stats.BarrelDiameter));
                }
                else
                {
                    float num = Mathf.Pow(__instance.Node.Stats.FlashEffect * packedPayload / 500f, 0.35f);
                    MuzzleFlashName muzzleType = MuzzleFlashName.none;
                    if (num > 2.3f) muzzleType = MuzzleFlashName.muzzleflash_gigant;
                    else if (num > 1.3f) muzzleType = MuzzleFlashName.muzzleflash_huge;
                    else if (num > 0.5f) muzzleType = MuzzleFlashName.muzzleflash_biggest;
                    else if (num > 0f) muzzleType = MuzzleFlashName.muzzleflash_bigger;
                    MainThreadDispatcher.Enqueue(() =>
                    {
                        GameObject obj = VFXManager.Create(muzzleType, __instance.GetFirePoint(0f), __instance.GetFireDirection());
                    });
                }
            }
            return false;
        }
    }
}
