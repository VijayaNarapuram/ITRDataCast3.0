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
    
    public partial class Notify
    {
        public int NotifyID { get; set; }
        public string UserEMail { get; set; }
        public string DataSeries { get; set; }
        public Nullable<bool> SentNotice { get; set; }
        public Nullable<System.DateTime> TimeCompleted { get; set; }
        public Nullable<short> NumSuccessfulIndicators { get; set; }
        public Nullable<short> NumMissedIndicators { get; set; }
        public bool IsActive { get; set; }
    }
}
