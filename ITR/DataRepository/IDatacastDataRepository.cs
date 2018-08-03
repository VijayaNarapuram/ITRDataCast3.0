using ITR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITR.DataRepository
{
    public class IDatacastRepository
    {
    }

    /// <summary>
    /// Created By :Vishnu
    /// Created On: 02-Nov-2016
    /// Defining all the interaces.
    /// The interface segregation principle encourages loose coupling and therefore makes a system easier to refactor, change, and redeploy
    /// </summary>
    public interface IDatacastHomeRepository
    {
        IList<uspSelectIndicatorForecastDataByIndicatorShortCode_Result> GetIndicatorHistoricalAndForeCastDataBySHORTCODE(string shotrCode);
        IList<uspSelectCompanyForecastDataByCompanyShortCode_Result> GetCompanyDataBySHORTCODE(string shotrCode);
        IList<uspSelectDataSetsByQBaseAccountID_Result> GetDataSetsByQBaseAccountIDAndDivisionID(int accountID, int divisionID);
        //IList<uspSelectLoadCorelationLeadLagByDataSetID_Result> GetIndicatorNamesAndCorrelationLeadLagValuesByDataSetID(string dataSetID, int iDisplayLength, int iDisplayStart);
        IList<uspSelectCompanyAndIndicatorDateByShordCodes_Result> GetIndicatorCompanyAndIndicatorDataByShortCodes(string CompanyShortCode, string IndicatorShortCode, bool InverseOrder, int MoveMonths, int CustomerCompanyID);
        IList<uspSelectCompanyDataForDataTrends_Result> GetCompanyDataForTrends(string CompanyShortCode, int CaluclationType);// Calucaltion Type : 0 -> MMT 1-->MMA
        int InsertExceptionLogDetails(string CustomerID, string ExceptionMessage, string PageName, string CreatedBy);
        IList<uspSelectCompanyDatasetsForInternalRatesOfChange_Result> GetCompanyDataForInternalRatesOfChange(string FirstCompanyDatasetName, string FirstDatasetCaluclationType, string SecondCompanyDatasetName, string SecondDatasetCaluclationType);
        IList<uspSelectLoadCorelationLeadLagByMultipleDatasetIDS_Result> GetIndicatorNamesAndCorrelationLeadLagValuesByMultipleDataSetID(string dataSetIDs, int iDisplayLength, int iDisplayStart);
        IList<uspSelectCompanyDataSetsInfoDownloadSection1_Result> GetCompanyDatasetsInfoForDownloadSection1(string dataSetIDs);
        IList<uspSelectIndicatorDataByIndicatorShortCodes_Result> GetIndicatorDataByIndicatorShortCodes(string indicatorshortcodes, string companycode);
        IList<uspSelectLoadCorelationLeadLagByDataSetID_Result> SelectLoadCorelationLeadLagByDataSetID(string dataSetID, int iDisplayLength, int iDisplayStart, string sSearch, int sortColumnIndex, string sortDirection, int userID);
        IList<uspSelectFavouriteListByUserID_Result> SelectFavoritesLoadCorelationLeadLagByDataSetID(string dataSetID, int userID, string SelFavName, int iDisplayLength, int iDisplayStart, string sSearch, int sortColumnIndex, string sortDirection);
        int AddFavorites(DatacastFavoritesModel model);
        int UpdateFavorites(DatacastFavoritesModel model);
        IList<uspSelectAllFavouriteListByUserID_Result> SelectAllFavoritesListgByDataSetIDUserID(string dataSetID, int userID, string SelFavName);
        int DeleteFavouriteList(string CompanyShortCode, int UserID, string FavouriteListName);
        IList<uspSelectFavouritesListByUserID_Result> SelectFavouritesListByUserID(string dataSetID, int userID);
        IList<uspSelectDatacastCategories_Result> GetCategoriesForDatacast();
        IList<uspSelectDatacastSubCategoriesByCategory_Result> GetSubCategoriesbyCategory(string Industry);
        IList<uspSelectDatacastSubSectorsBySector_Result> GetSubSectors1BySector(string Industry, string Sector);
        IList<uspSelectDatacastSubSector2BySubSector1_Result> GetSubSectors2BySubSectors1(string Industry, string Sector, string SubSector1);
        IList<uspSelectDatacastSubSector3BySubSector2_Result> GetSubSectors3BySubSectors2(string Industry, string Sector, string SubSector1, string SubSector2);
        IList<uspSelectIndicatorListByCategorySearch_Result> GetIndcatorListByCategorySearch(string dataSetID, int userID, string Industry,
                                    string Sector, string SubSector1, string SubSector2, string SubSector3, int iDisplayLength, int iDisplayStart,
                                    string sSearch, int sortColumnIndex, string sortDirection);
        int InsertPopupAccessDate(int UserID, string CompanyShortCode, DateTime LastAccessDate, int CreatedBy, DateTime CreatedDate);

        /* VIJAYA */
        int InsertDashboardWidgetsList(DatacastFavoritesModel model);
        int UpdateDashboardWidgetsList(DatacastFavoritesModel model);
        IList<uspSelectDBWidgetsListByCompanyID_Result> SelectDBWidgetsListByCompanyID(int userID, string dataSetID);
        //int DeleteDBWidgetIndicator(int DashboardWidgetsListId, string CompanyShortCode, int UserID);
        IList<uspSelectDBWidgetsCorrelationListByCompanyID_Result> SelectDBWidgetsCorrelationListByCompanyID(string dataSetID, int userID, int iDisplayLength, int iDisplayStart, string sSearch, int sortColumnIndex, string sortDirection);
              
    }

    /// <summary>
    /// Created By : Vijaya
    /// Created On: 2018/04/18
    /// Defining all the interaces.
    /// The interface segregation principle encourages loose coupling and therefore makes a system easier to refactor, change, and redeploy
    /// </summary>
    public interface DatacastDBWidgetIDataRepository
    {
        #region Dashboard Widgets
        IList<uspSelectPhaseValuesOfDBWidgetsByCompanyId_Result> SelectPhaseValuesOfDBWidgetsByCompanyId(string CompanyShortCode);
        IList<uspSelectPhaseValuesOfDBWidgetsByCompanyIdNew_Result> SelectPhaseValuesOfDBWidgetsByCompanyIdNew(string CompanyShortCode, int CompanyId);
        IList<uspSelectTimingValuesOfDBWidgetsByCompanyId_Result> SelectTimingValuesOfDBWidgetsByCompanyId(string CompanyShortCode);
        int DeleteDBWidgetIndicator(int DashboardWidgetsListId, string CompanyShortCode, int UserID);

        IList<uspSelectDBWidgetsListByCompanyID_Result> SelectDBWidgetsListByCompanyID(int userID, string dataSetID);
        IList<uspSelectCurrentMonthPhaseValuesOfDBWidgetsByCompanyId_Result> SelectCurrentMonthPhaseValuesOfDBWidgetsByCompanyId(string CompanyShortCode,int UserID);
        #endregion
    }
}