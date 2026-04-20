using BrilliantSkies.Ui.Consoles;
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
        public static class ToggleHelper
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

        public override void Build()
        {
            ScreenSegmentStandard toggles = CreateStandardSegment();
            
            toggles.BackgroundStyleWhereApplicable = ConsoleStyles.Instance.Styles.Segments.OptionalSegmentDarkBackground.Style;
            toggles.AddInterpretter(ToggleHelper.Bool(_focus, "Debug mode", "Enable debugging mode, some debugging logs can be found in the FtD log file", I => I.DEBUG_MODE));
            toggles.AddInterpretter(ToggleHelper.Bool(_focus, "Dynamic pooling", "Enable dynamic pooling, allowing more effects to be created (lag warning)", I => I.ADAPTIVE));
            toggles.AddInterpretter(ToggleHelper.Bool(_focus, "VFX: APS Gunpowder", "", I => I.E_MUZZLE));
            toggles.AddInterpretter(ToggleHelper.Bool(_focus, "VFX: APS Railgun", "", I => I.E_RAILGUN));
            toggles.AddInterpretter(ToggleHelper.Bool(_focus, "VFX: Explosions", "", I => I.E_EXPL));
            toggles.AddInterpretter(ToggleHelper.Bool(_focus, "VFX: 0Q Laser", "", I => I.E_CONTINUOUS));
            toggles.AddInterpretter(ToggleHelper.Bool(_focus, "VFX: Pulse Laser", "", I => I.E_PULSE));
            toggles.AddInterpretter(ToggleHelper.Bool(_focus, "VFX: PAC", "", I => I.E_PAC));
            toggles.AddInterpretter(ToggleHelper.Bool(_focus, "VFX: Plasma", "", I => I.E_PLASMA));
            toggles.AddInterpretter(ToggleHelper.Bool(_focus, "VFX: Flamer", "", I => I.E_FLAMER));
            toggles.AddInterpretter(ToggleHelper.Bool(_focus, "Ignore Degraded Mode", "", I => I.E_IN_DEGRADED));
        }
    }
}
