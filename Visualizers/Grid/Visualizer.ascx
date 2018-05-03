<%@ Control Language="C#" AutoEventWireup="true" Inherits="DotNetNuke.Modules.Reports.Visualizers.Grid.Visualizer" Codebehind="Visualizer.ascx.cs" %>
<asp:GridView ID="grdResults" runat="server" Width="100%"
              EnableTheming="True" AutoGenerateColumns="false" OnPageIndexChanging="grdResults_PageIndexChanging" OnSorting="grdResults_Sorting" OnRowDataBound="grdResults_RowDataBound">
    <HeaderStyle CssClass="Subhead DNN_Reports_Grid_Header" HorizontalAlign="Left"/>
    <RowStyle CssClass="Normal DNN_Reports_Grid_Row" HorizontalAlign="Left"/>
    <AlternatingRowStyle CssClass="Normal DNN_Reports_Grid_AlternatingRow"/>
    <PagerStyle CssClass="Normal DNN_Reports_Grid_Pager" HorizontalAlign="Center"/>
    <FooterStyle CssClass="Normal"/>
</asp:GridView>