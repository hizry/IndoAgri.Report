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
    public partial class CetakBukuPanen2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Reports/PPMS/CetakBukuPanen2.aspx?bkmDate=2023-04-04&divisi=04&gang=04HC03
                var divisi = Request.QueryString["divisi"] ?? "";
                var gang = Request.QueryString["gang"] ?? "";
                var bkmDateString = Request.QueryString["bkmDate"] ?? "";
                var bkmDate = DateTime.ParseExact(bkmDateString, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

                HMSDataSet hmsdset = new HMSDataSet();
                DataTable tblHeader = hmsdset.Tables["SPS_CETAK_BUKUPANEN_HEADER_REV1"];
                DataTable tblItemNormal = hmsdset.Tables["SPS_Cetak_BukuPanenItem_Rev1"];
                tblHeader = new Reporting().GetCetakBukuPanenHeader(bkmDate, divisi, gang, tblHeader);
                tblItemNormal = new Reporting().GetCetakBukuPanenItem(bkmDate, divisi, gang, 1, "01", tblItemNormal);

                this.ReportViewer1.Reset();

                ReportDataSource rds_hdr = new ReportDataSource("DSBukuPanenHdr", tblHeader);
                ReportDataSource rds_Normal = new ReportDataSource("DSITEM_NORMAL", tblItemNormal);
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("CetakBukuPanen2.rdlc");
                ReportViewer1.ShowExportControls = true;
                this.ReportViewer1.LocalReport.DataSources.Add(rds_hdr);
                this.ReportViewer1.LocalReport.DataSources.Add(rds_Normal);

                this.ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(LocalReport_Subreport_Normal_Processing);
                this.ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(LocalReport_Subreport_Unripe_Processing);
                this.ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(LocalReport_Subreport_EmptyBunch_Processing);
                this.ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(LocalReport_Subreport_Normal_GrandTotal_Processing);

                //this.reportViewer1.LocalReport.SubreportProcessing += new Microsoft.Reporting.WinForms.SubreportProcessingEventHandler(LocalReport_Subreport_BorongHarianSum_Processing);
                //this.ReportViewer1.LocalReport.DataSources.Clear();
            }
        }


        void LocalReport_Subreport_Normal_Processing(object sender, SubreportProcessingEventArgs e)
        {
            HMSDataSet ds = new HMSDataSet();
            string Nik = e.Parameters["Nik"].Values[0].ToString();
            DateTime Date = Convert.ToDateTime(e.Parameters["Date"].Values[0].ToString());
            string Location = e.Parameters["Location"].Values[0].ToString();
            string Crop = e.Parameters["Crop"].Values[0].ToString();
            string Achievement = e.Parameters["Achievement"].Values[0].ToString();
            string Gang = e.Parameters["Gang"].Values[0].ToString();
            DataTable result = new Reporting().GetTPH_Normal_PerNik(Nik, Date, Location, Crop, Achievement, ds.Tables["GetBukuPanenItemNormal"], Gang);

            if (e.ReportPath == "BPN_Normal")
            {
                e.DataSources.Add(new ReportDataSource("DS_Normal", result));
            }

            if (e.ReportPath == "BPN_NormalSum")
            {
                e.DataSources.Add(new ReportDataSource("DS_Normal", result));
            }

            if (e.ReportPath == "BPN_SumNAE")
            {
                DataTable resultUnripe = new Reporting().GetTPH_Unripe_PerNik(Nik, Date, Location, Crop, "01", ds.Tables["GetBukuPanenItemPenalty"], Gang);

                for (int i = 0; i < resultUnripe.Rows.Count; i++)
                {
                    DataRow drow = ds.Tables["GetBukuPanenItemNormal"].NewRow();
                    drow["Tph"] = "TPH" + ds.Tables["GetBukuPanenItemNormal"].Rows.Count;
                    drow["Qty"] = resultUnripe.Rows[i]["Qty"];
                    //ds.Tables["GetBukuPanenItemNormal"].Rows.Add(drow);

                    result.Rows.Add(drow);
                }

                DataTable resultEmpty = new Reporting().GetTPH_EmptyBunch_PerNik(Nik,
                   Date,
                   Location,
                   Crop,
                   Achievement == "01" ? "0" : Achievement,
                   ds.Tables["GetBukuPanenItemPenalty"], Gang);

                for (int i = 0; i < resultEmpty.Rows.Count; i++)
                {
                    DataRow drow = ds.Tables["GetBukuPanenItemNormal"].NewRow();
                    drow["Tph"] = "TPH" + ds.Tables["GetBukuPanenItemNormal"].Rows.Count;
                    drow["Qty"] = resultEmpty.Rows[i]["Qty"];
                    result.Rows.Add(drow);
                }

                e.DataSources.Add(new ReportDataSource("DS_Normal", result));
            }
        }

        void LocalReport_Subreport_Unripe_Processing(object sender, SubreportProcessingEventArgs e)
        {
            if (e.ReportPath == "BPN_UNripe")
            {
                HMSDataSet ds = new HMSDataSet();
                string Nik = e.Parameters["Nik"].Values[0].ToString();
                DateTime Date = Convert.ToDateTime(e.Parameters["Date"].Values[0].ToString());
                string Location = e.Parameters["Location"].Values[0].ToString();
                string Crop = e.Parameters["Crop"].Values[0].ToString();
                string Achievement = e.Parameters["Achievement"].Values[0].ToString();
                string Gang = e.Parameters["Gang"].Values[0].ToString();
                //DataTable result = new Reporting().GetTPH_Unripe_PerNik(Nik, Date, Location, Crop, Achievement, ds.Tables["GetBukuPanenItemNormal"], Gang);
                DataTable result = new Reporting().GetTPH_Unripe_PerNik(Nik, Date, Location, Crop, "01", ds.Tables["GetBukuPanenItemPenalty"], Gang);
                e.DataSources.Add(new ReportDataSource("DS_Normal", result));
            }

            if (e.ReportPath == "BPN_UNripeSum")
            {
                HMSDataSet ds = new HMSDataSet();
                string Nik = e.Parameters["Nik"].Values[0].ToString();
                DateTime Date = Convert.ToDateTime(e.Parameters["Date"].Values[0].ToString());
                string Location = e.Parameters["Location"].Values[0].ToString();
                string Crop = e.Parameters["Crop"].Values[0].ToString();
                string Achievement = e.Parameters["Achievement"].Values[0].ToString();
                string Gang = e.Parameters["Gang"].Values[0].ToString();
                //DataTable result = new Reporting().GetTPH_Unripe_PerNik(Nik, Date, Location, Crop, Achievement, ds.Tables["GetBukuPanenItemNormal"], Gang);
                DataTable result = new Reporting().GetTPH_Unripe_PerNik(Nik, Date, Location, Crop, "01", ds.Tables["GetBukuPanenItemPenalty"], Gang);
                e.DataSources.Add(new ReportDataSource("DS_Normal", result));
            }
        }

        void LocalReport_Subreport_EmptyBunch_Processing(object sender, SubreportProcessingEventArgs e)
        {
            if (e.ReportPath == "BPN_Empty")
            {
                HMSDataSet ds = new HMSDataSet();
                DataTable result = new DataTable();
                string Nik = e.Parameters["Nik"].Values[0].ToString();
                DateTime Date = Convert.ToDateTime(e.Parameters["Date"].Values[0].ToString());
                string Location = e.Parameters["Location"].Values[0].ToString();
                string Crop = e.Parameters["Crop"].Values[0].ToString();
                string Achievement = e.Parameters["Achievement"].Values[0].ToString();
                string Gang = e.Parameters["Gang"].Values[0].ToString();
                result = new Reporting().GetTPH_EmptyBunch_PerNik(Nik,
                    Date,
                    Location,
                    Crop,
                    Achievement == "01" ? "0" : Achievement,
                    ds.Tables["GetBukuPanenItemPenalty"], Gang);
                e.DataSources.Add(new ReportDataSource("DS_Normal", result));
            }

            if (e.ReportPath == "BPN_EmptySum")
            {
                HMSDataSet ds = new HMSDataSet();
                DataTable result = new DataTable();
                string Nik = e.Parameters["Nik"].Values[0].ToString();
                DateTime Date = Convert.ToDateTime(e.Parameters["Date"].Values[0].ToString());
                string Location = e.Parameters["Location"].Values[0].ToString();
                string Crop = e.Parameters["Crop"].Values[0].ToString();
                string Achievement = e.Parameters["Achievement"].Values[0].ToString();
                string Gang = e.Parameters["Gang"].Values[0].ToString();
                result = new Reporting().GetTPH_EmptyBunch_PerNik(Nik,
                    Date,
                    Location,
                    Crop,
                    Achievement == "01" ? "0" : Achievement,
                    ds.Tables["GetBukuPanenItemPenalty"], Gang);
                e.DataSources.Add(new ReportDataSource("DS_Normal", result));
            }
        }

        void LocalReport_Subreport_Normal_GrandTotal_Processing(object sender, SubreportProcessingEventArgs e)
        {
            HMSDataSet ds = new HMSDataSet();
            string Nik = e.Parameters["Nik"].Values[0].ToString();
            DateTime Date = Convert.ToDateTime(e.Parameters["Date"].Values[0].ToString());
            string Location = e.Parameters["Location"].Values[0].ToString();
            string Crop = e.Parameters["Crop"].Values[0].ToString();
            string Achievement = e.Parameters["Achievement"].Values[0].ToString();
            string Gang = e.Parameters["Gang"].Values[0].ToString();
            DataTable result = new Reporting().GetTPH_Normal_PerNik(Nik, Date, Location, Crop, Achievement, ds.Tables["GetBukuPanenItemNormal"], Gang);
            if (e.ReportPath == "BPN_Normal_GrandTotal")
            {
                e.DataSources.Add(new ReportDataSource("DS_Normal", result));
            }
        }
    }
}