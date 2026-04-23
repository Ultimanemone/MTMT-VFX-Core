using BrilliantSkies.Effects.Regulation;
using BrilliantSkies.PlayerProfiles;
using MTMTVFX.Core;
using MTMTVFX.Effects;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTMTVFX.UI
{
    public class SettingsConfig : ProfileModule<SettingsConfig.InternalData>
    {
        public class InternalData
        {
            public bool DEBUG_MODE { get; set; } = false;
            public bool ADAPTIVE { get; set; } = false;

            public bool E_MUZZLE { get; set; } = true;
            public int COUNT_MUZZLE { get; set; } = 100;
            public bool E_RAILGUN { get; set; } = true;
            public int COUNT_RAILGUN { get; set; } = 100;

            public bool E_EXPL { get; set; } = true;
            public int COUNT_EXPL { get; set; } = 100;
            public bool E_PULSE { get; set; } = true;
            public int COUNT_PULSE { get; set; } = 100;
            public bool E_PAC { get; set; } = true;
            public int COUNT_PAC { get; set; } = 50;
            public bool E_PLASMA { get; set; } = true;
            public int COUNT_PLASMA { get; set; } = 50;
            public bool E_FLAMER { get; set; } = true;
            public int COUNT_FLAMER { get; set; } = 50;

            public bool E_APS_TRAIL { get; set; } = false;
            public bool E_APS_MODEL { get; set; } = false;
            public bool E_CRAM_TRAIL { get; set; } = false;
            public bool E_CRAM_MODEL { get; set; } = false;
            public bool E_PLASMA_TRAIL { get; set; } = false;
            public bool E_PLASMA_MODEL { get; set; } = false;
            public bool E_MISSILE_TRAIL { get; set; } = false;
            public bool E_MISSILE_MODEL { get; set; } = false;

            public bool E_CONTINUOUS { get; set; } = false;
            public bool E_IN_DEGRADED { get; set; } = false;


        }

        public override ModuleType ModuleType => ModuleType.Options;
        protected override string FilenameAndExtension => "profile.MTMTConfig";

        public bool DEBUG_MODE
        {
            get { return Internal.DEBUG_MODE; }
            set { Internal.DEBUG_MODE = value; }
        }

        public bool ADAPTIVE
        {
            get { return Internal.ADAPTIVE; }
            set { Internal.ADAPTIVE = value; }
        }

        public bool E_MUZZLE
        {
            get { return Internal.E_MUZZLE; }
            set
            {
                Internal.E_MUZZLE = value;
                VFXManager.Instance.OnConfigUpdateAllPool();
            }
        }

        public int COUNT_MUZZLE
        {
            get { return Internal.COUNT_MUZZLE; }
            set
            {
                Internal.COUNT_MUZZLE = value;
                VFXManager.Instance.OnConfigUpdatePool<MuzzleFlashName>();
            }
        }

        public bool E_RAILGUN
        {
            get { return Internal.E_RAILGUN; }
            set { Internal.E_RAILGUN = value; }
        }

        public int COUNT_RAILGUN
        {
            get { return Internal.COUNT_RAILGUN; }
            set
            {
                Internal.COUNT_RAILGUN = value;
                VFXManager.Instance.OnConfigUpdatePool<RailgunName>();
            }
        }

        public bool E_EXPL
        {
            get { return Internal.E_EXPL; }
            set { Internal.E_EXPL = value; }
        }

        public int COUNT_EXPL
        {
            get { return Internal.COUNT_EXPL; }
            set
            {
                Internal.COUNT_EXPL = value;
                VFXManager.Instance.OnConfigUpdatePool<ExplosionName>();
            }
        }

        public bool E_PULSE
        {
            get { return Internal.E_PULSE; }
            set { Internal.E_PULSE = value; }
        }

        public int COUNT_PULSE
        {
            get { return Internal.COUNT_PULSE; }
            set
            {
                Internal.COUNT_PULSE = value;
                VFXManager.Instance.OnConfigUpdatePool(BeamName.laser_pulse);
            }
        }

        public bool E_PAC
        {
            get { return Internal.E_PAC; }
            set { Internal.E_PAC = value; }
        }

        public int COUNT_PAC
        {
            get { return Internal.COUNT_PAC; }
            set
            {
                Internal.COUNT_PAC = value;
                VFXManager.Instance.OnConfigUpdatePool(BeamName.pac_beam);
            }
        }

        public bool E_PLASMA
        {
            get { return Internal.E_PLASMA; }
            set { Internal.E_PLASMA = value; }
        }

        public int COUNT_PLASMA
        {
            get { return Internal.COUNT_PLASMA; }
            set
            {
                Internal.COUNT_PLASMA = value;
                //VFXManager.Instance.OnConfigUpdatePool(BeamName.plasma);
            }
        }

        public bool E_FLAMER
        {
            get { return Internal.E_FLAMER; }
            set
            {
                Internal.E_FLAMER = value;
                FlamerEmitterBase.ToggleVFX(value);
            }
        }

        public int COUNT_FLAMER
        {
            get { return Internal.COUNT_FLAMER; }
            set { Internal.COUNT_FLAMER = value; }
        }


        public bool E_APS_TRAIL
        {
            get { return Internal.E_APS_TRAIL; }
            set { Internal.E_APS_TRAIL = value; }
        }

        public bool E_APS_MODEL
        {
            get { return Internal.E_APS_MODEL; }
            set { Internal.E_APS_MODEL = value; }
        }

        public bool E_CRAM_TRAIL
        {
            get { return Internal.E_CRAM_TRAIL; }
            set { Internal.E_CRAM_TRAIL = value; }
        }

        public bool E_CRAM_MODEL
        {
            get { return Internal.E_CRAM_MODEL; }
            set { Internal.E_CRAM_MODEL = value; }
        }

        public bool E_PLASMA_TRAIL
        {
            get { return Internal.E_PLASMA_TRAIL; }
            set { Internal.E_PLASMA_TRAIL = value; }
        }

        public bool E_PLASMA_MODEL
        {
            get { return Internal.E_PLASMA_MODEL; }
            set { Internal.E_PLASMA_MODEL = value; }
        }

        public bool E_MISSILE_TRAIL
        {
            get { return Internal.E_MISSILE_TRAIL; }
            set { Internal.E_MISSILE_TRAIL = value; }
        }

        public bool E_MISSILE_MODEL
        {
            get { return Internal.E_MISSILE_MODEL; }
            set { Internal.E_MISSILE_MODEL = value; }
        }


        public bool E_CONTINUOUS
        {
            get { return Internal.E_CONTINUOUS; }
            set { Internal.E_CONTINUOUS = value; }
        }

        public bool E_IN_DEGRADED
        {
            get { return Internal.E_IN_DEGRADED; }
            set { Internal.E_IN_DEGRADED = value; }
        }

        public bool IS_DEGRADED
        {
            get
            {
                return !E_IN_DEGRADED && !ProcessorLoading.Instance.FullMode;
            }
        }
    }
}
