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
    
    public partial class StagingIndicatorTest
    {
        public int STAGIndicatorID { get; set; }
        public Nullable<System.DateTime> MonthYear { get; set; }
        public Nullable<decimal> RawValue { get; set; }
        public Nullable<decimal> AdjValue { get; set; }
        public string UpdateReason { get; set; }
        public Nullable<System.DateTime> ValueStartDate { get; set; }
        public Nullable<System.DateTime> ValueEndDate { get; set; }
        public string CurrentFlag { get; set; }
        public string SeriesName { get; set; }
        public string SourceFIle { get; set; }
        public string SourceSheet { get; set; }
        public Nullable<int> MasterMetadataID { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string Source { get; set; }
        public string Link { get; set; }
        public string Definition { get; set; }
    }
}
