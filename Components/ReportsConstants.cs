namespace Components
{
    public class ReportsConstants
    {
        #region  Public Constants

        // Localized Text
        public const string SER_KEY_ResourceKey = "ResourceKey";

        public const string SER_KEY_ResourceFile = "ResourceFile";
        public const string SER_KEY_FormatArgs = "FormatArg";
        public const string SER_KEY_FormatArgsCount = "FormatArgsCount";
        public const string RESX_ModuleCommon = "~/DesktopModules/Reports/App_LocalResources/SharedResources.resx";

        // Sql Date Provider
        public const string ProviderType = "data";

        // Visualizer Control Base
        public const string FILENAME_VisualizerASCX = "Visualizer.ascx";

        // Reports Settings Base
        public const string FILENAME_SettingsASCX = "Settings.ascx";

        // Reports Module Exception
        public const string SER_KEY_LocalizedText = "LocalizedText";

        // Reports Client API
        public const string ReportsScript = "~/DesktopModules/Reports/js/dnn.reports.js";

        // Settings
        public const string DEFAULT_Visualizer = "Grid";

        public const string DEFAULT_DataSource = "DotNetNuke";
        public const string FILENAME_RESX_DataSource = "DataSource.ascx.resx";
        public const string FILENAME_RESX_Visualizer = "Visualizer.ascx.resx";
        public const string FILENAME_RESX_Settings = "Settings.ascx.resx";

        // App Settings
        public const string APPSETTING_AllowCachingWithParameters = "Reports_AllowCachingAndParameters";

        // Prefixes
        public const string PREFIX_Reports = "dnn_Reports_";

        public const string PREFIX_Visualizer = "dnn_ReportsVis_";
        public const string PREFIX_DataSource = "dnn_ReportsDS_";

        // Package Types
        public const string PACKAGETYPE_Prefix = "DNN_Reports_";

        public const string PACKAGETYPE_Visualizer = PACKAGETYPE_Prefix + "Visualizer";
        public const string PACKAGETYPE_DataSource = PACKAGETYPE_Prefix + "DataSource";

        // Reports Module Settings
        public const string SETTING_ReportTitle = PREFIX_Reports + "Title";

        public const string SETTING_ReportDescription = PREFIX_Reports + "Description";
        public const string SETTING_ReportParameters = PREFIX_Reports + "Parameters";
        public const string SETTING_ReportCreatedBy = PREFIX_Reports + "CreatedBy";
        public const string SETTING_ReportCreatedOn = PREFIX_Reports + "CreatedOn";
        public const string SETTING_CacheDuration = PREFIX_Reports + "CacheDuration";
        public const string SETTING_DataSource = PREFIX_Reports + "DataSource";
        public const string SETTING_DataSourceClass = PREFIX_Reports + "DataSourceClass";
        public const string SETTING_Visualizer = PREFIX_Reports + "Visualizer";
        public const string SETTING_Converters = PREFIX_Reports + "Converters";
        public const string SETTING_ShowInfoPane = PREFIX_Reports + "ShowInfoPane";
        public const string SETTING_ShowControls = PREFIX_Reports + "ShowControls";
        public const string SETTING_AutoRunReport = PREFIX_Reports + "AutoRunReport";
        public const string SETTING_TokenReplace = PREFIX_Reports + "TokenReplace";

        // Data Source Settings

        // Common
        public const string SETTING_Query = "Query";

        public const string SETTING_ConnectionString = "ConnectionString";
        public const string SETTING_Server = "Server";
        public const string SETTING_Database = "Database";
        public const string SETTING_UserName = "UserName";
        public const string SETTING_Password = "Password";
        public const string SETTING_UseConnectionString = "UseConnectionString";

        // Sql
        public const string SETTING_Sql_UseIntegratedSecurity = "UseIntegratedSecurity";

        // ADO
        public const string SETTING_ADO_ProviderName = "ProviderName";

        public const string SETTING_ADO_ParamPrefix = "ParamPrefix";

        // Visualizer Settings

        // Common
        public const string SETTING_Height = "Height";

        public const string SETTING_Width = "Width";

        // Chart Visualizer
        public const string SETTING_Chart_Type = "Type";

        public const string SETTING_Chart_BarNameColumn = "BarNameColumn";
        public const string SETTING_Chart_BarValueColumn = "BarValueColumn";
        public const string SETTING_Chart_ColorMode = "ColorMode";
        public const string SETTING_Chart_BarColorColumn = "BarColorColumn";
        public const string SETTING_Chart_BarColor = "BarColor";
        public const string SETTING_Chart_XAxisTitle = "XAxisTitle";
        public const string SETTING_Chart_YAxisTitle = "YAxisTitle";

        // Grid Visualizer
        public const string SETTING_Grid_EnablePaging = "EnablePaging";

        public const string SETTING_Grid_EnableSorting = "EnableSorting";
        public const string SETTING_Grid_PageSize = "PageSize";
        public const string SETTING_Grid_ShowHeader = "ShowHeader";
        public const string SETTING_Grid_GridLines = "GridLines";
        public const string SETTING_Grid_AdditionalCSS = "AdditionalCSS";
        public const string SETTING_Grid_CSSClass = "CSSClass";
        public const string DEFAULT_Grid_GridLines = "None";

        // Reporting Services Visualizer
        public const string SETTING_RS_Mode = "Mode";

        public const string SETTING_RS_LocalReportFile = "LocalFile";
        public const string SETTING_RS_ServerReportUrl = "ServerUrl";
        public const string SETTING_RS_ServerReportPath = "ServerPath";
        public const string SETTING_RS_DataSourceName = "DataSourceName";
        public const string SETTING_RS_VisibleUIElements = "VisibleUI";
        public const string SETTING_RS_EnableExternalImages = "EnExImg";
        public const string SETTING_RS_EnableHyperlinks = "EnHyper";
        public const string SETTING_RS_Domain = "Domain";

        public const string DEFAULT_RS_VisibleUIElements =
                "DocumentMapButton,ExportControls,FindControls,NavigationControls,PrintButton,PromptAreaButton,RefreshButton,ToolBar,ZoomControl"
            ;

        // HTML Visualizer
        public const string SETTING_Html_TemplateFile = "TemplateFile";

        public const string SETTING_Html_Repeat = "Repeat";
        public const string SETTING_Html_Separator = "Separator";

        // XSLT Visualizer
        public const string SETTING_Xslt_TransformFile = "TransformFile";

        public const string SETTING_Xslt_ExtensionObject = "Ext";

        public const string CACHEKEY_Reports = "dnn_ReportCache";

        #endregion
    }
}