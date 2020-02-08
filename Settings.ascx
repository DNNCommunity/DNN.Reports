<%@ Control Language="C#" AutoEventWireup="true" Inherits="DotNetNuke.Modules.Reports.Settings"
CodeBehind="Settings.ascx.cs" %>
<%@ Register Src="../../controls/sectionheadcontrol.ascx" TagName="sectionheadcontrol"
TagPrefix="dnn" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="../../controls/LabelControl.ascx" %>
<%-- Data Source Settings Section Header --%>
<dnn:sectionheadcontrol id="dshDataSource" runat="server" resourcekey="DataSource.Section"
                        section="DataSourceSection" cssclass="Head" includerule="true" isexpanded="true"/>
<%-- Data Source Settings Section Body --%>
<div id="DataSourceSection" runat="Server">
    <%-- Contains the Report Data Source Settings, only visible to Host Users --%>
    <asp:MultiView ID="ReportsSettingsMultiView" runat="server">
        <%-- This view is displayed if the user is not a Host User --%>
        <asp:View ID="AccessDeniedView" runat="server">
            <div style="text-align: center; width: 90%">
                <asp:Image ID="ErrorImage" runat="server" ImageUrl="~/images/red-error.gif"/>
                <asp:Label ID="ErrorMessage" runat="server" ResourceKey="SuperUsersOnly" CssClass="NormalRed"/>
            </div>
        </asp:View>
        <%-- This view is displayed if the user is a Host User --%>
        <asp:View ID="SuperUserView" runat="server">
            <%-- Warning message, warns Host that this query will be run whenever a user browses to the page --%>
            <div style="padding: 2px 0px 2px 0px; text-align: center;">
                <div style="float: left; height: 100%">
                    <asp:Image ID="WarningImage" runat="server" ImageUrl="~/images/yellow-warning.gif"/>
                </div>
                <asp:Label ID="WarningMessage" runat="server" ResourceKey="WarningMessage" CssClass="NormalRed"/>
            </div>
            <%-- Table used to layout Data Source settings --%>
            <table id="tblDataSource" runat="server" cellspacing="0" cellpadding="2" border="0"
                   summary="Report Data Source Settings Design Table" style="width: 100%">
                <%-- Title of the Report --%>
                <tr>
                    <td class="dnn_rpt_form_label">
                        <dnn:Label ID="lblTitle" runat="server" CssClass="SubHead" ControlName="txtTitle"
                                   Suffix=":"/>
                    </td>
                    <td class="dnn_rpt_form_field">
                        <asp:TextBox ID="txtTitle" CssClass="NormalTextBox" Width="90%" runat="server"/>
                    </td>
                </tr>
                <%-- Description of the Report --%>
                <tr>
                    <td class="dnn_rpt_form_label">
                        <dnn:Label ID="lblDescription" runat="server" CssClass="SubHead" ControlName="teDescription"
                                   Suffix=":">
                        </dnn:Label>
                    </td>
                    <td class="dnn_rpt_form_field">
                        <asp:TextBox ID="txtDescription" CssClass="NormalTextBox" runat="server" Width="90%"
                                     TextMode="MultiLine" Rows="5"/>
                    </td>
                </tr>
                <%-- Url Parameters used by the Report --%>
                <tr>
                    <td class="dnn_rpt_form_label">
                        <dnn:Label ID="lblParameters" runat="server" CssClass="SubHead" ControlName="txtParameters"
                                   Suffix=":">
                        </dnn:Label>
                    </td>
                    <td class="dnn_rpt_form_field">
                        <asp:TextBox ID="txtParameters" CssClass="NormalTextBox" runat="server" Width="90%"/>
                    </td>
                </tr>
            </table>
            <%-- Data Source Settings --%>
            <%-- Data Source Selector --%>
            <div class="dnn_rpt_active_extension SubHead">
                <dnn:Label ID="lblDataSource" runat="server" ControlName="DataSourceDropDown" Suffix=":"/>
            </div>
            <asp:DropDownList ID="DataSourceDropDown" runat="server" AutoPostBack="true" CssClass="Normal"
                              Width="90%" OnSelectedIndexChanged="DataSourceDropDown_SelectedIndexChanged"/>
            <%-- Data Source Settings Host --%>
            <div style="padding-top: 10px">
                <%-- This view is displayed when no data source is configured --%>
                <asp:MultiView ID="DataSourceSettings" runat="server">
                    <asp:View ID="DataSourceNotConfiguredView" runat="server">
                        <div style="text-align: center; width: 90%">
                            <asp:Image ID="DataSourceNotConfiguredImage" runat="server" ImageUrl="~/images/yellow-warning.gif"/>
                            <asp:Label ID="DataSourceNotConfiguredLabel" runat="server" ResourceKey="DataSourceNotConfigured"
                                       CssClass="NormalRed"/>
                        </div>
                    </asp:View>
                </asp:MultiView>
            </div>
            <%-- Test Data Source and Show Xml Source Button --%>
            <div style="text-align: right; width: 90%;">
                <asp:LinkButton ID="btnTestDataSource" CssClass="CommandButton" runat="server" ResourceKey="btnTestDataSource"
                                CausesValidation="false" OnClick="btnTestQuery_Click"/>
                <asp:LinkButton ID="btnShowXml" CssClass="CommandButton" runat="server" ResourceKey="btnShowXml"
                                CausesValidation="false" OnClick="btnShowXml_Click"/>
            </div>
            <%-- Test Data Source Results Host --%>
            <div runat="server" id="rowResults" visible="false" class="dnn_rpt_test_results">
                <div style="float: left; height: 90%">
                    <asp:Image ID="imgQueryResults" runat="server"/>
                </div>
                <asp:Label ID="lblQueryResults" runat="server"/>
                <br/>
                <asp:LinkButton ID="btnHideTestResults" runat="server" CssClass="CommandButton" ResourceKey="btnHideTestResults" OnClick="btnHideTestResults_Click"/>
            </div>
            <%-- Show Xml Source Host --%>
            <div runat="server" id="rowXmlSource" visible="false" class="dnn_rpt_test_results">
                <dnn:Label ID="lblXmlSource" runat="server" ControlName="txtXmlSource" Suffix=":" CssClass="SubHead"/>
                <asp:TextBox ID="txtXmlSource" runat="server" ReadOnly="true" CssClass="NormalTextBox"
                             TextMode="MultiLine" Rows="7" Width="90%"/>
                <br/>
                <div style="text-align: center">
                    <asp:LinkButton ID="btnHideXmlSource" runat="server" CssClass="CommandButton" ResourceKey="btnHideXmlSource" OnClick="btnHideXmlSource_Click"/>
                </div>
            </div>
            <%-- Data Converters Settings Section Header --%>
            <dnn:sectionheadcontrol ID="dshConvertersSettings" runat="server" ResourceKey="Converters.Section"
                                    Section="ConvertersSection" CssClass="Head" IncludeRule="true" IsExpanded="true"/>
            <%-- Data Filtering Settings Section Body --%>
            <div id="ConvertersSection" runat="server">
                <%-- Table used to layout Converter settings --%>
                <table cellspacing="0" cellpadding="2" border="0" summary="Report Data Converter Settings Design Table"
                       style="width: 100%">
                    <%-- HtmlDecode Filter --%>
                    <tr>
                        <td class="dnn_rpt_form_label">
                            <dnn:Label CssClass="SubHead" ID="lblHtmlDecode" runat="server" ControlName="txtHtmlDecode"
                                       Suffix=":">
                            </dnn:Label>
                        </td>
                        <td class="dnn_rpt_form_field">
                            <asp:TextBox ID="txtHtmlDecode" CssClass="NormalTextBox" runat="server" Width="90%"/>
                        </td>
                    </tr>
                    <%-- HtmlEncode Filter --%>
                    <tr>
                        <td class="dnn_rpt_form_label">
                            <dnn:Label CssClass="SubHead" ID="lblHtmlEncode" runat="server" ControlName="txtHtmlEncode"
                                       Suffix=":">
                            </dnn:Label>
                        </td>
                        <td class="dnn_rpt_form_field">
                            <asp:TextBox ID="txtHtmlEncode" CssClass="NormalTextBox" runat="server" Width="90%"/>
                        </td>
                    </tr>
                </table>
            </div>
        </asp:View>
        <%-- SuperUserView --%>
    </asp:MultiView>
    <%-- ReportsSettingsMultiView --%>
</div>
<%-- Visualizer Settings Section Header --%>
<dnn:sectionheadcontrol id="dshVisualizerSettings" runat="server" resourcekey="Display.Section"
                        section="VisualizerSettingsOuter" cssclass="Head" includerule="true" isexpanded="true"/>
<%-- Visualizer Settings Section Body --%>
<div id="VisualizerSettingsOuter" runat="server">
    <%-- Show Info Pane and Run On Demand Options --%>
    <table cellspacing="0" cellpadding="2" border="0" summary="Report Display Settings Design Table"
           style="width: 100%">
        <%-- Cache Duration --%>
        <tr>
            <td class="dnn_rpt_form_label">
                <dnn:label id="lblCacheDuration" runat="server" cssclass="SubHead" controlname="txtCacheDuration"
                           suffix=":">
                </dnn:label>
            </td>
            <td class="dnn_rpt_form_field">
                <asp:CheckBox ID="chkCaching" CssClass="NormalTextBox" AutoPostBack="true" OnCheckChanged="chkCaching_CheckedChanged" runat="server"/>
                <span id="spanCacheDuration" runat="server" class="Normal">
                    <asp:Label ID="Label1" runat="server" resourcekey="CacheFor.Text"/>
                    <asp:TextBox ID="txtCacheDuration" CssClass="NormalTextBox" Columns="5" runat="server"/>
                    <asp:Label ID="Label2" runat="server" resourcekey="Minutes.Text"/>
                    <div style="text-align: center; width: 90%">
                    <asp:Image ID="CacheWarningImage" runat="server" ImageUrl="~/images/yellow-warning.gif"/>
                    <asp:Label ID="CacheWarningLabel" runat="server" resourcekey="CacheWarning" CssClass="NormalRed"/>
                </span>
            </td>
        </tr>
        <tr>
            <td class="dnn_rpt_form_label">
                <dnn:label id="lblShowInfoPane" runat="server" cssclass="SubHead" controlname="chkShowInfoPane"
                           suffix=":">
                </dnn:label>
            </td>
            <td class="dnn_rpt_form_field">
                <asp:CheckBox ID="chkShowInfoPane" runat="server"/>
            </td>
        </tr>
        <tr>
            <td class="dnn_rpt_form_label">
                <dnn:label id="lblShowControls" runat="server" cssclass="SubHead" controlname="chkShowControls"
                           suffix=":">
                </dnn:label>
            </td>
            <td class="dnn_rpt_form_field">
                <asp:CheckBox ID="chkShowControls" runat="server"/>
            </td>
        </tr>
        <tr>
            <td class="dnn_rpt_form_label">
                <dnn:label id="lblAutoRunReport" runat="server" cssclass="SubHead" controlname="chkAutoRunReport"
                           suffix=":">
                </dnn:label>
            </td>
            <td class="dnn_rpt_form_field">
                <asp:CheckBox ID="chkAutoRunReport" runat="server"/>
            </td>
        </tr>
		<tr>
            <td class="dnn_rpt_form_label">
                <dnn:label id="lblExportExcel" runat="server" cssclass="SubHead" controlname="chkExportExcel"
                    suffix=":">
                </dnn:label>
            </td>
            <td class="dnn_rpt_form_field">
                <asp:CheckBox ID="chkExportExcel" runat="server" />
            </td>
        </tr>

        <tr>
            <td class="dnn_rpt_form_label">
                <dnn:label id="lblTokenReplace" runat="server" cssclass="SubHead" controlname="chkTokenReplace"
                           suffix=":">
                </dnn:label>
            </td>
            <td class="dnn_rpt_form_field">
                <asp:CheckBox ID="chkTokenReplace" runat="server"/>
            </td>
        </tr>
    </table>
    <%-- Visualizer Selector --%>
    <div class="dnn_rpt_active_extension SubHead">
        <dnn:label id="lblVisualizer" runat="server" controlname="VisualizerDropDown" suffix=":"/>
    </div>
    <asp:DropDownList ID="VisualizerDropDown" runat="server" AutoPostBack="true" CssClass="Normal"
                      Width="90%" OnSelectedIndexChanged="VisualizerDropDown_SelectedIndexChanged"/>
    <%-- Visualizer Settings Host --%>
    <div style="padding-top: 10px">
        <asp:MultiView ID="VisualizerSettings" runat="server">
        </asp:MultiView>
    </div>
</div>