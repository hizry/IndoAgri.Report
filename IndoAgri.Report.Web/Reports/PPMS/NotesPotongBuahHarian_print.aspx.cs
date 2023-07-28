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
    public partial class NotesPotongBuahHarian_print : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Reports/PPMS/NotesPotongBuahHarian.aspx?startDate=2022-12-20&finishDate=2022-12-22&nik=200500686&gang=03HC01
                var nik = Request.QueryString["nik"] ?? "";
                var gang = Request.QueryString["gang"] ?? "";
                var startDateString = Request.QueryString["fromDate"] ?? "";
                var startDate = DateTime.ParseExact(startDateString, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                var finishDateString = Request.QueryString["toDate"] ?? "";
                var finishDate = DateTime.ParseExact(finishDateString, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

                HMSDataSet hmsdset = new HMSDataSet();
                DataTable tbl = hmsdset.Tables["SPS_POTONGBUAH"];
                tbl = new Reporting().GetRptPotongBuah_Detail(startDate, finishDate, gang, nik, tbl);
                this.ReportViewer1.Reset();
                ReportDataSource rds = new ReportDataSource("DSNotesPotBuahPerDay", tbl);
                ReportParameter[] param = new ReportParameter[3];
                param[0] = new ReportParameter("Dari",startDate.ToString("dd-MM-yyyy"));
                param[1] = new ReportParameter("Sampai", finishDate.ToString("dd-MM-yyyy"));
                param[2] = new ReportParameter("Gang", gang);
                ReportViewer1.ShowExportControls = true;
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("NotesPotongBuahHarian.rdlc");
                this.ReportViewer1.LocalReport.SetParameters(param);
                this.ReportViewer1.LocalReport.DataSources.Add(rds);

                this.ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(LocalReport_Subreport_PotongBuah_Processing);
            }
        }

        void LocalReport_Subreport_PotongBuah_Processing(object sender, SubreportProcessingEventArgs e)
        {
            HMSDataSet hmsdset = new HMSDataSet();
            DataTable tbl = hmsdset.Tables["SPS_POTONGBUAH"];

            DateTime Start = Convert.ToDateTime(e.Parameters["Start"].Values[0].ToString());
            DateTime Finish = Convert.ToDateTime(e.Parameters["Finish"].Values[0].ToString());
            string Gang = e.Parameters["Gang"].Values[0].ToString();
            string Nik = e.Parameters["Nik"].Values[0].ToString();
            DataTable result = new Reporting().GetRptPotongBuah_Detail(Start, Finish, Gang, Nik, tbl);
            if (e.ReportPath == "NotesPotongBuah")
            {
                e.DataSources.Add(new ReportDataSource("DS_PotongBuah", result));
            }
        }
    }
}