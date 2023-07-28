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
    public partial class CetakBukuPanen_print : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Reports/PPMS/CetakBukuPanen2.aspx?bkmDate=2023-04-04&divisi=04&gang=04HC03
                var divisi = Request.QueryString["divisi"] ?? "";
                var estate = Request.QueryString["estate"] ?? "";
                var gang = Request.QueryString["gang"] ?? "";
                var bkmDateString = Request.QueryString["bkmDate"] ?? "";
                var bkmDate = DateTime.ParseExact(bkmDateString, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

                HMSDataSet hmsdset = new HMSDataSet();
                DataTable tblHeader = hmsdset.Tables["spReport_CETAK_BUKUPANEN_HEADER_Online"]; //hmsdset.Tables["spReport_CETAK_BUKUPANEN_HEADER_Online"];//
                DataTable tblItemNormal = hmsdset.Tables["SPS_Cetak_BukuPanenItem_Rev1"];// hmsdset.Tables["spReport_Cetak_BukuPanenItem_Rev1"];// hmsdset.Tables["SPS_Cetak_BukuPanenItem_Rev1"];
                DataTable tblSigning = hmsdset.Tables["spReport_Signing"];

                tblHeader = new Reporting().GetCetakBukuPanenHeader(estate, bkmDate, divisi, gang, tblHeader);
                tblItemNormal = new Reporting().GetCetakBukuPanenItem(estate, bkmDate, divisi, gang, 1, "01", tblItemNormal);
                tblSigning = new Reporting().GetSigning(bkmDate, estate, divisi, gang, tblSigning);

                this.ReportViewer1.Reset();
                this.ReportViewer1.KeepSessionAlive = false;
                ReportDataSource rds_Signing = new ReportDataSource("DSSigning", tblSigning);
                ReportDataSource rds_hdr = new ReportDataSource("DSBukuPanenHdr", tblHeader);
                ReportDataSource rds_Normal = new ReportDataSource("DSITEM_NORMAL", tblItemNormal);
                
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.ShowExportControls = true;

                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("CetakBukuPanenV5.rdlc");

                this.ReportViewer1.LocalReport.DataSources.Add(rds_Signing);
                this.ReportViewer1.LocalReport.DataSources.Add(rds_hdr);
                this.ReportViewer1.LocalReport.DataSources.Add(rds_Normal);

                this.ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(LocalReport_Subreport_Normal_Processing);
                this.ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(LocalReport_Subreport_Unripe_Processing);
                this.ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(LocalReport_Subreport_EmptyBunch_Processing);
                this.ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(LocalReport_Subreport_Normal_GrandTotal_Processing);
               // this.ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(Subreport_Signing);


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
            var estate = Request.QueryString["estate"] ?? "";

            DataTable result = new Reporting().GetTPH_Normal_PerNik( estate, Nik, Date, Location, Crop, Achievement, ds.Tables["GetBukuPanenItemNormal"], Gang);

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
                DataTable resultUnripe = new Reporting().GetTPH_Unripe_PerNik( estate, Nik, Date, Location, Crop, "01", ds.Tables["GetBukuPanenItemPenalty"], Gang);

                for (int i = 0; i < resultUnripe.Rows.Count; i++)
                {
                    DataRow drow = ds.Tables["GetBukuPanenItemNormal"].NewRow();
                    drow["Tph"] = "TPH" + ds.Tables["GetBukuPanenItemNormal"].Rows.Count;
                    drow["Qty"] = resultUnripe.Rows[i]["Qty"];
                    //ds.Tables["GetBukuPanenItemNormal"].Rows.Add(drow);

                    result.Rows.Add(drow);
                }

                DataTable resultEmpty = new Reporting().GetTPH_EmptyBunch_PerNik( estate, Nik,
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

        void Subreport_Signing(object sender, SubreportProcessingEventArgs e)
        {
            var divisi = Request.QueryString["divisi"] ?? "";
            var estate = Request.QueryString["estate"] ?? "";
            var gang = Request.QueryString["gang"] ?? "";
            var bkmDateString = Request.QueryString["bkmDate"] ?? "";
            var bkmDate = DateTime.ParseExact(bkmDateString, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

            HMSDataSet hmsdset = new HMSDataSet();
            DataTable reportSignin = hmsdset.Tables["spReport_Signing"];

            reportSignin = new Reporting().GetSigning(bkmDate, estate, divisi, gang, reportSignin);
            e.DataSources.Add(new ReportDataSource("DataSetSigning", reportSignin));
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
                var estate = Request.QueryString["estate"] ?? "";
                //DataTable result = new Reporting().GetTPH_Unripe_PerNik(Nik, Date, Location, Crop, Achievement, ds.Tables["GetBukuPanenItemNormal"], Gang);
                DataTable result = new Reporting().GetTPH_Unripe_PerNik( estate, Nik, Date, Location, Crop, "01", ds.Tables["GetBukuPanenItemPenalty"], Gang);
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
                var estate = Request.QueryString["estate"] ?? "";
                //DataTable result = new Reporting().GetTPH_Unripe_PerNik(Nik, Date, Location, Crop, Achievement, ds.Tables["GetBukuPanenItemNormal"], Gang);
                DataTable result = new Reporting().GetTPH_Unripe_PerNik( estate ,Nik, Date, Location, Crop, "01", ds.Tables["GetBukuPanenItemPenalty"], Gang);
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
                var estate = Request.QueryString["estate"] ?? "";
                result = new Reporting().GetTPH_EmptyBunch_PerNik( estate, Nik,
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
                var estate = Request.QueryString["estate"] ?? "";
                result = new Reporting().GetTPH_EmptyBunch_PerNik( estate , Nik,
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
            var estate = Request.QueryString["estate"] ?? "";
            DataTable result = new Reporting().GetTPH_Normal_PerNik( estate , Nik, Date, Location, Crop, Achievement, ds.Tables["GetBukuPanenItemNormal"], Gang);
            if (e.ReportPath == "BPN_Normal_GrandTotal")
            {
                e.DataSources.Add(new ReportDataSource("DS_Normal", result));
            }
        }
    }
}