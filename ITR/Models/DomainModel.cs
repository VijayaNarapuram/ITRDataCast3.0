using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITR.Models
{
    public class DomainModel
    {
    }

    /// <summary>
    /// Created By:Vishnu
    /// Created On: 21-Aug-2016
    /// Defines the Template data model
    /// </summary>
    public class TemplateModel
    {
        
        public int AccountID { get; set; }
        public int DivisionID { get; set; }
        public string DatasetName { get; set; }
        public string DatasetDescription { get; set; }
        public string CustomerID { get; set; }
        public string CompanyName { get; set; }
        public string DataType { get; set; }
        public string Base { get; set; }
        public string Units { get; set; }
        public string CompanyWebsite { get; set; }
        public string MonthYears { get; set; }
        public string RawValues { get; set; }
        public string AdjValues { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ProdID { get; set; }
        public int CustomerDataSetID { get; set; }
        public bool IsDataPortalAdmin { get; set; }
        public string UserName { get; set; }
        public string Indicator { get; set; }

        public string Accounts { get; set; }
        public string Divisions { get; set; }

        //For Account and Division dropdowns
        public string DivisionsRecordId { get; set; }
        public string AccountName { get; set; }
        public string RecordId { get; set; }

        public string Indicators { get; set; }

        // Added below two fields to template by Raghuveer on 21 July 2017
        public int UserID { get; set; }
        public string FavouriteListName { get; set; }
        public string ViewStatuses { get; set; }

    }

    /// <summary>
    /// Created By:Vishnu
    /// Created On:01-Sept-2016
    /// Defines the Session model which will be passed from Drupal Iframe
    /// </summary>
    public class SessionModel
    {
        public string SessionID { get; set; }
        public string CompanyID { get; set; }
        public string DivisonID { get; set; }
        public string UserID { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public string Value3 { get; set; }
        public string IsDataPortalAdmin { get; set; }
        public string EmailID { get; set; }

    }

    /// <summary>
    /// Created By:Vishnu
    /// Created On: 21-Aug-2016
    /// Defines the UpdateDataset model
    /// </summary>
    public class UpdateDataset
    {
        public string ProdID { get; set; }
        public string MonthYears { get; set; }
        public string RawValues { get; set; }
        public string AdjValues { get; set; }
        public string DatasetDescription { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }


    /// <summary>
    /// Created by Raghuveer    
    /// Fields are used for Favorites
    /// </summary>   
    public class FavoritesModel
    {
        public string CompanyShortCode { get; set; }
        public string IndicatorShortCodes { get; set; }
        public int UserID { get; set; }
        public string FavouriteListName { get; set; }
        public string CreatedBY { get; set; }
        public DateTime CreatedDate { get; set; }

    }

    /// <summary>
    ///Created By: Ram Mohan
    ///Description: Fields are used while generating jqueru datatable
    /// </summary> 
    public class JQueryDataTableParamModel
    {
        /// <summary>
        /// Request sequence number sent by DataTable, same value must be returned in response
        /// </summary>       
        public string sEcho { get; set; }

        /// <summary>
        /// Text used for filtering
        /// </summary>
        public string sSearch { get; set; }

        /// <summary>
        /// Number of records that should be shown in table
        /// </summary>
        public int iDisplayLength { get; set; }

        /// <summary>
        /// First record that should be shown(used for paging)
        /// </summary>
        public int iDisplayStart { get; set; }

        /// <summary>
        /// Number of columns in table
        /// </summary>
        public int iColumns { get; set; }

        /// <summary>
        /// Number of columns that are used in sorting
        /// </summary>
        public int iSortingCols { get; set; }

        /// <summary>
        /// Comma separated list of column names
        /// </summary>
        public string sColumns { get; set; }

    }
}