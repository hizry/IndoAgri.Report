using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IndoAgri.Report.Web.DataSets;
using IndoAgri.Report.Web.Models;
using Microsoft.Reporting.WebForms;

namespace IndoAgri.Report.Web.Reports.PPMS
{
    public partial class ReportAnomaliPanenVsAngkut_print : System.Web.UI.Page   
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (! IsPostBack)
            {
                // Reports/PPMS/ReportAnomaliPanenVsAngkut.aspx?from=2023-03-25&to=2023-04-04&block=H22P17&estate=3520
                var block = Request.QueryString["block"] ?? "";
                var estate = Request.QueryString["estate"] ?? "";
                var fromString = Request.QueryString["fromDate"] ?? "";
                var fromDate = DateTime.ParseExact(fromString, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                var toString = Request.QueryString["toDate"] ?? "";
                var toDate = DateTime.ParseExact(toString, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

                HMSDataSet hmsdset = new HMSDataSet();
                DataTable tbl = hmsdset.Tables["spReport_ANOMALIPANENVSANGKUT"];
                DataTable tblHeader = hmsdset.Tables["spReport_Header"];

                tbl = new Reporting().GetRptAnomaliPanenVsAngkut(fromDate, toDate, block, estate,tbl);
                tblHeader = new Reporting().GetReportHeader(estate, tblHeader);
                this.ReportViewer1.Reset();
                ReportDataSource rds = new ReportDataSource("DSAnomaliPanenVsAngkut", tbl);
                ReportDataSource rdsHeader = new ReportDataSource("DataSetHeader", tblHeader);

                ReportParameter[] param = new ReportParameter[3];
                param[0] = new ReportParameter("Start", fromDate.ToString("dd-MM-yyyy"));
                param[1] = new ReportParameter("End", toDate.ToString("dd-MM-yyyy"));
                param[2] = new ReportParameter("Block", block);
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("ReportAnomaliPanenVsAngkut.rdlc");
                ReportViewer1.ShowExportControls = true;
                this.ReportViewer1.LocalReport.SetParameters(param);
                this.ReportViewer1.LocalReport.DataSources.Add(rds);
                this.ReportViewer1.LocalReport.DataSources.Add(rdsHeader);

            }
        }
    }
}