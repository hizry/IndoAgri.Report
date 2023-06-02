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
    public partial class RptAnomali : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Reports/PPMS/RptAnomali.aspx?bkmDate=2023-04-04&divisi=04
                HMSDataSet hmsdset = new HMSDataSet();
                DataTable tblAnomali = hmsdset.Tables["SPS_REPORT_ANOMALI"];

                var divisi = Request.QueryString["divisi"] ?? "";
                var bkmDateString = Request.QueryString["bkmDate"] ?? "";
                var bkmDate = DateTime.ParseExact(bkmDateString, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);


                tblAnomali = new Reporting().GetReportAnomali(bkmDate.Date, divisi, tblAnomali);

                ReportDataSource rds_hdr = new ReportDataSource("DataSet1", tblAnomali);

                ReportParameter[] param = new ReportParameter[2];
                param[0] = new ReportParameter("Date", bkmDate.Date.ToString("dd-MM-yyyy"));
                param[1] = new ReportParameter("Division", divisi);

                this.ReportViewer1.LocalReport.ReportEmbeddedResource = "RptAnomali.rdlc";
                ReportViewer1.ShowExportControls = true;
                this.ReportViewer1.LocalReport.DataSources.Add(rds_hdr);
                this.ReportViewer1.LocalReport.SetParameters(param);

            }
        }
    }
}