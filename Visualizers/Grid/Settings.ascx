<%@ Control Language="C#" AutoEventWireup="true" Inherits="DotNetNuke.Modules.Reports.Visualizers.Grid.Settings" Codebehind="Settings.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<table id="tblDataSource" runat="server" cellspacing="0" cellpadding="2" border="0"
       summary="Grid Visualizer Settings Design Table" style="width: 100%">
    <tr>
        <td class="dnn_rpt_form_label">
            <dnn:Label ID="lblPageData" runat="server" ControlName="chkPageData" Suffix=":" CssClass="SubHead"/>
        </td>
        <td class="dnn_rpt_form_field">
            <asp:CheckBox ID="chkPageData" runat="server"/>
        </td>
    </tr>
    <tr id="rowPageSize" runat="server">
        <td class="dnn_rpt_form_label">
            <dnn:Label ID="lblPageSize" runat="server" ControlName="txtPageSize" Suffix=":" CssClass="SubHead"/>
        </td>
        <td class="dnn_rpt_form_field">
            <asp:TextBox ID="txtPageSize" runat="server" Columns="5"/>
            <asp:CompareValidator ID="typvalPageSize" runat="server" ControlToValidate="txtPageSize"
                                  Type="Integer" Operator="DataTypeCheck" ResourceKey="typvalPageSize" CssClass="NormalRed"/>
        </td>
    </tr>
    <tr>
        <td class="dnn_rpt_form_label">
            <dnn:Label ID="lblSortData" runat="server" ControlName="chkSortData" Suffix=":" CssClass="SubHead"/>
        </td>
        <td class="dnn_rpt_form_field">
            <asp:CheckBox ID="chkSortData" runat="server"/>
        </td>
    </tr>
    <tr>
        <td class="dnn_rpt_form_label">
            <dnn:Label ID="lblShowHeader" runat="server" ControlName="chkShowHeader" Suffix=":" CssClass="SubHead"/>
        </td>
        <td class="dnn_rpt_form_field">
            <asp:CheckBox ID="chkShowHeader" runat="server"/>
        </td>
    </tr>
    <tr>
        <td class="dnn_rpt_form_label">
            <dnn:Label ID="lblGridLines" runat="server" ControlName="ddlGridLines" Suffix=":" CssClass="SubHead"/>
        </td>
        <td class="dnn_rpt_form_field">
            <asp:DropDownList ID="ddlGridLines" runat="server">
                <asp:ListItem Text="Both" Value="Both" Selected="True"/>
                <asp:ListItem Text="Horizontal" Value="Horizontal"/>
                <asp:ListItem Text="None" Value="None"/>
                <asp:ListItem Text="Vertical" Value="Vertical"/>
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td class="dnn_rpt_form_label">
            <dnn:Label ID="lblAdditionalCSS" runat="server" ControlName="txtAdditionalCSS" Suffix=":" CssClass="SubHead"/>
        </td>
        <td class="dnn_rpt_form_field">
            <asp:TextBox ID="txtAdditionalCSS" runat="server" Width="90%"/>
        </td>
    </tr>
    <tr>
        <td class="dnn_rpt_form_label">
            <dnn:Label ID="lblCSSClass" runat="server" ControlName="txtCSSClass" Suffix=":" CssClass="SubHead"/>
        </td>
        <td class="dnn_rpt_form_field">
            <asp:TextBox ID="txtCSSClass" runat="server" Width="90%"/>
        </td>
    </tr>
</table>