//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ITR.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class UI_ForeCastData
    {
        public int ForeCastDataID { get; set; }
        public Nullable<int> ForeCastMetaDataID { get; set; }
        public string ShortCode { get; set; }
        public Nullable<System.DateTime> ForeCastDate { get; set; }
        public Nullable<System.DateTime> MonthYear { get; set; }
        public Nullable<decimal> LFR12MMA { get; set; }
        public Nullable<decimal> Avg12MMA { get; set; }
        public Nullable<decimal> UFR12MMA { get; set; }
        public Nullable<decimal> LFR3MMA { get; set; }
        public Nullable<decimal> Avg3MMA { get; set; }
        public Nullable<decimal> UFR3MMA { get; set; }
        public Nullable<decimal> LFR12by12 { get; set; }
        public Nullable<decimal> Avg12by12 { get; set; }
        public Nullable<decimal> UFR12by12 { get; set; }
        public Nullable<decimal> LFR3by12 { get; set; }
        public Nullable<decimal> Avg3by12 { get; set; }
        public Nullable<decimal> UFR3by12 { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}
