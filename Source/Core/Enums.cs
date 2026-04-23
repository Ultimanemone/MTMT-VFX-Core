using BrilliantSkies.PlayerProfiles;
using MTMTVFX.UI;
using System;

namespace MTMTVFX.Core
{
    public static class Enums
    {
        public static int GetCount(Enum type)
        {
            SettingsConfig config = ProfileManager.Instance.GetModule<SettingsConfig>();
            if (type.GetType() == typeof(MuzzleFlash)) return config.COUNT_MUZZLE;
            if (type.GetType() == typeof(Explosion)) return config.COUNT_EXPL;
            if (type.GetType() == typeof(RailgunName)) return config.COUNT_RAILGUN;
            if (type is BeamName.laser_pulse) return config.COUNT_PULSE;
            if (type is BeamName.pac_beam) return config.COUNT_PAC;

            return -1;
        }

        public static MuzzleFlash GetMuzzleEnum(float gauge)
        {
            if (gauge <= 0.05f)
            {
                return MuzzleFlash.muzzleflash_tiny;
            }
            else if (gauge <= 0.127f)
            {
                return MuzzleFlash.muzzleflash_small;
            }
            else if (gauge <= 0.225f)
            {
                return MuzzleFlash.muzzleflash_medium;
            }
            else if (gauge <= 0.305f)
            {
                return MuzzleFlash.muzzleflash_big;
            }
            else if (gauge <= 0.406f)
            {
                return MuzzleFlash.muzzleflash_bigger;
            }
            else if (gauge <= 0.5f)
            {
                return MuzzleFlash.muzzleflash_biggest;
            }
            else
            {
                return MuzzleFlash.none;
            }
        }
    }

    public enum Explosion
    {
        none,
        expl_tiny = 4,
        expl_small = 8,
        expl_medium = 16,
        expl_big = 30,
        expl_huge = 31,
        //expl_nuclear,
        //tinysplash,
        //largesplash,
        //hugesplash,
        //largesplash_pure,
        //splashbase,
        //distshockwave,
    }

    public enum MuzzleFlash
    {
        none,
        muzzleflash_tiny,
        muzzleflash_small,
        muzzleflash_medium,
        muzzleflash_big,
        muzzleflash_bigger,
        muzzleflash_biggest,
        muzzleflash_huge,
        muzzleflash_gigant,
    }

    public enum RailgunName
    {
        none,
        muzzlerail_small,
        muzzlerail_medium,
        muzzlerail_big,
    }

    public enum BeamName
    {
        none,
        laser_pulse,
        pac_beam
    }

    public enum Trail
    {
        aps,
        railgun,
        cram,
        missile,
        plasma
    }

    public enum SpecialName
    {
        none,
        laser_cont
    }
}
