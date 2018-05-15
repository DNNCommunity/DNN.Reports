namespace DotNetNuke.Modules.Reports.Visualizers.Razor
{
    using System;
    using System.Collections.Generic;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Modules.Reports.Extensions;
    using DotNetNuke.Security;

    public class Settings : ReportsSettingsBase
    {
        private RazorHost.Settings RazorSettings;

        protected void Page_Init(object sender, EventArgs e)
        {
            this.LoadRazorSettingsControl();
            this.EnsureEditScriptControlIsRegistered();
        }

        public override void LoadSettings(Dictionary<string, string> Settings)
        {
            this.RazorSettings.LoadSettings();
        }

        public override void SaveSettings(Dictionary<string, string> Settings)
        {
            this.RazorSettings.UpdateSettings();
        }

        private void EnsureEditScriptControlIsRegistered()
        {
            var moduleDefId = this.ParentModule.ModuleConfiguration.ModuleDefID;
            if (ReferenceEquals(ModuleControlController.GetModuleControlByControlKey("EditRazorScript", moduleDefId),
                                null))
            {
                var m = default(ModuleControlInfo);
                m = new ModuleControlInfo
                        {
                            ControlKey = "EditRazorScript",
                            ControlSrc = "DesktopModules/RazorModules/RazorHost/EditScript.ascx",
                            ControlTitle = "Edit Script",
                            ControlType = SecurityAccessLevel.Host,
                            ModuleDefID = moduleDefId
                        };
                ModuleControlController.UpdateModuleControl(m);
            }
        }

        private void LoadRazorSettingsControl()
        {
            this.RazorSettings =
                (RazorHost.Settings) this.LoadControl("~/DesktopModules/RazorModules/RazorHost/Settings.ascx");
            this.RazorSettings.ModuleConfiguration = this.ParentModule.ModuleConfiguration;
            this.RazorSettings.LocalResourceFile = this.LocalResourceFile;
            this.Controls.Add(this.RazorSettings);
        }
    }
}