<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SqlDataSourceCommonSettingsControl.ascx.cs" Inherits="DotNetNuke.Modules.Reports.Controls.SqlDataSourceCommonSettingsControl" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<table cellspacing="0" cellpadding="2" border="0" style="width: 100%">
    <tr>
        <td class="dnn_rpt_form_label">
            <dnn:Label ID="QueryUploadLabel" runat="server" ControlName="fileQuery" Suffix=":" CssClass="SubHead"/>
        </td>
        <td class="dnn_rpt_form_field">
            <asp:FileUpload id="QueryUploadControl" cssclass="NormalTextBox" width="90%" runat="server"/>
            <asp:LinkButton id="QueryUploadButton" cssclass="dnnPrimaryAction" resourcekey="QueryUploadButton" runat="server"/>
        </td>
    </tr>
    <tr>
        <td class="dnn_rpt_form_label">
            <dnn:Label ID="QueryLabel" runat="server" ControlName="txtQuery" Suffix=":" CssClass="SubHead"/>
        </td>
        <td class="dnn_rpt_form_field">
            <asp:TextBox ID="QueryTextBox" CssClass="NormalTextBox" Width="90%" runat="server" TextMode="MultiLine"
                         Rows="10"/>
        </td>
    </tr>
</table>