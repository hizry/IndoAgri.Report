﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CetakBukuPanen.aspx.cs" Inherits="IndoAgri.Report.Web.Reports.PPMS.CetakBukuPanen" %>
<%@Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" AsyncRendering="false"></rsweb:ReportViewer>
    </form>
</body>
</html>
