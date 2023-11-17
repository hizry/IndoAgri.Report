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
    public partial class RptAnomali_print : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Reports/PPMS/RptAnomali.aspx?bkmDate=2023-04-04&divisi=04
                HMSDataSet hmsdset = new HMSDataSet();
                DataTable tblAnomali = hmsdset.Tables["spReport_ANOMALI"];
                DataTable tblHeader = hmsdset.Tables["spReport_Header"];

                var estate = Request.QueryString["estate"] ?? "";
                bool isEncrypt = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["isEncrypt"]);
                if (isEncrypt)
                {
                    var estateEncrypt = Request.QueryString["estate"] ?? "";
                    var key = System.Configuration.ConfigurationManager.AppSettings["key"];
                    estate = Md5Config.Decrypt(estateEncrypt, key, true);
                }
                var divisi = Request.QueryString["divisi"] ?? "";
                var bkmDateString = Request.QueryString["bkmDate"] ?? "";
                var bkmDate = DateTime.ParseExact(bkmDateString, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);

               // new Reporting().spReport_ANOMALI(estate, bkmDate.Date, divisi, tblAnomali);

                tblAnomali = new Reporting().GetReportAnomali(estate, bkmDate.Date, divisi, tblAnomali);
                tblHeader = new Reporting().GetReportHeader(estate, tblHeader);

                ReportDataSource rds_hdr = new ReportDataSource("DataSet1", tblAnomali);
                ReportDataSource rds_header = new ReportDataSource("DataSetHeader", tblHeader);


                ReportParameter[] param = new ReportParameter[2];
                param[0] = new ReportParameter("Date", bkmDate.Date.ToString("yyyy-MM-dd"));
                param[1] = new ReportParameter("Division", divisi);

                ReportViewer1.LocalReport.ReportPath = Server.MapPath("RptAnomali.rdlc");
                //this.ReportViewer1.LocalReport.ReportEmbeddedResource = "RptAnomali.rdlc";
                ReportViewer1.ShowExportControls = true;
                this.ReportViewer1.LocalReport.DataSources.Add(rds_hdr);
                this.ReportViewer1.LocalReport.DataSources.Add(rds_header);
                this.ReportViewer1.LocalReport.SetParameters(param);
            }
        }
    }
}