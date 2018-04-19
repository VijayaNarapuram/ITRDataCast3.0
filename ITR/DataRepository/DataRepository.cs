using ITR.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ITR.DataRepository
{
    public class DataRepository
    {
    }

    /// <summary>
    /// Created By:Vishnu
    /// Created On: 23-Aug-2016
    /// </summary>
    public class HomeRepository : IHomeRepository, IDisposable
    {
        //Creating object for Database entities
        private ITRDevEntities objEntities = new ITRDevEntities();

        //Maintaining objMCOEntities Object dispose value
        private bool disposed = false;

        /// <summary>
        /// Created By:Vishnu
        /// Created On: 23-Aug-2016
        /// Inserts the Customer Datasets to DataBase
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int InsertInsertCustomerDataSet(TemplateModel model)
        {
            int finalResult = 0;

            try
            {
                //System.Data.Objects.ObjectResult<int?>
                System.Data.Objects.ObjectResult<int?> ID = objEntities.uspInsertCustomerDataSet(model.AccountID, model.DivisionID, model.DatasetName, model.DatasetDescription, model.CustomerID, model.CompanyName, model.DataType,
                    model.Base, model.Units, model.CompanyWebsite, model.MonthYears, model.AdjValues,model.UserName,model.IsDataPortalAdmin, model.CreatedBy, model.CreatedDate,model.AccountName);

                var result = ID.FirstOrDefault<int?>();

                if (result.HasValue)
                {
                    if (result > 0)
                    {
                        //on Success
                        finalResult = result.Value;
                    }
                }
                else
                {
                    // on failure 
                    finalResult = 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return finalResult;
        }


      


        public int ExecuteCaluclations(int CustomerDataSetID)
        {
            int finalResult = 0;
            SqlConnection m_odbcon =new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SqlConnString"].ToString());

            try
            {

                SqlCommand command = new SqlCommand("exec uspSelectCompanySalesPortalByCustomerDataSetID @CustomerDataSetID='" + CustomerDataSetID + "'", m_odbcon);
                command.CommandType = CommandType.Text;
                command.CommandTimeout = 900;
                if (m_odbcon.State==ConnectionState.Closed)
                {
                    m_odbcon.Open();
                }
                finalResult =    Convert.ToInt32(command.ExecuteScalar());
                if (m_odbcon.State == ConnectionState.Open)
                {
                    m_odbcon.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (m_odbcon.State == ConnectionState.Open)
                {
                    m_odbcon.Close();
                }
            }
            return finalResult;
        }



        /// <summary>
        /// Created By:Vishnu
        /// Created On: 23-Aug-2016
        /// Gets the Specific DataSet details
        /// </summary>
        /// <param name="dataSetID"></param>
        /// <returns></returns>
        public IList<uspSelectDataSetsDetailsByDataSetID_Result> GetDataSetsDetailsByDataSetID(int dataSetID,string Table)
        {
            IList<uspSelectDataSetsDetailsByDataSetID_Result> dataSetDetails = new List<uspSelectDataSetsDetailsByDataSetID_Result>();
            foreach (uspSelectDataSetsDetailsByDataSetID_Result item in objEntities.uspSelectDataSetsDetailsByDataSetID(dataSetID, Table))
            {
                dataSetDetails.Add(item);
            }
            return dataSetDetails;
        }

        /// <summary>
        /// Created By:Vishnu
        /// Created On: 23-Aug-2016
        /// Gets the Specific DataSet details for Banner
        /// </summary>
        /// <param name="dataSetID"></param>
        /// <returns></returns>
        public IList<uspSelectDataSetsDetailsByDataSetIDForBanner_Result> GetDataSetsDetailsByDataSetIDForBanner(int dataSetID,string Table)
        {
            IList<uspSelectDataSetsDetailsByDataSetIDForBanner_Result> dataSetDetails = new List<uspSelectDataSetsDetailsByDataSetIDForBanner_Result>();
            foreach (uspSelectDataSetsDetailsByDataSetIDForBanner_Result item in objEntities.uspSelectDataSetsDetailsByDataSetIDForBanner(dataSetID, Table))
            {
                dataSetDetails.Add(item);
            }
            return dataSetDetails;
        }

        /// <summary>
        /// Created By:Vishnu
        /// Created On: 23-Aug-2016
        /// Gets the DataSet List of Specific Company and Division 
        /// </summary>
        /// <param name="comapnyId"></param>
        /// <param name="divisionId"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public IList<uspSelectDataSetsByCompanyDivision_Result> GetDataSetsByCompanyDivision(int comapnyId, int divisionId,string customerId)
        {
            IList<uspSelectDataSetsByCompanyDivision_Result> dataSetDetails = new List<uspSelectDataSetsByCompanyDivision_Result>();
            foreach (uspSelectDataSetsByCompanyDivision_Result item in objEntities.uspSelectDataSetsByCompanyDivision(comapnyId, divisionId, customerId))
            {
                dataSetDetails.Add(item);
            }
            return dataSetDetails;
        }

        /// <summary>
        ///  Created By:Ram Mohan
        /// Created On: 24-Aug-2016
        /// Deletes specific Dataset and returns the result
        /// </summary>
        /// <param name="customerDataSetID"></param>
        /// <returns></returns>
        public int DeleteDataSet(int customerDataSetID)
        {
            int finalResult = 0;
            try
            {
                //System.Data.Objects.ObjectResult<int?>
                System.Data.Objects.ObjectResult<int?> ID = objEntities.uspDeleteCustomerDataSet(customerDataSetID);
                var result = ID.FirstOrDefault<int?>();
                if (result.HasValue)
                {
                    if (result > 0)
                    {
                        finalResult = result.Value;
                    }

                }
                else
                {
                    finalResult = 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return finalResult;
        }


        /// <summary>
        /// Created By:Ram Mohan
        /// Created On: 13-Sep-2016
        /// Pushing specific Dataset and returns the result
        /// </summary>
        /// <param name="customerDataSetID"></param>
        /// <returns></returns>
        public int PushDataSet(int customerDataSetID)
        {
            int finalResult = 0;
            try
            {
                //System.Data.Objects.ObjectResult<int?>
                System.Data.Objects.ObjectResult<int?> ID = objEntities.uspPushDataSetToProd(customerDataSetID);
                var result = ID.FirstOrDefault<int?>();

                if (result.HasValue)
                {
                    if (result > 0)
                    {
                        finalResult = result.Value;
                    }

                }
                else
                {
                    finalResult = 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return finalResult;
        }


        /// <summary>
        /// Created By:Ram Mohan
        /// Created On: 258-Aug-2016
        /// Replace the Customer Datasets to DataBase
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int ReplaceCustomerDataSet(TemplateModel model)
        {
            int finalResult = 0;

            try
            {
                //System.Data.Objects.ObjectResult<int?>
                System.Data.Objects.ObjectResult<int?> ID = objEntities.uspReplaceCustomerDataSet(model.CustomerDataSetID, model.CompanyName, model.DataType,
                    model.Base, model.Units, model.CompanyWebsite, model.MonthYears, model.AdjValues,model.UserName,model.IsDataPortalAdmin, model.CreatedBy, model.CreatedDate);

                var result = ID.FirstOrDefault<int?>();

                if (result.HasValue)
                {
                    if (result > 0)
                    {
                        //on Success
                        finalResult = result.Value;
                    }
                }
                else
                {
                    // on failure 
                    finalResult = 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return finalResult;
        }

        /// <summary>
        /// Created By:Nagendra
        /// Created On: 29-Aug-2016
        /// Updates the  Customer Datasets 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateCustomerDataSet(UpdateDataset model)
        {
            int finalResult = 0;

            try
            {
                System.Data.Objects.ObjectResult<int?> ID = objEntities.uspUpdateCustomerDataSet(model.ProdID, model.MonthYears, model.AdjValues, model.DatasetDescription, model.ModifiedBy, model.ModifiedDate);

                var result = ID.FirstOrDefault<int?>();

                if (result.HasValue)
                {
                    if (result > 0)
                    {
                    //on Success
                    finalResult = result.Value;
                    }
                }
                else
                {
                    // on failure 
                    finalResult = 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return finalResult;
        }

        /// <summary>
        /// Created By:Nagendra
        /// Created On: 30-Aug-2016
        /// Delete the row from Customer Datasets 
        /// </summary>
        /// <param name="prodID"></param>
        /// <returns></returns>
        public int DeleteRowFromDataSet(int prodID)
        {
            int finalResult = 0;
            try
            {
                System.Data.Objects.ObjectResult<int?> ID = objEntities.uspDeleteRowFromDataSet(prodID);
                var result = ID.FirstOrDefault<int?>();
                if (result.HasValue)
                {
                    if (result > 0)
                    {
                        finalResult = result.Value;
                    }

                }
                else
                {
                    finalResult = 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return finalResult;
        }

        /// <summary>
        /// Created By:Ram Mohan
        /// Created On: 30-Aug-2016
        /// Gets the Specific DataSet details
        /// </summary>
        /// <param name="dataSetID"></param>
        /// <returns></returns>
        public IList<uspSelectRowsByDataSetID_Result> GeRowsByDataSetID(int dataSetID,string Table)
        {
            IList<uspSelectRowsByDataSetID_Result> dataSetDetails = new List<uspSelectRowsByDataSetID_Result>();
            foreach (uspSelectRowsByDataSetID_Result item in objEntities.uspSelectRowsByDataSetID(dataSetID, Table))
            {
                dataSetDetails.Add(item);
            }
            return dataSetDetails;
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
            System.Data.Objects.ObjectResult<int?> newRoleID = objEntities.uspInsertExceptionLog(CustomerID, ExceptionMessage, PageName, CreatedBy,DateTime.Now);
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
        /// Created By: Ram Mohan
        /// Created On: 14-09-2015
        /// Gets List of Accounts
        /// </summary>
        /// <returns></returns>
        public IList<uspSelectAccounts_Result> GetAccounts()
        {
            IList<uspSelectAccounts_Result> account = new List<uspSelectAccounts_Result>();
            foreach (uspSelectAccounts_Result item in objEntities.uspSelectAccounts())
            {
                account.Add(item);
            }
            return account;
        }


        /// <summary>
        /// Created By: Ram Mohan
        /// Created On: 14-09-2015
        /// Gets List of Divisions by Accounts
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IList<uspSelectDivisionsByAccountID_Result> GetDivisionsByAccountID(int id)
        {
            IList<uspSelectDivisionsByAccountID_Result> List = new List<uspSelectDivisionsByAccountID_Result>();
            foreach (uspSelectDivisionsByAccountID_Result item in objEntities.uspSelectDivisionsByAccountID(id))
            {
                List.Add(item);
            }
            return List;
        }

        /// <summary>
        /// Created By : Raghuveer
        /// Created Date : 11 April 2017
        /// For Gettign Indicator Series Dropdown Dynamically for Dashboard page
        /// </summary>
        /// <returns></returns>
        public IList<uspSelectIndicatorCategoryDropdown_Result> GetIndicatorSeriesCategoryDropdowns()
        {
            IList<uspSelectIndicatorCategoryDropdown_Result> List = new List<uspSelectIndicatorCategoryDropdown_Result>();
            foreach (uspSelectIndicatorCategoryDropdown_Result item in objEntities.uspSelectIndicatorCategoryDropdown())
            {
                List.Add(item);
            }
            return List;
        }

        /// <summary>
        /// Created By:Raghuveer
        /// Created On: 07-June-2017
        /// Gets the Datasets by Logged in person QBaseAccountId and QbaseDivisionID For Datacast 2.1
        /// </summary>
        /// <param name="accountID"></param>
        /// <param name="divisionID"></param>
        /// <returns></returns>
        public IList<uspSelectDataSetsByQBaseAccountIDForDatacast_Result> GetDataSetsByQBaseAccountIDAndDivisionIDForDatacast(int accountID, int divisionID)
        {
            IList<uspSelectDataSetsByQBaseAccountIDForDatacast_Result> infoList = new List<uspSelectDataSetsByQBaseAccountIDForDatacast_Result>();
            foreach (uspSelectDataSetsByQBaseAccountIDForDatacast_Result info in objEntities.uspSelectDataSetsByQBaseAccountIDForDatacast(accountID, divisionID))
            {
                infoList.Add(info);
            }
            return infoList;
        }


        /// <summary>
        ///  Created By : Raghuveer
        ///  Created on : 7-June-2017
        ///  For Getting Indicators based on Company ID 
        /// </summary>
        /// <param name="dataSetID"></param>
        /// <param name="iDisplayLength"></param>
        /// <param name="iDisplayStart"></param>
        /// <returns></returns>
        public IList<uspSelectLoadCorelationLeadLagByDataSetIDForDatacast_Result> GetIndicatorNamesAndCorrelationLeadLagValuesByDataSetIDForDatacast(string dataSetID, int iDisplayLength, int iDisplayStart,string iSearch,int iSortCol,string iSortDir,int UserID)
        {
            IList<uspSelectLoadCorelationLeadLagByDataSetIDForDatacast_Result> infoList = new List<uspSelectLoadCorelationLeadLagByDataSetIDForDatacast_Result>();
            foreach (uspSelectLoadCorelationLeadLagByDataSetIDForDatacast_Result info in objEntities.uspSelectLoadCorelationLeadLagByDataSetIDForDatacast(dataSetID, iDisplayLength, iDisplayStart, iSearch, iSortCol, iSortDir,UserID))
            {
                infoList.Add(info);
            }
            return infoList;

        }



        /// <summary>
        /// Created By : Raghuveer
        /// Created On : 7 -June 2017
        /// For Adding Favourite List Name from ITR Customer Portal
        /// </summary>
        /// <param name="Company"></param>
        /// <param name="Indicators"></param>
        /// <param name="UserID"></param>
        /// <param name="FavouriteListName"></param>
        /// <param name="CreatedBy"></param>
        /// <param name="CreatedDate"></param>string Company,string Indicators,int UserID,string FavouriteListName,string CreatedBy,DateTime CreatedDate
        /// <returns></returns>
        public int InsertFavouriteList(string Company,string  Indicators,int UserID,string  FavouriteListName,string CreatedBy,DateTime CreatedDate)
        {
            int finalResult = 0;

            try
            {
                //System.Data.Objects.ObjectResult<int?>
                System.Data.Objects.ObjectResult<int?> ID = objEntities.uspInsertFavouriteListForDatacast(Company, Indicators, UserID, FavouriteListName, CreatedBy, CreatedDate);

                var result = ID.FirstOrDefault<int?>();

                if (result.HasValue)
                {
                    if (result > 0)
                    {
                        //on Success
                        finalResult = result.Value;
                    }
                }
                else
                {
                    // on failure 
                    finalResult = 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return finalResult;
        }


     
        /// <summary>
        /// Created by :Khajavali
        /// Created on : 01-06-2017
        /// /// select the Favorites CorelationLead data along with Search and Sort by dataSetID and userid
        /// <param name="param"></param>
        /// <param name="dataSetID"></param>
        /// <returns></returns>
        public IList<uspSelectFavouriteListByUserIDForDatacast_Result> SelectFavoritesLoadCorelationLeadLagByDataSetID(string dataSetID, int userID, string FavouriteListName,int iDisplayLength, int iDisplayStart, string sSearch, int sortColumnIndex, string sortDirection)
        {
            IList<uspSelectFavouriteListByUserIDForDatacast_Result> PortalItemsList = new List<uspSelectFavouriteListByUserIDForDatacast_Result>();
            foreach (uspSelectFavouriteListByUserIDForDatacast_Result PortalItem in objEntities.uspSelectFavouriteListByUserIDForDatacast(dataSetID, userID,FavouriteListName, iDisplayLength, iDisplayStart, sSearch, sortColumnIndex, sortDirection))
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
        public IList<uspSelectAllFavouriteListByUserIDForDatacast_Result> SelectAllFavoritesListgByDataSetIDUserID(string dataSetID, int userID,string FavouriteListName)
        {
            IList<uspSelectAllFavouriteListByUserIDForDatacast_Result> PortalItemsList = new List<uspSelectAllFavouriteListByUserIDForDatacast_Result>();
            foreach (uspSelectAllFavouriteListByUserIDForDatacast_Result PortalItem in objEntities.uspSelectAllFavouriteListByUserIDForDatacast(dataSetID, userID, FavouriteListName))
            {
                PortalItemsList.Add(PortalItem);
            }
            return PortalItemsList;
        }


        /// <summary>
        /// Created By : Raghuveer
        /// Created Date : 21 June 2017
        /// For Geting FavouriteList Items based on Company Short Code and UserID
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="CompanyShortCode"></param>
        /// <returns></returns>
        public IList<uspSelectFavouritesListByUserIDAndCompanyShortCode_Result> GetFavouriteListByUserIDAndCompanyShortCode(int UserID,string CompanyShortCode)
        {
            IList<uspSelectFavouritesListByUserIDAndCompanyShortCode_Result> FavList = new List<uspSelectFavouritesListByUserIDAndCompanyShortCode_Result>();
            foreach (uspSelectFavouritesListByUserIDAndCompanyShortCode_Result FavListItem in objEntities.uspSelectFavouritesListByUserIDAndCompanyShortCode(UserID, CompanyShortCode))
            {
                FavList.Add(FavListItem);
            }
            return FavList;
        }


        /// <summary>
        /// Created  BY : Raghuveer
        /// Created on : 22 June 2017
        /// For Getting List of Favourite Items based on AccountID, DivisionID and UserID
        /// </summary>
        /// <param name="AccountID"></param>
        /// <param name="DivisionID"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public IList<uspSelectDataSetsByFavouriteListsOfUserID_Result> GetFavoritesLoadCorelationLeadLagByDataSetID(int AccountID, int DivisionID, int UserID)
        {
            IList<uspSelectDataSetsByFavouriteListsOfUserID_Result> PortalItemsList = new List<uspSelectDataSetsByFavouriteListsOfUserID_Result>();
            foreach (uspSelectDataSetsByFavouriteListsOfUserID_Result PortalItem in objEntities.uspSelectDataSetsByFavouriteListsOfUserID(AccountID,  DivisionID,  UserID))
            {
                PortalItemsList.Add(PortalItem);
            }
            return PortalItemsList;
        }



        /// <summary>
        /// Created by :Raghuveer
        /// Created on : 19 July 2017
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
        /// Created By Raghuveer
        /// Created on : 19 July 2017
        /// For Getting 
        /// </summary>
        /// <param name="dataSetID"></param>
        /// <param name="iDisplayLength"></param>
        /// <param name="iDisplayStart"></param>
        /// <param name="sSearch"></param>
        /// <param name="sortColumnIndex"></param>
        /// <param name="sortDirection"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
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
        public IList<uspSelectFavouriteListByUserID_Result> SelectFavoritesLoadCorelationLeadLagByDataSetIDForPortal(string dataSetID, int userID, string SelFavName, int iDisplayLength, int iDisplayStart, string sSearch, int sortColumnIndex, string sortDirection)
        {
            IList<uspSelectFavouriteListByUserID_Result> PortalItemsList = new List<uspSelectFavouriteListByUserID_Result>();
            foreach (uspSelectFavouriteListByUserID_Result PortalItem in objEntities.uspSelectFavouriteListByUserID(dataSetID, userID, SelFavName, iDisplayLength, iDisplayStart, sSearch, sortColumnIndex, sortDirection))
            {
                PortalItemsList.Add(PortalItem);
            }
            return PortalItemsList;
        }



        /// <summary>     
        /// Created by: Raghuveer
        /// Created On: July 20 2017
        /// Inserting Favorites values
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddFavorites(FavoritesModel model)
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
        /// Created by: Raghuveer
        /// Created On: July 20 2017
        /// Update Favorites values
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateFavorites(FavoritesModel model)
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
           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public IList<uspSelectAllFavouritesListByUserID_Result> SelectAllFavouritesListByUserID( int UserID)
        {
            IList<uspSelectAllFavouritesListByUserID_Result> FavoriteItemsList = new List<uspSelectAllFavouritesListByUserID_Result>();
            foreach (uspSelectAllFavouritesListByUserID_Result FavoriteItem in objEntities.uspSelectAllFavouritesListByUserID(UserID))
            {
                FavoriteItemsList.Add(FavoriteItem);
            }
            return FavoriteItemsList;
        }


        /// <summary>
        /// Created By : Raghuveer
        /// Created Date : 20 July 2017
        /// For Getting All Favourite List by UserID 
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public IList<uspSelectDataSetsByUserFavouriteList_Result> SelectAllFavouriteListBYUserID(int AccountID ,int DivisionID ,int UserID ,string FavouriteListName)
        {
            IList<uspSelectDataSetsByUserFavouriteList_Result> FavoriteItemsList = new List<uspSelectDataSetsByUserFavouriteList_Result>();
            foreach (uspSelectDataSetsByUserFavouriteList_Result FavoriteItem in objEntities.uspSelectDataSetsByUserFavouriteList(AccountID , DivisionID , UserID , FavouriteListName))
            {
                FavoriteItemsList.Add(FavoriteItem);
            }
            return FavoriteItemsList;
        }


        /// <summary>
        /// Created By : Raghuveer
        /// Created Date : 21 July 2017
        /// For Getting AllResults table count based on CustomerDataSetID
        /// </summary>
        /// <returns></returns>
        public int GetAllResultsCount(int CustomerDataSetID)
        {
            int finalResult = 0;
            try
            {

                System.Data.Objects.ObjectResult<int?> returnValue = objEntities.uspSelectAllResultsCountByDataSetID(CustomerDataSetID);
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
                throw ex;
            }
            return finalResult;
        }



        /// <summary>
        /// Created By : Raghuveer
        /// Created On: 29 August 2017
        /// Purpose : For Datacast 2.3 Search By Category
        /// </summary>
        /// <returns></returns>
        public IList<uspSelectDatacastCategoriesPortal_Result> GetCategoriesForDatacast()
        {
            IList<uspSelectDatacastCategoriesPortal_Result> industryList = new List<uspSelectDatacastCategoriesPortal_Result>();
            foreach (uspSelectDatacastCategoriesPortal_Result industry in objEntities.uspSelectDatacastCategoriesPortal())
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
        public IList<uspSelectDatacastSubCategoriesByCategoryPortal_Result> GetSubCategoriesbyCategory(string Industry)
        {
            IList<uspSelectDatacastSubCategoriesByCategoryPortal_Result> sectorList = new List<uspSelectDatacastSubCategoriesByCategoryPortal_Result>();
            foreach (uspSelectDatacastSubCategoriesByCategoryPortal_Result sector in objEntities.uspSelectDatacastSubCategoriesByCategoryPortal(Industry))
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
        public IList<uspSelectDatacastSubSectorsBySectorPortal_Result> GetSubSectors1BySector(string Industry, string Sector)
        {
            IList<uspSelectDatacastSubSectorsBySectorPortal_Result> subSector1List = new List<uspSelectDatacastSubSectorsBySectorPortal_Result>();
            foreach (uspSelectDatacastSubSectorsBySectorPortal_Result subsector1 in objEntities.uspSelectDatacastSubSectorsBySectorPortal(Industry, Sector))
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
        public IList<uspSelectDatacastSubSector2BySubSector1Portal_Result> GetSubSectors2BySubSectors1(string Industry, string Sector, string SubSector1)
        {
            IList<uspSelectDatacastSubSector2BySubSector1Portal_Result> subSector2List = new List<uspSelectDatacastSubSector2BySubSector1Portal_Result>();
            foreach (uspSelectDatacastSubSector2BySubSector1Portal_Result subsector2 in objEntities.uspSelectDatacastSubSector2BySubSector1Portal(Industry, Sector, SubSector1))
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
        public IList<uspSelectDatacastSubSector3BySubSector2Portal_Result> GetSubSectors3BySubSectors2(string Industry, string Sector, string SubSector1, string SubSector2)
        {
            IList<uspSelectDatacastSubSector3BySubSector2Portal_Result> subSector3List = new List<uspSelectDatacastSubSector3BySubSector2Portal_Result>();
            foreach (uspSelectDatacastSubSector3BySubSector2Portal_Result subsector3 in objEntities.uspSelectDatacastSubSector3BySubSector2Portal(Industry, Sector, SubSector1, SubSector2))
            {
                subSector3List.Add(subsector3);
            }
            return subSector3List;
        }




        public IList<uspSelectIndicatorListByCategorySearchPortal_Result> GetIndcatorListByCategorySearch(string dataSetID, int userID, string Industry,
                                  string Sector, string SubSector1, string SubSector2, string SubSector3, int iDisplayLength, int iDisplayStart,
                                  string sSearch, int sortColumnIndex, string sortDirection)
        {
            IList<uspSelectIndicatorListByCategorySearchPortal_Result> corelationList = new List<uspSelectIndicatorListByCategorySearchPortal_Result>();
            foreach (uspSelectIndicatorListByCategorySearchPortal_Result corelation in objEntities.uspSelectIndicatorListByCategorySearchPortal(dataSetID, userID, Industry,
                                     Sector, SubSector1, SubSector2, SubSector3, iDisplayLength, iDisplayStart,
                                     sSearch, sortColumnIndex, sortDirection))
            {
                corelationList.Add(corelation);
            }
            return corelationList;
        }




        /// <summary>

        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>

        private string ManipulateTEXT(string str)
        {
            string resstr = str;
            if (str.IndexOf("'") != -1)
            {
                resstr = str.Replace("'", "''");
            }
            return resstr;
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
        ///  Created By: Vijaya
        /// Created On:04/03/2018
        /// Clear specific Dataset's analysis and returns the result
        /// </summary>
        /// <param name="customerDataSetID"></param>
        /// <returns></returns>
        public int ClearDataSetAnalysis(int customerDataSetID)
        {
            int finalResult = 0;
            try
            {
                //System.Data.Objects.ObjectResult<int?>
                System.Data.Objects.ObjectResult<int?> ID = objEntities.uspClearDataSetAnalysis(customerDataSetID);

                var result = ID.FirstOrDefault<int?>();
                if (result.HasValue)
                {
                    if (result > 0)
                    {
                        finalResult = result.Value;
                    }

                }
                else
                {
                    finalResult = 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return finalResult;
        }

    }

}