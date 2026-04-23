using BrilliantSkies.Ui.Consoles;
using BrilliantSkies.Ui.Consoles.Interpretters;
using BrilliantSkies.Ui.Consoles.Interpretters.Subjective.Choices;
using BrilliantSkies.Ui.Consoles.Segments;
using BrilliantSkies.Ui.Consoles.Styles;
using BrilliantSkies.Ui.Tips;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace MTMTVFX.UI
{
    public class SettingsTab : SuperScreen<SettingsConfig>
    {
        public static class ButtonHelper
        {
            public static SubjectiveToggle<T> Bool<T>(T focus, string label, string tooltip, Expression<Func<T, bool>> property)
            {
                var member = (MemberExpression)property.Body;
                var propInfo = (PropertyInfo)member.Member;

                return SubjectiveToggle<T>.Quick(
                    focus,
                    label,
                    new ToolTip(tooltip),
                    (obj, value) => propInfo.SetValue(obj, value),
                    obj => (bool)propInfo.GetValue(obj)
                );
            }
        }

        public SettingsTab(ConsoleWindow window, SettingsConfig config) : base(window, config) { }
        
        public override Content Name => new Content("MTMT VFX Settings", new ToolTip("Adjust the configuration for MTMT VFX mods here"));

        private float _spacing = 40f;

        public override void Build()
        {
            VFXToggles();
            MiscSection();
        }

        private void VFXToggles()
        {
            ScreenSegmentTable screenSegmentTable = CreateTableSegment(2, 8);
            screenSegmentTable.SqueezeTable = true;
            screenSegmentTable.SpaceBelow = _spacing;
            screenSegmentTable.SetColumnFractionalWidths(new float[] { 0.2f, 0.8f });
            screenSegmentTable.BackgroundStyleWhereApplicable = ConsoleStyles.Instance.Styles.Segments.OptionalSegmentDarkBackgroundWithHeader.Style;
            screenSegmentTable.NameWhereApplicable = "VFX Toggles";

            screenSegmentTable.AddInterpretter(ButtonHelper.Bool(_focus, "VFX: APS Gunpowder", "Enable or disable APS gunpowder VFX", I => I.E_MUZZLE));
            screenSegmentTable.AddInterpretter(ButtonHelper.Bool(_focus, "VFX: APS Railgun", "Enable or disable APS railgun VFX", I => I.E_RAILGUN));
            screenSegmentTable.AddInterpretter(ButtonHelper.Bool(_focus, "VFX: Explosions", "Enable or disable explosion VFX", I => I.E_EXPL));
            screenSegmentTable.AddInterpretter(ButtonHelper.Bool(_focus, "VFX: 0Q Laser", "Enable or disable 0Q laser VFX", I => I.E_CONTINUOUS));
            screenSegmentTable.AddInterpretter(ButtonHelper.Bool(_focus, "VFX: Pulse Laser", "Enable or disable pulse laser VFX", I => I.E_PULSE));
            screenSegmentTable.AddInterpretter(ButtonHelper.Bool(_focus, "VFX: PAC", "Enable or disable PAC VFX", I => I.E_PAC));
            screenSegmentTable.AddInterpretter(ButtonHelper.Bool(_focus, "VFX: Plasma", "NOT IMPLEMENTED YET", I => I.E_PLASMA));
            screenSegmentTable.AddInterpretter(ButtonHelper.Bool(_focus, "VFX: Flamer", "NOT IMPLEMENTED YET", I => I.E_FLAMER));
            screenSegmentTable.AddInterpretter(ButtonHelper.Bool(_focus, "VFX: APS Trail", "NOT IMPLEMENTED YET", I => I.E_APS_TRAIL));

            //screenSegmentTable.AddInterpretter(ButtonHelper.Bool(_focus, "VFX: APS Gunpowder", "Enable or disable APS gunpowder VFX", I => I.E_MUZZLE), 0, 1);
            //screenSegmentTable.AddInterpretter(ButtonHelper.Bool(_focus, "VFX: APS Railgun", "Enable or disable APS railgun VFX", I => I.E_RAILGUN), 0, 2);
            //screenSegmentTable.AddInterpretter(ButtonHelper.Bool(_focus, "VFX: Explosions", "Enable or disable explosion VFX", I => I.E_EXPL), 0, 3);
            //screenSegmentTable.AddInterpretter(ButtonHelper.Bool(_focus, "VFX: 0Q Laser", "Enable or disable 0Q laser VFX", I => I.E_CONTINUOUS), 0, 4);
            //screenSegmentTable.AddInterpretter(ButtonHelper.Bool(_focus, "VFX: Pulse Laser", "Enable or disable pulse laser VFX", I => I.E_PULSE), 0, 5);
            //screenSegmentTable.AddInterpretter(ButtonHelper.Bool(_focus, "VFX: PAC", "Enable or disable PAC VFX", I => I.E_PAC), 0, 6);
            //screenSegmentTable.AddInterpretter(ButtonHelper.Bool(_focus, "VFX: Plasma", "NOT IMPLEMENTED YET", I => I.E_PLASMA), 0, 7);
            //screenSegmentTable.AddInterpretter(ButtonHelper.Bool(_focus, "VFX: Flamer", "NOT IMPLEMENTED YET", I => I.E_FLAMER), 0, 8);
        }

        private void MiscSection()
        {
            ScreenSegmentTable screenSegmentTable = CreateTableSegment(3, 1);
            screenSegmentTable.SqueezeTable = false;
            screenSegmentTable.SpaceBelow = _spacing;
            screenSegmentTable.BackgroundStyleWhereApplicable = ConsoleStyles.Instance.Styles.Segments.OptionalSegmentDarkBackgroundWithHeader.Style;
            screenSegmentTable.NameWhereApplicable = "Miscellaneous Settings";

            screenSegmentTable.AddInterpretter(ButtonHelper.Bool(_focus, "Debug mode", "Enable debugging mode, some debugging logs can be found in the FtD log file", I => I.DEBUG_MODE));
            screenSegmentTable.AddInterpretter(ButtonHelper.Bool(_focus, "Dynamic pooling", "Enable dynamic pooling, allowing more effects to be created (lag warning)", I => I.ADAPTIVE));
            screenSegmentTable.AddInterpretter(ButtonHelper.Bool(_focus, "Ignore Degraded Mode", "Play all custom VFX even in degraded mode", I => I.E_IN_DEGRADED));
        }
    }
}
