<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewReport.aspx.cs" Inherits="triluatsoft.tls.Web.Reports.ViewReport" %>

<%@ Register assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692FBEA5521E1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" 
            AutoDataBind="true" OnInit="CrystalReportViewer1_Init" HasToggleParameterPanelButton="false"
        HasToggleGroupTreeButton="false"
        HasCrystalLogo="False"
        HasSearchButton="False"
        DisplayToolbar="True"
        ToolPanelView="None"
        EnableDatabaseLogonPrompt="False"
        EnableParameterPrompt="False"/>
        
    </form>
</body>
</html>
