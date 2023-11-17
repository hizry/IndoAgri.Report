using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IndoAgri.Report.Web.DataSets;
using IndoAgri.Report.Web.Models;
using IndoAgri.Security;
using Microsoft.Reporting.WebForms;

namespace IndoAgri.Report.Web.Reports.PPMS
{
    public partial class ReportPrestasiDivisi : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                // Reports/PPMS/ReportPrestasiDivisi.aspx?bkmDate=2023-04-04&divisi=04&gang=04HC03&estate=6920&company=9600
                var estate = Request.QueryString["estate"] ?? "";
                bool isEncrypt = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["isEncrypt"]);
                if (isEncrypt)
                {
                    var estateEncrypt = Request.QueryString["estate"] ?? "";
                    var key = System.Configuration.ConfigurationManager.AppSettings["key"];
                    estate = Md5Config.Decrypt(estateEncrypt, key, true);
                }
                var divisi = Request.QueryString["divisi"] ?? "";
                var company = Request.QueryString["company"] ?? "";
                var bkmDateString = Request.QueryString["bkmDate"] ?? "";
                var bkmDate = DateTime.ParseExact(bkmDateString, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

                HMSDataSet hmsdset = new HMSDataSet();
                DataTable tblPrestasi = hmsdset.Tables["spReport_PrestasiOutputHKDivisi"];  //hmsdset.Tables["SP_PrestasiOuputHKDivisi"];
                DataTable tblHeader = hmsdset.Tables["spReport_Header"];
                DataTable tblSigning = hmsdset.Tables["spReport_Signing"];

                var prestasiDivisi = new Reporting().GetReportPrestasiDivisiV2(estate, divisi, bkmDate, tblPrestasi);
                var header = new Reporting().GetReportHeader(estate, tblHeader);
                var reportSigning = new Reporting().GetSigning(bkmDate, estate, divisi, "", tblSigning);

                CheckrollPeriod checkRollperiod = new CheckrollPeriod();
                int period = checkRollperiod.GetCheckRollPeriodPeriodBy(estate, bkmDate, true);
                ReportParameter[] param = new ReportParameter[5];
                param[0] = new ReportParameter("CompanyName", company);
                param[1] = new ReportParameter("PlantName", estate);
                param[2] = new ReportParameter("Period", Convert.ToString(period));
                param[3] = new ReportParameter("BKMDate", bkmDate.ToString("yyyy-MM-dd"));
                param[4] = new ReportParameter("Division", divisi);

                var reportDataSource1 = new ReportDataSource();
                reportDataSource1.Name = "DataSetPrestasiDivisi";
                reportDataSource1.Value = prestasiDivisi;

                var reportHeader = new ReportDataSource();
                reportHeader.Name = "DataSetHeader";
                reportHeader.Value = header;

                var dsReportSigning = new ReportDataSource();
                dsReportSigning.Name = "DataSetSigning";
                dsReportSigning.Value = reportSigning;

                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.ShowExportControls = true;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("ReportPrestasiDivisi.rdlc");
                this.ReportViewer1.LocalReport.SetParameters(param);

                this.ReportViewer1.LocalReport.DataSources.Add(reportDataSource1);
                this.ReportViewer1.LocalReport.DataSources.Add(reportHeader);
                this.ReportViewer1.LocalReport.DataSources.Add(dsReportSigning);
            }
        }
    }
}