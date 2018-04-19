using ITR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITR.DataRepository
{
    public class IDataRepository
    {
    }

    /// <summary>
    /// Created By :Vishnu
    /// Created On: 23-Aug-2016
    /// Defining all the interaces.
    /// The interface segregation principle encourages loose coupling and therefore makes a system easier to refactor, change, and redeploy
    /// </summary>
    public interface IHomeRepository
    {
        int InsertInsertCustomerDataSet(TemplateModel model);
        int ExecuteCaluclations(int CustomerDataSetID);
        IList<uspSelectDataSetsDetailsByDataSetID_Result> GetDataSetsDetailsByDataSetID(int dataSetID, string Table);
        IList<uspSelectDataSetsByCompanyDivision_Result> GetDataSetsByCompanyDivision(int comapnyId, int divisionId, string customerId);
        IList<uspSelectDataSetsDetailsByDataSetIDForBanner_Result> GetDataSetsDetailsByDataSetIDForBanner(int dataSetID, string Table);
        int DeleteDataSet(int customerDataSetID);
        int ReplaceCustomerDataSet(TemplateModel model);
        int UpdateCustomerDataSet(UpdateDataset model);
        int DeleteRowFromDataSet(int prodID);

        int PushDataSet(int customerDataSetID);

        int InsertExceptionLogDetails(string CustomerID, string ExceptionMessage, string PageName, string CreatedBy);
        IList<uspSelectRowsByDataSetID_Result> GeRowsByDataSetID(int dataSetID, string Table);

        IList<uspSelectAccounts_Result> GetAccounts();
        IList<uspSelectDivisionsByAccountID_Result> GetDivisionsByAccountID(int id);
        IList<uspSelectIndicatorCategoryDropdown_Result> GetIndicatorSeriesCategoryDropdowns();
        IList<uspSelectDataSetsByQBaseAccountIDForDatacast_Result> GetDataSetsByQBaseAccountIDAndDivisionIDForDatacast(int accountID, int divisionID);
        IList<uspSelectLoadCorelationLeadLagByDataSetIDForDatacast_Result> GetIndicatorNamesAndCorrelationLeadLagValuesByDataSetIDForDatacast(string dataSetID, int iDisplayLength, int iDisplayStart, string iSearch, int iSortCol, string iSortDir, int UserID);
        int InsertFavouriteList(string Company, string Indicators, int UserID, string FavouriteListName, string CreatedBy, DateTime CreatedDate);
        IList<uspSelectFavouriteListByUserIDForDatacast_Result> SelectFavoritesLoadCorelationLeadLagByDataSetID(string dataSetID, int userID, string FavouriteListName, int iDisplayLength, int iDisplayStart, string sSearch, int sortColumnIndex, string sortDirection);
        IList<uspSelectAllFavouriteListByUserIDForDatacast_Result> SelectAllFavoritesListgByDataSetIDUserID(string dataSetID, int userID, string FavouriteListName);
        IList<uspSelectFavouritesListByUserIDAndCompanyShortCode_Result> GetFavouriteListByUserIDAndCompanyShortCode(int UserID, string CompanyShortCode);
        IList<uspSelectDataSetsByFavouriteListsOfUserID_Result> GetFavoritesLoadCorelationLeadLagByDataSetID(int AccountID, int DivisionID, int UserID);
        IList<uspSelectFavouritesListByUserID_Result> SelectFavouritesListByUserID(string dataSetID, int userID);
        IList<uspSelectLoadCorelationLeadLagByDataSetID_Result> SelectLoadCorelationLeadLagByDataSetID(string dataSetID, int iDisplayLength, int iDisplayStart, string sSearch, int sortColumnIndex, string sortDirection, int userID);
        IList<uspSelectFavouriteListByUserID_Result> SelectFavoritesLoadCorelationLeadLagByDataSetIDForPortal(string dataSetID, int userID, string SelFavName, int iDisplayLength, int iDisplayStart, string sSearch, int sortColumnIndex, string sortDirection);
        int AddFavorites(FavoritesModel model);
        int UpdateFavorites(FavoritesModel model);
        int DeleteFavouriteList(string CompanyShortCode, int UserID, string FavouriteListName);
        IList<uspSelectAllFavouritesListByUserID_Result> SelectAllFavouritesListByUserID(int UserID);
        IList<uspSelectDataSetsByUserFavouriteList_Result> SelectAllFavouriteListBYUserID(int AccountID, int DivisionID, int UserID, string FavouriteListName);
        int GetAllResultsCount(int CustomerDataSetID);
        IList<uspSelectDatacastCategoriesPortal_Result> GetCategoriesForDatacast();
        IList<uspSelectDatacastSubCategoriesByCategoryPortal_Result> GetSubCategoriesbyCategory(string Industry);
        IList<uspSelectDatacastSubSectorsBySectorPortal_Result> GetSubSectors1BySector(string Industry, string Sector);
        IList<uspSelectDatacastSubSector2BySubSector1Portal_Result> GetSubSectors2BySubSectors1(string Industry, string Sector, string SubSector1);
        IList<uspSelectDatacastSubSector3BySubSector2Portal_Result> GetSubSectors3BySubSectors2(string Industry, string Sector, string SubSector1, string SubSector2);
        IList<uspSelectIndicatorListByCategorySearchPortal_Result> GetIndcatorListByCategorySearch(string dataSetID, int userID, string Industry,
                                    string Sector, string SubSector1, string SubSector2, string SubSector3, int iDisplayLength, int iDisplayStart,
                                    string sSearch, int sortColumnIndex, string sortDirection);

        int ClearDataSetAnalysis(int customerDataSetID);
    }
}