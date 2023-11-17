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
    public partial class FingerScanList_print : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Reports/PPMS/CetakBukuPanen2.aspx?bkmDate=2023-04-04&divisi=04&gang=04HC03

                //ReportParameter[] parm = new ReportParameter[9];
                //parm[0] = new ReportParameter("sCompanyCode", ddlCompany.SelectedValue);
                //parm[1] = new ReportParameter("sBACode", ddlBusinessArea.SelectedValue);
                //parm[2] = new ReportParameter("sPeriod1", DateTime.Parse(tbDateFrom.Text).ToString());
                //parm[3] = new ReportParameter("sPeriod2", DateTime.Parse(tbDateUntil.Text).ToString());
                //parm[4] = new ReportParameter("sNIK", txtNIK.Text);
                //parm[5] = new ReportParameter("sDivisi", ddlDivision.SelectedValue);
                //parm[6] = new ReportParameter("Info", strCompany + " / " + strBa);
                //parm[7] = new ReportParameter("UserLogin", User.Identity.Name);
                //parm[8] = new ReportParameter("ShowWatermark", ShowWaterMark);

                var divisi = Request.QueryString["companyCode"] ?? "";
                var estate = Request.QueryString["estate"] ?? "";
                var gang = Request.QueryString["gang"] ?? "";
                var bkmDateString = Request.QueryString["bkmDate"] ?? "";
                var bkmDate = DateTime.ParseExact(bkmDateString, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

                

                HMSDataSet hmsdset = new HMSDataSet();
                DataTable tblHeader = hmsdset.Tables["spFingerScanList"]; 
                tblHeader = new Reporting().GetCetakBukuPanenHeader(estate, bkmDate, divisi, gang, tblHeader);

                this.ReportViewer1.Reset();
                this.ReportViewer1.KeepSessionAlive = false;
                ReportDataSource rds_hdr = new ReportDataSource("DSBukuPanenHdr", tblHeader);
                
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.ShowExportControls = true;

                this.ReportViewer1.LocalReport.ReportPath = Server.MapPath("FingerScanList.rdlc");
                this.ReportViewer1.LocalReport.DataSources.Add(rds_hdr);

                //this.ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(LocalReport_Subreport_Normal_Processing);
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

            if (e.ReportPath == "BPN_NSum")
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
    }
}