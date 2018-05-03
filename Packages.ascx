<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Packages.ascx.cs" Inherits="DotNetNuke.Modules.Reports.Packages" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="../../controls/LabelControl.ascx" %>
<div class="Subhead" style="padding-bottom: 2px; text-align: left;">
    <dnn:label id="PackagesLabel" runat="server" controlname="PackagesGrid">
    </dnn:label>
</div>
<asp:GridView ID="PackagesGrid" runat="server" AutoGenerateColumns="False" BorderStyle="None"
              Width="100%" CellPadding="2" DataKeyNames="PackageID">
    <HeaderStyle CssClass="Subhead"/>
    <RowStyle CssClass="Normal"/>
    <Columns>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:HyperLink ID="linkDelete" runat="server" NavigateUrl='<%#
                                        this.InstallUrl((int) DataBinder.Eval(Container.DataItem, "PackageID")) %>'
                               Visible='<%# !this.IsBuiltIn((int) DataBinder.Eval(Container.DataItem, "PackageID")) %>'>
                    <asp:Image ID="imgDelete" runat="server" ResourceKey="imgDelete" ImageUrl="../../images/delete.gif"/>
                </asp:HyperLink>
                <asp:Image ID="imgBuiltIn" runat="server" ResourceKey="imgBuiltIn" ImageUrl="images/BuiltInPackage.gif"
                           Visible='<%#
                                        this.IsBuiltIn((int) DataBinder.Eval(Container.DataItem, "PackageID")) %>'/>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField HeaderText="FriendlyName" DataField="FriendlyName"/>
        <asp:BoundField HeaderText="Version" DataField="Version"/>
        <asp:TemplateField HeaderText="PackageType">
            <ItemTemplate>
                <asp:Label ID="label" runat="server" Text='<%#
                                        this.StripPrefix((string) DataBinder.Eval(Container.DataItem, "PackageType")) %>'/>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField HeaderText="Description" DataField="Description"/>
    </Columns>
</asp:GridView>
<p>
    <asp:Image ID="imgBuiltIn" ResourceKey="imgBuiltIn" runat="server" ImageUrl="images/BuiltInPackage.gif"/>
    <asp:Label ID="BuiltInAddInNotice" runat="server" CssClass="NormalBold" resourcekey="BuiltInAddInNotice"/>
</p>
<asp:HyperLink ID="InstallVisualizerLink" runat="server" CssClass="CommandButton"
               resourcekey="InstallVisualizerLink"/>
<asp:HyperLink ID="InstallDataSourceLink" runat="server" CssClass="CommandButton"
               resourcekey="InstallDataSourceLink"/>