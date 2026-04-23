using BrilliantSkies.PlayerProfiles;
using BrilliantSkies.Ui.Consoles;
using BrilliantSkies.Ui.Consoles.Getters;
using BrilliantSkies.Ui.Consoles.Interpretters;
using BrilliantSkies.Ui.Consoles.Interpretters.Simple;
using BrilliantSkies.Ui.Consoles.Interpretters.Subjective.Choices;
using BrilliantSkies.Ui.Consoles.Interpretters.Subjective.Numbers;
using BrilliantSkies.Ui.Consoles.Segments;
using BrilliantSkies.Ui.Consoles.Styles;
using BrilliantSkies.Ui.Examples.OptionsMenu;
using BrilliantSkies.Ui.Tips;
using System;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;

namespace MTMTVFX.UI
{
    public class SettingsTab : SuperScreen<SettingsConfig>
    {
        public static class UIHelper
        {
            public static SubjectiveToggle<T> Bool<T>
                (
                T focus,
                string label,
                string tooltip,
                Expression<Func<T, bool>> property
                )
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

            public static SubjectiveFloatClampedWithBarFromMiddle<T> FloatClampedWithBarFromMiddle<T>
                (
                T focus,
                float min,
                float max,
                float inc,
                float middle,
                string display,
                Expression<Func<T, int>> property,
                string tooltip
                )
            {
                var member = (MemberExpression)property.Body;
                var propInfo = (PropertyInfo)member.Member;
                var getter = property.Compile();

                return SubjectiveFloatClampedWithBarFromMiddle<T>.Quick(
                    focus,
                    min,
                    max,
                    inc,
                    middle,
                    M.m((T obj) => (float)getter(obj)),
                    display,
                    (obj, value) => propInfo.SetValue(obj, (int)value),
                    new ToolTip(tooltip)
                );
            }
        }

        public SettingsTab(ConsoleWindow window, SettingsConfig config) : base(window, config) { }

        public override Content Name => new Content("MTMT VFX", new ToolTip("Adjust the configuration for MTMT VFX mods here"));

        private float _spacing = 40f;

        public override void Build()
        {
            MiscSection();
            VFXToggles();
            TrailSection();
        }

        private void VFXToggles()
        {
            int col = 4;
            int row = 8;
            ++row;
            ScreenSegmentTable screenSegmentTable = CreateTableSegment(col, row);
            screenSegmentTable.SqueezeTable = false;
            screenSegmentTable.SpaceBelow = _spacing;
            screenSegmentTable.SetColumnFractionalWidths(new float[] { 0.2f, 0.2f, 0.4f, 0.2f });
            screenSegmentTable.BackgroundStyleWhereApplicable = ConsoleStyles.Instance.Styles.Segments.OptionalSegmentDarkBackgroundWithHeader.Style;
            screenSegmentTable.NameWhereApplicable = "VFX Toggles";

            for (int i = 0; i < row; ++i)
            {
                screenSegmentTable.AddInterpretter(new Empty(), i, 0);
                screenSegmentTable.AddInterpretter(new Empty(), i, col - 1);
            }

            screenSegmentTable.AddInterpretter(StringDisplay.Quick(""), 0, 1);
            screenSegmentTable.AddInterpretter(StringDisplay.Quick("Maximum number of VFX to use <color=#F00>(lag when changed!)</color>"), 0, 2).Justify = new TextAnchor?(TextAnchor.MiddleLeft);

            screenSegmentTable.AddInterpretter(UIHelper.Bool(_focus, "VFX: APS Gunpowder", "Enable or disable APS gunpowder VFX", I => I.E_MUZZLE), 1, 1);
            screenSegmentTable.AddInterpretter(UIHelper.Bool(_focus, "VFX: APS Railgun", "Enable or disable APS railgun VFX", I => I.E_RAILGUN), 2, 1);
            screenSegmentTable.AddInterpretter(UIHelper.Bool(_focus, "VFX: Explosions", "Enable or disable explosion VFX", I => I.E_EXPL), 3, 1);
            screenSegmentTable.AddInterpretter(UIHelper.Bool(_focus, "VFX: 0Q Laser", "Enable or disable 0Q laser VFX", I => I.E_CONTINUOUS), 4, 1);
            screenSegmentTable.AddInterpretter(UIHelper.Bool(_focus, "VFX: Pulse Laser", "Enable or disable pulse laser VFX", I => I.E_PULSE), 5, 1);
            screenSegmentTable.AddInterpretter(UIHelper.Bool(_focus, "VFX: PAC", "Enable or disable PAC VFX", I => I.E_PAC), 6, 1);
            screenSegmentTable.AddInterpretter(UIHelper.Bool(_focus, "VFX: Plasma", "NOT IMPLEMENTED YET", I => I.E_PLASMA), 7, 1);
            screenSegmentTable.AddInterpretter(UIHelper.Bool(_focus, "VFX: Flamer", "NOT IMPLEMENTED YET", I => I.E_FLAMER), 8, 1);

            screenSegmentTable.AddInterpretter(UIHelper.FloatClampedWithBarFromMiddle(_focus, 5, 500, 1, 100, "Maximum count", I => I.COUNT_MUZZLE, "Set the maxmimum number of APS gunpowder VFX"), 1, 2);
            screenSegmentTable.AddInterpretter(UIHelper.FloatClampedWithBarFromMiddle(_focus, 5, 500, 1, 100, "Maximum count", I => I.COUNT_RAILGUN, "Set the maxmimum number of APS gunpowder VFX"), 2, 2);
            screenSegmentTable.AddInterpretter(UIHelper.FloatClampedWithBarFromMiddle(_focus, 5, 500, 1, 100, "Maximum count", I => I.COUNT_EXPL, "Set the maxmimum number of APS gunpowder VFX"), 3, 2);
            screenSegmentTable.AddInterpretter(StringDisplay.Quick("(This is made when a combiner is placed)"), 4, 2).Justify = new TextAnchor?(TextAnchor.MiddleLeft);
            screenSegmentTable.AddInterpretter(UIHelper.FloatClampedWithBarFromMiddle(_focus, 5, 500, 1, 100, "Maximum count", I => I.COUNT_PULSE, "Set the maxmimum number of APS gunpowder VFX"), 5, 2);
            screenSegmentTable.AddInterpretter(UIHelper.FloatClampedWithBarFromMiddle(_focus, 5, 500, 1, 100, "Maximum count", I => I.COUNT_PAC, "Set the maxmimum number of APS gunpowder VFX"), 6, 2);
            screenSegmentTable.AddInterpretter(UIHelper.FloatClampedWithBarFromMiddle(_focus, 5, 500, 1, 100, "Maximum count", I => I.COUNT_PLASMA, "Set the maxmimum number of APS gunpowder VFX"), 7, 2);
            screenSegmentTable.AddInterpretter(UIHelper.FloatClampedWithBarFromMiddle(_focus, 5, 500, 1, 100, "Maximum count", I => I.COUNT_FLAMER, "Set the maxmimum number of APS gunpowder VFX"), 8, 2);

            //screenSegmentTable.AddInterpretter(StringDisplay.Quick(""));
            //screenSegmentTable.AddInterpretter(StringDisplay.Quick("Maximum number of VFX to use <color=#F00>(lag when changed!)</color>")).Justify = new TextAnchor?(TextAnchor.MiddleLeft);

            //screenSegmentTable.AddInterpretter(UIHelper.Bool(_focus, "VFX: APS Gunpowder", "Enable or disable APS gunpowder VFX", I => I.E_MUZZLE));
            //screenSegmentTable.AddInterpretter(UIHelper.FloatClampedWithBarFromMiddle(_focus, 5, 500, 1, 100, "Maximum count", I => I.COUNT_MUZZLE, "Set the maxmimum number of APS gunpowder VFX"));
            //screenSegmentTable.AddInterpretter(UIHelper.Bool(_focus, "VFX: APS Railgun", "Enable or disable APS railgun VFX", I => I.E_RAILGUN));
            //screenSegmentTable.AddInterpretter(UIHelper.FloatClampedWithBarFromMiddle(_focus, 5, 500, 1, 100, "Maximum count", I => I.COUNT_RAILGUN, "Set the maxmimum number of APS gunpowder VFX"));
            //screenSegmentTable.AddInterpretter(UIHelper.Bool(_focus, "VFX: Explosions", "Enable or disable explosion VFX", I => I.E_EXPL));
            //screenSegmentTable.AddInterpretter(UIHelper.FloatClampedWithBarFromMiddle(_focus, 5, 500, 1, 100, "Maximum count", I => I.COUNT_EXPL, "Set the maxmimum number of APS gunpowder VFX"));
            //screenSegmentTable.AddInterpretter(UIHelper.Bool(_focus, "VFX: 0Q Laser", "Enable or disable 0Q laser VFX", I => I.E_CONTINUOUS));
            //screenSegmentTable.AddInterpretter(StringDisplay.Quick(""));
            //screenSegmentTable.AddInterpretter(UIHelper.Bool(_focus, "VFX: Pulse Laser", "Enable or disable pulse laser VFX", I => I.E_PULSE));
            //screenSegmentTable.AddInterpretter(UIHelper.FloatClampedWithBarFromMiddle(_focus, 5, 500, 1, 100, "Maximum count", I => I.COUNT_PULSE, "Set the maxmimum number of APS gunpowder VFX"));
            //screenSegmentTable.AddInterpretter(UIHelper.Bool(_focus, "VFX: PAC", "Enable or disable PAC VFX", I => I.E_PAC));
            //screenSegmentTable.AddInterpretter(UIHelper.FloatClampedWithBarFromMiddle(_focus, 5, 500, 1, 100, "Maximum count", I => I.COUNT_PAC, "Set the maxmimum number of APS gunpowder VFX"));
            //screenSegmentTable.AddInterpretter(UIHelper.Bool(_focus, "VFX: Plasma", "NOT IMPLEMENTED YET", I => I.E_PLASMA));
            //screenSegmentTable.AddInterpretter(UIHelper.FloatClampedWithBarFromMiddle(_focus, 5, 500, 1, 100, "Maximum count", I => I.COUNT_PLASMA, "Set the maxmimum number of APS gunpowder VFX"));
            //screenSegmentTable.AddInterpretter(UIHelper.Bool(_focus, "VFX: Flamer", "NOT IMPLEMENTED YET", I => I.E_FLAMER));
            //screenSegmentTable.AddInterpretter(UIHelper.FloatClampedWithBarFromMiddle(_focus, 5, 500, 1, 100, "Maximum count", I => I.COUNT_FLAMER, "Set the maxmimum number of APS gunpowder VFX"));
        }

        private void TrailSection()
        {
            int col = 4;
            int row = 5;
            ++row;
            ScreenSegmentTable screenSegmentTable = CreateTableSegment(col, row);
            screenSegmentTable.SqueezeTable = false;
            screenSegmentTable.SpaceBelow = _spacing;
            screenSegmentTable.SetColumnFractionalWidths(new float[] { 0.2f, 0.2f, 0.4f, 0.2f });
            screenSegmentTable.BackgroundStyleWhereApplicable = ConsoleStyles.Instance.Styles.Segments.OptionalSegmentDarkBackgroundWithHeader.Style;
            screenSegmentTable.NameWhereApplicable = "Trail Toggles";

            for (int i = 0; i < row; ++i)
            {
                screenSegmentTable.AddInterpretter(StringDisplay.Quick(""), i, 1);
                screenSegmentTable.AddInterpretter(StringDisplay.Quick(""), i, col - 1);
            }
        }

        private void MiscSection()
        {
            ScreenSegmentTable screenSegmentTable = CreateTableSegment(3, 1);
            screenSegmentTable.SqueezeTable = false;
            screenSegmentTable.SpaceBelow = _spacing;
            screenSegmentTable.BackgroundStyleWhereApplicable = ConsoleStyles.Instance.Styles.Segments.OptionalSegmentDarkBackgroundWithHeader.Style;
            screenSegmentTable.NameWhereApplicable = "Miscellaneous Settings";

            screenSegmentTable.AddInterpretter(UIHelper.Bool(_focus, "Debug mode", "Enable debugging mode, some debugging logs can be found in the FtD log file", I => I.DEBUG_MODE));
            screenSegmentTable.AddInterpretter(UIHelper.Bool(_focus, "Dynamic pooling", "Enable dynamic pooling, allowing more effects to be created (lag warning)", I => I.ADAPTIVE));
            screenSegmentTable.AddInterpretter(UIHelper.Bool(_focus, "Ignore Degraded Mode", "Play all custom VFX even in degraded mode", I => I.E_IN_DEGRADED));
        }
    }
}