<%@ Control Language="vb" AutoEventWireup="false" Codebehind="Settings.ascx.vb" Inherits="DotNetNuke.Modules.Reports.DataSources.ADO.Settings" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register Src="~/DesktopModules/Reports/Controls/SqlDataSourceCommonSettingsControl.ascx" TagName="SqlDataSourceCommonSettingsControl"
    TagPrefix="dnnreports" %>
<table cellspacing="0" cellpadding="2" border="0" style="width: 100%">
    <tr>
        <td class="dnn_rpt_form_label">
            <dnn:Label ID="ProviderName" runat="server" ControlName="ProviderNameDropDown" Suffix=":" CssClass="SubHead" />
        </td>
        <td class="dnn_rpt_form_field">
            <asp:DropDownList CssClass="NormalTextBox" ID="ProviderNameDropDown" runat="server" Width="100%" />
        </td>
    </tr>
    <tr>
        <td class="dnn_rpt_form_label">
            <dnn:Label ID="ConnStrLabel" runat="server" ControlName="ConnStrTextBox" Suffix=":" CssClass="SubHead" />
        </td>
        <td class="dnn_rpt_form_field">
            <asp:TextBox CssClass="NormalTextBox" ID="ConnStrTextBox" runat="server" Width="100%" TextMode="SingleLine" />
        </td>
    </tr>
    <tr>
        <td class="dnn_rpt_form_label">
            <dnn:Label ID="ParamPrefixLabel" runat="server" ControlName="ParamPrefixTextBox" Suffix=":" CssClass="SubHead" />
        </td>
        <td class="dnn_rpt_form_field">
            <asp:TextBox CssClass="NormalTextBox" ID="ParamPrefixTextBox" runat="server" Width="100%" TextMode="SingleLine" />
        </td>
    </tr>
</table>
<dnnreports:SqlDataSourceCommonSettingsControl id="SqlDataSourceCommonSettingsControl"
    runat="server">
</dnnreports:SqlDataSourceCommonSettingsControl>

