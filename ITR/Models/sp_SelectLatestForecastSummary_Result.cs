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
    
    public partial class sp_SelectLatestForecastSummary_Result
    {
        public string ForecastStatus { get; set; }
        public Nullable<System.DateTime> DataThrough { get; set; }
        public string CheckedBy { get; set; }
        public string UpcomingHighLow { get; set; }
        public string NextHighLow { get; set; }
        public string ThirdHighLow { get; set; }
        public string ForecastCheckNote { get; set; }
    }
}