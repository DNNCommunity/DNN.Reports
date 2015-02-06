<%@ Control Language="VB" AutoEventWireup="false" Inherits="DotNetNuke.Modules.Reports.Visualizers.Xslt.Settings" Codebehind="Settings.ascx.vb" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Url" Src="~/controls/urlcontrol.ascx" %>
<div class="SubHead">
    <dnn:Label ID="lblTransformFile" runat="server" ControlName="ctlTransform" Suffix=":" />
</div>
<dnn:Url ID="ctlTransform" runat="server" Width="400px" ShowUrls="True" ShowTabs="False"
                ShowLog="False" ShowTrack="False" Required="True" FileFilter="xsl,xslt"></dnn:Url>
      
<asp:MultiView ID="mvExtensionObjects" runat="server">
    <asp:View ID="vwNotAllowed" runat="server">
        <div style="text-align: center; width: 100%">
            <asp:Image ID="ErrorImage" runat="server" ImageUrl="~/images/yellow-warning.gif" />
            <asp:Label ID="ErrorMessage" runat="server" ResourceKey="SuperUsersOnly" CssClass="NormalRed" />
        </div>
    </asp:View>
    <asp:View ID="vwAllowed" runat="server">
        <div class="SubHead">
            <dnn:Label ID="lblExtensionObjects" runat="server" ControlName="grdExtensionObjects" Suffix=":" />
        </div>
        <asp:GridView ID="grdExtensionObjects" runat="server" 
            AutoGenerateColumns="False">
            <Columns>
                <asp:CommandField DeleteImageUrl="~/images/delete.gif" ButtonType="Image"
                    ShowDeleteButton="True" />
                <asp:BoundField DataField="XmlNamespace" HeaderText="XmlNamespace" />
                <asp:BoundField DataField="ClrType" HeaderText="ClrType" />
            </Columns>
            <EmptyDataTemplate>
                <asp:Label runat="server" ResourceKey="NoObjects.Text" />
            </EmptyDataTemplate>
        </asp:GridView>
        <table id="tblAddExtensionObjectForm" runat="server">
            <tr>
                <td class="SubHead">
                    <dnn:Label ID="lblXmlns" runat="server" ControlName="txtXmlns" Suffix=":" />
                </td>
                <td>
                    <asp:TextBox ID="txtXmlns" runat="server" Width="100%" />
                </td>
            </tr>
            <tr>
                <td class="SubHead">
                    <dnn:Label ID="lblClrType" runat="server" ControlName="txtClrType" Suffix=":" />
                </td>
                <td>
                    <asp:TextBox ID="txtClrType" runat="server" Width="100%" />
                </td>
            </tr>
        </table>
        <asp:LinkButton ID="btnAddExtensionObject" runat="server" ResourceKey="btnAddExtensionObject" />
    </asp:View>
</asp:MultiView>