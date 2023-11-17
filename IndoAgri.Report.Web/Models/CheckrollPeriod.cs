using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IndoAgri.Report.Web.Models
{
    public class CheckrollPeriod
    {
        //private static string connstring = new BaseClass().ConnectionString();

        public void InsertRange(List<CheckrollPeriodData> activityTypeDatas, out int error)
        {
            error = 0;
            using (PPMSEntities context = new PPMSEntities())
            {
                foreach (var item in activityTypeDatas)
                {
                    var delete = context.tblM_CheckrollPeriod.Where(sbd => sbd.Estate == item.Estate && sbd.ZYear == item.ZYear && sbd.Period == item.Period).SingleOrDefault();
                    if (delete != null)
                    {
                        context.tblM_CheckrollPeriod.Remove(delete);
                    }

                    try
                    {
                        context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        error += 1;
                    }
                }

                foreach (var item in activityTypeDatas)
                {
                    var chechrollData = new tblM_CheckrollPeriod();
                    chechrollData.Estate = item.Estate;
                    chechrollData.ZYear = item.ZYear;
                    chechrollData.Period = Convert.ToInt16(item.Period);
                    chechrollData.ClosingDate = item.ClosingDate;
                    chechrollData.Status = item.Status;
                    chechrollData.Active = item.Active;
                    chechrollData.CreatedBy = item.CreatedBy;
                    chechrollData.CreatedDate = item.CreatedDate;
                    chechrollData.ModifiedBy = item.ModifiedBy;
                    chechrollData.ModifiedDate = item.ModifiedDate;
                    //chechrollData.RowVersion = item.RowVersion;
                    context.tblM_CheckrollPeriod.Add(chechrollData);
                    try
                    {
                        context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        error += 1;
                    }
                }
            }
        }

        public List<CheckrollPeriodData> GetBy(string estate)
        {
            var results = new List<CheckrollPeriodData>();
            using (PPMSEntities context = new PPMSEntities())
            {
                var empAbsentGroup = from a in context.tblM_CheckrollPeriod where a.Estate == estate select a;

                foreach (var item in empAbsentGroup)
                {
                    var data = new CheckrollPeriodData();
                    data.Active = item.Active;
                    data.ClosingDate = item.ClosingDate;
                    data.CreatedBy = item.CreatedBy;
                    data.CreatedDate = item.CreatedDate;
                    data.Estate = item.Estate;
                    data.ModifiedBy = item.ModifiedBy;
                    data.ModifiedDate = item.ModifiedDate;
                    data.Period = item.Period;
                    //data.RowVersion = item.RowVersion;
                    data.Status = item.Status;
                    data.ZYear = item.ZYear;
                    results.Add(data);
                }
            }

            return results;
        }

        public int GetCheckRollPeriodPeriodBy(string estate, DateTime closingDate, bool isActive)
        {
            var checkrollPeriodDatas = new List<CheckrollPeriodData>();
            using (PPMSEntities context = new PPMSEntities())
            {
                var activities = (from a in context.tblM_CheckrollPeriod where a.Estate == estate && a.ClosingDate >= closingDate.Date && a.Active == isActive select a).OrderBy(a => a.ClosingDate).FirstOrDefault();
                if (activities == null)
                {
                    return 0;
                }
                else
                {
                    return activities.Period;
                }
            }
        }

        public CheckrollPeriodData GetCeilCheckRollPeriodBy(string estate, DateTime closingDate, bool isActive)
        {
            // var checkrollPeriodDatas = new List<CheckrollPeriodData>();
            using (PPMSEntities context = new PPMSEntities())
            {
                var activities = (from a in context.tblM_CheckrollPeriod where a.Estate == estate && a.ClosingDate >= closingDate.Date && a.Active == isActive select a).OrderBy(a => a.ClosingDate).FirstOrDefault();
                if (activities == null)
                {
                    return null;
                }
                else
                {
                    CheckrollPeriodData result = new CheckrollPeriodData();
                    result.Active = activities.Active;
                    result.ClosingDate = activities.ClosingDate;
                    result.CreatedBy = activities.CreatedBy;
                    result.CreatedDate = activities.CreatedDate;
                    result.Estate = activities.Estate;
                    result.ModifiedBy = activities.ModifiedBy;
                    result.ModifiedDate = activities.ModifiedDate;
                    result.Period = activities.Period;
                   // result.RowVersion = activities.RowVersion;
                    result.Status = activities.Status;
                    result.ZYear = activities.ZYear;
                    return result;
                }
            }
        }


        public CheckrollPeriodData GetFloorCheckRollPeriodBy(string estate, DateTime date, bool isActive)
        {
            // var checkrollPeriodDatas = new List<CheckrollPeriodData>();
            using (PPMSEntities context = new PPMSEntities())
            {
                var activities = (from a in context.tblM_CheckrollPeriod where a.Estate == estate && a.ClosingDate <= date && a.Active == isActive select a).OrderByDescending(a => a.ClosingDate).FirstOrDefault();
                if (activities == null)
                {
                    return null;
                }
                else
                {
                    CheckrollPeriodData result = new CheckrollPeriodData();
                    result.Active = activities.Active;
                    result.ClosingDate = activities.ClosingDate;
                    result.CreatedBy = activities.CreatedBy;
                    result.CreatedDate = activities.CreatedDate;
                    result.Estate = activities.Estate;
                    result.ModifiedBy = activities.ModifiedBy;
                    result.ModifiedDate = activities.ModifiedDate;
                    result.Period = activities.Period;
                    //result.RowVersion = activities.RowVersion;
                    result.Status = activities.Status;
                    result.ZYear = activities.ZYear;
                    return result;
                }
            }
        }

        public int GetAccountingPeriodBy(string estate, DateTime closingDate, bool isActive)
        {
            var checkrollPeriodDatas = new List<CheckrollPeriodData>();
            using (PPMSEntities context = new PPMSEntities())
            {
                var activities = (from a in context.tblM_AccountingPeriod where a.Estate == estate && a.ClosingDate >= closingDate.Date && a.Active == isActive select a).OrderBy(a => a.ClosingDate).FirstOrDefault();
                if (activities == null)
                {
                    return 0;
                }
                else
                {
                    return activities.Period;
                }
            }
        }


        public List<CheckrollPeriodData> GetBy(string estate, int year)
        {
            var results = new List<CheckrollPeriodData>();
            using (PPMSEntities context = new PPMSEntities())
            {
                var empAbsentGroup = from a in context.tblM_CheckrollPeriod where a.Estate == estate && a.ZYear == year select a;

                foreach (var item in empAbsentGroup)
                {
                    var data = new CheckrollPeriodData();
                    data.Active = item.Active;
                    data.ClosingDate = item.ClosingDate;
                    data.CreatedBy = item.CreatedBy;
                    data.CreatedDate = item.CreatedDate;
                    data.Estate = item.Estate;
                    data.ModifiedBy = item.ModifiedBy;
                    data.ModifiedDate = item.ModifiedDate;
                    data.Period = item.Period;
                    //data.RowVersion = item.RowVersion;
                    data.Status = item.Status;
                    data.ZYear = item.ZYear;
                    results.Add(data);
                }
            }

            return results;
        }
    }

    public class CheckrollPeriodData
    {
        public string Estate { get; set; }
        public int ZYear { get; set; }
        public int Period { get; set; }
        public DateTime? ClosingDate { get; set; }
        public string Status { get; set; }
        public bool? Active { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public byte[] RowVersion { get; set; }
    }
}