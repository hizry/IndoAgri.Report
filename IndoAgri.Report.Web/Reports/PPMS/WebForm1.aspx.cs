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
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Reports/PPMS/CETAK_BKMHK.aspx?bkmDate=2023-04-04&divisi=04&gang=04HC03
                var divisi = Request.QueryString["divisi"] ?? "";
                var gang = Request.QueryString["gang"] ?? "";
                var bkmDateString = Request.QueryString["bkmDate"] ?? "";
                var bkmDate = DateTime.ParseExact(bkmDateString, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

                HMSDataSet hmsdset = new HMSDataSet();
                DataTable tblHeader = hmsdset.Tables["SPS_CETAK_BKMHK"];
                DataTable tblDetail = hmsdset.Tables["SPS_CETAK_BKMHK_DETAIL_DDT"];
                tblHeader = new Reporting().GetCetakBKMHK_Header(bkmDate, divisi, gang, tblHeader);
                tblDetail = new Reporting().GetCetakBKMHK_Detail(bkmDate, divisi, gang, tblDetail);

                this.ReportViewer1.Reset();
                ReportDataSource rds_hdr = new ReportDataSource("DSHdr", tblHeader);
                ReportDataSource rds_Detail = new ReportDataSource("DSDetail", tblDetail);
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.ShowExportControls = true;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("CETAK_BKMHK.rdlc");

                this.ReportViewer1.LocalReport.DataSources.Add(rds_hdr);
                this.ReportViewer1.LocalReport.DataSources.Add(rds_Detail);
            }
        }
    }
}