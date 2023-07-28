using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace IndoAgri.Report.Web.Models
{
    public class Reporting
    {
        //private static string connstring = new BaseClass().ConnectionString();

        //public DataTable GetRptSummaryOutputJjgPerBlock(DateTime start, DateTime finish, string block, DataTable tblin)
        //{

        //    DataTable tbl = new DataTable();
        //    using (PPMSEntities context = new PPMSEntities(connstring))
        //    {
        //        var qry = context.SPS_SUMMARYOUPUTPERBLOCK(start, finish, block);
        //        foreach (var item in qry)
        //        {
        //            DataRow dr = tblin.NewRow();
        //            dr["Estate"] = item.Estate;
        //            dr["HarvestDate"] = item.HarvestDate;
        //            dr["Block"] = item.Block;
        //            dr["QtyJjg"] = item.QtyJjg == null ? 0 : item.QtyJjg;
        //            dr["BJR"] = item.BJR == null ? 0 : item.BJR;
        //            dr["KgJjg"] = item.KgJjg == null ? 0 : item.KgJjg;
        //            dr["KgLooseFruit"] = item.KgLooseFruit == null ? 0 : item.KgLooseFruit;
        //            dr["Total_jjg_LooseFruit"] = item.Total_jjg_LooseFruit == null ? 0 : item.Total_jjg_LooseFruit;
        //            dr["HADone"] = item.HADone == null ? 0 : item.HADone;
        //            tblin.Rows.Add(dr);
        //        }
        //    }
        //    tbl = tblin;
        //    return tbl;
        //}
        //public DataTable GetRptSummaryOutputJjgPerNik(DateTime start, DateTime finish, string Gang, string Nik, DataTable tblin)
        //{
        //    DataTable tbl = new DataTable();
        //    using (HMSEntities context = new HMSEntities(connstring))
        //    {
        //        var qry = context.SPS_SUMMARYOUTPUTJJGPERHARVESTER(start, finish, Gang, Nik);
        //        foreach (var item in qry)
        //        {
        //            DataRow dr = tblin.NewRow();
        //            dr["Estate"] = item.Estate;
        //            dr["Nik"] = item.Nik;
        //            dr["Nama"] = item.Nama;
        //            dr["Harvest_DATE"] = item.Harvest_DATE;
        //            dr["Block"] = item.Block;
        //            dr["TPH"] = item.TPH == null ? " " : item.TPH;
        //            dr["QtyJjg"] = item.QtyJjg == null ? 0 : item.QtyJjg;
        //            dr["KgJjg"] = item.KgJjg == null ? 0 : item.KgJjg;
        //            dr["KgLooseFruit"] = item.KgLooseFruit == null ? 0 : item.KgLooseFruit;
        //            dr["Total"] = item.Total == null ? 0 : item.Total;
        //            dr["Output"] = item.Output == null ? 0 : item.Output;
        //            tblin.Rows.Add(dr);
        //        }

        //    }
        //    tbl = tblin;
        //    return tbl;
        //}
        public DataTable GetRptAnomaliPanenVsAngkut(DateTime start, DateTime finish, string Block, string estate ,DataTable tblIn)
        {
            DataTable tbl = new DataTable();
            using (PPMSEntities context = new PPMSEntities())
            {
                context.Database.CommandTimeout = 300;
                var qry = context.spReport_ANOMALIPANENVSANGKUT(start, finish, Block, estate ).ToList();
                foreach (var item in qry)
                {
                    DataRow dr = tblIn.NewRow();

                    dr["HarvestDate"] = item.HarvestDate;
                    dr["Location"] = item.location;
                    dr["TPH"] = item.tph;
                    dr["QtyJjg"] = item.QtyJjg == null ? 0 : item.QtyJjg;
                    dr["QtyAngkut"] = item.QtyAngkut == null ? 0 : item.QtyAngkut;
                    dr["Restan"] = item.Restan == null ? 0 : item.Restan;
                    dr["KodeKendaraan"] = item.KodeKendaraan == null ? "" : item.KodeKendaraan;
                    dr["Mandor"] = item.Mandor == null ? " " : item.Mandor;
                    dr["Driver"] = item.driver == null ? " " : item.driver;
                    dr["Achievement"] = item.Achievement == null ? " " : item.Achievement;

                    tblIn.Rows.Add(dr);
                }

            }
            tbl = tblIn;
            return tbl;
        }
        public DataTable GetCetakBKMHK_Header(DateTime BKMDate, string estate , string Division, string GangCode, DataTable tblIn)
        {
            DataTable tb = new DataTable();
            using (PPMSEntities context = new PPMSEntities())
            {
                var Qry_Hdr = context.spReport_Cetak_BKM_HK(estate, BKMDate, Division, GangCode);
                foreach (var item in Qry_Hdr)
                {
                    DataRow dr = tblIn.NewRow();
                    dr["Id"] = item.Id;
                    dr["BKMNumber"] = item.BKMNumber;
                    dr["BKMDate"] = item.BKMDate;
                    dr["Company"] = item.Company;
                    dr["Division"] = item.Division;
                    dr["Gang"] = item.Gang;
                    dr["Estate"] = item.Estate;
                    dr["Nik_Foreman"] = item.Nik_Foreman == null ? "" : item.Nik_Foreman;
                    dr["Foreman"] = item.Foreman == null ? "" : item.Foreman;
                    dr["Nik_Clerk"] = item.Nik_Clerk == null ? "" : item.Nik_Clerk;
                    dr["Clerk"] = item.Clerk == null ? "" : item.Clerk;
                    tblIn.Rows.Add(dr);
                }
            }
            tb = tblIn;
            return tb;
        }

        public DataTable GetReportHeader(string estate, DataTable tblIn)
        {
            DataTable tb = new DataTable();
            using (PPMSEntities context = new PPMSEntities())
            {
                var Qry_Hdr = context.spReport_Header(estate);
                foreach (var item in Qry_Hdr)
                {
                    DataRow dr = tblIn.NewRow();
                    dr["Company"] = item.Company;
                    dr["Estate"] = item.Estate;
                    tblIn.Rows.Add(dr);
                }
            }
            tb = tblIn;
            return tb;
        }

        public DataTable GetSigning(DateTime BKMDate, string estate, string Division, string GangCode, DataTable tblIn)
        {
            using (PPMSEntities context = new PPMSEntities())
            {
                var QryDetail = context.spReport_Signing(estate, BKMDate, Division, GangCode);

                foreach (var item in QryDetail)
                {
                    DataRow drow = tblIn.NewRow();
                    //drow["Number"] = item.Number;
                    drow["bacode"] = item.bacode;
                    drow["Divisi"] = item.Divisi;
                    drow["NIKAMA"] = item.NIKAMA;
                    drow["AMA"] = item.AMA;
                    drow["NIKMANAGER"] = item.NIKMANAGER;
                    drow["MANAGER"] = item.MANAGER;
                    drow["NIKASKEP"] = item.NIKASKEP;
                    drow["ASKEP"] = item.ASKEP;
                    drow["nik_asisten"] = item.nik_asisten;
                    drow["ASISTEN"] = item.ASISTEN;
                    drow["Foreman"] = item.Foreman;
                    drow["Nik_Foreman"] = item.Nik_Foreman;
                    drow["NIKKraniDivisi"] = item.NIKKraniDivisi;
                    drow["KraniDivisi"] = item.KraniDivisi;
                    tblIn.Rows.Add(drow);
                }
            }
        
            return tblIn;
        }
      
        public DataTable GetCetakBKMHK_Detail(DateTime BKMDate, string estate, string Division, string GangCode, DataTable tblIn)
        {
            DataTable tbRes = new DataTable();

            if (GangCode.Contains("HC"))
            {
                using (PPMSEntities context = new PPMSEntities())
                {
                    var QryDetail = context.spReport_CETAK_BKMHK_DETAIL_Online(estate, BKMDate, Division, GangCode);
                    foreach (var item in QryDetail)
                    {
                        DataRow drow = tblIn.NewRow();
                        //drow["Number"] = item.Number;
                        drow["Nik"] = item.NIK;
                        drow["Name"] = item.NAME;
                        drow["AbsentType"] = item.AbsentType;
                        drow["ActivityType"] = item.ActivityType;
                        drow["Block"] = item.Block;
                        drow["Mandays"] = Convert.ToDecimal(item.Mandays);
                        drow["Output"] = item.Output;
                        drow["JobPos"] = item.JobPos;
                        drow["Premi"] = item.Premi;
                        drow["Type"] = "BL";
                        drow["Uom"] = item.Uom;
                        drow["Penalty"] = item.Penalty;
                        drow["Overtime"] = 0;
                        tblIn.Rows.Add(drow);
                    }
                }
                tbRes = tblIn;
            }
            else
            {
                using (PPMSEntities context = new PPMSEntities())
                {
                    var QryDetail = context.SPS_CETAK_BKMHK_DETAIL_DDT(BKMDate, Division, GangCode);
                    foreach (var item in QryDetail)
                    {
                        DataRow drow = tblIn.NewRow();
                        //drow["Number"] = item.Number;
                        drow["Nik"] = item.Nik;
                        drow["Name"] = item.Name;
                        drow["AbsentType"] = item.AbsentType;
                        drow["Type"] = item.Type;
                        drow["ActivityType"] = item.ActivityType;
                        drow["Block"] = item.Block;
                        drow["Mandays"] = Convert.ToDecimal(item.Mandays);
                        drow["Output"] = item.Output;
                        drow["Uom"] = item.Uom;
                        drow["JobPos"] = item.JobPos;
                        drow["Premi"] = item.Premi;
                        drow["CreatedDate"] = item.CreatedDate;
                        drow["Penalty"] = item.Penalty;
                        drow["Overtime"] = item.Overtime;
                        tblIn.Rows.Add(drow);
                    }
                }
                tbRes = tblIn;
            }

            return tbRes;
        }

        //public DataTable GetCetakSPBS_Header(string NoSPBS, DataTable tblIn)
        //{
        //    DataTable tb = new DataTable();
        //    using (PPMSEntities context = new PPMSEntities(connstring))
        //    {
        //        //var Qry_Hdr = context.CETAK_SPBS_REPORT_HEADER(NoSPBS);
        //        var Qry_Hdr = context.SPS_SPBSREPORT(NoSPBS);

        //        foreach (var item in Qry_Hdr)
        //        {
        //            DataRow dr = tblIn.NewRow();
        //            dr["SPBSDate"] = item.SPBSDate;
        //            dr["Division"] = item.Division;
        //            dr["SPBSNumb"] = item.SPBSNumb;
        //            dr["Driver"] = item.Driver;
        //            dr["LicensePlate"] = item.LicensePlate;
        //            dr["Kernet"] = item.Kernet;
        //            dr["Assistant"] = item.Assistant;
        //            dr["RunningAccount"] = item.RunningAccount;
        //            dr["CreatedDate"] = item.CreatedDate;

        //            tblIn.Rows.Add(dr);
        //        }
        //    }
        //    tb = tblIn;
        //    return tb;
        //}

        //public DataTable GetCetakSPBS_DETAIL(string NoSPBS, DataTable tblIn)
        //{
        //    DataTable tbRes = new DataTable();
        //    using (HMSEntities context = new HMSEntities(connstring))
        //    {
        //        var QryDetail = context.SPS_CETAKSPBS_DETAIL(NoSPBS);
        //        foreach (var item in QryDetail)
        //        {
        //            DataRow drow = tblIn.NewRow();
        //            //drow["Number"] = item.Number;
        //            drow["HarvestDate"] = item.HarvestDate;
        //            drow["SPBSNumb"] = item.SPBSNumb;
        //            drow["BlockId"] = item.BlockId;
        //            drow["JumlJjg"] = item.JumlJjg == null ? 0 : item.JumlJjg;
        //            drow["JumlBrodolan"] = item.JumlBrodolan == null ? 0 : item.JumlBrodolan;

        //            tblIn.Rows.Add(drow);
        //        }
        //    }
        //    tbRes = tblIn;
        //    return tbRes;
        //}

        public DataTable GetCetakBukuPanenHeader(string estate, DateTime HarvestDate, string Division, string Gang, DataTable tbIn)
        {
            DataTable tbRes = new DataTable();
            using (PPMSEntities context = new PPMSEntities())
            {
                //var a = context.SPS_CETAK_BUKUPANEN_HEADER(estate, HarvestDate, Division, Gang);
                var Qry = context.spReport_CETAK_BUKUPANEN_HEADER_Online(estate, HarvestDate, Division, Gang);
                foreach (var item in Qry)
                {
                   DataRow dr = tbIn.NewRow();
                    dr["DIVISION"] = item.DIVISION;
                    dr["GANG"] = item.GANG;
                    dr["HarvestDate"] = item.HarvestDate.Date.ToString("yyyy-MM-dd");
                    dr["ESTATE"] = item.ESTATE;
                    dr["BKMNo"] = item.BKMNo;
                    dr["Location"] = item.Location;
                    dr["Harvested"] = item.Harvested;
                    dr["MANDOR"] = item.MANDOR;
                    dr["CLERK"] = item.CLERK;
                    tbIn.Rows.Add(dr);
                }
            }

            tbRes = tbIn;
            return tbRes;
        }

        public DataTable GetCetakBukuPanenItem(string estate, DateTime HarvestDate, string Division, string Gang, int Crop, string Achievement, DataTable tbIn)
        {
            DataTable tbRes = new DataTable();
            using (PPMSEntities context = new PPMSEntities())
            {
                //var cetakBukuPanen = context.spReport_Cetak_BukuPanenItem_Rev1(estate, HarvestDate, Division, Gang, Crop, Achievement);
                var Qry = context.spReport_Cetak_BukuPanenItem_Rev1(estate , HarvestDate.Date, Division, Gang, Crop, Achievement);
                foreach (var item in Qry)
                {
                    DataRow dr = tbIn.NewRow();
                    // dr["Nomor"] = item.Nomor;
                    dr["NikHarvester"] = item.NikHarvester;
                    dr["Name"] = item.Name;
                    dr["Location"] = item.Location;
                    dr["HK"] = item.HK == null ? 0 : item.HK;
                    dr["HA"] = item.HA == null ? 0 : item.HA;
                    dr["Gang"] = item.Gang;
                    dr["JjgDinas"] = item.JjgDinas;
                    dr["OverBasic"] = item.OverBasic;
                    dr["HarvestDate"] = item.HarvestDate;
                    dr["BlockBasic"] = item.BlockBasic;
                    tbIn.Rows.Add(dr);
                }
            }
            tbRes = tbIn;

            return tbRes;
        }

        //public DataTable GetCetakBukuPanenItem_Quality(DateTime HarvestDate, string Division, string Gang, int Crop, string Achievement, DataTable tbIn)
        //{
        //    DataTable tbRes = new DataTable();
        //    using (HMSEntities context = new HMSEntities(connstring))
        //    {
        //        // var Qry = context.CETAK_BUKUPANENITEM_QUALITY_REV1(HarvestDate, Division, Gang, Crop, Achievement);
        //        var Qry = context.SPS_Cetak_BukuPanenItem_Quality_Rev1(HarvestDate, Division, Gang, Crop, Achievement);

        //        foreach (var item in Qry)
        //        {
        //            DataRow dr = tbIn.NewRow();
        //            dr["Nomor"] = item.Nomor;
        //            dr["NikHarvester"] = item.NikHarvester;
        //            dr["Name"] = item.Name;
        //            dr["Location"] = item.Location;
        //            dr["HK"] = item.HK == null ? 0 : item.HK;
        //            dr["HA"] = item.HA == null ? 0 : item.HA;
        //            dr["TPH"] = item.TPH;
        //            dr["Qty"] = item.Qty;
        //            tbIn.Rows.Add(dr);
        //        }
        //    }
        //    tbRes = tbIn;

        //    return tbRes;
        //}

        public DataTable GetTPH_Normal_PerNik(string estate, string Nik, DateTime HarvestDate, string Location, string Crop, string Achievement, DataTable temp, string Gang)
        {
            DataTable tb = new DataTable();
            using (PPMSEntities context = new PPMSEntities())
            {
                // var Qry = context.GetBukuPanenItemNormal1(Nik, HarvestDate, Location, Crop, Achievement, Gang);
                var Qry = context.spReport_GetBukuPanenItemNormal(estate, Nik, HarvestDate, Location, Crop, Achievement, Gang);

                foreach (var item in Qry)
                {

                    DataRow drow = temp.NewRow();
                    drow["Tph"] = item.Tph;
                    drow["Qty"] = item.Qty;
                    temp.Rows.Add(drow);


                }
                //cek apakah jumlah TPH sudah 8 kalau belum tambahkan sampai 8 isi quality dgn 0 utk ngepasin Column

                List<string> tphs = temp.AsEnumerable().Select(t => t.Field<string>("Tph")).Distinct().ToList();
                int countColumns = tphs.Count;
                if (countColumns < 8)
                {
                    while (countColumns < 8)
                    {
                        DataRow drow = temp.NewRow();
                        drow["Tph"] = "TPH" + temp.Rows.Count;
                        drow["Qty"] = 0;
                        temp.Rows.Add(drow);
                        countColumns += 1;
                    }
                }
                tb = temp;
            }

            return tb;
        }

        public DataTable GetTPH_Unripe_PerNik( string estate, string Nik, DateTime HarvestDate, string Location, string Crop, string Achievement, DataTable temp, string Gang)
        {
            DataTable tb = new DataTable();
            using (PPMSEntities context = new PPMSEntities())
            {
                //var Qry = context.GetBukuPanenItemNormal1(Nik, HarvestDate, Location, Crop, Achievement, Gang);
                var Qry = context.spReport_GetBukuPanenItemPenalty(estate, Nik, HarvestDate, Location, Crop, Achievement, Gang);
                foreach (var item in Qry)
                {
                    if (temp.Rows.Count < 4)
                    {
                        DataRow drow = temp.NewRow();
                        drow["Tph"] = item.Tph;
                        drow["Qty"] = item.Qty;
                        temp.Rows.Add(drow);
                    }
                    else
                    {
                        break;
                    }
                }
                //cek apakah jumlah TPH sudah 8 kalau belum tambahkan sampai 8 isi quality dgn 0 utk ngepasin Column

                List<string> tphs = temp.AsEnumerable().Select(t => t.Field<string>("Tph")).Distinct().ToList();
                int countColumns = tphs.Count;
                if (countColumns < 4)
                {
                    while (countColumns < 4)
                    {
                        DataRow drow = temp.NewRow();
                        drow["Tph"] = "TPH" + temp.Rows.Count;
                        drow["Qty"] = 0;
                        temp.Rows.Add(drow);
                        countColumns += 1;
                    }
                }
                tb = temp;
            }

            return tb;
        }

        public DataTable GetTPH_EmptyBunch_PerNik(string estate, string Nik, DateTime HarvestDate, string Location, string Crop, string Achievement, DataTable temp, string Gang)
        {
            DataTable tb = new DataTable();
            using (PPMSEntities context = new PPMSEntities())
            {
                var Qry = context.spReport_GetBukuPanenItemPenalty(estate, Nik, HarvestDate, Location, Crop, Achievement, Gang);
                foreach (var item in Qry)
                {
                    if (temp.Rows.Count < 4)
                    {
                        DataRow drow = temp.NewRow();
                        drow["Tph"] = item.Tph;
                        drow["Qty"] = item.Qty;
                        temp.Rows.Add(drow);
                    }
                    else
                    {
                        break;
                    }
                }
                //cek apakah jumlah TPH sudah 8 kalau belum tambahkan sampai 8 isi quality dgn 0 utk ngepasin Column

                List<string> tphs = temp.AsEnumerable().Select(t => t.Field<string>("Tph")).Distinct().ToList();
                int countColumns = tphs.Count;
                if (countColumns < 4)
                {
                    while (countColumns < 4)
                    {
                        DataRow drow = temp.NewRow();
                        drow["Tph"] = "TPH" + temp.Rows.Count;
                        drow["Qty"] = 0;
                        temp.Rows.Add(drow);
                        countColumns += 1;
                    }
                }
                tb = temp;
            }

            return tb;
        }

        public DataTable GetRptPotongBuah(DateTime start, DateTime finish, string estate, string Gang, string Nik, DataTable tblin)
        {
            DataTable tbl = new DataTable();
            using (PPMSEntities context = new PPMSEntities())
            {
                var qry = context.spReport_POTONGBUAH_HDR(start, finish, estate ,Gang, Nik);
                foreach (var item in qry)
                {
                    DataRow dr = tblin.NewRow();
                    dr["Start"] = item.Start;
                    dr["Finish"] = item.Finish;
                    dr["Nik"] = item.Nik;
                    dr["Name"] = item.Name;
                    dr["Gang"] = item.Gang;
                    tblin.Rows.Add(dr);
                }

            }
            tbl = tblin;
            return tbl;
        }

        public DataTable GetRptPotongBuah_Detail(DateTime start, DateTime finish, string Gang, string Nik, DataTable tblin)
        {
            DataTable tbl = new DataTable();
            using (PPMSEntities context = new PPMSEntities())
            {
                var qry = context.SPS_POTONGBUAH(start, finish, Gang, Nik);
                foreach (var item in qry)
                {
                    DataRow dr = tblin.NewRow();
                    dr["Start"] = item.Start;
                    dr["Finish"] = item.Finish;
                    dr["Nik"] = item.Nik;
                    dr["Name"] = item.Name;
                    dr["BkmDate"] = item.BkmDate;
                    dr["Gang"] = item.Gang;
                    dr["Location"] = item.Location;
                    dr["BlockBasic"] = item.BlockBasic == null ? 0 : item.BlockBasic;
                    dr["Achievement"] = item.Achievement;
                    dr["BasisHarian"] = item.BasisHarian == null ? 0 : item.BasisHarian;
                    dr["OverBasic"] = item.OverBasic == null ? 0 : item.OverBasic;
                    dr["PremiAllocation"] = item.PremiAllocation == null ? 0 : item.PremiAllocation;
                    dr["OveBasicPremi"] = item.OveBasicPremi == null ? 0 : item.OveBasicPremi;
                    dr["TotalPremiAllocation"] = item.TotalPremiAllocation == null ? 0 : item.TotalPremiAllocation;
                    dr["RpPenalty"] = item.RpPenalty == null ? 0 : item.RpPenalty;
                    dr["NettPremi"] = item.NettPremi == null ? 0 : item.NettPremi;
                    tblin.Rows.Add(dr);
                }

            }
            tbl = tblin;
            return tbl;
        }

        
        public void spReport_ANOMALI(string estate, DateTime tanggal, string division, DataTable dtIn)
        {
            SqlConnection sqlCon = null;
            //String SqlconString = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;
            PPMSEntities db = new PPMSEntities();
            using (sqlCon = new SqlConnection(db.Database.Connection.ConnectionString))
            {
                sqlCon.Open();
                SqlCommand sql_cmnd = new SqlCommand("spReport_ANOMALI", sqlCon);
                sql_cmnd.CommandType = CommandType.StoredProcedure;
                sql_cmnd.Parameters.AddWithValue("@estate", SqlDbType.NVarChar).Value = estate;
                sql_cmnd.Parameters.AddWithValue("@date", SqlDbType.DateTime).Value = tanggal;
                sql_cmnd.Parameters.AddWithValue("@division", SqlDbType.NVarChar).Value = division;
                SqlDataReader reader = sql_cmnd.ExecuteReader();
                sqlCon.Close();
            }
        }

        public DataTable GetReportAnomali(string estate , DateTime tanggal, string Division, DataTable dtIn)
        {
            DataTable tbl = new DataTable();

            using (PPMSEntities context = new PPMSEntities())
            {
                var qry = context.spReport_ANOMALI(estate, tanggal, Division);
                foreach (var item in qry)
                {
                    DataRow dr = dtIn.NewRow();
                    dr["Gang"] = item.Gang;
                    dr["Nik"] = item.Nik;
                    dr["MandorOrKeani"] = item.MandorOrKeani;
                    dr["Name"] = item.Name;
                    dr["AbsentType"] = item.AbsentType;
                    if (item.Mandays == null)
                    {
                        dr["Mandays"] = DBNull.Value;
                    }
                    else
                    {
                        dr["Mandays"] = item.Mandays;
                    }
                    
                    dr["Block"] = item.Block;
                    dr["Output"] = item.Output == null ? "" : item.Output;
                    dr["Location"] = item.Location;
                    dr["FFB"] = item.FFB == null ? "" : item.FFB;
                    dr["LooseFruit"] = item.LooseFruit == null ? "" : item.LooseFruit;
                    dtIn.Rows.Add(dr);
                }
            }

            tbl = dtIn;
            return tbl;
        }

        //public DataTable GetReportRotasiPanen(DateTime from, DateTime To, int Division, DataTable dtIn)
        //{
        //    DataTable tbl = new DataTable();

        //    using (HMSEntities context = new HMSEntities(connstring))
        //    {
        //        var qry = context.SPS_Report_Rotasi_Panen_2(from, To, Division);
        //        foreach (var item in qry)
        //        {
        //            DataRow dr = dtIn.NewRow();
        //            dr["block"] = item.block;
        //            dr["harvested"] = item.harvested;
        //            dr["prod_trees"] = item.prod_trees;
        //            dr["bkmdate"] = item.bkmdate;
        //            dr["hektar"] = item.hektar;

        //            dtIn.Rows.Add(dr);
        //        }
        //    }

        //    tbl = dtIn;
        //    return tbl;
        //}

        //public DataTable GetReportPusinganPanen(DateTime from, DateTime To, int Division, DataTable dtIn)
        //{
        //    DataTable tbl = new DataTable();

        //    using (HMSEntities context = new HMSEntities(connstring))
        //    {
        //        var qry = context.SPS_Report_Pusingan_Panen_2(from, To, Division);
        //        foreach (var item in qry)
        //        {
        //            DataRow dr = dtIn.NewRow();
        //            dr["block"] = item.block;
        //            dr["harvested"] = item.harvested;
        //            dr["prod_trees"] = item.prod_trees;
        //            dr["bkmdate"] = item.bkmdate;
        //            dr["hektar"] = item.hektar;
        //            dr["pusingan"] = item.pusingan;
        //            dtIn.Rows.Add(dr);
        //        }
        //    }

        //    tbl = dtIn;
        //    return tbl;
        //}

        //public DataTable GetReportDistribusiHK(DateTime from, DateTime To, int Division, DataTable dtIn)
        //{
        //    DataTable tbl = new DataTable();

        //    using (HMSEntities context = new HMSEntities(connstring))
        //    {
        //        //var qry = context.SPS_Report_Distribusi_HK(from, To, Division);
        //        var qry = context.SPS_Report_Distribusi_HK(from, To, Division);

        //        foreach (var item in qry)
        //        {
        //            DataRow dr = dtIn.NewRow();
        //            dr["nik"] = item.nik;
        //            dr["name"] = item.name;
        //            dr["bkmdate"] = item.bkmdate;
        //            dr["mandays"] = item.mandays;
        //            dtIn.Rows.Add(dr);
        //        }
        //    }

        //    tbl = dtIn;
        //    return tbl;
        //}

        //public DataTable GetReportPrestasiDivisi(string estate, string divisi, DateTime BKMDate, DataTable dtIn)
        //{
        //    DataTable tbl = new DataTable();
        //    using (HMSEntities context = new HMSEntities(connstring))
        //    {
        //        var qry = context.SP_PrestasiOuputHKDivisi(divisi, BKMDate);
        //        foreach (var item in qry)
        //        {
        //            DataRow dr = dtIn.NewRow();
        //            dr["Gang"] = item.Gang;
        //            dr["bkmdate"] = item.BKMDate;
        //            dr["Activity"] = item.ActivityType;
        //            dr["block"] = item.Block;
        //            dr["hectare"] = item.Hectare;
        //            dr["plantingyear"] = item.PlantingYear;
        //            dr["uom"] = item.Uom;
        //            dr["output_hi"] = item.OutputHi == null ? 0 : item.OutputHi;
        //            dr["output_sdi"] = item.OutputSdi == null ? 0 : item.OutputSdi;
        //            dr["output_total"] = item.OutputTotal == null ? 0 : item.OutputTotal;
        //            dr["hk_hi"] = item.HKHi == null ? 0 : item.HKHi;
        //            dr["hk_sdi"] = item.HKSdi == null ? 0 : item.HKSdi;
        //            dr["hk_total"] = item.HKTotal == null ? 0 : item.HKTotal;
        //            dtIn.Rows.Add(dr);
        //        }
        //    }
        //    tbl = dtIn;
        //    return tbl;
        //}


        public DataTable GetReportPrestasiDivisiV2(string estate, string divisi, DateTime BKMDate, DataTable dtIn)
        {
            DataTable tbl = new DataTable();
            using (PPMSEntities context = new PPMSEntities())
            {
                //context.Database.CommandTimeout = 360;
                var qry = context.spReport_PrestasiOutputHKDivisi(estate, divisi, BKMDate);
                //var qry = context.SP_PrestasiOuputHKDivisi(divisi, BKMDate.Date);
                foreach (var item in qry)
                {
                    DataRow dr = dtIn.NewRow();
                    dr["Gang"] = item.gang;
                    dr["BKMDate"] = item.BKMDate;
                    dr["ActivityType"] = item.ActivityType;
                    dr["ActivityName"] = item.ActivityName;
                    dr["Block"] = item.Block;
                    dr["Hectare"] = item.Hectare;
                    dr["PlantingYear"] = item.PlantingYear;
                    dr["Uom"] = item.Uom;
                    dr["OutputHi"] = item.OutputHi == null ? 0 : item.OutputHi;
                    dr["OutputSdi"] = item.Outputsdi == null ? 0 : item.Outputsdi;
                    dr["OutputTotal"] = item.Outputtotal == null ? 0 : item.Outputtotal;
                    dr["HKHi"] = item.HKHi == null ? 0 : item.HKHi;
                    dr["HKSdi"] = item.HKSdi == null ? 0 : item.HKSdi;
                    dr["PremiHi"] = item.PremiHi == null ? 0 : item.PremiHi;
                    dr["premiSdi"] = item.premiSdi == null ? 0 : item.premiSdi;
                    dr["OvertimeHi"] = item.OvertimeHi == null ? 0 : item.OvertimeHi;
                    dr["OvertimeSdi"] = item.OvertimeSdi == null ? 0 : item.OvertimeSdi;
                    dr["MatName"] = item.MatName;
                    dr["MatHi"] = item.MatHi == null ? 0 : item.MatHi;
                    dr["MatSdi"] = item.MatSdi == null ? 0 : item.MatSdi;
                    dr["rotasi"] = item.rotasi == null ? 0 : item.rotasi;
                    dr["HKTotal"] = item.HKTotal == null ? 0 : item.HKTotal;
                    dr["MatUom"] = item.MatUom;
                    dtIn.Rows.Add(dr);
                }
            }
            tbl = dtIn;
            return tbl;
        }


        //public DataTable GetReportApprovalKetidakhadiran(DateTime bkmDate, string estate, string divisi, DataTable dtIn)
        //{
        //    //  DataTable tbl = new DataTable();

        //    // SP_GetApprovalKetidakhadiran

        //    DataTable dt = new DataTable();
        //    dt.Clear();
        //    dt.Columns.Add("Gang");
        //    dt.Columns.Add("BKMDate");
        //    dt.Columns.Add("ActivityType");
        //    dt.Columns.Add("ActivityTypeDescription");
        //    dt.Columns.Add("Mandays");
        //    dt.Columns.Add("Time_In");
        //    dt.Columns.Add("Time_Out");
        //    dt.Columns.Add("Persentage");
        //    dt.Columns.Add("AbsentType");
        //    dt.Columns.Add("Reason");

        //    using (HMSEntities context = new HMSEntities(connstring))
        //    {
        //        var qry = context.SP_GetApprovalKetidakhadiran(bkmDate, estate, divisi);
        //        foreach (var item in qry)
        //        {
        //            DataRow dr = dt.NewRow();
        //            dr["Gang"] = item.Gang;
        //            dr["BKMDate"] = item.BKMDate;
        //            dr["ActivityType"] = item.ActivityType;
        //            dr["ActivityTypeDescription"] = item.ActivityTypeDescription;
        //            dr["Mandays"] = item.Mandays;
        //            dr["Time_In"] = item.Time_In;
        //            dr["Time_Out"] = item.Time_Out;
        //            dr["Persentage"] = item.Persentage;
        //            dr["AbsentType"] = item.AbsentType;
        //            dr["Reason"] = item.Reason;
        //            dt.Rows.Add(dr);
        //        }
        //    }

        //    return dt;
        //}

        ////public DataTable GetReportAnomaliSPBS(DateTime bkmDate, string estate, string divisi)
        //public IEnumerable<SPS_ANOMALI_SPBS_Result> GetReportAnomaliSPBS(DateTime bkmDate, string estate, string divisi)
        //{
        //    //DataTable dt = new DataTable();
        //    //dt.Clear();
        //    //dt.Columns.Add("Estate");
        //    //dt.Columns.Add("SPBSNumb");
        //    //dt.Columns.Add("Division");
        //    //dt.Columns.Add("Gang");
        //    //dt.Columns.Add("BlockId");
        //    //dt.Columns.Add("TPH");
        //    //dt.Columns.Add("ProductionType");
        //    //dt.Columns.Add("HarvestDate");
        //    //dt.Columns.Add("ProductQty");
        //    //dt.Columns.Add("Uom");
        //    //dt.Columns.Add("BPN_ID");
        //    //dt.Columns.Add("QtyAngkut");
        //    //dt.Columns.Add("NikHarvester");
        //    //dt.Columns.Add("QtyRemaining");
        //    //dt.Columns.Add("BPN_IDHarvestBook");

        //    //using (HMSEntities context = new HMSEntities(connstring))
        //    //{
        //    //    var qry = context.SPS_ANOMALI_SPBS(bkmDate.Date, divisi, estate);
        //    //    foreach (var item in qry)
        //    //    {
        //    //        DataRow dr = dt.NewRow();
        //    //        dr["Estate"] = item.Estate;
        //    //        dr["SPBSNumb"] = item.SPBSNumb;
        //    //        dr["Division"] = item.Division;
        //    //        dr["Gang"] = item.Gang;
        //    //        dr["BlockId"] = item.BlockId;
        //    //        dr["TPH"] = item.TPH;
        //    //        dr["ProductionType"] = item.ProductionType;
        //    //        dr["HarvestDate"] = item.HarvestDate;
        //    //        dr["ProductQty"] = item.ProductQty;
        //    //        dr["Uom"] = item.Uom;
        //    //        dr["BPN_ID"] = item.BPN_ID;
        //    //        dr["QtyAngkut"] = item.QtyAngkut;
        //    //        dr["NikHarvester"] = item.NikHarvester;
        //    //        dr["QtyRemaining"] = item.QtyRemaining;
        //    //        dr["BPN_IDHarvestBook"] = item.BPN_IDHarvestBook;
        //    //        dt.Rows.Add(dr);
        //    //    }
        //    //}

        //    //return dt;
        //    var result = new List<SPS_ANOMALI_SPBS_Result>();
        //    using (HMSEntities context = new HMSEntities(connstring))
        //    {

        //        result = context.SPS_ANOMALI_SPBS(bkmDate.Date, divisi, estate).ToList();
        //    }
        //    return result;
        //}
    }

}