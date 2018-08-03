using ITR.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ITR.DataRepository
{
    public class DatacastDataRepository
    {
    }


    public class DatacastHomeRepository : IDatacastHomeRepository, IDisposable
    {
        //Creating object for Database entities
        private DatacastEntities objEntities = new DatacastEntities();

        //Maintaining objMCOEntities Object dispose value
        private bool disposed = false;

        /// <summary>
        /// Created By:Vishnu
        /// Created On: 02-Nov-2016
        /// Gets the indicator historical and forecast data by indicator shortcode
        /// </summary>
        /// <param name="shotrCode"></param>
        /// <returns></returns>
        public IList<uspSelectIndicatorForecastDataByIndicatorShortCode_Result> GetIndicatorHistoricalAndForeCastDataBySHORTCODE(string shotrCode)
        {
            IList<uspSelectIndicatorForecastDataByIndicatorShortCode_Result> infoList;
            try
            {

                infoList = new List<uspSelectIndicatorForecastDataByIndicatorShortCode_Result>(); new List<uspSelectIndicatorForecastDataByIndicatorShortCode_Result>();
                foreach (uspSelectIndicatorForecastDataByIndicatorShortCode_Result info in objEntities.uspSelectIndicatorForecastDataByIndicatorShortCode(shotrCode))
                {
                    infoList.Add(info);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return infoList;
        }

        /// <summary>
        /// Created By:Vishnu
        /// Created On: 03-Nov-2016
        /// Gets the Company data by company shortcode
        /// </summary>
        /// <param name="shotrCode"></param>
        /// <returns></returns>
        public IList<uspSelectCompanyForecastDataByCompanyShortCode_Result> GetCompanyDataBySHORTCODE(string shotrCode)
        {
            IList<uspSelectCompanyForecastDataByCompanyShortCode_Result> infoList = new List<uspSelectCompanyForecastDataByCompanyShortCode_Result>();
            foreach (uspSelectCompanyForecastDataByCompanyShortCode_Result info in objEntities.uspSelectCompanyForecastDataByCompanyShortCode(shotrCode))
            {
                infoList.Add(info);
            }
            return infoList;
        }

        /// <summary>
        /// Created By:Vishnu
        /// Created On: 03-Nov-2016
        /// Gets the Datasets by Logged in person QBaseAccountId and QbaseDivisionID
        /// </summary>
        /// <param name="accountID"></param>
        /// <param name="divisionID"></param>
        /// <returns></returns>
        public IList<uspSelectDataSetsByQBaseAccountID_Result> GetDataSetsByQBaseAccountIDAndDivisionID(int accountID, int divisionID)
        {
            IList<uspSelectDataSetsByQBaseAccountID_Result> infoList = new List<uspSelectDataSetsByQBaseAccountID_Result>();
            foreach (uspSelectDataSetsByQBaseAccountID_Result info in objEntities.uspSelectDataSetsByQBaseAccountID(accountID, divisionID))
            {
                infoList.Add(info);
            }
            return infoList;
        }

        /// <summary>
        /// Created By:Vishnu
        /// Created On: 03-Nov-2016
        /// Gets the Indicator Names and Correlation,Lead/Lag values by selected DataSetID
        /// </summary>
        /// <param name="dataSetID"></param>
        /// <returns></returns>
        //public IList<uspSelectLoadCorelationLeadLagByDataSetID_Result> GetIndicatorNamesAndCorrelationLeadLagValuesByDataSetID(string dataSetID, int iDisplayLength, int iDisplayStart)
        //{
        //    IList<uspSelectLoadCorelationLeadLagByDataSetID_Result> infoList = new List<uspSelectLoadCorelationLeadLagByDataSetID_Result>();
        //    foreach (uspSelectLoadCorelationLeadLagByDataSetID_Result info in objEntities.uspSelectLoadCorelationLeadLagByDataSetID(dataSetID, iDisplayLength, iDisplayStart))
        //    {
        //        infoList.Add(info);
        //    }
        //    return infoList;

        //}

        /// <summary>
        /// Created By:Vishnu
        /// Created On: 04-Nov-2016
        /// Gets Company and Indicator Data By shortcodes to plot DataCast chart in same dates range
        /// </summary>
        /// <param name="CompanyShortCode"></param>
        /// <param name="IndicatorShortCode"></param>
        /// <returns></returns>
        public IList<uspSelectCompanyAndIndicatorDateByShordCodes_Result> GetIndicatorCompanyAndIndicatorDataByShortCodes(string CompanyShortCode, string IndicatorShortCode, bool InverseOrder, int MoveMonths, int CustomerCompanyID)
        {
            IList<uspSelectCompanyAndIndicatorDateByShordCodes_Result> infoList = new List<uspSelectCompanyAndIndicatorDateByShordCodes_Result>();
            foreach (uspSelectCompanyAndIndicatorDateByShordCodes_Result info in objEntities.uspSelectCompanyAndIndicatorDateByShordCodes(CompanyShortCode, IndicatorShortCode, InverseOrder, MoveMonths, CustomerCompanyID))
            {
                infoList.Add(info);
            }
            return infoList;
        }
        /// <summary>
        /// Raghuveer Baddula
        /// January 18 2017
        /// For Getting Company Data for Trends (3MMA,3MMT,12MMA,12MMT)
        /// </summary>
        /// <param name="CompanyShortCode"></param>
        /// <param name="IndicatorShortCode"></param>
        /// <param name="InverseOrder"></param>
        /// <param name="MoveMonths"></param>
        /// <returns></returns>
        public IList<uspSelectCompanyDataForDataTrends_Result> GetCompanyDataForTrends(string CompanyShortCode, int CaluclationType) // Calucaltion Type : 0 -> MMT 1-->MMA
        {
            IList<uspSelectCompanyDataForDataTrends_Result> infoList = new List<uspSelectCompanyDataForDataTrends_Result>();
            foreach (uspSelectCompanyDataForDataTrends_Result info in objEntities.uspSelectCompanyDataForDataTrends(CompanyShortCode, CaluclationType))
            {
                infoList.Add(info);
            }
            return infoList;
        }
        /// <summary>       
        /// Created by: Ram Mohan
        /// Created On: 01-09-2016
        /// Inserts the Exception Log Details
        /// </summary>
        /// <param name=""></param>
        /// <param name="model"></param>
        public int InsertExceptionLogDetails(string CustomerID, string ExceptionMessage, string PageName, string CreatedBy)
        {
            System.Data.Objects.ObjectResult<int?> newRoleID = objEntities.uspInsertExceptionLog(CustomerID, ExceptionMessage, PageName, CreatedBy, DateTime.Now);
            var result = newRoleID.FirstOrDefault<int?>();
            if (result.HasValue)
            {
                if (result > 0)
                {
                    //on Success
                    return 1;
                }
                else
                {
                    // when the user is trying to add same UserRole name then donot allow to insert/Add
                    return 2;
                }
            }
            else
            {
                // on failure 
                return 0;
            }

        }


        /// <summary>
        /// Raghuveer
        /// 7 Feb 2017
        /// For Gettign data regarding Internal Rates of Change tab in ITR Chart Application.
        /// </summary>
        /// <param name="CompanyShortCode"></param>
        /// <param name="CaluclationType"></param>
        /// <returns></returns>
        public IList<uspSelectCompanyDatasetsForInternalRatesOfChange_Result> GetCompanyDataForInternalRatesOfChange(string FirstCompanyDatasetName, string FirstDatasetCaluclationType, string SecondCompanyDatasetName, string SecondDatasetCaluclationType)
        {
            IList<uspSelectCompanyDatasetsForInternalRatesOfChange_Result> infoList = new List<uspSelectCompanyDatasetsForInternalRatesOfChange_Result>();
            foreach (uspSelectCompanyDatasetsForInternalRatesOfChange_Result info in objEntities.uspSelectCompanyDatasetsForInternalRatesOfChange(FirstCompanyDatasetName, FirstDatasetCaluclationType, SecondCompanyDatasetName, SecondDatasetCaluclationType))
            {
                infoList.Add(info);
            }
            return infoList;
        }


        /// <summary>
        /// Created By:Vishnu
        /// Created On: 03-Nov-2016
        /// Gets the Indicator Names and Correlation,Lead/Lag values by selected DataSetID
        /// </summary>
        /// <param name="dataSetID"></param>
        /// <returns></returns>
        public IList<uspSelectLoadCorelationLeadLagByMultipleDatasetIDS_Result> GetIndicatorNamesAndCorrelationLeadLagValuesByMultipleDataSetID(string dataSetIDs, int iDisplayLength, int iDisplayStart)
        {
            IList<uspSelectLoadCorelationLeadLagByMultipleDatasetIDS_Result> infoList = new List<uspSelectLoadCorelationLeadLagByMultipleDatasetIDS_Result>();
            foreach (uspSelectLoadCorelationLeadLagByMultipleDatasetIDS_Result info in objEntities.uspSelectLoadCorelationLeadLagByMultipleDatasetIDS(dataSetIDs, iDisplayLength, iDisplayStart))
            {
                infoList.Add(info);
            }
            return infoList;
        }

        /// <summary>
        /// Created By:Vishnu
        /// Created On: 03-Nov-2016
        /// Gets the Indicator Names and Correlation,Lead/Lag values by selected DataSetID
        /// </summary>
        /// <param name="dataSetID"></param>
        /// <returns></returns>
        public IList<uspSelectCompanyDataSetsInfoDownloadSection1_Result> GetCompanyDatasetsInfoForDownloadSection1(string dataSetIDs)
        {
            IList<uspSelectCompanyDataSetsInfoDownloadSection1_Result> infoList = new List<uspSelectCompanyDataSetsInfoDownloadSection1_Result>();
            foreach (uspSelectCompanyDataSetsInfoDownloadSection1_Result info in objEntities.uspSelectCompanyDataSetsInfoDownloadSection1(dataSetIDs))
            {
                infoList.Add(info);
            }
            return infoList;
        }


        /// <summary>
        /// Created By:Vishnu
        /// Created On: 03-Nov-2016
        /// Gets the Indicator Names and Correlation,Lead/Lag values by selected DataSetID
        /// </summary>
        /// <param name="dataSetID"></param>
        /// <returns></returns>
        public IList<uspSelectIndicatorDataByIndicatorShortCodes_Result> GetIndicatorDataByIndicatorShortCodes(string indicatorshortcodes, string companycode)
        {
            IList<uspSelectIndicatorDataByIndicatorShortCodes_Result> infoList = new List<uspSelectIndicatorDataByIndicatorShortCodes_Result>();
            foreach (uspSelectIndicatorDataByIndicatorShortCodes_Result info in objEntities.uspSelectIndicatorDataByIndicatorShortCodes(indicatorshortcodes, companycode))
            {
                infoList.Add(info);
            }
            return infoList;
        }


        public IList<uspSelectLoadCorelationLeadLagByDataSetID_Result> SelectLoadCorelationLeadLagByDataSetID(string dataSetID, int iDisplayLength, int iDisplayStart, string sSearch, int sortColumnIndex, string sortDirection, int userID)
        {
            IList<uspSelectLoadCorelationLeadLagByDataSetID_Result> PortalItemsList = new List<uspSelectLoadCorelationLeadLagByDataSetID_Result>();
            foreach (uspSelectLoadCorelationLeadLagByDataSetID_Result PortalItem in objEntities.uspSelectLoadCorelationLeadLagByDataSetID(dataSetID, iDisplayLength, iDisplayStart, sSearch, sortColumnIndex, sortDirection, userID))
            {
                PortalItemsList.Add(PortalItem);
            }
            return PortalItemsList;
        }

        /// <summary>
        /// Created by :Khajavali
        /// Created on : 01-06-2017
        /// /// select the Favorites CorelationLead data along with Search and Sort by dataSetID and userid
        /// <param name="param"></param>
        /// <param name="dataSetID"></param>
        /// <returns></returns>
        public IList<uspSelectFavouriteListByUserID_Result> SelectFavoritesLoadCorelationLeadLagByDataSetID(string dataSetID, int userID, string SelFavName, int iDisplayLength, int iDisplayStart, string sSearch, int sortColumnIndex, string sortDirection)
        {
            IList<uspSelectFavouriteListByUserID_Result> PortalItemsList = new List<uspSelectFavouriteListByUserID_Result>();
            foreach (uspSelectFavouriteListByUserID_Result PortalItem in objEntities.uspSelectFavouriteListByUserID(dataSetID, userID, SelFavName, iDisplayLength, iDisplayStart, sSearch, sortColumnIndex, sortDirection))
            {
                PortalItemsList.Add(PortalItem);
            }
            return PortalItemsList;
        }

        /// <summary>
        /// Created by :Khajavali
        /// Created on : 20-06-2017
        /// /// select the Favorites list names by dataSetID and userid
        /// <param name="param"></param>
        /// <param name="dataSetID"></param>
        /// <returns></returns>
        public IList<uspSelectFavouritesListByUserID_Result> SelectFavouritesListByUserID(string dataSetID, int userID)
        {
            IList<uspSelectFavouritesListByUserID_Result> PortalItemsList = new List<uspSelectFavouritesListByUserID_Result>();
            foreach (uspSelectFavouritesListByUserID_Result PortalItem in objEntities.uspSelectFavouritesListByUserID(userID, dataSetID))
            {
                PortalItemsList.Add(PortalItem);
            }
            return PortalItemsList;
        }


        /// <summary>
        /// Created by :Khajavali
        /// Created on : 07-06-2017
        /// /// select the Favorites List by dataSetID and userid
        /// <param name="param"></param>
        /// <param name="dataSetID"></param>
        /// <returns></returns>
        public IList<uspSelectAllFavouriteListByUserID_Result> SelectAllFavoritesListgByDataSetIDUserID(string dataSetID, int userID, string SelFavName)
        {
            IList<uspSelectAllFavouriteListByUserID_Result> PortalItemsList = new List<uspSelectAllFavouriteListByUserID_Result>();
            foreach (uspSelectAllFavouriteListByUserID_Result PortalItem in objEntities.uspSelectAllFavouriteListByUserID(dataSetID, userID, SelFavName))
            {
                PortalItemsList.Add(PortalItem);
            }
            return PortalItemsList;
        }


        /// <summary>
        /// Dispose the object
        /// </summary>       
        /// <returns></returns>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    objEntities.Dispose();
                }
            }
            this.disposed = true;
        }

        /// <summary>
        /// Dispose the object by calling GC
        /// </summary>       
        /// <returns></returns>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>     
        /// Created by: Khaja
        /// Created On: 31-05-2017
        /// Inserting Favorites values
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddFavorites(DatacastFavoritesModel model)
        {
            try
            {

                objEntities.uspInsertFavouriteList(model.CompanyShortCode, model.IndicatorShortCodes, model.UserID, model.FavouriteListName, model.CreatedBY, model.CreatedDate);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>     
        /// Created by: Khaja
        /// Created On: 05-06-2017
        /// Update Favorites values
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateFavorites(DatacastFavoritesModel model)
        {
            try
            {

                objEntities.uspUpdateFavouriteList(model.CompanyShortCode, model.IndicatorShortCodes, model.UserID, model.FavouriteListName, model.CreatedBY, model.CreatedDate);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }



        /// <summary>
        /// Created  By : Raghuveer
        /// Created On : 22 June 2017
        /// For Delete Favourite List name based on CompanyShortCode, UserID and FavouriteListName
        /// </summary>
        /// <param name="CompanyShortCode"></param>
        /// <param name="UserID"></param>
        /// <param name="FavouriteListName"></param>
        /// <returns></returns>
        public int DeleteFavouriteList(string CompanyShortCode, int UserID, string FavouriteListName)
        {
            try
            {
                System.Data.Objects.ObjectResult<int?> returnValue = objEntities.uspDeleteFavoriteListByCompanyAndUserID(CompanyShortCode, UserID, FavouriteListName);
                var result = returnValue.FirstOrDefault<int?>();
                if (result.HasValue)
                {
                    if (result > 0)
                    {
                        //on Success
                        return 1;
                    }
                    else
                    {
                        // when the user is trying to add same UserRole name then donot allow to insert/Add
                        return 0;
                    }
                }
                else
                {
                    // on failure 
                    return 0;
                }

            }
            catch (Exception ex)
            {
                return 0;
            }
            return 1;
        }


        /// <summary>
        /// Created By : Raghuveer
        /// Created On: 29 August 2017
        /// Purpose : For Datacast 2.3 Search By Category
        /// </summary>
        /// <returns></returns>
        public IList<uspSelectDatacastCategories_Result> GetCategoriesForDatacast()
        {
            IList<uspSelectDatacastCategories_Result> industryList = new List<uspSelectDatacastCategories_Result>();
            foreach (uspSelectDatacastCategories_Result industry in objEntities.uspSelectDatacastCategories())
            {
                industryList.Add(industry);
            }
            return industryList;
        }


        /// <summary>
        /// Created By : Raghuveer
        /// Created Date : 30 August 2017
        ///  Getting Sectors BY Industry column in LDBTEST database
        /// </summary>
        /// <param name="Industry"></param>
        /// <returns></returns>
        public IList<uspSelectDatacastSubCategoriesByCategory_Result> GetSubCategoriesbyCategory(string Industry)
        {
            IList<uspSelectDatacastSubCategoriesByCategory_Result> sectorList = new List<uspSelectDatacastSubCategoriesByCategory_Result>();
            foreach (uspSelectDatacastSubCategoriesByCategory_Result sector in objEntities.uspSelectDatacastSubCategoriesByCategory(Industry))
            {
                sectorList.Add(sector);
            }
            return sectorList;
        }

        /// <summary>
        /// Created By : Raghuveer
        /// Created Date : 30 August 2017
        /// Getting SubSectos1 By Sector and Industry column value from LDBTEST database
        /// </summary>
        /// <param name="Industry"></param>
        /// <param name="Sector"></param>
        /// <returns></returns>
        public IList<uspSelectDatacastSubSectorsBySector_Result> GetSubSectors1BySector(string Industry, string Sector)
        {
            IList<uspSelectDatacastSubSectorsBySector_Result> subSector1List = new List<uspSelectDatacastSubSectorsBySector_Result>();
            foreach (uspSelectDatacastSubSectorsBySector_Result subsector1 in objEntities.uspSelectDatacastSubSectorsBySector(Industry, Sector))
            {
                subSector1List.Add(subsector1);
            }
            return subSector1List;
        }


        /// <summary>
        /// Created By : Raghuveer
        /// Created Date : 30 August 2017
        /// Getting SubSectos2 By Sector and Industry, SubSector1 column value from LDBTEST database
        /// </summary>
        /// <param name="Industry"></param>
        /// <param name="Sector"></param>
        /// <param name="SubSector1"></param>
        /// <returns></returns>
        public IList<uspSelectDatacastSubSector2BySubSector1_Result> GetSubSectors2BySubSectors1(string Industry,string Sector,string SubSector1)
        {
            IList<uspSelectDatacastSubSector2BySubSector1_Result> subSector2List = new List<uspSelectDatacastSubSector2BySubSector1_Result>();
            foreach (uspSelectDatacastSubSector2BySubSector1_Result subsector2 in objEntities.uspSelectDatacastSubSector2BySubSector1(Industry, Sector, SubSector1))
            {
                subSector2List.Add(subsector2);
            }
            return subSector2List;
        }

        /// <summary>
        /// Created By : Raghuveer
        /// Created Date : 30 August 2017
        /// Getting SubSectos3 By Sector and Industry, SubSector1, SubSector2 column values from LDBTEST database
        /// </summary>
        /// <param name="Industry"></param>
        /// <param name="Sector"></param>
        /// <param name="SubSector1"></param>
        /// <param name="SubSector2"></param>
        /// <returns></returns>
        public IList<uspSelectDatacastSubSector3BySubSector2_Result> GetSubSectors3BySubSectors2(string Industry, string Sector, string SubSector1, string SubSector2)
        {
            IList<uspSelectDatacastSubSector3BySubSector2_Result> subSector3List = new List<uspSelectDatacastSubSector3BySubSector2_Result>();
            foreach (uspSelectDatacastSubSector3BySubSector2_Result subsector3 in objEntities.uspSelectDatacastSubSector3BySubSector2(Industry, Sector, SubSector1, SubSector2))
            {
                subSector3List.Add(subsector3);
            }
            return subSector3List;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Industry"></param>
        /// <param name="Sector"></param>
        /// <param name="SubSector1"></param>
        /// <param name="SubSector2"></param>
        /// <returns></returns>
        public IList<uspSelectIndicatorListByCategorySearch_Result> GetIndcatorListByCategorySearch(string dataSetID, int userID, string Industry ,
                                    string Sector ,string SubSector1 ,string  SubSector2 ,string SubSector3 , int iDisplayLength, int iDisplayStart,
                                    string sSearch, int sortColumnIndex, string sortDirection)
        {
            IList<uspSelectIndicatorListByCategorySearch_Result> corelationList = new List<uspSelectIndicatorListByCategorySearch_Result>();
            foreach (uspSelectIndicatorListByCategorySearch_Result corelation in objEntities.uspSelectIndicatorListByCategorySearch(dataSetID,  userID,  Industry ,
                                     Sector , SubSector1 ,  SubSector2 , SubSector3 ,  iDisplayLength,  iDisplayStart,
                                     sSearch,  sortColumnIndex,  sortDirection))
            {
                corelationList.Add(corelation);
            }
            return corelationList;
        }



        /// <summary>
        /// Created  By : Raghuveer
        /// Created On : 8 Dec  2017
        /// For Insert Last Popup Accessed Date
        /// </summary>
        /// <param name="CompanyShortCode"></param>
        /// <param name="UserID"></param>
        /// <param name="FavouriteListName"></param>
        /// <returns></returns>
        public int InsertPopupAccessDate(int UserID,string CompanyShortCode,DateTime LastAccessDate,int CreatedBy,DateTime CreatedDate)
        {
            try
            {
                System.Data.Objects.ObjectResult<int?> returnValue = objEntities.uspInsertPopupAccessingDate(UserID, CompanyShortCode, LastAccessDate, CreatedBy, CreatedDate);
                var result = returnValue.FirstOrDefault<int?>();
                if (result.HasValue)
                {
                    if (result > 0)
                    {
                        //on Success
                        return 1;
                    }
                    else
                    {
                        // when the user is trying to add same UserRole name then donot allow to insert/Add
                        return 0;
                    }
                }
                else
                {
                    // on failure 
                    return 0;
                }

            }
            catch (Exception ex)
            {
                return 0;
            }
            return 1;
        }

        /* VIJAYA */
        /// <summary>
        /// Created by: VIJAYA
        /// Created On: 2018/04/12
        /// Inserting DASHBOARD WIDGETS values...
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int InsertDashboardWidgetsList(DatacastFavoritesModel model)
        {
            try
            {

                objEntities.uspInsertDashboardWidgetsList(model.CompanyShortCode, 
                                                          model.IndicatorShortCodes, 
                                                          model.UserID, 
                                                          model.FavouriteListName, 
                                                          model.CreatedBY, 
                                                          model.CreatedDate);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>  
        /// Created by: VIJAYA
        /// Created On: 2018/04/12
        /// Update DASHBOARD WIDGETS values...
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateDashboardWidgetsList(DatacastFavoritesModel model)
        {
            try
            {

                objEntities.uspUpdateDashboardWidgetsList(model.CompanyShortCode, 
                                                          model.IndicatorShortCodes, 
                                                          model.UserID, 
                                                          model.FavouriteListName, 
                                                          model.CreatedBY, 
                                                          model.CreatedDate);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>  
        /// Created by: VIJAYA
        /// Created On: 2018/04/12
        /// Selects DASHBOARD WIDGETS values by CompanyId, UserId...
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IList<uspSelectDBWidgetsListByCompanyID_Result> SelectDBWidgetsListByCompanyID(int userID, string dataSetID)
        {
            IList<uspSelectDBWidgetsListByCompanyID_Result> PortalItemsList = new List<uspSelectDBWidgetsListByCompanyID_Result>();
            foreach (uspSelectDBWidgetsListByCompanyID_Result PortalItem in objEntities.uspSelectDBWidgetsListByCompanyID(userID, dataSetID))
            {
                PortalItemsList.Add(PortalItem);
            }
            return PortalItemsList;
        }        

        ///// <summary>  
        ///// Created by: VIJAYA
        ///// Created On: 2018/04/12
        ///// Selects DASHBOARD WIDGETS values by CompanyId, UserId...
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>       
        public IList<uspSelectDBWidgetsCorrelationListByCompanyID_Result> SelectDBWidgetsCorrelationListByCompanyID(string dataSetID, int userID, int iDisplayLength, int iDisplayStart, string sSearch, int sortColumnIndex, string sortDirection)
        {
            IList<uspSelectDBWidgetsCorrelationListByCompanyID_Result> PortalItemsList = new List<uspSelectDBWidgetsCorrelationListByCompanyID_Result>();
            foreach (uspSelectDBWidgetsCorrelationListByCompanyID_Result PortalItem in objEntities.uspSelectDBWidgetsCorrelationListByCompanyID(dataSetID, userID, iDisplayLength, iDisplayStart, sSearch, sortColumnIndex, sortDirection))
            {
                PortalItemsList.Add(PortalItem);
            }
            return PortalItemsList;
        }
        
    }

    public class DatacastDBWidgetDataRepository : DatacastDBWidgetIDataRepository, IDisposable
    {
        //Creating object for Database entities
        private DatacastEntities objEntities = new DatacastEntities();

        //Maintaining objMCOEntities Object dispose value
        private bool disposed = false;

        #region Dashboard Widgets
        public IList<uspSelectPhaseValuesOfDBWidgetsByCompanyId_Result> SelectPhaseValuesOfDBWidgetsByCompanyId(string CompanyShortCode)
        {
            IList<uspSelectPhaseValuesOfDBWidgetsByCompanyId_Result> PortalItemsList = new List<uspSelectPhaseValuesOfDBWidgetsByCompanyId_Result>();
            foreach (uspSelectPhaseValuesOfDBWidgetsByCompanyId_Result PortalItem in objEntities.uspSelectPhaseValuesOfDBWidgetsByCompanyId(CompanyShortCode))
            {
                PortalItemsList.Add(PortalItem);
            }
            return PortalItemsList;
        }

        public IList<uspSelectPhaseValuesOfDBWidgetsByCompanyIdNew_Result> SelectPhaseValuesOfDBWidgetsByCompanyIdNew(string CompanyShortCode, int CompanyId)
        {
            IList<uspSelectPhaseValuesOfDBWidgetsByCompanyIdNew_Result> PortalItemsList = new List<uspSelectPhaseValuesOfDBWidgetsByCompanyIdNew_Result>();
            foreach (uspSelectPhaseValuesOfDBWidgetsByCompanyIdNew_Result PortalItem in objEntities.uspSelectPhaseValuesOfDBWidgetsByCompanyIdNew(CompanyShortCode, CompanyId))
            {
                PortalItemsList.Add(PortalItem);
            }
            return PortalItemsList;
        }

        public IList<uspSelectCurrentMonthPhaseValuesOfDBWidgetsByCompanyId_Result> SelectCurrentMonthPhaseValuesOfDBWidgetsByCompanyId(string CompanyShortCode,int UserID)
        {
            IList<uspSelectCurrentMonthPhaseValuesOfDBWidgetsByCompanyId_Result> PortalItemsList = new List<uspSelectCurrentMonthPhaseValuesOfDBWidgetsByCompanyId_Result>();
            foreach (uspSelectCurrentMonthPhaseValuesOfDBWidgetsByCompanyId_Result PortalItem in objEntities.uspSelectCurrentMonthPhaseValuesOfDBWidgetsByCompanyId(CompanyShortCode, UserID))
            {
                PortalItemsList.Add(PortalItem);
            }
            return PortalItemsList;
        }

        public IList<uspSelectTimingValuesOfDBWidgetsByCompanyId_Result> SelectTimingValuesOfDBWidgetsByCompanyId(string CompanyShortCode)
        {
            IList<uspSelectTimingValuesOfDBWidgetsByCompanyId_Result> PortalItemsList = new List<uspSelectTimingValuesOfDBWidgetsByCompanyId_Result>();
            foreach (uspSelectTimingValuesOfDBWidgetsByCompanyId_Result PortalItem in objEntities.uspSelectTimingValuesOfDBWidgetsByCompanyId(CompanyShortCode))
            {
                PortalItemsList.Add(PortalItem);
            }
            return PortalItemsList;
        }

        /// <summary>  
        /// Created by: VIJAYA
        /// Created On: 2018/04/19
        /// To Delete an Indicator from Dashboard Widgets List...
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int DeleteDBWidgetIndicator(int DashboardWidgetsListId, string CompanyShortCode, int UserID)
        {
            try
            {
                System.Data.Objects.ObjectResult<int?> returnValue = objEntities.uspDeleteDBWidgetsListByCompanyAndUserID(DashboardWidgetsListId, CompanyShortCode, UserID);
                var result = returnValue.FirstOrDefault<int?>();
                if (result.HasValue)
                {
                    if (result > 0)
                    {
                        //on Success
                        return 1;
                    }
                    else
                    {
                        // when the user is trying to add same UserRole name then donot allow to insert/Add
                        return 0;
                    }
                }
                else
                {
                    // on failure 
                    return 0;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>  
        /// Created by: VIJAYA
        /// Created On: 2018/04/12
        /// Selects DASHBOARD WIDGETS values by CompanyId, UserId...
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IList<uspSelectDBWidgetsListByCompanyID_Result> SelectDBWidgetsListByCompanyID(int userID, string dataSetID)
        {
            IList<uspSelectDBWidgetsListByCompanyID_Result> PortalItemsList = new List<uspSelectDBWidgetsListByCompanyID_Result>();
            foreach (uspSelectDBWidgetsListByCompanyID_Result PortalItem in objEntities.uspSelectDBWidgetsListByCompanyID(userID, dataSetID))
            {
                PortalItemsList.Add(PortalItem);
            }
            return PortalItemsList;
        }        

        #endregion

        /// <summary>
        /// Dispose the object
        /// </summary>       
        /// <returns></returns>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    objEntities.Dispose();
                }
            }
            this.disposed = true;
        }

        /// <summary>
        /// Dispose the object by calling GC
        /// </summary>       
        /// <returns></returns>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
   
}