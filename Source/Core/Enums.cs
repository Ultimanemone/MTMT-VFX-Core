using System;

namespace MTMTVFX.Core
{
    public class Enums
    {
        public static MuzzleFlashName GetMuzzleEnum(float gauge)
        {
            if (gauge <= 0.05f)
            {
                return MuzzleFlashName.muzzleflashtinytiny;
            }
            else if (gauge < 0.1f)
            {
                return MuzzleFlashName.muzzleflashtiny;
            }
            else if (gauge < 0.2f)
            {
                return MuzzleFlashName.muzzleflashsmall;
            }
            else if (gauge < 0.279f)
            {
                return MuzzleFlashName.muzzleflashmedium;
            }
            else if (gauge < 0.356f)
            {
                return MuzzleFlashName.muzzleflashlarge;
            }
            else if (gauge < 0.43f)
            {
                return MuzzleFlashName.muzzleflashlargest;
            }
            else if (gauge <= 0.5f)
            {
                return MuzzleFlashName.muzzleflashmammoth;
            }
            else
            {
                return MuzzleFlashName.none;
            }
        }
    }

    public enum ExplosionName
    {
        tinybom,
        normalbom,
        mediumbom,
        largebom,
        hugebom,
        tinysplash,
        largesplash,
        hugesplash,
        largesplash_pure,
        splashbase,
        distshockwave,
        atomicbom,
        none
    }

    public enum MuzzleFlashName
    {
        muzzleflashtinytiny,
        muzzleflashtiny,
        muzzleflashsmall,
        muzzleflashmedium,
        muzzleflashlarge,
        muzzleflashlargest,
        muzzleflashhuge,
        muzzleflashmammoth,
        muzzleflashrail_small,
        muzzleflashrail_medium,
        muzzleflashrail_big,
        none
    }

    public enum BeamName
    {
        pulselaser,
        paceffect,
        laserflash,
        laserhit,
        shockwave,
        none
    }
}
