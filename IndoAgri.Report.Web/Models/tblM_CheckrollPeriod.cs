//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IndoAgri.Report.Web.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblM_CheckrollPeriod
    {
        public string Estate { get; set; }
        public int ZYear { get; set; }
        public short Period { get; set; }
        public Nullable<System.DateTime> ClosingDate { get; set; }
        public string Status { get; set; }
        public Nullable<bool> Active { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
