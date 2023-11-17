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
    public partial class Taksasi : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                HMSDataSet hmsdset = new HMSDataSet();
                DataTable tblHeader = hmsdset.Tables["spReport_HeaderTaksasi"];
                DataTable tblTaksasi = hmsdset.Tables["spReport_Taksasi"];

                var estate = Request.QueryString["estate"] ?? "";
                bool isEncrypt = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["isEncrypt"]);
                if (isEncrypt)
                {
                    var estateEncrypt = Request.QueryString["estate"] ?? "";
                    var key = System.Configuration.ConfigurationManager.AppSettings["key"];
                    estate = Md5Config.Decrypt(estateEncrypt, key, true);
                }
                var divisi = Request.QueryString["divisi"] ?? "";
                var year = Request.QueryString["year"] ?? "";
                var period = Request.QueryString["period"] ?? "";
                var startDateString = Request.QueryString["fromDate"] ?? "";
                var endDateString = Request.QueryString["toDate"] ?? "";

                DateTime startDate = DateTime.ParseExact(startDateString, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                DateTime endDate = DateTime.ParseExact(endDateString, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

                tblHeader = new Reporting().GetReportHeaderTaksasi(estate, divisi, Convert.ToInt32(year), tblHeader);
                tblTaksasi = new Reporting().GetReportTaksasi(estate, divisi, Convert.ToInt32(year), Convert.ToInt32(period), startDate, endDate, tblTaksasi);

                ReportDataSource rds_header = new ReportDataSource("DataSetHeader", tblHeader);
                ReportDataSource rds_taksasi = new ReportDataSource("DataSetTaksasi", tblTaksasi);


                ReportParameter[] param = new ReportParameter[6];
                param[0] = new ReportParameter("estate", estate);
                param[1] = new ReportParameter("division", divisi);
                param[2] = new ReportParameter("year", year);
                param[3] = new ReportParameter("year", period);
                param[4] = new ReportParameter("startDate", startDateString);
                param[5] = new ReportParameter("startDate", endDateString);


                ReportViewer1.LocalReport.ReportPath = Server.MapPath("Taksasi.rdlc");
                ReportViewer1.ShowExportControls = true;
                this.ReportViewer1.LocalReport.DataSources.Add(rds_header);
                this.ReportViewer1.LocalReport.DataSources.Add(rds_taksasi);
            }
        }
    }
}