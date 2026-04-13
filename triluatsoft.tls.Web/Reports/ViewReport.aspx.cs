using Abp.Authorization;
using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using triluatsoft.tls.Authorization;

namespace triluatsoft.tls.Web.Reports
{
    public partial class ViewReport : System.Web.UI.Page
    {
        //ReportDocument RptDoc;
        protected void Page_Load(object sender, EventArgs e)
        {
            ////CrystalReportViewer1.ReportSource = Session["report"];
        }
        private void Page_Init(object sender, EventArgs e)
        {
            //move from page_load to page_init 
            //to fix load only 2 pages
            if (Session["report"] != null)
            {
                ReportDocument rpt = (ReportDocument)Session["report"];
                CrystalReportViewer1.ReportSource = (ReportDocument)Session["report"];
                CrystalReportViewer1.DataBind();
                if (rpt.FileName.Contains("ClaimBordereauxReport") || rpt.FileName.Contains("ExpenseReport")
                    || rpt.FileName.Contains("RevenueWIPReport") || rpt.FileName.Contains("AccountsReceivableReport"))
                {
                    CrystalReportViewer1.Zoom(75);
                }
                
            }
        }
        protected void CrystalReportViewer1_Init(object sender, EventArgs e)
        {

        }
    }
}