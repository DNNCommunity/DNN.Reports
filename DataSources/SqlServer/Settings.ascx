<%@ Control Language="vb" AutoEventWireup="false" Codebehind="Settings.ascx.vb" Inherits="DotNetNuke.Modules.Reports.DataSources.SqlServer.Settings" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register Src="~/DesktopModules/Reports/Controls/SqlDataSourceCommonSettingsControl.ascx" TagName="SqlDataSourceCommonSettingsControl"
    TagPrefix="dnnreports" %>
<table style="width: 100%" border="0"
    cellpadding="2" cellspacing="0">
    <tr>
        <td class="dnn_rpt_form_label">
            <dnn:Label ID="ConnStrModeLabel" runat="server" Suffix=":" CssClass="SubHead" />
        </td>
        <td class="dnn_rpt_form_field">
            <asp:RadioButton ID="AutomaticConnStrRadio" runat="server" GroupName="ConnStrModeRadioGroup" />
            &nbsp;<dnn:Label ID="AutomaticConnStrLabel" runat="server" ControlName="AutomaticConnStrRadio" CssClass="SubHead" />
            <asp:RadioButton ID="ManualConnStrRadio" runat="server" GroupName="ConnStrModeRadioGroup" />
            &nbsp;<dnn:Label ID="ManualConnStrLabel" runat="server" ControlName="ManualConnStrRadio" CssClass="SubHead" />
        </td>
    </tr>
</table>
<table id="ManualConnStrConfig" runat="server" style="width: 100%"
    border="0" cellpadding="2" cellspacing="0">
    <tr>
        <td class="dnn_rpt_form_label">
            <dnn:Label ID="ConnStrLabel" runat="server" ControlName="ConnStrTextBox" Suffix=":" CssClass="SubHead" />
        </td>
        <td class="dnn_rpt_form_field">
            <asp:TextBox CssClass="NormalTextBox" ID="ConnStrTextBox" runat="server" Width="100%" TextMode="SingleLine" />
        </td>
    </tr>
</table>
<table id="AutomaticConnStrConfig" runat="server" style="width: 100%"
    border="0" cellpadding="2" cellspacing="0">
    <tr>
        <td class="dnn_rpt_form_label">
            <dnn:Label ID="ServerLabel" runat="server" ControlName="ServerTextBox" Suffix=":" CssClass="SubHead" />
        </td>
        <td class="dnn_rpt_form_field">
            <asp:TextBox CssClass="NormalTextBox" ID="ServerTextBox" runat="server" Width="100%" TextMode="SingleLine" />
        </td>
    </tr>
    <tr>
        <td class="dnn_rpt_form_label">
            <dnn:Label ID="DatabaseLabel" runat="server" ControlName="DatabaseTextBox" Suffix=":" CssClass="SubHead" />
        </td>
        <td class="dnn_rpt_form_field">
            <asp:TextBox CssClass="NormalTextBox" ID="DatabaseTextBox" runat="server" Width="100%" TextMode="SingleLine" />
        </td>
    </tr>
    <tr>
        <td class="dnn_rpt_form_label">
            <dnn:Label ID="IntegratedSecurityLabel" runat="server" ControlName="IntegratedSecurityCheckBox"
                Suffix=":" CssClass="SubHead" />
        </td>
        <td class="dnn_rpt_form_field">
            <asp:CheckBox ID="IntegratedSecurityCheckBox" runat="server" />
        </td>
    </tr>
    <tr id="UserNameRow" runat="server">
        <td class="dnn_rpt_form_label">
            <dnn:Label ID="UserNameLabel" runat="server" ControlName="UserNameTextBox" Suffix=":" CssClass="SubHead" />
        </td>
        <td class="dnn_rpt_form_field">
            <asp:TextBox CssClass="NormalTextBox" ID="UserNameTextBox" runat="server" Width="100%" TextMode="SingleLine" />
        </td>
    </tr>
    <tr id="PasswordRow" runat="server">
        <td class="dnn_rpt_form_label">
            <dnn:Label ID="PasswordLabel" runat="server" ControlName="PasswordTextBox" Suffix=":" CssClass="SubHead" />
        </td>
        <td class="dnn_rpt_form_field">
            <asp:TextBox CssClass="NormalTextBox" ID="PasswordTextBox" runat="server" Width="100%" TextMode="SingleLine" />
        </td>
    </tr>
</table>
<dnnreports:SqlDataSourceCommonSettingsControl id="SqlDataSourceCommonSettingsControl"
    runat="server">
</dnnreports:SqlDataSourceCommonSettingsControl>

