using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using ITR.DataRepository;
using ITR.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http.Cors;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace ITR.Controllers
{

    public class DatacastHomeController : Controller
    {

        private IDatacastHomeRepository _homeRepository;

        //Dependency Injection resolver using the interface
        public DatacastHomeController(IDatacastHomeRepository _repo)
        {
            _homeRepository = _repo;

        }
        private string m_sdbconstr = System.Configuration.ConfigurationManager.ConnectionStrings["DatacastSqlConnString"].ToString();

        //
        // GET: /Home/
        /// <summary>
        /// Created By:Vishnu
        /// Created Date: 28-Oct-2016
        /// Loads the Default view
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Created By:Vishnu
        /// Created Date: 28-Oct-2016
        /// Loads the DataCast view
        /// </summary>
        /// <returns></returns>
        public ActionResult DataCast()
        {
            GetAccounts();
            return View();
        }

        /// <summary>
        /// Created By:VIJAYA
        /// Created Date: 2018/04/12
        /// Loads the DashboardWidgets view
        /// </summary>
        /// <returns></returns>
        public ActionResult DashboardWidgets()
        {
            GetAccounts();
            return View();
        }

        public ActionResult DataCastFromServer()
        {
            GetAccounts();
            return View();
        }

        /// <summary>
        /// Created By:Vishnu
        /// Created On: 02-Nov-2016
        /// </summary>
        /// <param name="shotrCode"></param>
        /// Gets the indicator historical and forecast data by indicator shortcode
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetIndicatorHistoricalAndForeCastDataBySHORTCODE(string shotrCode)
        {
            var listInfo = _homeRepository.GetIndicatorHistoricalAndForeCastDataBySHORTCODE(shotrCode);
            //IList<uspSelectIndicatorForecastDataByIndicatorShortCode_Result> temp = listInfo;
            //listInfo = temp;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = int.MaxValue;
            string rettval = serializer.Serialize(listInfo);
            return Json(rettval, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Created By:Vishnu
        /// Created On: 03-Nov-2016
        /// </summary>
        /// <param name="shotrCode"></param>
        /// Gets the Company data by company shortcode
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetCompanyDataBySHORTCODE(string shotrCode)
        {
            var listInfo = _homeRepository.GetCompanyDataBySHORTCODE(shotrCode);
            //IList<uspSelectCompanyForecastDataByCompanyShortCode_Result> temp = listInfo;
            //listInfo = temp;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = int.MaxValue;
            string rettval = serializer.Serialize(listInfo);
            return Json(rettval, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Created By:Vishnu
        /// Created On: 03-Nov-2016
        /// Gets the Datasets by Logged in person QBaseAccountId and QbaseDivisionID
        /// </summary>
        /// <param name="accountID"></param>
        /// <param name="divisionID"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetDataSetsByQBaseAccountIDAndDivisionID(int accountID, int divisionID)
        {
            JsonResult result = null;
            try
            {

                var dt1 = _homeRepository.GetDataSetsByQBaseAccountIDAndDivisionID(accountID, divisionID);
                var divisions = (from dr in dt1.AsEnumerable()
                                 select new SelectListItem
                                 {
                                     Text = dr.ShortCode,
                                     Value = dr.MasterMetaDataID.ToString()
                                     //Value= dr.ShortCode
                                 }).ToList();

                result = Json(divisions, JsonRequestBehavior.AllowGet);

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        
       
        /// <summary>
        /// Created By:Vishnu
        /// Created On: 03-Nov-2016
        /// Gets the Indicator Names and Correlation,Lead/Lag values by selected DataSetID
        /// </summary>
        /// <param name="dataSetID"></param>
        /// <returns></returns>
        //[AcceptVerbs(HttpVerbs.Get)]
        //public JsonResult GetIndicatorNamesAndCorrelationLeadLagValuesByDataSetID(string dataSetID, int iDisplayLength, int iDisplayStart)
        //{
        //    var listInfo = _homeRepository.GetIndicatorNamesAndCorrelationLeadLagValuesByDataSetID(dataSetID, iDisplayLength, iDisplayStart);
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    serializer.MaxJsonLength = int.MaxValue;
        //    string rettval = serializer.Serialize(listInfo);
        //    return Json(rettval, JsonRequestBehavior.AllowGet);
        //}

        /// <summary>
        /// Created By:Vishnu
        /// Created On: 04-Nov-2016
        /// Gets Company and Indicator Data By shortcodes to plot DataCast chart in same dates range
        /// </summary>
        /// <param name="CompanyShortCode"></param>
        /// <param name="IndicatorShortCode"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetIndicatorCompanyAndIndicatorDataByShortCodes(string CompanyShortCode, string IndicatorShortCode, bool InverseOrder, int MoveMonths, int CustomerCompanyID)
        {
            IndicatorShortCode = HttpUtility.UrlDecode(IndicatorShortCode);
            CompanyShortCode = HttpUtility.UrlDecode(CompanyShortCode);
            var listInfo = _homeRepository.GetIndicatorCompanyAndIndicatorDataByShortCodes(CompanyShortCode, IndicatorShortCode, InverseOrder, MoveMonths, CustomerCompanyID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = int.MaxValue;
            string rettval = serializer.Serialize(listInfo);
            return Json(rettval, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Created By:Vishnu
        /// Created On: 04-Nov-2016
        /// 
        /// Gets Company Data for Internal Data Trends Tab
        /// </summary>
        /// <param name="CompanyShortCode"></param>
        /// <param name="CaluclationType"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetCompanyDataForTrends(string CompanyShortCode, int CaluclationType)
        {
            var listInfo = _homeRepository.GetCompanyDataForTrends(CompanyShortCode, CaluclationType);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = int.MaxValue;
            string rettval = serializer.Serialize(listInfo);
            return Json(rettval, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// Created By:Vishnu
        /// Created On: 04-Nov-2016
        /// Gets Company Data for Internal Rates Of Change Tab
        /// </summary>
        /// <param name="FirstCompanyDatasetName"></param>
        /// <param name="FirstDatasetCaluclationType"></param>
        /// <param name="SecondCompanyDatasetName"></param>
        /// <param name="SecondDatasetCaluclationType"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetCompaniesDataForInternalRatesOfChange(string FirstCompanyDatasetName, string FirstDatasetCaluclationType, string SecondCompanyDatasetName, string SecondDatasetCaluclationType)
        {
            var listInfo = _homeRepository.GetCompanyDataForInternalRatesOfChange(FirstCompanyDatasetName, FirstDatasetCaluclationType, SecondCompanyDatasetName, SecondDatasetCaluclationType);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = int.MaxValue;
            string rettval = serializer.Serialize(listInfo);
            return Json(rettval, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Created By:Vishnu
        /// Created On: Feb 16 2017
        /// Gets the Indicator Names and Correlation,Lead/Lag values by selected DataSetID
        /// </summary>
        /// <param name="dataSetID"></param>
        /// <returns></returns>
        //[AcceptVerbs(HttpVerbs.Get)]
        //public JsonResult GetIndicatorNamesAndCorrelationLeadLagValuesByMultipleDataSetID(string dataSetIDs)
        //{
        //    var listInfo = _homeRepository.GetIndicatorNamesAndCorrelationLeadLagValuesByMultipleDataSetID(dataSetIDs);
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    serializer.MaxJsonLength = int.MaxValue;
        //    string rettval = serializer.Serialize(listInfo);
        //    return Json(rettval, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult FillIndicatorNamesAndCorrelationLeadLagValuesByMultipleDataSetID(DatacastJQueryDataTableParamModel param, string dataSetIDs)
        {
            int sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            string sortDirection = Request["sSortDir_0"];

            IEnumerable<uspSelectLoadCorelationLeadLagByMultipleDatasetIDS_Result> filteredEmployees = _homeRepository.GetIndicatorNamesAndCorrelationLeadLagValuesByMultipleDataSetID(dataSetIDs, param.iDisplayLength, param.iDisplayStart);
            IEnumerable<uspSelectLoadCorelationLeadLagByMultipleDatasetIDS_Result> searchEmployess = null;


            var result = from c in filteredEmployees
                         select new[] { Convert.ToString(c.ASeriesName),Convert.ToString(c.CorrValue),Convert.ToString(c.lagValue),
                             Convert.ToString(c.Company), Convert.ToString(c.Indicator)};

            // var totalcount = filteredEmployees.FirstOrDefault().TotalCount.Value.ToString();

            //  var iTotalRecordsCount = filteredEmployees.FirstOrDefault().iTotalRecords.Value.ToString();

            var totalcount = filteredEmployees.Count() == 0 ? 0 : filteredEmployees.FirstOrDefault().TotalCount.Value;
            var iTotalRecordsCount = filteredEmployees.Count() == 0 ? 0 : filteredEmployees.FirstOrDefault().iTotalRecords.Value;

            if (totalcount == 0)// for search  with ZERO records as return case.
            {
                searchEmployess = _homeRepository.GetIndicatorNamesAndCorrelationLeadLagValuesByMultipleDataSetID(dataSetIDs, param.iDisplayLength, param.iDisplayStart)
                .Where(c => Convert.ToString(c.ASeriesName).Contains(param.sSearch)
                                ||
                       c.Company.ToLower().Contains(param.sSearch)
                                ||
                     c.Indicator.ToLower().Contains(param.sSearch)
                                );

                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = iTotalRecordsCount,
                    iTotalDisplayRecords = totalcount,
                    aaData = searchEmployess
                },
                         JsonRequestBehavior.AllowGet);
            }
            else
            {

                return Json(new
                {

                    sEcho = param.sEcho,
                    iTotalRecords = iTotalRecordsCount,
                    iTotalDisplayRecords = totalcount,
                    aaData = result
                },
                        JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        /// Created By:Vishnu
        /// Created On: Feb 16 2017
        /// Gets the Indicator Names and Correlation,Lead/Lag values by selected DataSetID
        /// </summary>
        /// <param name="dataSetID"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetCompanyDatasetsInfoForDownloadSection1(string dataSetIDs)
        {
            var listInfo = _homeRepository.GetCompanyDatasetsInfoForDownloadSection1(dataSetIDs);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = int.MaxValue;
            string rettval = serializer.Serialize(listInfo);
            return Json(rettval, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Created By:Vishnu
        /// Created On: Feb 16 2017
        /// Gets the IndicatorData based on Multiple Indicator Shortcodes (comma seperated)
        /// </summary>
        /// <param name="indicatorshortcodes"></param>
        /// <param name="companyCode"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetIndicatorDataByIndicatorShortCodes(string indicatorshortcodes, string companyCode)
        {
            var listInfo = _homeRepository.GetIndicatorDataByIndicatorShortCodes(indicatorshortcodes, companyCode);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = int.MaxValue;
            string rettval = serializer.Serialize(listInfo);
            return Json(rettval, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Created by :Nishanth
        /// Created on : 30-05-2017
        /// /// Fill the CorelationLead data along with Search and Sort
        /// <param name="param"></param>
        /// <param name="dataSetID"></param>
        /// <returns></returns>
        public ActionResult FillCorelationLeadLagByDataSetID_Vij(DatacastJQueryDataTableParamModel param, string dataSetID, int userID, Boolean IsFavoritesFlag, string SelFavName,
            string CategorySearch, string IndustryName, string SectorName, string SubSector1Name, string SubSector2Name, string SubSector3Name, Boolean IsDBWidgetsFlag)
       {
            //FIRST TIME TO LOAD DATACAST & INDICATOR SUMMARY TABS IN THE DATATABLE...
           if (IsFavoritesFlag == false && IsDBWidgetsFlag == false)
            //if (IsFavoritesFlag == false )
            {

                int sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                string sortDirection = Request["sSortDir_0"];
                if (sortColumnIndex == null)
                {
                    sortColumnIndex = 0;
                }
                if (sortDirection == null)
                {
                    sortDirection = "ASC";
                }
                if (param.sSearch == null)
                {
                    param.sSearch = "";
                }

               // IEnumerable<uspSelectLoadCorelationLeadLagByDataSetID_Result> filteredEmployees = _homeRepository.SelectLoadCorelationLeadLagByDataSetID(dataSetID, param.iDisplayLength, param.iDisplayStart, param.sSearch, sortColumnIndex, sortDirection, Convert.ToInt32(Session["UserId"]));
                IEnumerable<uspSelectLoadCorelationLeadLagByDataSetID_Result> filteredEmployees = _homeRepository.SelectLoadCorelationLeadLagByDataSetID(dataSetID, param.iDisplayLength, param.iDisplayStart, param.sSearch, sortColumnIndex, sortDirection, Convert.ToInt32(userID));
                IEnumerable<uspSelectLoadCorelationLeadLagByDataSetID_Result> searchEmployess = null;


                var result = from c in filteredEmployees
                             select new[] { Convert.ToString(c.ASeriesName),
                                            Convert.ToString(c.CorrValue),
                                            Convert.ToString(c.lagValue),
                                            Convert.ToString(c.Company), 
                                            Convert.ToString(c.Indicator), 
                                            Convert.ToString(c.Description), 
                                            Convert.ToString(c.FavouriteListName),
                                            Convert.ToString(c.Indicator),
                                            Convert.ToString(c.Indicator)};

                var totalcount = filteredEmployees.Count() == 0 ? 0 : filteredEmployees.FirstOrDefault().TotalCount.Value;
                var iTotalRecordsCount = filteredEmployees.Count() == 0 ? 0 : filteredEmployees.FirstOrDefault().iTotalRecords.Value;

                //  var totalcount = filteredEmployees.FirstOrDefault().TotalCount.Value;

                // var iTotalRecordsCount = filteredEmployees.FirstOrDefault().iTotalRecords.Value.ToString();

               // myDatatable();

                if (totalcount == 0)// for search  with ZERO records as return case.
                {
                    searchEmployess = _homeRepository.SelectLoadCorelationLeadLagByDataSetID(dataSetID, param.iDisplayLength, param.iDisplayStart, param.sSearch, sortColumnIndex, sortDirection, Convert.ToInt32(userID))
                    .Where(c => Convert.ToString(c.ASeriesName).Contains(param.sSearch)
                                    ||
                           c.Company.ToLower().Contains(param.sSearch)
                                    ||
                         c.Indicator.ToLower().Contains(param.sSearch)
                                    );

                    return Json(new
                    {
                        sEcho = param.sEcho,
                        iTotalRecords = iTotalRecordsCount,
                        iTotalDisplayRecords = totalcount,
                        aaData = searchEmployess
                    },
                             JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        sEcho = param.sEcho,
                        iTotalRecords = iTotalRecordsCount,
                        iTotalDisplayRecords = totalcount,
                        aaData = result
                    },
                            JsonRequestBehavior.AllowGet);
                }
            }
            else if (CategorySearch == "CategorySearch")
            {
                if (SectorName == "Select") SectorName = "";
                if (SubSector1Name == "Select") SubSector1Name = "";
                if (SubSector2Name == "Select") SubSector2Name = "";
                if (SubSector3Name == "Select") SubSector3Name = "";
                return FillFavoritesCorelationLeadLagByCategorySearch(param, dataSetID, userID, IndustryName,SectorName, SubSector1Name, SubSector2Name, SubSector3Name);
            }
           else if (IsDBWidgetsFlag == true)//If coming from ShowDBWidgetsList...(We need to show the selected indicators of that Company)
           {
               return FillDBWidgetsCorelationLeadLagByDataSetIDUesrID(param, dataSetID, userID);
           }
            else
                return FillFavoritesCorelationLeadLagByDataSetIDUesrID(param, dataSetID, userID, SelFavName);
            //return FillFavoritesCorelationLeadLagByDataSetIDUesrID(param, dataSetID, Convert.ToInt32(Session["UserId"]), SelFavName);

        }

        public ActionResult FillCorelationLeadLagByDataSetID(DatacastJQueryDataTableParamModel param, string dataSetID, int userID, Boolean IsFavoritesFlag, string SelFavName,
           string CategorySearch, string IndustryName, string SectorName, string SubSector1Name, string SubSector2Name, string SubSector3Name)
        {           
            if (IsFavoritesFlag == false)
            {

                int sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                string sortDirection = Request["sSortDir_0"];
                if (sortColumnIndex == null)
                {
                    sortColumnIndex = 0;
                }
                if (sortDirection == null)
                {
                    sortDirection = "ASC";
                }
                if (param.sSearch == null)
                {
                    param.sSearch = "";
                }

                //IEnumerable<uspSelectLoadCorelationLeadLagByDataSetID_Result> filteredEmployees = _homeRepository.SelectLoadCorelationLeadLagByDataSetID(dataSetID, param.iDisplayLength, param.iDisplayStart, param.sSearch, sortColumnIndex, sortDirection, Convert.ToInt32(Session["UserId"]));
                IEnumerable<uspSelectLoadCorelationLeadLagByDataSetID_Result> filteredEmployees = _homeRepository.SelectLoadCorelationLeadLagByDataSetID(dataSetID, param.iDisplayLength, param.iDisplayStart, param.sSearch, sortColumnIndex, sortDirection, Convert.ToInt32(userID));
                IEnumerable<uspSelectLoadCorelationLeadLagByDataSetID_Result> searchEmployess = null;


                var result = from c in filteredEmployees
                             select new[] { Convert.ToString(c.ASeriesName),
                                            Convert.ToString(c.CorrValue),
                                            Convert.ToString(c.lagValue),
                             Convert.ToString(c.Company), Convert.ToString(c.Indicator), Convert.ToString(c.Description), Convert.ToString(c.FavouriteListName) ,Convert.ToString(c.Indicator)};



                var totalcount = filteredEmployees.Count() == 0 ? 0 : filteredEmployees.FirstOrDefault().TotalCount.Value;
                var iTotalRecordsCount = filteredEmployees.Count() == 0 ? 0 : filteredEmployees.FirstOrDefault().iTotalRecords.Value;

                //  var totalcount = filteredEmployees.FirstOrDefault().TotalCount.Value;

                // var iTotalRecordsCount = filteredEmployees.FirstOrDefault().iTotalRecords.Value.ToString();



                if (totalcount == 0)// for search  with ZERO records as return case.
                {
                    searchEmployess = _homeRepository.SelectLoadCorelationLeadLagByDataSetID(dataSetID, param.iDisplayLength, param.iDisplayStart, param.sSearch, sortColumnIndex, sortDirection, Convert.ToInt32(userID))
                    .Where(c => Convert.ToString(c.ASeriesName).Contains(param.sSearch)
                                    ||
                           c.Company.ToLower().Contains(param.sSearch)
                                    ||
                         c.Indicator.ToLower().Contains(param.sSearch)
                                    );

                    return Json(new
                    {
                        sEcho = param.sEcho,
                        iTotalRecords = iTotalRecordsCount,
                        iTotalDisplayRecords = totalcount,
                        aaData = searchEmployess
                    },
                             JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        sEcho = param.sEcho,
                        iTotalRecords = iTotalRecordsCount,
                        iTotalDisplayRecords = totalcount,
                        aaData = result
                    },
                            JsonRequestBehavior.AllowGet);
                }
            }
            else if (CategorySearch == "CategorySearch")
            {
                if (SectorName == "Select") SectorName = "";
                if (SubSector1Name == "Select") SubSector1Name = "";
                if (SubSector2Name == "Select") SubSector2Name = "";
                if (SubSector3Name == "Select") SubSector3Name = "";
                return FillFavoritesCorelationLeadLagByCategorySearch(param, dataSetID, userID, IndustryName, SectorName, SubSector1Name, SubSector2Name, SubSector3Name);
            }         
            else
                return FillFavoritesCorelationLeadLagByDataSetIDUesrID(param, dataSetID, userID, SelFavName);           

        }

        /// <summary>
        /// Created by :Khajavali
        /// Created on : 07-06-2017
        /// /// Get All Favorites List by dataSetID and userid
        /// <param name="param"></param>
        /// <param name="dataSetID"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetAllFavoritesListByDataSetIDUesrID(string dataSetID, int userID, string SelFavName)
        {
           // var listInfo = _homeRepository.SelectAllFavoritesListgByDataSetIDUserID(dataSetID, Convert.ToInt32(Session["UserId"]), SelFavName);
            var listInfo = _homeRepository.SelectAllFavoritesListgByDataSetIDUserID(dataSetID, userID, SelFavName);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = int.MaxValue;
            string rettval = serializer.Serialize(listInfo);
            return Json(rettval, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Created by :Khajavali
        /// Created on : 01-06-2017
        /// /// Fill the Favorites CorelationLead data along with Search and Sort by dataSetID and userid
        /// <param name="param"></param>
        /// <param name="dataSetID"></param>
        /// <returns></returns>
        public ActionResult FillFavoritesCorelationLeadLagByDataSetIDUesrID(DatacastJQueryDataTableParamModel param, string dataSetID, int userID, string SelFavName)
        {
            int sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            string sortDirection = Request["sSortDir_0"];
            if (sortColumnIndex == null)
            {
                sortColumnIndex = 0;
            }
            if (sortDirection == null)
            {
                sortDirection = "ASC";
            }
            if (param.sSearch == null)
            {
                param.sSearch = "";
            }

            //  IEnumerable<uspSelectFavouriteListByUserID_Result> filteredFavorites = _homeRepository.SelectFavoritesLoadCorelationLeadLagByDataSetID(dataSetID, Convert.ToInt32(Session["UserId"]), SelFavName, param.iDisplayLength, param.iDisplayStart, param.sSearch, sortColumnIndex, sortDirection);
            IEnumerable<uspSelectFavouriteListByUserID_Result> filteredFavorites = _homeRepository.SelectFavoritesLoadCorelationLeadLagByDataSetID(dataSetID, userID, SelFavName, param.iDisplayLength, param.iDisplayStart, param.sSearch, sortColumnIndex, sortDirection);
            IEnumerable<uspSelectFavouriteListByUserID_Result> searchFavorites = null;


            var result = from c in filteredFavorites
                         select new[] { Convert.ToString(c.ASeriesName),
                                        Convert.ToString(c.CorrValue),
                                        Convert.ToString(c.lagValue),
                                        Convert.ToString(c.Company), 
                                        Convert.ToString(c.Indicator), 
                                        Convert.ToString(c.Description), 
                                        Convert.ToString(c.FavouriteListName),
                                        Convert.ToString(c.Indicator), //FOR FAVOURITES CHECKBOX...
                                        Convert.ToString(c.Indicator) //FOR DB WIDGET CHECKBOX...
                         };

            var totalcount = filteredFavorites.Count() == 0 ? 0 : filteredFavorites.FirstOrDefault().TotalCount.Value;
            var iTotalRecordsCount = filteredFavorites.Count() == 0 ? 0 : filteredFavorites.FirstOrDefault().iTotalRecords.Value;

            //  var totalcount = filteredEmployees.FirstOrDefault().TotalCount.Value;

            // var iTotalRecordsCount = filteredEmployees.FirstOrDefault().iTotalRecords.Value.ToString();

            if (totalcount == 0)// for search  with ZERO records as return case.
            {
                // searchFavorites = _homeRepository.SelectFavoritesLoadCorelationLeadLagByDataSetID(dataSetID, Convert.ToInt32(Session["UserId"]), SelFavName, param.iDisplayLength, param.iDisplayStart, param.sSearch, sortColumnIndex, sortDirection)
                searchFavorites = _homeRepository.SelectFavoritesLoadCorelationLeadLagByDataSetID(dataSetID, userID, SelFavName, param.iDisplayLength, param.iDisplayStart, param.sSearch, sortColumnIndex, sortDirection)
                .Where(c => Convert.ToString(c.ASeriesName).Contains(param.sSearch)
                                ||
                       c.Company.ToLower().Contains(param.sSearch)
                                ||
                     c.Indicator.ToLower().Contains(param.sSearch)
                                );

                /* Added by Vijaya, Throwing error if favourites list name doesn't have any indicators */ 
                var result1 = from c in searchFavorites
                              select new[] { Convert.ToString(c.ASeriesName),
                                        Convert.ToString(c.CorrValue),
                                        Convert.ToString(c.lagValue),
                                        Convert.ToString(c.Company), 
                                        Convert.ToString(c.Indicator), 
                                        Convert.ToString(c.Description), 
                                        Convert.ToString(c.FavouriteListName),
                                        Convert.ToString(c.Indicator), //FOR FAVOURITES CHECKBOX...
                                        Convert.ToString(c.Indicator) //FOR DB WIDGET CHECKBOX...
                         };

                //return Json(new
                //{
                //    sEcho = param.sEcho,
                //    iTotalRecords = iTotalRecordsCount,
                //    iTotalDisplayRecords = totalcount,
                //    aaData = searchFavorites
                //}, JsonRequestBehavior.AllowGet);
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = iTotalRecordsCount,
                    iTotalDisplayRecords = totalcount,
                    aaData = result1
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {

                return Json(new
                {

                    sEcho = param.sEcho,
                    iTotalRecords = iTotalRecordsCount,
                    iTotalDisplayRecords = totalcount,
                    aaData = result
                },
                        JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        /// Created by :Khajavali
        /// Created on : 20-06-2017
        /// /// Fill the Favorites list names by dataSetID and userid
        /// <param name="param"></param>
        /// <param name="dataSetID"></param>
        /// <returns></returns>
        public ActionResult FillFavlistNamesByDataSetID(DatacastJQueryDataTableParamModel param, string dataSetID, int userID)
        {
            int sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            string sortDirection = Request["sSortDir_0"];
            if (sortColumnIndex == null)
            {
                sortColumnIndex = 0;
            }
            if (sortDirection == null)
            {
                sortDirection = "ASC";
            }
            if (param.sSearch == null)
            {
                param.sSearch = "";
            }

            IEnumerable<uspSelectFavouritesListByUserID_Result> filteredFavorites = _homeRepository.SelectFavouritesListByUserID(dataSetID, userID);// Convert.ToInt32(Session["UserId"]));
            IEnumerable<uspSelectFavouritesListByUserID_Result> searchFavorites = null;


            var result = from c in filteredFavorites
                         select new[] { Convert.ToString(c.FavouriteListName), Convert.ToString(c.FavouriteListName) };



            var totalcount = filteredFavorites.Count();// == 0 ? 0 : filteredFavorites.FirstOrDefault().TotalCount.Value;
            var iTotalRecordsCount = filteredFavorites.Count();// == 0 ? 0 : filteredFavorites.FirstOrDefault().iTotalRecords.Value;

            //  var totalcount = filteredEmployees.FirstOrDefault().TotalCount.Value;

            // var iTotalRecordsCount = filteredEmployees.FirstOrDefault().iTotalRecords.Value.ToString();



            if (totalcount == 0)// for search  with ZERO records as return case.
            {
                searchFavorites = _homeRepository.SelectFavouritesListByUserID(dataSetID,userID)// Convert.ToInt32(Session["UserId"]))
                .Where(c => Convert.ToString(c.FavouriteListName).Contains(param.sSearch)
                                ||
                       c.FavouriteListName.ToLower().Contains(param.sSearch)
                                ||
                     c.FavouriteListName.ToLower().Contains(param.sSearch)
                                );

                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = iTotalRecordsCount,
                    iTotalDisplayRecords = totalcount,
                    aaData = searchFavorites
                },
                         JsonRequestBehavior.AllowGet);
            }
            else
            {

                return Json(new
                {

                    sEcho = param.sEcho,
                    iTotalRecords = iTotalRecordsCount,
                    iTotalDisplayRecords = totalcount,
                    aaData = result
                },
                        JsonRequestBehavior.AllowGet);
            }

        }



        public void GetIndicatorSeriesForDownloadSection2(string CompanyShortCode)
        {
            SqlConnection m_odbcon = new System.Data.SqlClient.SqlConnection(m_sdbconstr);

            DataSet retds = new DataSet();

            System.Data.SqlClient.SqlDataAdapter odbadpt = new System.Data.SqlClient.SqlDataAdapter("exec dbo.uspSelectIndicatorSeriesItemsForDownloadData  @ShortCode='" + CompanyShortCode + "'", m_odbcon);

            try
            {
                odbadpt.SelectCommand.CommandType = CommandType.Text;
                odbadpt.SelectCommand.CommandTimeout = 1200;
                if (m_odbcon.State == ConnectionState.Closed)
                {
                    m_odbcon.Open();
                }
                odbadpt.Fill(retds);

                // ExportDataSetToExcel(retds,outputpath);
                // ExportToExcel(retds, outputpath);
                GridView GridView1 = new GridView();
                GridView1.AllowPaging = false;
                GridView1.DataSource = retds;
                GridView1.DataBind();
                Response.Clear();
                Response.Buffer = true;
                // Response.AddHeader("content-disposition",
                // "attachment;filename=IndicatorList.csv");
                //Response.Charset = "";
                //Response.ContentType = "application/vnd.ms-excel";
                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                GridView1.RenderControl(hw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
                m_odbcon.Close();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                if (m_odbcon.State == ConnectionState.Open)
                {
                    m_odbcon.Close();
                }
                odbadpt = null;
            }
            // return retds;

        }


        public void GetForecastDataByIndicatorForDownloadSection3(string IndicatorShortCode, int CompanyID)
        {
            SqlConnection m_odbcon = new System.Data.SqlClient.SqlConnection(m_sdbconstr);

            DataSet retds = new DataSet();

            System.Data.SqlClient.SqlDataAdapter odbadpt = new System.Data.SqlClient.SqlDataAdapter("exec dbo.uspSelectForecastDataByIndicatorForDownload  @IndicatorShortCode='" + IndicatorShortCode + "',@CompanyID='" + CompanyID + "'", m_odbcon);

            try
            {
                odbadpt.SelectCommand.CommandType = CommandType.Text;
                odbadpt.SelectCommand.CommandTimeout = 1200;
                if (m_odbcon.State == ConnectionState.Closed)
                {
                    m_odbcon.Open();
                }
                odbadpt.Fill(retds);
                DataTable table = retds.Tables[0];
               
                foreach (DataRow row in table.Rows)
                {
                   
                    DateTime dt = DateTime.Parse(row["MonthYear"].ToString());
                 //   DateTime dt1 = DateTime.Parse(row["ForeCastDate"].ToString());
                 
                 //   row["ForeCastDate"] = dt1.ToString("MMMM dd yyyy");
                    row["MonthYear"] = dt.ToString("MMMM dd yyyy");

                }

                // ExportDataSetToExcel(retds,outputpath);
                // ExportToExcel(retds, outputpath);
                GridView GridView1 = new GridView();
                GridView1.AllowPaging = false;
                GridView1.DataSource = retds;
                GridView1.DataBind();
                Response.Clear();
                Response.Buffer = true;
                // Response.AddHeader("content-disposition",
                // "attachment;filename=IndicatorList.csv");
                //Response.Charset = "";
                //Response.ContentType = "application/vnd.ms-excel";
                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                GridView1.RenderControl(hw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
                m_odbcon.Close();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                if (m_odbcon.State == ConnectionState.Open)
                {
                    m_odbcon.Close();
                }
                odbadpt = null;
            }
            // return retds;

        }













        /// <summary>
        /// Created By: Vishnu
        /// Created On: 21-Feb-2016
        /// To get the Account picklist values from the API
        /// Modified On: 06-December-2016
        /// Loads the Accounts dropdownlist 
        /// </summary>
        /// <returns></returns>
        private void GetAccounts()
        {

            List<DatacastTemplateModel> companyData = new List<DatacastTemplateModel>();
            string account_name = "";
            string record_id_ = "";

            WebClient client = new WebClient();
            string url = "https://itreconomics.quickbase.com/db/bgnqhu58k?a=API_DoQuery&usertoken=b2555s_ynh_bdhajpwdb3qxm4cn7n4jidps9pwp&query={90.EX.'true'}OR{133.EX.'true'}AND{8.EX.'true'}&clist=6.3";

            string xmlString = client.DownloadString(url);
            var doc = new XmlDocument();
            doc.LoadXml(xmlString);

            XmlNode nodes = doc.SelectSingleNode("/qdbapi");
            for (int i = 0; i < nodes.ChildNodes.Count; i++)
            {
                if (nodes.ChildNodes[i].Name == "record")
                {
                    for (int j = 0; j < nodes.ChildNodes[i].ChildNodes.Count; j++)
                    {
                        if (nodes.ChildNodes[i].ChildNodes[j].Name == "account_name")
                        {
                            account_name = nodes.ChildNodes[i].ChildNodes[j].InnerText;
                        }
                        else if (nodes.ChildNodes[i].ChildNodes[j].Name == "record_id_")
                        {
                            record_id_ = nodes.ChildNodes[i].ChildNodes[j].InnerText;
                        }
                    }
                    companyData.Add(new DatacastTemplateModel
                    {
                        AccountName = account_name,
                        RecordId = record_id_
                    });
                }
            }

            if (companyData.Count > 1)
            {
                var companyData1 = companyData.OrderBy(x => x.AccountName);
                //ViewBag.Accounts = companyData1;
                ViewBag.Accounts = JsonConvert.SerializeObject(companyData1);
            }
            else
            {
                // ViewBag.Accounts = companyData;
                ViewBag.Accounts = JsonConvert.SerializeObject(companyData);

            }

        }
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetDivisionsByAccountID(int id)
        {
            JsonResult result = null;
            try
            {

                //var dt1 = _homeRepository.GetDivisionsByAccountID(id);
                //var divisions = (from dr in dt1.AsEnumerable()
                //                 select new SelectListItem
                //                 {
                //                     Text = dr.DivisionName,
                //                     Value = dr.DivisionID.ToString()
                //                 }).ToList();
                //result = Json(divisions, JsonRequestBehavior.AllowGet);
                //return result;

                List<DatacastTemplateModel> companyData = new List<DatacastTemplateModel>();
                string division_name = "";
                string record_id_ = "";

                WebClient client = new WebClient();
                string url = "https://itreconomics.quickbase.com/db/bg794fvvy?a=API_DoQuery&usertoken=b2555s_ynh_bdhajpwdb3qxm4cn7n4jidps9pwp&query={66.TV." + id + "}&clist=23.66.6.3";

                string xmlString = client.DownloadString(url);
                var doc = new XmlDocument();
                doc.LoadXml(xmlString);

                XmlNode nodes = doc.SelectSingleNode("/qdbapi");
                for (int i = 0; i < nodes.ChildNodes.Count; i++)
                {
                    if (nodes.ChildNodes[i].Name == "record")
                    {
                        for (int j = 0; j < nodes.ChildNodes[i].ChildNodes.Count; j++)
                        {
                            if (nodes.ChildNodes[i].ChildNodes[j].Name == "division_name")
                            {
                                division_name = nodes.ChildNodes[i].ChildNodes[j].InnerText;
                            }
                            else if (nodes.ChildNodes[i].ChildNodes[j].Name == "record_id_")
                            {
                                record_id_ = nodes.ChildNodes[i].ChildNodes[j].InnerText;
                            }
                        }
                        companyData.Add(new DatacastTemplateModel
                        {
                            Divisions = division_name,
                            DivisionsRecordId = record_id_
                        });
                    }
                }

                var dt1 = companyData.OrderBy(x => x.Divisions);
                var divisions = (from dr in dt1.AsEnumerable()
                                 select new SelectListItem
                                 {
                                     Text = dr.Divisions,
                                     Value = dr.DivisionsRecordId.ToString()
                                 }).ToList();
                result = Json(divisions, JsonRequestBehavior.AllowGet);

                return result;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        ///// <summary>
        ///// Created By: Vishnu 
        ///// Created On: 21-Feb-2014 
        ///// To get the Divisions  picklist values from the API
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>   
        //[AcceptVerbs(HttpVerbs.Get)]
        //public JsonResult GetDivisionsByAccountID(int id)
        //{
        //    JsonResult result = null;
        //    try
        //    {

        //        List<TemplateModel> companyData = new List<TemplateModel>();
        //        string division_name = "";
        //        string record_id_ = "";

        //        WebClient client = new WebClient();
        //        string url = "https://itreconomics.quickbase.com/db/bg794fvvy?a=API_DoQuery&usertoken=b2555s_ynh_bdhajpwdb3qxm4cn7n4jidps9pwp&query={66.TV." + id + "}&clist=23.66.6.3";

        //        string xmlString = client.DownloadString(url);
        //        var doc = new XmlDocument();
        //        doc.LoadXml(xmlString);

        //        XmlNode nodes = doc.SelectSingleNode("/qdbapi");
        //        for (int i = 0; i < nodes.ChildNodes.Count; i++)
        //        {
        //            if (nodes.ChildNodes[i].Name == "record")
        //            {
        //                for (int j = 0; j < nodes.ChildNodes[i].ChildNodes.Count; j++)
        //                {
        //                    if (nodes.ChildNodes[i].ChildNodes[j].Name == "division_name")
        //                    {
        //                        division_name = nodes.ChildNodes[i].ChildNodes[j].InnerText;
        //                    }
        //                    else if (nodes.ChildNodes[i].ChildNodes[j].Name == "record_id_")
        //                    {
        //                        record_id_ = nodes.ChildNodes[i].ChildNodes[j].InnerText;
        //                    }
        //                }
        //                companyData.Add(new TemplateModel
        //                {
        //                    Divisions = division_name,
        //                    DivisionsRecordId = record_id_
        //                });
        //            }
        //        }

        //        var dt1 = companyData.OrderBy(x => x.Divisions);
        //        var divisions = (from dr in dt1.AsEnumerable()
        //                         select new SelectListItem
        //                         {
        //                             Text = dr.Divisions,
        //                             Value = dr.DivisionsRecordId.ToString()
        //                         }).ToList();
        //        result = Json(divisions, JsonRequestBehavior.AllowGet);

        //        return result;


        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        /// <summary>
        /// Created By: Vishnu
        /// Created On:07-Sept-2016
        /// Handles Session request from drupal 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ITRSessionRequest(DatacastSessionModel model)
        {
            ActionResult response = null;
            try
            {
                string DomainName = HttpContext.Request.Url.GetLeftPart(UriPartial.Authority);

                //Add the Json object values to Session so that they can be used throughout this application                
                //if (Session.Count > 0)
                //{
                //    //Removing all session information
                //    //Session.RemoveAll();
                //    //Session.Abandon();
                //    Session.Clear();
                //    Session.Abandon();
                //    Session.RemoveAll();
                //}

                //if (model.UserID != null)
                //    Session["UserId"] = model.UserID;
                //if (model.CompanyID != null)
                //    Session["CompanyID"] = model.CompanyID;
                //if (model.DivisonID != null)
                //    Session["DivisonID"] = model.DivisonID;
                //if (model.IsDataPortalAdmin != null)
                //    Session["IsDataPortalAdmin"] = model.IsDataPortalAdmin;
                //if (model.Value1 != null)
                //    Session["UserName"] = model.Value1;

                response = Json(new { result = "Success", url = DomainName + "/Home/DataCast", ComapnyId = model.CompanyID, DivisonID = model.DivisonID, IsDataPortalAdmin = model.IsDataPortalAdmin, UserID = model.UserID, UserName = model.Value1 });
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        /// <summary>
        /// Created By:Vishnu
        /// Created On: Sept-08-2016
        /// it is loaded by drupal Iframe src and handles session variables on document ready
        /// </summary>
        /// <returns></returns>
        public ActionResult ITRSession()
        {
            return View();
        }

        [HttpPost]
        // [EnableCors(origins: "http://ontrack.verudix.net/", headers: "*", methods: "*")]
        public ActionResult ITRSessionRequest2(DatacastSessionModel model)
        {
            ActionResult response = null;
            string DomainName = HttpContext.Request.Url.GetLeftPart(UriPartial.Authority);

            //Add the Json object values to Session so that they can be used throughout this application
            if (model.CompanyID != null && model.CompanyID != "" && model.DivisonID != null && model.DivisonID != "")
            {
                Session["CompanyID"] = model.CompanyID;
                Session["DivisonID"] = model.DivisonID;
                response = Json(new { result = "Success", url = DomainName + "/Home/DataCast" });
            }
            else
            {
                response = Json(new { result = "Failure", url = DomainName + "/Home/DataCast" });
            }

            return response;
        }

        /// <summary>
        /// Created by Khajavali  
        /// Created Date : 31-05-2017
        /// Insering the Favorites values into db.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddFavorites(DatacastFavoritesModel model)
        {
            ActionResult response = null;

            try
            {
                //for holding response value of DBCall
                int intResult = 0;

                //if (Session["UserId"] != null && Session["UserId"].ToString() != string.Empty)
                //{
                //    model.CreatedBY = Session["UserId"].ToString();
                //    model.CreatedDate = DateTime.Now;

                //}

                // model.UserID = Convert.ToInt32(Session["UserId"]);
                //inserts the details to table
                intResult = _homeRepository.AddFavorites(model);

                if (intResult == 1) //inserted successfully
                {
                    response = Json(new { result = "Success" });
                }
                else //On Error
                {
                    response = Json(new { result = "Error" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;
        }

        /// <summary>
        /// Created by Khajavali  
        /// Created Date : 05-06-2017
        /// Update the Favorites values into db.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateFavorites(DatacastFavoritesModel model)
        {
            ActionResult response = null;

            try
            {
                //for holding response value of DBCall
                int intResult = 0;

                //if (Session["UserId"] != null && Session["UserId"].ToString() != string.Empty)
                //{
                //    model.CreatedBY = Session["UserId"].ToString();
                //    model.CreatedDate = DateTime.Now;
                //    // model.UserID = Session["UserId"].ToString();

                //}



                //inserts the details to table
                intResult = _homeRepository.UpdateFavorites(model);

                if (intResult == 1) //inserted successfully
                {
                    response = Json(new { result = "Success" });
                }
                else //On Error
                {
                    response = Json(new { result = "Error" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;
        }




        /// <summary>
        /// Created by Raghuveer  
        /// Created Date : 22 June 2017
        /// Delete the Favorites from Database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Deletefavourites(string CompanyShortCode, int UserID, string FavouriteListName)
        {
            ActionResult response = null;

            try
            {
                //for holding response value of DBCall
                int intResult = 0;

                //if (Session["UserId"] != null && Session["UserId"].ToString() != string.Empty)
                //{
                //    model.CreatedBY = Session["UserId"].ToString();
                //    model.CreatedDate = DateTime.Now;

                //}

                //inserts the details to table
                intResult = _homeRepository.DeleteFavouriteList(CompanyShortCode, UserID, FavouriteListName);

                if (intResult == 1) //inserted successfully
                {
                    response = Json(new { result = "1" });
                }
                else //On Error
                {
                    response = Json(new { result = "0" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;
        }

        /// <summary>
        /// Created By : Raghuveer
        /// Created On : 29 August 2017
        /// Purpose : Datacast 2.3 Search by Category in Datacast page
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetCategoriesForDatacast()
        {
            JsonResult result = null;
            try
            {

                var dt1 = _homeRepository.GetCategoriesForDatacast();
                var divisions = (from dr in dt1.AsEnumerable()
                                 select new SelectListItem
                                 {
                                     Text = dr.Industry,
                                     Value = dr.IndustryID.ToString()
                                     //Value= dr.ShortCode
                                 }).ToList();

                result = Json(divisions, JsonRequestBehavior.AllowGet);

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Created By : Raghuveer
        /// Created On : 29 August 2017
        /// Purpose : Datacast 2.3 SubCategories By Category in Datacast page
        /// <param name="Category"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetSubCategoriesbyCategory(string Industry)
        {
            JsonResult result = null;
            try
            {

                var dt1 = _homeRepository.GetSubCategoriesbyCategory(Industry);
                var divisions = (from dr in dt1.AsEnumerable()
                                 select new SelectListItem
                                 {
                                     Text = dr.Sector,
                                     Value = dr.SectorID.ToString()
                                     //Value= dr.ShortCode
                                 }).ToList();

                result = Json(divisions, JsonRequestBehavior.AllowGet);

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Created BY : Raghuveer
        /// Created Date : 30 AUgust 2017
        /// For Getting SubSectors1 values based on Industry and Sector values
        /// </summary>
        /// <param name="Industry"></param>
        /// <param name="Sector"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetSubSectors1BySector(string Industry,string Sector)
        {
            JsonResult result = null;
            try
            {

                var dt1 = _homeRepository.GetSubSectors1BySector(Industry, Sector);
                var divisions = (from dr in dt1.AsEnumerable()
                                 select new SelectListItem
                                 {
                                     Text = dr.SubSector1,
                                     Value = dr.SubSector1ID.ToString()
                                     //Value= dr.ShortCode
                                 }).ToList();

                result = Json(divisions, JsonRequestBehavior.AllowGet);

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Created BY : Raghuveer
        /// Created Date : 30 AUgust 2017
        /// For Getting SubSectors2 values based on Industry,Sector and SubSector1 values
        /// </summary>
        /// <param name="Industry"></param>
        /// <param name="Sector"></param>
        /// <param name="SubSector1"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetSubSectors2BySubSectors1(string Industry, string Sector, string SubSector1)
        {
            JsonResult result = null;
            try
            {

                var dt1 = _homeRepository.GetSubSectors2BySubSectors1(Industry, Sector, SubSector1);
                var divisions = (from dr in dt1.AsEnumerable()
                                 select new SelectListItem
                                 {
                                     Text = dr.SubSector2,
                                     Value = dr.SubSector2ID.ToString()
                                     //Value= dr.ShortCode
                                 }).ToList();

                result = Json(divisions, JsonRequestBehavior.AllowGet);

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Created BY : Raghuveer
        /// Created Date : 30 AUgust 2017
        /// For Getting SubSectors3 values based on Industry,Sector and SubSector2 values
        /// </summary>
        /// <param name="Industry"></param>
        /// <param name="Sector"></param>
        /// <param name="SubSector1"></param>
        /// <param name="SubSector2"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetSubSectors3BySubSectors2(string Industry, string Sector, string SubSector1, string SubSector2)
        {
            JsonResult result = null;
            try
            {

                var dt1 = _homeRepository.GetSubSectors3BySubSectors2(Industry, Sector,SubSector1,  SubSector2);
                var divisions = (from dr in dt1.AsEnumerable()
                                 select new SelectListItem
                                 {
                                     Text = dr.SubSector3,
                                     Value = dr.SubSector3ID.ToString()
                                     //Value= dr.ShortCode
                                 }).ToList();

                result = Json(divisions, JsonRequestBehavior.AllowGet);

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }






        /// <summary>
        /// Created by :Khajavali
        /// Created on : August 30 2017
        /// /// Fill the Favorites CorelationLead data along with  Category Search
        /// <param name="param"></param>
        /// <param name="dataSetID"></param>
        /// <returns></returns>
        public ActionResult FillFavoritesCorelationLeadLagByCategorySearch(DatacastJQueryDataTableParamModel param, string dataSetID, int userID, string IndustryName,
            string SectorName, string SubSector1Name, string SubSector2Name, string SubSector3Name)
        {

            
            int sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            string sortDirection = Request["sSortDir_0"];
            if (sortColumnIndex == null)
            {
                sortColumnIndex = 0;
            }
            if (sortDirection == null)
            {
                sortDirection = "ASC";
            }
            if (param.sSearch == null)
            {
                param.sSearch = "";
            }

            IEnumerable<uspSelectIndicatorListByCategorySearch_Result> filteredFavorites = _homeRepository.GetIndcatorListByCategorySearch(dataSetID, Convert.ToInt32(userID), IndustryName, SectorName, SubSector1Name, SubSector2Name, SubSector3Name, param.iDisplayLength, param.iDisplayStart, param.sSearch, sortColumnIndex, sortDirection);
            IEnumerable<uspSelectIndicatorListByCategorySearch_Result> searchFavorites = null;


            var result = from c in filteredFavorites
                         select new[] { Convert.ToString(c.ASeriesName),Convert.ToString(c.CorrValue),Convert.ToString(c.lagValue),
                             Convert.ToString(c.Company), Convert.ToString(c.Indicator), Convert.ToString(c.Description), Convert.ToString(c.FavouriteListName) ,Convert.ToString(c.Indicator)};



            var totalcount = filteredFavorites.Count() == 0 ? 0 : filteredFavorites.FirstOrDefault().TotalCount.Value;
            var iTotalRecordsCount = filteredFavorites.Count() == 0 ? 0 : filteredFavorites.FirstOrDefault().iTotalRecords.Value;

            //  var totalcount = filteredEmployees.FirstOrDefault().TotalCount.Value;

            // var iTotalRecordsCount = filteredEmployees.FirstOrDefault().iTotalRecords.Value.ToString();



            if (totalcount == 0)// for search  with ZERO records as return case.
            {
                searchFavorites = _homeRepository.GetIndcatorListByCategorySearch(dataSetID, Convert.ToInt32(userID), IndustryName, SectorName, SubSector1Name, SubSector2Name, SubSector3Name, param.iDisplayLength, param.iDisplayStart, param.sSearch, sortColumnIndex, sortDirection)
                .Where(c => Convert.ToString(c.ASeriesName).Contains(param.sSearch)
                                ||
                       c.Company.ToLower().Contains(param.sSearch)
                                ||
                     c.Indicator.ToLower().Contains(param.sSearch)
                                );

                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = iTotalRecordsCount,
                    iTotalDisplayRecords = totalcount,
                    aaData = searchFavorites
                },
                         JsonRequestBehavior.AllowGet);
            }
            else
            {

                return Json(new
                {

                    sEcho = param.sEcho,
                    iTotalRecords = iTotalRecordsCount,
                    iTotalDisplayRecords = totalcount,
                    aaData = result
                },
                        JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        /// Created By : Raghuveer
        /// Created Date : December 8 , 2017
        /// For Adding Popup Access Date
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="CompanyShortCode"></param>
        /// <param name="LastAccessDate"></param>
        /// <param name="CreatedBy"></param>
        /// <param name="CreatedDate"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult InsertPopupAccessDate(int UserID, string CompanyShortCode)
        {
            ActionResult response = null;
            try
            {
                //for holding response value of DBCall
                int intResult = 0;
                CompanyShortCode = HttpUtility.UrlDecode(CompanyShortCode);
                intResult = _homeRepository.InsertPopupAccessDate(UserID, CompanyShortCode, DateTime.Now, UserID, DateTime.Now);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;
        }

        /* BY VIJAYA */
        /// <summary>
        /// Created by VIJAYA  
        /// Created Date : 2018/04/12
        /// Insering the DASHBOARD WIDGETS VALUES TO DB...
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult InsertDashboardWidgetsList(DatacastFavoritesModel model)
        {
            ActionResult response = null;

            try
            {
                //for holding response value of DBCall
                int intResult = 0;

                //if (Session["UserId"] != null && Session["UserId"].ToString() != string.Empty)
                //{
                //    model.CreatedBY = Session["UserId"].ToString();
                //    model.CreatedDate = DateTime.Now;
                //}
                
                //inserts the details to table
                intResult = _homeRepository.InsertDashboardWidgetsList(model);

                if (intResult == 1) //inserted successfully
                {
                    response = Json(new { result = "Success" });
                }
                else //On Error
                {
                    response = Json(new { result = "Error" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;
        }

        /// <summary>
        /// Created by VIJAYA  
        /// Created Date : 2018/04/12
        /// Updateing the DASHBOARD WIDGETS VALUES TO DB...
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateDashboardWidgetsList(DatacastFavoritesModel model)
        {
            ActionResult response = null;

            try
            {
                //for holding response value of DBCall
                int intResult = 0;

                //if (Session["UserId"] != null && Session["UserId"].ToString() != string.Empty)
                //{
                //    model.CreatedBY = Session["UserId"].ToString();
                //    model.CreatedDate = DateTime.Now;
                //}

                //inserts the details to table
                intResult = _homeRepository.UpdateDashboardWidgetsList(model);

                if (intResult == 1) //inserted successfully
                {
                    response = Json(new { result = "Success" });
                }
                else //On Error
                {
                    response = Json(new { result = "Error" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;
        }

        /// <summary>
        /// Created by :Khajavali
        /// Created on : 20-06-2017
        /// Fill the Dashboard Widget names by dataSetID and userid
        /// <param name="param"></param>
        /// <param name="dataSetID"></param>
        /// <returns></returns>
        public ActionResult FillDBWidgetsListByCompanyID(DatacastJQueryDataTableParamModel param, string dataSetID, int userID)
        {
            int sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            string sortDirection = Request["sSortDir_0"];
            if (sortColumnIndex == null)
            {
                sortColumnIndex = 0;
            }
            if (sortDirection == null)
            {
                sortDirection = "ASC";
            }
            if (param.sSearch == null)
            {
                param.sSearch = "";
            }

            IEnumerable<uspSelectDBWidgetsListByCompanyID_Result> filteredFavorites = _homeRepository.SelectDBWidgetsListByCompanyID(userID,dataSetID);// Convert.ToInt32(Session["UserId"]));
            IEnumerable<uspSelectDBWidgetsListByCompanyID_Result> searchFavorites = null;


            var result = from c in filteredFavorites
                         select new[] { Convert.ToString(c.DashboardWidgetsListId),
                                        Convert.ToString(c.Indicator), 
                                        Convert.ToString(c.Indicator) };

            var totalcount = filteredFavorites.Count();// == 0 ? 0 : filteredFavorites.FirstOrDefault().TotalCount.Value;
            var iTotalRecordsCount = filteredFavorites.Count();// == 0 ? 0 : filteredFavorites.FirstOrDefault().iTotalRecords.Value;

            if (totalcount == 0)// for search  with ZERO records as return case.
            {
                searchFavorites = _homeRepository.SelectDBWidgetsListByCompanyID(userID, dataSetID)// Convert.ToInt32(Session["UserId"]))
                .Where(c => Convert.ToString(c.Indicator).Contains(param.sSearch)
                                ||
                            c.IndicatorShortCode.ToLower().Contains(param.sSearch)
                                ||
                            c.Indicator.ToLower().Contains(param.sSearch)
                       );

                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = iTotalRecordsCount,
                    iTotalDisplayRecords = totalcount,
                    aaData = searchFavorites
                },
                         JsonRequestBehavior.AllowGet);
            }
            else
            {

                return Json(new
                {

                    sEcho = param.sEcho,
                    iTotalRecords = iTotalRecordsCount,
                    iTotalDisplayRecords = totalcount,
                    aaData = result
                },
                        JsonRequestBehavior.AllowGet);
            }
        }       

        /// <summary>
        /// Created by VIJAYA  
        /// Created Date : 2018/04/12
        /// GET ALL DASHBOARD WIDGETS OF A COMPANY...
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult SelectDBWidgetsListByCompanyID(string dataSetID, int userID)
        {           
            var listInfo = _homeRepository.SelectDBWidgetsListByCompanyID(userID, dataSetID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = int.MaxValue;
            string rettval = serializer.Serialize(listInfo);
            return Json(rettval, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Created by :Khajavali
        /// Created on : 01-06-2017
        /// /// Fill the Favorites CorelationLead data along with Search and Sort by dataSetID and userid
        /// <param name="param"></param>
        /// <param name="dataSetID"></param>
        /// <returns></returns>
        public ActionResult FillDBWidgetsCorelationLeadLagByDataSetIDUesrID(DatacastJQueryDataTableParamModel param, string dataSetID, int userID)
        {
            int sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            string sortDirection = Request["sSortDir_0"];
            if (sortColumnIndex == null)
            {
                sortColumnIndex = 0;
            }
            if (sortDirection == null)
            {
                sortDirection = "ASC";
            }
            if (param.sSearch == null)
            {
                param.sSearch = "";
            }

            //  IEnumerable<uspSelectFavouriteListByUserID_Result> filteredFavorites = _homeRepository.SelectFavoritesLoadCorelationLeadLagByDataSetID(dataSetID, Convert.ToInt32(Session["UserId"]), SelFavName, param.iDisplayLength, param.iDisplayStart, param.sSearch, sortColumnIndex, sortDirection);
            IEnumerable<uspSelectDBWidgetsCorrelationListByCompanyID_Result> filteredFavorites = _homeRepository.SelectDBWidgetsCorrelationListByCompanyID(dataSetID, userID, param.iDisplayLength, param.iDisplayStart, param.sSearch, sortColumnIndex, sortDirection);
            IEnumerable<uspSelectDBWidgetsCorrelationListByCompanyID_Result> searchFavorites = null;


            var result = from c in filteredFavorites
                         select new[] { Convert.ToString(c.ASeriesName),
                                        Convert.ToString(c.CorrValue),
                                        Convert.ToString(c.lagValue),
                                        Convert.ToString(c.Company), 
                                        Convert.ToString(c.Indicator), 
                                        Convert.ToString(c.Description), 
                                        Convert.ToString(c.Indicator), //FavouriteListName, We don't get FavouriteListName in DBWidgets list...
                                        Convert.ToString(c.Indicator), //FOR FAVOURITES CHECKBOX...
                                        Convert.ToString(c.Indicator) //FOR DB WIDGET CHECKBOX...
                         };

            var totalcount = filteredFavorites.Count() == 0 ? 0 : filteredFavorites.FirstOrDefault().TotalCount.Value;
            var iTotalRecordsCount = filteredFavorites.Count() == 0 ? 0 : filteredFavorites.FirstOrDefault().iTotalRecords.Value;

            if (totalcount == 0)// for search  with ZERO records as return case.
            {                
                searchFavorites = _homeRepository.SelectDBWidgetsCorrelationListByCompanyID(dataSetID, userID, param.iDisplayLength, param.iDisplayStart, param.sSearch, sortColumnIndex, sortDirection)
                .Where(c => Convert.ToString(c.ASeriesName).Contains(param.sSearch)
                                ||
                            c.Company.ToLower().Contains(param.sSearch)
                                ||
                            c.Indicator.ToLower().Contains(param.sSearch)
                                );

                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = iTotalRecordsCount,
                    iTotalDisplayRecords = totalcount,
                    aaData = searchFavorites
                },
                         JsonRequestBehavior.AllowGet);
            }
            else
            {

                return Json(new
                {

                    sEcho = param.sEcho,
                    iTotalRecords = iTotalRecordsCount,
                    iTotalDisplayRecords = totalcount,
                    aaData = result
                },
                        JsonRequestBehavior.AllowGet);
            }

        }

        public void myDatatable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MonthYear");
            dt.Columns.Add("Phase");

            string currentYear = DateTime.Now.Year.ToString();

            DateTime currentDate = DateTime.Now;
            DateTime nextYearDate = currentDate.AddYears(1);
            DateTime NNYearDate = currentDate.AddYears(2);

            DataRow tempRow = dt.NewRow();
            tempRow["MonthYear"] = "Mar-" + currentYear;
            tempRow["Phase"] = "N/A";
            dt.Rows.Add(tempRow);

            tempRow = dt.NewRow();
            tempRow[0] = "Jun-" + currentYear ;
            tempRow[1] = "N/A";
            dt.Rows.Add(tempRow);

            tempRow = dt.NewRow();
            tempRow[0] = "Sep-" + currentYear;
            tempRow[1] = "N/A";
            dt.Rows.Add(tempRow);

            tempRow = dt.NewRow();
            tempRow[0] = "Dec-" + currentYear;
            tempRow[1] = "N/A";
            dt.Rows.Add(tempRow);

            tempRow = dt.NewRow();
            tempRow[0] = "Year-End " + nextYearDate;
            tempRow[1] = "N/A";
            dt.Rows.Add(tempRow);

            tempRow = dt.NewRow();
            tempRow[0] = "Year-End " + NNYearDate;
            tempRow[1] = "N/A";
            dt.Rows.Add(tempRow);

            DataTable transposedTable = GenerateTransposedTable(dt);

        }

        private DataTable GenerateTransposedTable(DataTable inputTable)
        {
            DataTable outputTable = new DataTable();

            // Add columns by looping rows

            // Header row's first column is same as in inputTable
            outputTable.Columns.Add(inputTable.Columns[0].ColumnName.ToString());

            // Header row's second column onwards, 'inputTable's first column taken
            foreach (DataRow inRow in inputTable.Rows)
            {
                string newColName = inRow[0].ToString();
                outputTable.Columns.Add(newColName);
            }

            // Add rows by looping columns        
            for (int rCount = 1; rCount <= inputTable.Columns.Count - 1; rCount++)
            {
                DataRow newRow = outputTable.NewRow();

                // First column is inputTable's Header row's second column
                newRow[0] = inputTable.Columns[rCount].ColumnName.ToString();
                for (int cCount = 0; cCount <= inputTable.Rows.Count - 1; cCount++)
                {
                    string colValue = inputTable.Rows[cCount][rCount].ToString();
                    newRow[cCount + 1] = colValue;
                }
                outputTable.Rows.Add(newRow);
            }

            return outputTable;
        }
    }
}