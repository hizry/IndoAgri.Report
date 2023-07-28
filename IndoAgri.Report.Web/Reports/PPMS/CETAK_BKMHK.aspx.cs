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
    public partial class CETAK_BKMHK : System.Web.UI.Page   
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Reports/PPMS/CETAK_BKMHK.aspx?bkmDate=2023-04-04&divisi=04&gang=04HC03
                var divisi = Request.QueryString["divisi"] ?? "";
                var estate = Request.QueryString["estate"] ?? "";
                var gang = Request.QueryString["gang"] ?? "";
                var bkmDateString = Request.QueryString["bkmDate"] ?? "";
                var bkmDate = DateTime.ParseExact(bkmDateString, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

                HMSDataSet hmsdset = new HMSDataSet();
                DataTable tblHeader = hmsdset.Tables["spReport_Cetak_BKM_HK"];
                DataTable tblDetail = hmsdset.Tables["spReport_CETAK_BKMHK_DETAIL_Online"];
                DataTable reportSignin = hmsdset.Tables["spReport_Signing"];
                tblHeader = new Reporting().GetCetakBKMHK_Header(bkmDate, estate, divisi, gang, tblHeader);
                tblDetail = new Reporting().GetCetakBKMHK_Detail(bkmDate, estate, divisi, gang, tblDetail);
                reportSignin = new Reporting().GetSigning(bkmDate, estate, divisi, gang, reportSignin);

                this.ReportViewer1.Reset();
                this.ReportViewer1.KeepSessionAlive = false;
                //this.ReportViewer1.AsyncRendering = false;
                ReportDataSource rds_hdr = new ReportDataSource("DSHdr", tblHeader);
                ReportDataSource rds_Detail = new ReportDataSource("DSDetail", tblDetail);
                ReportDataSource rds_reportSign = new ReportDataSource("DSSigning", reportSignin);

                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.ShowExportControls = true;

                ReportViewer1.LocalReport.ReportPath = Server.MapPath("CETAK_BKMHK.rdlc");

                this.ReportViewer1.LocalReport.DataSources.Add(rds_hdr);
                this.ReportViewer1.LocalReport.DataSources.Add(rds_Detail);
                this.ReportViewer1.LocalReport.DataSources.Add(rds_reportSign);

            }
        }
    }
}