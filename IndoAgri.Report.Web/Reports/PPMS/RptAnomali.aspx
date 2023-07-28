<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RptAnomali.aspx.cs" Inherits="IndoAgri.Report.Web.Reports.PPMS.RptAnomali" %>
<%@Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
   <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" AsyncRendering="false" Height="900px" Style="display: block; min-width: 1150px;
        min-height: 250px" ShowToolBar="false"></rsweb:ReportViewer>
    </form>
</body>
</html>
