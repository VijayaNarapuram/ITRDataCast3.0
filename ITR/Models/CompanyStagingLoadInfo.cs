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
    
    public partial class CompanyStagingLoadInfo
    {
        public int StagingLoadInfoID { get; set; }
        public Nullable<System.DateTime> LoadDate { get; set; }
        public string LoadSource { get; set; }
        public Nullable<int> LoadType { get; set; }
        public string Update { get; set; }
        public string Insert { get; set; }
        public Nullable<int> NumberOfRecords { get; set; }
        public string ReleaseNotes { get; set; }
        public Nullable<int> ShortCodesID { get; set; }
    }
}
