<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Excel.aspx.cs" Inherits="DotNetNuke.Modules.Reports.Excel"  EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
       <asp:GridView runat="server" ID="grd" AutoGenerateColumns="true" CellPadding="4" CellSpacing="1" CssClass="Normal">
		</asp:GridView>
    </form>
</body>
</html>
