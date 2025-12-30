namespace MTMTVFX.Core
{
    public class Enums
    {
        public static MuzzleFlashName GetMuzzleEnum(float gauge)
        {
            if (gauge <= 0.05f)
            {
                return MuzzleFlashName.muzzleflash_tiny;
            }
            else if (gauge < 0.1f)
            {
                return MuzzleFlashName.muzzleflash_small;
            }
            else if (gauge < 0.2f)
            {
                return MuzzleFlashName.muzzleflash_medium;
            }
            else if (gauge < 0.279f)
            {
                return MuzzleFlashName.muzzleflash_big;
            }
            else if (gauge < 0.356f)
            {
                return MuzzleFlashName.muzzleflash_bigger;
            }
            else if (gauge < 0.43f)
            {
                return MuzzleFlashName.muzzleflash_biggest;
            }
            else if (gauge <= 0.5f)
            {
                return MuzzleFlashName.muzzleflash_gigant;
            }
            else
            {
                return MuzzleFlashName.none;
            }
        }
    }

    public enum ExplosionName
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

    public enum MuzzleFlashName
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
        //pac
    }

    public enum SpecialName
    {
        none,
        laser_cont
    }
}
