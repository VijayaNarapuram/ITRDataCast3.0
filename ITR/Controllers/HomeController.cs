using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using ITR.DataRepository;
using ITR.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http.Cors;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;

namespace ITR.Controllers
{
     
    public class HomeController : Controller
    {
        //Created By: Vishnu on 23-Aug-2016
        //Dependency Injection resolver using the interface
        private IHomeRepository _homeRepository;

        public HomeController(IHomeRepository _repo)
        {
            _homeRepository = _repo;
        }

        /// <summary>
        /// Created By: Vishnu
        /// Created On: 10-Sep-2016
        /// Loads the Login / Lading Page of the application
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
           
            return View();
        }

        /// <summary>
        /// Created By: Vishnu
        /// Created On: 10-Sep-2016
        /// Loads the Dashboard Page 
        /// </summary>
        /// <returns></returns>
        public ActionResult Dashboard()
        {
            GetAccounts();
            GetIndicatorCategories();
            return View();
        }

        /// <summary>
        /// Created By: Ram Mohan
        /// Modified By:Vishnu , to get the Account picklist values from the API
        /// Modified On: 06-December-2016
        /// Created On: 14-Sep-2016
        /// Loads the Accounts dropdownlist 
        /// </summary>
        /// <returns></returns>
        private void GetAccounts()
        {
            //var AccountsPL = _homeRepository.GetAccounts();
            //ViewBag.Accounts = AccountsPL;

            List<TemplateModel> companyData = new List<TemplateModel>();
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
                    companyData.Add(new TemplateModel
                    {
                        AccountName = account_name,
                        RecordId = record_id_
                    });
                }
            }
           
            if(companyData.Count>1)
            {
             var companyData1=  companyData.OrderBy(x => x.AccountName);
            // ViewBag.Accounts = companyData1;
             ViewBag.Accounts = JsonConvert.SerializeObject(companyData1);
            }
            else
            {
              //  ViewBag.Accounts = companyData;
                ViewBag.Accounts = JsonConvert.SerializeObject(companyData);

            }
            //var AccountsPL = companyData;
           // ViewBag.Accounts = AccountsPL;
           
        }

        /// <summary>
        /// Created by Ram Mohan 
        /// Created Date: 25-08-2014 
        /// Modified By: Vishnu , to get the Divisions  picklist values from the API
        /// Modified On: 06-December-2016
        /// Gets List Of Assessment Area by Assessment ID       
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>   
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

                List<TemplateModel> companyData = new List<TemplateModel>();
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
                        companyData.Add(new TemplateModel
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



        //public void GetDivisionsByAccountID(int id)
        //{

        //    try
        //    {

                ////var dt1 = _homeRepository.GetDivisionsByAccountID(id);
                ////var divisions = (from dr in dt1.AsEnumerable()
                ////                 select new SelectListItem
                ////                 {
                ////                     Text = dr.DivisionName,
                ////                     Value = dr.DivisionID.ToString()
                ////                 }).ToList();
                ////result = Json(divisions, JsonRequestBehavior.AllowGet);
                ////return result;

                //List<TemplateModel> companyData = new List<TemplateModel>();
                //string division_name = "";
                //string record_id_ = "";

                //WebClient client = new WebClient();
                //string url = "https://itreconomics.quickbase.com/db/bg794fvvy?a=API_DoQuery&usertoken=b2555s_ynh_bdhajpwdb3qxm4cn7n4jidps9pwp&query={66.TV." + id + "}&clist=23.66.6.3";

                //string xmlString = client.DownloadString(url);
                //var doc = new XmlDocument();
                //doc.LoadXml(xmlString);

                //XmlNode nodes = doc.SelectSingleNode("/qdbapi");
                //for (int i = 0; i < nodes.ChildNodes.Count; i++)
                //{
                //    if (nodes.ChildNodes[i].Name == "record")
                //    {
                //        for (int j = 0; j < nodes.ChildNodes[i].ChildNodes.Count; j++)
                //        {
                //            if (nodes.ChildNodes[i].ChildNodes[j].Name == "division_name")
                //            {
                //                division_name = nodes.ChildNodes[i].ChildNodes[j].InnerText;
                //            }
                //            else if (nodes.ChildNodes[i].ChildNodes[j].Name == "record_id_")
                //            {
                //                record_id_ = nodes.ChildNodes[i].ChildNodes[j].InnerText;
                //            }
                //        }
                //        companyData.Add(new TemplateModel
                //        {
                //            Divisions = division_name,
                //            DivisionsRecordId = record_id_
                //        });
                //    }
                //}


                //if (companyData.Count > 1)
                //{
                //    var companyData1 = companyData.OrderBy(x => x.Divisions);
                //    // ViewBag.Accounts = companyData1;
                //    ViewBag.Divisions = JsonConvert.SerializeObject(companyData1);
                //}
                //else
                //{
        //            //  ViewBag.Accounts = companyData;
        //            ViewBag.Divisions = JsonConvert.SerializeObject(companyData);

        //        }
        //        //var AccountsPL = companyData;
        //        // ViewBag.Accounts = AccountsPL;

        //        //var dt1 = companyData.OrderBy(x => x.Divisions);
        //        //var divisions = (from dr in dt1.AsEnumerable()
        //        //                 select new SelectListItem
        //        //                 {
        //        //                     Text = dr.Divisions,
        //        //                     Value = dr.DivisionsRecordId.ToString()
        //        //                 }).ToList();
        //        //result = Json(divisions, JsonRequestBehavior.AllowGet);




        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public JsonResult GetAccountID()
        {
            JsonResult result = null;
            try
            {

                var dt1 = _homeRepository.GetAccounts();
                var divisions = (from dr in dt1.AsEnumerable()
                                 select new SelectListItem
                                 {
                                     Text = dr.AccountName,
                                     Value = dr.AccountID.ToString()
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
        /// Created By: Vishnu
        /// Created On: 10-Sep-2016
        /// Loads the View / Edit DataSet Page 
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewDataSet()
        {
            return View();
        }

        /// <summary>
        /// Created By:Nagendra
        /// Created On:24-August-2016
        /// </summary>
        /// <param name="dataSetID"></param>
        /// <returns></returns>
        public JsonResult GetDataSet(int DSID,string Table)
        {
            int dataSetID = DSID;
            string rettval = "";

            IEnumerable<uspSelectDataSetsDetailsByDataSetID_Result> list = _homeRepository.GetDataSetsDetailsByDataSetID(dataSetID, Table).AsEnumerable();
           // list = list.OrderByDescending(step => step.MonthYear);
            //now serialize it
          //  JavaScriptSerializer serializer = new JavaScriptSerializer();
           // var exportInfo = list;
           // rettval = serializer.Serialize(list);
          
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Created By: Vishnu
        /// Created On: 10-Sep-2016
        /// Loads the Replace DataSet Page 
        /// </summary>
        /// <returns></returns>
        public ActionResult ReplaceDataSet()
        {
            return View();
        }

        public ActionResult AddTemplate()
        {
            return View();
        }

        /// <summary>
        /// Created By: Vishnu
        /// Created On: 10-Sep-2016
        /// Loads Add DataSet Page 
        /// </summary>
        /// <returns></returns>
        public ActionResult AddDatasets()
        {
            GetAccounts();
            return View();
        }

        /// <summary>
        /// Created By: Vishnu
        /// Reads the File stream from Excel file and Returns the Result as Dataset
        /// </summary>
        /// <param name="MyExcelStream"></param>
        /// <param name="ReadOnly"></param>
        /// <returns></returns>
        public static DataSet GetDataTableFromSpreadsheet(Stream MyExcelStream, bool ReadOnly,string Selectedsheetname)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            int sheetIndex = 0;

            try
            {
                using (SpreadsheetDocument sDoc = SpreadsheetDocument.Open(MyExcelStream, ReadOnly))
                {
                    WorkbookPart workbookPart = sDoc.WorkbookPart;

                    IEnumerable<Sheet> sheets = sDoc.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();

                    foreach (var sheet in sheets)
                    {
                        string relationshipId = sheet.Id.Value;
                        WorksheetPart worksheetPart = (WorksheetPart)sDoc.WorkbookPart.GetPartById(relationshipId);

                        Worksheet workSheet = worksheetPart.Worksheet;
                        SheetData sheetData = workSheet.GetFirstChild<SheetData>();
                        IEnumerable<Row> rows = sheetData.Descendants<Row>();

                        if (rows.Count() > 0)
                        {
                            //long  iTotalRows = workSheet.UsedRange.Rows.Count;
                            //retrieve Tab names from excel sheet using OpenXML
                            string sheetName = workbookPart.Workbook.Descendants<Sheet>().ElementAt(sheetIndex).Name;
                            sheetIndex++;

                            if (sheetName == Selectedsheetname)
                            {
                                dt = ds.Tables.Add(sheetName);
                                //foreach (Cell cell in rows.ElementAt(5))
                                //{

                                //    dt.Columns.Add(GetCellValue(sDoc, cell));
                                //}
                                //Cell cell = rows.ElementAt(5);
                                for (int i = 0; i < 4; i++)
                                {
                                    //if (i != 1 && i != 2 && i !=4)
                                    //{
                                    //    //dt.Columns.Add(GetCellValue(sDoc, rows.ElementAt(5).Descendants<Cell>().ElementAt(i)));
                                    //}
                                    if (i == 0)
                                    {
                                        dt.Columns.Add("MonthYear");
                                    }
                                    else if (i == 3)
                                    {
                                        dt.Columns.Add("Value");
                                    }
                                }

                                dt.Columns.Add("OtherParams");

                                string companyName = "", dataType = "", bases = "", unit = "", webSite = "";

                                foreach (Row row in rows) //this will also include your header row...  && row.RowIndex < 371
                                {
                                    DataRow tempRow = dt.NewRow();
                                    if (row.RowIndex == 1)
                                    {
                                        companyName = GetCellValue(sDoc, row.Elements<Cell>().Where(c => string.Compare(c.CellReference.Value, "H" + row.RowIndex, true) == 0).First());
                                    }
                                    else if (row.RowIndex == 2)
                                    {
                                        dataType = GetCellValue(sDoc, row.Elements<Cell>().Where(c => string.Compare(c.CellReference.Value, "H" + row.RowIndex, true) == 0).First());
                                    }
                                    else if (row.RowIndex == 3)
                                    {
                                        bases = GetCellValue(sDoc, row.Elements<Cell>().Where(c => string.Compare(c.CellReference.Value, "H" + row.RowIndex, true) == 0).First());
                                    }
                                    else if (row.RowIndex == 4)
                                    {
                                        unit = GetCellValue(sDoc, row.Elements<Cell>().Where(c => string.Compare(c.CellReference.Value, "H" + row.RowIndex, true) == 0).First());
                                    }
                                    else if (row.RowIndex == 5)
                                    {
                                        webSite = GetCellValue(sDoc, row.Elements<Cell>().Where(c => string.Compare(c.CellReference.Value, "H" + row.RowIndex, true) == 0).First());
                                    }

                                    if (row.RowIndex > 5)
                                    {
                                        bool addTempRowFlag = true;
                                        //for (int i = 0; i < row.Descendants<Cell>().Count(); i++) // 
                                        for (int i = 0; i < 4; i++)
                                        {
                                            //if (row.Descendants<Cell>().ElementAt(i) != null)
                                            //{
                                            if (i != 1 && i != 2)
                                            {
                                                var cellValue = ""; //= GetCellValue(sDoc, row.Descendants<Cell>().ElementAt(i));
                                                var cell = row.Descendants<Cell>().ElementAt(i);

                                                //if (i == 0)
                                                //{
                                                //    cellValue = GetCellValue(sDoc, row.Elements<Cell>().Where(c => string.Compare(c.CellReference.Value, "A" + row.RowIndex, true) == 0).First());
                                                //    tempRow[0] = cellValue;
                                                //}
                                                if (i == 0)
                                                {//Date Column
                                                    try
                                                    {
                                                        DateTime convertedDate = new DateTime();
                                                        cellValue = GetCellValue(sDoc, row.Elements<Cell>().Where(c => string.Compare(c.CellReference.Value, "A" + row.RowIndex, true) == 0).First());
                                                        if (cellValue != null && cellValue != "")
                                                        {
                                                            //Converts the Excel Date(in series format) to Datetime
                                                            convertedDate = DateTime.FromOADate(Convert.ToDouble(cellValue));
                                                            tempRow[0] = convertedDate.ToString("MM/dd/yyyy");
                                                        }
                                                        else
                                                        {
                                                            addTempRowFlag = false;//To skip the column to be added for datatable
                                                            tempRow[0] = "";
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                    }
                                                    finally
                                                    {
                                                        if (cellValue == null && cellValue == "")
                                                        {
                                                            addTempRowFlag = false;//To skip the column to be added for datatable
                                                            tempRow[0] = "";
                                                        }
                                                    }

                                                    // tempRow[0] = cellValue;
                                                }
                                                else if (i == 3)
                                                {
                                                    cellValue = GetCellValue(sDoc, row.Elements<Cell>().Where(c => string.Compare(c.CellReference.Value, "D" + row.RowIndex, true) == 0).First());

                                                    var FirstCell = row.Descendants<Cell>().ElementAt(0);
                                                    try
                                                    {
                                                        if (cell.CellValue == null && (cell.InnerXml == "" || cell.InnerXml == null) && (cell.InnerText == "" || cell.InnerText == null) && (FirstCell.CellValue == null && (FirstCell.InnerXml == "" || FirstCell.InnerXml == null) && (FirstCell.InnerText == "" || FirstCell.InnerText == null)))
                                                        {
                                                            addTempRowFlag = false;
                                                        }

                                                        if(cellValue == "")
                                                        { /* Added By Vishnu,On 28-November-2016 , To Prevent Empty Rows As per Email From Rich On 23 rd November 2016 (So we are skipping adding the current Row to the Dataset )
                                                           *  When importing templates, we need to delete the empty data rows prior to the data and after the data.  
                                                           *  If this is not done, especially after the data, the system will calculate 12/12’s using 0 as the monthly data points causing rows to be created that show future values,
                                                           *  and those values plummet.  When I uploaded data and deleted the rows myself or prior to the upload.                                                           * 
                                                           * */
                                                            addTempRowFlag = false;
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        //  throw ex;
                                                    }
                                                    tempRow[1] = cellValue;
                                                }
                                                //else if (i == 4)
                                                //{
                                                //    tempRow[2] = cellValue;
                                                //}
                                            }
                                        }
                                        if (addTempRowFlag)
                                        {
                                            tempRow[2] = companyName + "!" + dataType + "!" + bases + "!" + unit + "!" + webSite;
                                            dt.Rows.Add(tempRow);
                                            ds.Merge(dt);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return ds;
        }

        /// <summary>
        /// Created By:Nagendra
        /// Created On:5-oct-2016
        /// Reads the File stream from Excel file and get the sheet names
        /// </summary>
        /// <param name="MyExcelStream"></param>
        /// <param name="ReadOnly"></param>
        /// <returns></returns>
        public static List<string> Getsheet(Stream MyExcelStream, bool ReadOnly)
        {
            string sheetName = "";
            int sheetIndex = 0;
            List<string> sheetlist = new List<string>();

            using (SpreadsheetDocument sDoc = SpreadsheetDocument.Open(MyExcelStream, ReadOnly))
            {
                WorkbookPart workbookPart = sDoc.WorkbookPart;

                IEnumerable<Sheet> sheets = sDoc.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();

                foreach (var sheet in sheets)
                {
                    string relationshipId = sheet.Id.Value;
                    WorksheetPart worksheetPart = (WorksheetPart)sDoc.WorkbookPart.GetPartById(relationshipId);

                    Worksheet workSheet = worksheetPart.Worksheet;
                    SheetData sheetData = workSheet.GetFirstChild<SheetData>();
                    IEnumerable<Row> rows = sheetData.Descendants<Row>();

                    sheetName =  workbookPart.Workbook.Descendants<Sheet>().ElementAt(sheetIndex).Name;
                    sheetIndex++;
                    sheetlist.Add(sheetName);
                }
            }
            return sheetlist;
        }

        /// <summary>
        /// Created By: Vishnu
        /// Gets the Cell Value from specific Cell
        /// </summary>
        /// <param name="document"></param>
        /// <param name="cell"></param>
        /// <returns></returns>
        public static string GetCellValue(SpreadsheetDocument document, Cell cell)
        {
            SharedStringTablePart stringTablePart = document.WorkbookPart.SharedStringTablePart;
            string value = "";
            string origanlvalue = "";

            try
            {
                if (cell.CellValue != null)
                {
                    value = cell.CellValue.InnerXml;

                    origanlvalue = Convert.ToDouble(value).ToString();

                    if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
                    {
                        return stringTablePart.SharedStringTable.ChildElements[Int32.Parse(value)].InnerText;
                    }
                    else
                    {
                        return origanlvalue;
                        //  return stringTablePart.SharedStringTable.ChildElements[Int32.Parse(value)].InnerText;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }

        /// Created By:Nagendra
        /// Created On:5-Oct-2016
        /// For getting sheet name from excel
        /// <returns></returns>
        [HttpPost]
        public  JsonResult GetSheetName()
        {
            string rettval = "";
            List<string> sheetlist=null;

            foreach (string files in Request.Files)
            {
                var fileContent = Request.Files[files];
                if (fileContent != null && fileContent.ContentLength > 0)
                {
                    //save file in folder
                    var fileName = Path.GetFileName(fileContent.FileName);
                    //var path = Path.Combine(Server.MapPath("~/SaveFile"), fileName);
                    //fileContent.SaveAs(path);

                    // get a stream
                    var stream = fileContent.InputStream;
                    sheetlist = Getsheet(stream, false);

                }
                //now serialize it
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                rettval = serializer.Serialize(sheetlist);
            }
            return Json(rettval, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Created By:Nagendra
        /// Created On:18-August-2016
        /// For Converting table data to json object
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DisplayTable()
        {
            string rettval = "";
            try
            {
                string selectedsheetname = Request.Form.AllKeys[0].ToString();

                foreach (string files in Request.Files)
                {
                    var fileContent = Request.Files[files];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        //save file in folder
                        var fileName = Path.GetFileName(fileContent.FileName);
                        //var path = Path.Combine(Server.MapPath("~/SaveFile"), fileName);
                        //fileContent.SaveAs(path);

                        // get a stream
                        var stream = fileContent.InputStream;
                        DataSet ds = new DataSet();
                        ds = GetDataTableFromSpreadsheet(stream, false, selectedsheetname);


                        for (int i = 0; i < ds.Tables.Count; i++)
                        {

                            var lst = ds.Tables[i].AsEnumerable()
        .Select(r => r.Table.Columns.Cast<DataColumn>()
                .Select(c => new KeyValuePair<string, object>(c.ColumnName, r[c.Ordinal])
               ).ToDictionary(z => z.Key, z => z.Value)
        ).ToList();

                            //now serialize it
                            JavaScriptSerializer serializer = new JavaScriptSerializer();
                            var exportInfo = lst;
                            rettval = serializer.Serialize(lst);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                return Json(ex.ToString());
            }

            //return Json(rettval, JsonRequestBehavior.AllowGet);
            return Json(rettval, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Created By:Vishnu
        /// Created On 23-Aug-2016
        /// Inserts the customer dataset values to DB
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public int InsertCustomerDataSet(TemplateModel model)
        {
            int finalResult = 0;
            try
            {              
                if (Session.Count > 0)
                {
                    if (Session["UserID"] != null && Session["UserID"].ToString() != string.Empty)
                    {
                        model.CustomerID = Convert.ToString(Session["UserID"].ToString());
                        model.CreatedBy = Convert.ToString(Session["UserID"].ToString());
                        model.UserName = Convert.ToString(Session["UserName"].ToString());
                        string isITREmployee = Convert.ToString(Session["IsDataPortalAdmin"].ToString());
                        int isDataPotralAdminVal = 0;
                        if (isITREmployee == "true" || isITREmployee == "TRUE")
                        {
                            isDataPotralAdminVal = 1;
                        }
                        else
                        {
                            isDataPotralAdminVal = 0;
                        }
                        model.IsDataPortalAdmin = Convert.ToBoolean(isDataPotralAdminVal);
                    }
                }
                else
                {
                    model.CreatedBy = model.UserID.ToString();
                }
                model.CreatedDate = DateTime.Now;

                finalResult = _homeRepository.InsertInsertCustomerDataSet(model);
                //if(finalResult>0)
                //{
                //   finalValue= _homeRepository.ExecuteCaluclations(finalResult); 
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return finalResult;
        }

        /// <summary>
        /// Created By:Nagendra
        /// Created On:24-August-2016
        /// Update the customer dataset values to DB
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateCustomerDataSet(UpdateDataset model)
        {
            int finalResult = 0;
            try
            {
                model.ModifiedBy = "1";//Convert.ToInt32(Session["UserId"].ToString());UpdateCustomerDataSet
                model.ModifiedDate = DateTime.Now;

                finalResult = _homeRepository.UpdateCustomerDataSet(model);
                //if(finalResult>0)
                //{
                //    finalValue = _homeRepository.ExecuteCaluclations(finalResult);
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return finalResult;
        }

        /// <summary>
        /// Created By:Nagendra
        /// Created On:30-August-2016
        /// Delete the row from Dataset
        /// </summary>
        /// <param name="ProdID"></param>
        /// <returns></returns>
        [HttpPost]
        public int DeleteRowFromDataSet(int ProdID)
        {
            int finalResult = 0;

            try
            {
                finalResult = _homeRepository.DeleteRowFromDataSet(ProdID);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return finalResult;
        }

        /// <summary>
        /// Created By:Ram Mohan
        /// Created On 23-Aug-2016
        /// Filling Datasets by Company and Division
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult FillDataSetsDataTable(JQueryDataTableParamModel param, int comapnyId, int divisionId, string FavouriteListName,int UserID)
        {
            ActionResult response = null;
            if(FavouriteListName == "")
            {
            string customerid = string.Empty;
            if (Session.Count > 0)
            {
                if (Session["UserID"] != null && Session["UserID"].ToString() != string.Empty)
                {
                    customerid = Convert.ToString(Session["UserID"].ToString());
                }
            }

            var verifyList = _homeRepository.GetDataSetsByCompanyDivision(comapnyId, divisionId, UserID.ToString());

            var displayRecordCount = 0; //for passing displayed records count(if search is true pass the filtered record count, else pass the entire list count

            IEnumerable<uspSelectDataSetsByCompanyDivision_Result> filteredVerifyList = null; //used to hold the filtered data of taskList            

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                string searchText = param.sSearch.ToLower();

                filteredVerifyList = _homeRepository.GetDataSetsByCompanyDivision(comapnyId, divisionId, UserID.ToString())
                    .Where(c => ((c.DataSetName != null) && Convert.ToString(c.DataSetName).ToLower().Contains(searchText)) ||
                            ((c.DataSetDescription != null) && Convert.ToString(c.DataSetDescription).ToLower().Contains(searchText)) ||
                            (Convert.ToString(c.CustomerDataSetID).ToLower().Contains(searchText)) ||
                            ((c.CreatedDate != null) && String.Format("{0:MM/dd/yyyy}", c.CreatedDate).Contains(searchText))
                     );



                if (filteredVerifyList.Count() > 0)
                {
                    displayRecordCount = filteredVerifyList.Count();
                }
                else
                {
                    displayRecordCount = 0;
                }
            }
            else
            {
                filteredVerifyList = verifyList;
                displayRecordCount = verifyList.Count();
            }

            int sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]); //Gets the SortColumnIndex from the DataTable on User Interaction

            Func<uspSelectDataSetsByCompanyDivision_Result, string> orderingFunction = (c => sortColumnIndex == 1 ? Convert.ToString(c.CustomerDataSetID) :
                                                                                                sortColumnIndex == 2 ? Convert.ToString(c.DataSetName) :
                                                                                                 sortColumnIndex == 3 ? Convert.ToString(c.DataSetDescription) :
                                                                                                  sortColumnIndex == 4 ? Convert.ToString(String.Format("{0:yyyyMMdd}", c.CreatedDate)) :

                                                                              Convert.ToString(c.CustomerDataSetID)
                                                                             
                                                                ); //Sorts the List based on supplied sortColumn index

            string sortDirection = Request["sSortDir_0"]; // asc or desc

            if (sortDirection == "asc")
                filteredVerifyList = filteredVerifyList.OrderBy(orderingFunction); //Sort the List in ascending Order and Re assigns to the List
            else
                filteredVerifyList = filteredVerifyList.OrderByDescending(orderingFunction); //Sort the List in descending Order and Re assigns to the List


            //if (string.IsNullOrEmpty(param.sSearch)) //If seach is null then display the records as seleted in 'records per page' DropDown
            //{
            if (param.iDisplayLength != -1)
            {
                filteredVerifyList = filteredVerifyList.Skip(param.iDisplayStart)//skip to start Record in the List which will be supplied as param.iDisplayStart
                                                     .Take(param.iDisplayLength); //Gets up to param.iDisplayLength from param.iDisplayStart record      
            }
            //}

            var displayedTaskList = filteredVerifyList;

            var result = from c in displayedTaskList
                         select new[] {
                                        Convert.ToString(c.CustomerDataSetID),
                                        Convert.ToString(c.CustomerDataSetID),
                                        Convert.ToString(c.DataSetName),
                                        Convert.ToString(c.DataSetDescription),
                                        String.Format("{0:MM/dd/yyyy}",c.CreatedDate),
                                        Convert.ToString(c.CustomerDataSetID),
                                        Convert.ToString(c.CustomerDataSetID),
                                        //Convert.ToString(c.CustomerDataSetID),
                                        Convert.ToString(c.CustomerDataSetID),
                                        Convert.ToString( c.ViewStatus),
                                        Convert.ToString( c.CustomerDataSetID) //Clear Analysis...

                         };

            response = Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = verifyList.Count(),
                iTotalDisplayRecords = displayRecordCount,
                aaData = result
            }, JsonRequestBehavior.AllowGet);
            return response;
            }
            else
            {
               return FillFavouriteSetsDataTableByUserID(param,comapnyId,  divisionId, UserID, FavouriteListName);
            }


           

        }



        /// <summary>
        /// Created By:Raghuyveer
        /// Created On 20 July 2017
        /// Filling Datasets by Company and Division Based on Account and DivisionID
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult FillFavouriteSetsDataTableByUserID(JQueryDataTableParamModel param, int comapnyId, int divisionId, int UserID, string FavouriteListName)
        {
            ActionResult response = null;
           
                string customerid = string.Empty;
                //if (Session.Count > 0)
                //{
                //    if (Session["UserID"] != null && Session["UserID"].ToString() != string.Empty)
                //    {
                //        customerid = Convert.ToString(Session["UserID"].ToString());
                //    }
                //}

                var verifyList = _homeRepository.SelectAllFavouriteListBYUserID(comapnyId, divisionId, UserID,FavouriteListName);

                var displayRecordCount = 0; //for passing displayed records count(if search is true pass the filtered record count, else pass the entire list count

                IEnumerable<uspSelectDataSetsByUserFavouriteList_Result> filteredVerifyList = null; //used to hold the filtered data of taskList            

                if (!string.IsNullOrEmpty(param.sSearch))
                {
                    string searchText = param.sSearch.ToLower();

                    filteredVerifyList = _homeRepository.SelectAllFavouriteListBYUserID(comapnyId, divisionId, UserID, FavouriteListName)
                        .Where(c => ((c.DataSetName != null) && Convert.ToString(c.DataSetName).ToLower().Contains(searchText)) ||
                                ((c.DataSetDescription != null) && Convert.ToString(c.DataSetDescription).ToLower().Contains(searchText)) ||
                                (Convert.ToString(c.CustomerDataSetID).ToLower().Contains(searchText)) ||
                                ((c.CreatedDate != null) && String.Format("{0:MM/dd/yyyy}", c.CreatedDate).Contains(searchText))
                         );



                    if (filteredVerifyList.Count() > 0)
                    {
                        displayRecordCount = filteredVerifyList.Count();
                    }
                    else
                    {
                        displayRecordCount = 0;
                    }
                }
                else
                {
                    filteredVerifyList = verifyList;
                    displayRecordCount = verifyList.Count();
                }

                int sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]); //Gets the SortColumnIndex from the DataTable on User Interaction

                Func<uspSelectDataSetsByUserFavouriteList_Result, string> orderingFunction = (c => sortColumnIndex == 1 ? Convert.ToString(c.CustomerDataSetID) :
                                                                                                    sortColumnIndex == 2 ? Convert.ToString(c.DataSetName) :
                                                                                                     sortColumnIndex == 3 ? Convert.ToString(c.DataSetDescription) :
                                                                                                      sortColumnIndex == 4 ? Convert.ToString(String.Format("{0:yyyyMMdd}", c.CreatedDate)) :

                                                                                  Convert.ToString(c.CustomerDataSetID)
                                                                    ); //Sorts the List based on supplied sortColumn index

                string sortDirection = Request["sSortDir_0"]; // asc or desc

                


                //if (string.IsNullOrEmpty(param.sSearch)) //If seach is null then display the records as seleted in 'records per page' DropDown
                //{
                if (param.iDisplayLength != -1)
                {
                    filteredVerifyList = filteredVerifyList.Skip(param.iDisplayStart)//skip to start Record in the List which will be supplied as param.iDisplayStart
                                                         .Take(param.iDisplayLength); //Gets up to param.iDisplayLength from param.iDisplayStart record      
                }
                //}

                var displayedTaskList = filteredVerifyList;

                var result = from c in displayedTaskList
                             select new[] {
                                        Convert.ToString(c.CustomerDataSetID),
                                        Convert.ToString(c.CustomerDataSetID),
                                        Convert.ToString(c.DataSetName),
                                        Convert.ToString(c.DataSetDescription),
                                        String.Format("{0:MM/dd/yyyy}",c.CreatedDate),
                                        Convert.ToString(c.CustomerDataSetID),
                                        Convert.ToString(c.CustomerDataSetID),
                                        //Convert.ToString(c.CustomerDataSetID),
                                         Convert.ToString(c.CustomerDataSetID),
                                          Convert.ToString(c.ViewStatus)
                         };

                response = Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = verifyList.Count(),
                    iTotalDisplayRecords = displayRecordCount,
                    aaData = result
                }, JsonRequestBehavior.AllowGet);
               
           


            return response;

        }


        /// <summary>
        /// Created By: Ram Mohan
        /// Created On 23-08-2016
        /// Delete the Dataset by Dataset ID
        /// </summary>
        /// <param name="DatasetID"></param>
        /// <returns></returns>
        [HttpPost]
        public int DatasetDelete(int DatasetID)
        {

            int finalResult = 0;

            try
            {
                finalResult = _homeRepository.DeleteDataSet(DatasetID);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return finalResult;
        }

        /// <summary>
        /// Created By:Ram Mohan
        /// Created On 25-Aug-2016
        /// Replaces the customer dataset values to DB
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public int ReplaceCustomerDataSet(TemplateModel model)
        {
            int finalResult = 0;
            try
            {
                if (Session.Count > 0)
                {
                    if (Session["UserID"] != null && Session["UserID"].ToString() != string.Empty)
                    {
                        model.CustomerID = Convert.ToString(Session["UserID"].ToString());
                        model.CreatedBy = Convert.ToString(Session["UserID"].ToString());
                        model.UserName =  Convert.ToString(Session["UserName"].ToString()) ;
                        string   isITREmployee = Convert.ToString(Session["IsDataPortalAdmin"].ToString());
                        int isDataPotralAdminVal = 0;
                        if (isITREmployee == "true" || isITREmployee == "TRUE")
                        {
                            isDataPotralAdminVal = 1;
                        }
                        else
                        {
                            isDataPotralAdminVal = 0;
                        }
                        model.IsDataPortalAdmin = Convert.ToBoolean(isDataPotralAdminVal);

                    }
                }
               
                model.CreatedDate = DateTime.Now;

                finalResult = _homeRepository.ReplaceCustomerDataSet(model);
                //if(finalResult>0)
                //{
                //    finalValue = _homeRepository.ExecuteCaluclations(finalValue);
                //}
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return finalResult;
        }

        /// <summary>
        ///  Created By:Vishnu
        /// Created On 24-Aug-2016
        /// Gets the DatasetDetails by id for displaying in Banner
        /// </summary>
        /// <param name="DSID"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetDataSetDetailsForBanner(int DSID,string Table)
        {
            var dataSetInfo = _homeRepository.GetDataSetsDetailsByDataSetIDForBanner(DSID,Table);
            return Json(dataSetInfo, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Created By:Ram Mohan
        /// Created On:30-August-2016
        /// On Hover displays the bottom 3 rows of selected dataset
        /// </summary>
        /// <param name="dataSetID"></param>
        /// <returns></returns>
        public JsonResult GetHoverRowsByDataSetID(int DSID,string Table)
        {
            try
            {
                string finalResult = "";
                var dt1 = _homeRepository.GeRowsByDataSetID(DSID,Table);
                finalResult = "<table class='table table-striped table - bordered'>" +
                              "<thead class='the - box dark full'> " +
                              "  <tr> " +
                                      "<th style='color:black !important;'>Month Year</th>" +
                                      "<th style='color:black !important;'>Value</th>" +
                                      //"<th style='color:black !important;'>Adj Value</th>" +
                              "</tr> " +
                              "</thead>" +
                              " <tbody>";

                foreach (var item in dt1)
                {
                    if (item != null)
                    {

                        finalResult += " <tr>" +
                                       "<td>" + string.Format("{0:MM/dd/yyyy}", item.MonthYear) + "</td>" +
                                       //"<td>" + item.RawValue + "</td>" +
                                       "<td>" + string.Format("{0:0.00}", item.AdjValue)  + "</td>";
                        finalResult += "</tr>";
                    }
                }

                finalResult += " </tbody> </table>";

                return Json(finalResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Created By: Ram Mohan
        /// Created On 13-09-2016
        /// Push the Dataset by Dataset ID
        /// </summary>
        /// <param name="DatasetID"></param>
        /// <returns></returns>
        [HttpPost]
        public int DatasetPush(int DatasetID)
        {

            int finalResult = 0;

            try
            {
                finalResult = _homeRepository.PushDataSet(DatasetID);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return finalResult;
        }

        /// <summary>
        /// Created By: Vishnu
        /// Created On:07-Sept-2016
        /// Handles Session request from drupal 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ITRSessionRequest(SessionModel model)
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

                if (model.UserID != null)
                    Session["UserID"] = model.UserID;
                if (model.CompanyID != null)
                    Session["CompanyID"] = model.CompanyID;
                if (model.DivisonID != null)
                    Session["DivisonID"] = model.DivisonID;
                if (model.IsDataPortalAdmin != null)
                    Session["IsDataPortalAdmin"] = model.IsDataPortalAdmin;
                if (model.Value1 != null)
                    Session["UserName"] = model.Value1;
                if (model.EmailID != null)
                    Session["EmailID"] = model.EmailID;

                response = Json(new { result = "Success", url = DomainName + "/Home/Dashboard", ComapnyId = model.CompanyID, DivisonID = model.DivisonID, IsDataPortalAdmin = model.IsDataPortalAdmin, UserID = model.UserID, UserName = model.Value1 });
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
        public ActionResult ITRSessionRequest2(SessionModel model)
        {
            ActionResult response = null;
            string DomainName = HttpContext.Request.Url.GetLeftPart(UriPartial.Authority);

            //Add the Json object values to Session so that they can be used throughout this application
            if (model.CompanyID != null && model.CompanyID != "" && model.DivisonID != null && model.DivisonID != "")
            {
                Session["CompanyID"] = model.CompanyID;
                Session["DivisonID"] = model.DivisonID;
                response = Json(new { result = "Success", url = DomainName + "/Home/Dashboard" });
            }
            else
            {
                response = Json(new { result = "Failure", url = DomainName + "/Home/Login"});
            }

            return response;
        }


        /// <summary>
        /// Created By:Vishnu
        /// Created On: 18-Nov-2016
        /// Calls the R Script from code and returns the success or failure values
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public int RunAnalysis(TemplateModel model)
        {

            int finalResult = 0;

            try
            {
               // string directoryPath = @"C:\RFiles";
                string directoryPath = @"" + ConfigurationManager.AppSettings["RFilesFolderPath"]; 
                if (!System.IO.Directory.Exists(directoryPath))
                {
                    System.IO.Directory.CreateDirectory(directoryPath);
                }
                var tempString=System.Uri.UnescapeDataString(model.DatasetName);
                var tempViewStatus = System.Uri.UnescapeDataString(model.ViewStatuses);

                string[] dataSetNames = tempString.Split('|');
                string[] viewStatusValues = tempViewStatus.Split('|');
                string FavouriteListName = "", Email = "";
                for (int i = 0; i < dataSetNames.Length; i++)
                {
                   // var fileName = System.DateTime.Now.Ticks +i+ ".txt";
                    var fileName = "Rscript"+System.DateTime.Now.Ticks + i + ".bat";
                    string filePath = System.IO.Path.Combine(directoryPath, fileName);
                    if(model.FavouriteListName!=null)
                    {
                        FavouriteListName = model.FavouriteListName;
                    }
                    else
                    {
                        FavouriteListName = "";
                    }


                    if(Session["EmailID"]!=null)
                    {
                        Email = Session["EmailID"].ToString();
                    }
                    else
                    {
                        Email = "";
                    }
                    var rScriptString = "";
                    System.IO.File.Create(filePath).Close();
                    //rScriptString = "RunAnalysis(" + "'" + dataSetNames[i] + "', 'CP', '1990-01-01', '" + String.Format("{0:yyyy-MM-dd}", DateTime.Now.Date) + "', '" + model.Indicator + "')";

                    /* Here we need to create the bat file content as below
                     * "C:\Program Files\R\R-3.3.2\bin\Rscript.exe" C:\Users\Sirisha\Documents\VishnuR\Second.r "Vishnu a" baddula 10/23/2016 10/10/2017 > SampleOutput.txt
                     * */

                   // rScriptString = "'" + ConfigurationManager.AppSettings["RScriptExeFilePath"] + "'  " + ConfigurationManager.AppSettings["RFilePath"] + "  " + "'" + dataSetNames[i] + "' 'CP' '1990-01-01' '" + String.Format("{0:yyyy-MM-dd}", DateTime.Now.Date) + "' '" + model.Indicator + "'";

                    if (viewStatusValues[i] == "1")
                    {
                        rScriptString = "\"" + ConfigurationManager.AppSettings["RScriptExeFilePath"] + "\" " + ConfigurationManager.AppSettings["RFilePath"] + "  " + "\"" + dataSetNames[i] + "\" " + "\"CP\"" + " " + "1990-01-01" + " " + String.Format("{0:yyyy-MM-dd}", DateTime.Now.Date) + " " + "\"" + model.Indicator + "\" " + "\"" + Email + "\" "
                      + "\"" + model.UserID + "\" " + "\"" + FavouriteListName + "\" ";
                    }
                    else
                    {
                        rScriptString = "\"" + ConfigurationManager.AppSettings["RScriptExeFilePath"] + "\" " + ConfigurationManager.AppSettings["RFilePath"] + "  " + "\"" + dataSetNames[i] + "\" " + "\"C\"" + " " + "1990-01-01" + " " + String.Format("{0:yyyy-MM-dd}", DateTime.Now.Date) + " " + "\"" + model.Indicator + "\" " + "\"" + Email + "\" "
                      + "\"" + model.UserID + "\" " + "\"" + FavouriteListName + "\" ";
                    }
                  
                    //Here CP indicates Customer Portal                   
                    System.IO.File.AppendAllText(filePath, rScriptString);
                }

                finalResult = 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return finalResult;
        }




        /// <summary>
        /// Created by : Raghuveer
        /// Created On  : April 7, 2017
        /// For Making Indicator Series Dropdown as Dynamic in Dashboard page
        /// </summary>
        private void GetIndicatorCategories()
        {
            var indicatorCategories = _homeRepository.GetIndicatorSeriesCategoryDropdowns();
            ViewBag.Indicators = indicatorCategories;
        }


        /// <summary>
        /// Created by : Raghuveer
        /// Created On : 6 June 2017
        /// For Creating Favourite List page for Datacast 2_1
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateFavouriteList()
        {
            GetAccounts();
            GetIndicatorCategories();
            return View();
        }


        /// <summary>
        /// Created By:Raghuveer
        /// Created On: 07-June-2017
        /// Gets the Datasets by Logged in person QBaseAccountId and QbaseDivisionID for Datacast 2.1
        /// </summary>
        /// <param name="accountID"></param>
        /// <param name="divisionID"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetDataSetsByQBaseAccountIDAndDivisionIDForDatacast(int accountID, int divisionID)
        {
            JsonResult result = null;
            try
            {

                var dt1 = _homeRepository.GetDataSetsByQBaseAccountIDAndDivisionIDForDatacast(accountID, divisionID);
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
        ///  Created  By : Raghuveer
        ///  Created On : 7-Jun-2017
        ///  For Getting List Of Indicators to Save FavouritesList
        /// </summary>
        /// <param name="param"></param>
        /// <param name="dataSetID"></param>
        /// <returns></returns>
        public ActionResult FillCorelationLeadLagByDataSetIDForDatacast(JQueryDataTableParamModel param, string dataSetID, Boolean IsFavoritesFlag,string FavouriteListName)
        {

            if (IsFavoritesFlag == false)
            {
                int sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                string sortDirection = Request["sSortDir_0"];

                if (param.sSearch == null)
                    param.sSearch = "";



                IEnumerable<uspSelectLoadCorelationLeadLagByDataSetIDForDatacast_Result> filteredIndicatorList = _homeRepository.GetIndicatorNamesAndCorrelationLeadLagValuesByDataSetIDForDatacast(dataSetID, param.iDisplayLength, param.iDisplayStart, param.sSearch, sortColumnIndex, sortDirection, Convert.ToInt32(Session["UserID"]));
                IEnumerable<uspSelectLoadCorelationLeadLagByDataSetIDForDatacast_Result> searchIndicators = null;


                var result = from c in filteredIndicatorList
                             select new[] { Convert.ToString(c.ASeriesName),Convert.ToString(c.CorrValue),Convert.ToString(c.lagValue),
                             Convert.ToString(c.Company), Convert.ToString(c.Indicator), Convert.ToString(c.Indicator)};



                var totalcount = filteredIndicatorList.Count() == 0 ? 0 : filteredIndicatorList.FirstOrDefault().TotalCount.Value;
                var iTotalRecordsCount = filteredIndicatorList.Count() == 0 ? 0 : filteredIndicatorList.FirstOrDefault().iTotalRecords.Value;

                //  var totalcount = filteredEmployees.FirstOrDefault().TotalCount.Value;

                // var iTotalRecordsCount = filteredEmployees.FirstOrDefault().iTotalRecords.Value.ToString();



                if (totalcount == 0)// for search  with ZERO records as return case.
                {
                    searchIndicators = _homeRepository.GetIndicatorNamesAndCorrelationLeadLagValuesByDataSetIDForDatacast(dataSetID, param.iDisplayLength, param.iDisplayStart, param.sSearch, sortColumnIndex, sortDirection, Convert.ToInt32(Session["UserID"]))
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
                        aaData = searchIndicators
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
           
            else
            {
                return FillFavoritesCorelationLeadLagByDataSetIDByUserID(param, dataSetID, FavouriteListName);
            }

        }


        /// <summary>
        /// Created By : Raghuveer
        /// Created on : 7 June 2017
        /// For Insert Favourite List from ITR Customer Portal site
        /// </summary>
        /// <param name="Company"></param>
        /// <param name="Indicators"></param>
        /// <param name="UserID"></param>
        /// <param name="FavouriteListName"></param>
        /// <returns></returns>
        [HttpPost]
        public int InsertFavouriteList(string Company, string Indicators, string FavouriteListName)
        {
            int finalResult = 0;
            try
            {
                //if (Session.Count > 0)
                //{
                //    if (Session["UserID"] != null && Session["UserID"].ToString() != string.Empty)
                //    {
                finalResult = _homeRepository.InsertFavouriteList(Company, Indicators, Convert.ToInt32(Session["UserID"]), FavouriteListName, Session["UserName"].ToString(), DateTime.Now);
                //    }
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return finalResult;
        }





        /// <summary>
        /// Created By :Raghuveer
        /// Created On : 6 June 2017
        /// For Getting Favourites List
        /// </summary>
        /// <param name="dataSetID"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetAllFavoritesListByDataSetIDUesrID(string dataSetID, int userID, string SelFavName)
        {
            var listInfo = _homeRepository.SelectAllFavoritesListgByDataSetIDUserID(dataSetID, userID, SelFavName);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = int.MaxValue;
            string rettval = serializer.Serialize(listInfo);
            return Json(rettval, JsonRequestBehavior.AllowGet);
        }


        ///// <summary>
        ///// Created By : Raghuveer
        ///// Created on : 21 June 2017
        ///// For Getting Favourites List based on UserID and Company ShortCode
        ///// </summary>
        ///// <param name="accountID"></param>
        ///// <param name="divisionID"></param>
        ///// <returns></returns>
        //[AcceptVerbs(HttpVerbs.Get)]
        //public JsonResult GetFavouriteListByUserIDAndCompanyShortCode( string CompanyShortCode)
        //{
        //    JsonResult result = null;
        //    try
        //    {

        //        var dt1 = _homeRepository.GetFavouriteListByUserIDAndCompanyShortCode(Convert.ToInt32(Session["UserID"]), CompanyShortCode);
        //        var divisions = (from dr in dt1.AsEnumerable()
        //                         select new SelectListItem
        //                         {
        //                             Text = dr.FavouriteListName,
        //                             Value = dr.ID.ToString()
        //                             //Value= dr.ShortCode
        //                         }).ToList();

        //        result = Json(divisions, JsonRequestBehavior.AllowGet);

        //        return result;

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}





        ///// <summary>
        ///// Created By:Raghubveer
        ///// Created On 20 July 2017S
        ///// Filling Datasets by Company, Division and UserID For FavouriteList items
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //public ActionResult FillFavouriteListDataSetsDataTable(JQueryDataTableParamModel param, int CompanyID, int DivisionID)
        //{
        //    ActionResult response = null;

        //    string customerid = string.Empty;
        //    if (Session.Count > 0)
        //    {
        //        if (Session["UserID"] != null && Session["UserID"].ToString() != string.Empty)
        //        {
        //            customerid = Convert.ToString(Session["UserID"].ToString());
        //        }
        //    }

        //    var verifyList = _homeRepository.GetFavoritesLoadCorelationLeadLagByDataSetID(CompanyID, DivisionID, Convert.ToInt32(Session["UserID"]));  // Need to Update Session[UserID]

        //    var displayRecordCount = 0; //for passing displayed records count(if search is true pass the filtered record count, else pass the entire list count

        //    IEnumerable<uspSelectDataSetsByFavouriteListsOfUserID_Result> filteredVerifyList = null; //used to hold the filtered data of taskList            

        //    if (!string.IsNullOrEmpty(param.sSearch))
        //    {
        //        string searchText = param.sSearch.ToLower();

        //        filteredVerifyList = _homeRepository.GetFavoritesLoadCorelationLeadLagByDataSetID(CompanyID, DivisionID, Convert.ToInt32(Session["UserID"]))
        //            .Where(c => ((c.DataSetName != null) && Convert.ToString(c.DataSetName).ToLower().Contains(searchText)) ||
        //                    ((c.DataSetDescription != null) && Convert.ToString(c.DataSetDescription).ToLower().Contains(searchText)) ||
        //                    (Convert.ToString(c.CustomerDataSetID).ToLower().Contains(searchText)) ||
        //                    ((c.CreatedDate != null) && String.Format("{0:MM/dd/yyyy}", c.CreatedDate).Contains(searchText))
        //             );



        //        if (filteredVerifyList.Count() > 0)
        //        {
        //            displayRecordCount = filteredVerifyList.Count();
        //        }
        //        else
        //        {
        //            displayRecordCount = 0;
        //        }
        //    }
        //    else
        //    {
        //        filteredVerifyList = verifyList;
        //        displayRecordCount = verifyList.Count();
        //    }

        //    int sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]); //Gets the SortColumnIndex from the DataTable on User Interaction

        //    Func<uspSelectDataSetsByFavouriteListsOfUserID_Result, string> orderingFunction = (c => sortColumnIndex == 1 ? Convert.ToString(c.CustomerDataSetID) :
        //                                                                                        sortColumnIndex == 2 ? Convert.ToString(c.DataSetName) :
        //                                                                                         sortColumnIndex == 3 ? Convert.ToString(c.DataSetDescription) :
        //                                                                                          sortColumnIndex == 4 ? Convert.ToString(String.Format("{0:yyyyMMdd}", c.CreatedDate)) :

        //                                                                      Convert.ToString(c.CustomerDataSetID)
        //                                                        ); //Sorts the List based on supplied sortColumn index

        //    string sortDirection = Request["sSortDir_0"]; // asc or desc

        //    if (sortDirection == "asc")
        //        filteredVerifyList = filteredVerifyList.OrderBy(orderingFunction); //Sort the List in ascending Order and Re assigns to the List
        //    else
        //        filteredVerifyList = filteredVerifyList.OrderByDescending(orderingFunction); //Sort the List in descending Order and Re assigns to the List


        //    //if (string.IsNullOrEmpty(param.sSearch)) //If seach is null then display the records as seleted in 'records per page' DropDown
        //    //{
        //    if (param.iDisplayLength != -1)
        //    {
        //        filteredVerifyList = filteredVerifyList.Skip(param.iDisplayStart)//skip to start Record in the List which will be supplied as param.iDisplayStart
        //                                             .Take(param.iDisplayLength); //Gets up to param.iDisplayLength from param.iDisplayStart record      
        //    }
        //    //}

        //    var displayedTaskList = filteredVerifyList;

        //    var result = from c in displayedTaskList
        //                 select new[] {
        //                                Convert.ToString(c.CustomerDataSetID),
        //                                Convert.ToString(c.CustomerDataSetID),
        //                                Convert.ToString(c.DataSetName),
        //                                Convert.ToString(c.DataSetDescription),
        //                                String.Format("{0:MM/dd/yyyy}",c.CreatedDate),
        //                                Convert.ToString(c.CustomerDataSetID),
        //                                Convert.ToString(c.CustomerDataSetID),
        //                                //Convert.ToString(c.CustomerDataSetID),
        //                                 Convert.ToString(c.CustomerDataSetID)
        //                 };

        //    response = Json(new
        //    {
        //        sEcho = param.sEcho,
        //        iTotalRecords = verifyList.Count(),
        //        iTotalDisplayRecords = displayRecordCount,
        //        aaData = result
        //    }, JsonRequestBehavior.AllowGet);

        //    return response;


        //}




        #region FavouritesSection Starts For Datacast 2.2

                /// <summary>
                /// Created by : Raghuveer
                /// Created on : 20 July 2017
                /// purpose : For Getting All List of Favourite based on UserID
                /// </summary>
                /// <param name="UserID"></param>
                /// <returns></returns>
                [AcceptVerbs(HttpVerbs.Get)]
                public JsonResult GetAllFavouritesListByUserID(int UserID)
                {
                    JsonResult result = null;
                    try
                    {

                        var dt1 = _homeRepository.SelectAllFavouritesListByUserID(UserID);
                        var divisions = (from dr in dt1.AsEnumerable()
                                         select new SelectListItem
                                         {
                                             Text = dr.FavouriteListName,
                                             Value = dr.FavouriteListName.ToString()
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
                /// Created by :Raghuveer
                /// Created on : 19 July 2017
                /// /// Fill the Favorites list names by dataSetID and userid
                /// <param name="param"></param>
                /// <param name="dataSetID"></param>
                /// <returns></returns>
                public ActionResult FillFavlistNamesByDataSetID(JQueryDataTableParamModel param, string dataSetID, int userID)
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

                    IEnumerable<uspSelectFavouritesListByUserID_Result> filteredFavorites = _homeRepository.SelectFavouritesListByUserID(dataSetID, userID);
                    IEnumerable<uspSelectFavouritesListByUserID_Result> searchFavorites = null;


                    var result = from c in filteredFavorites
                                 select new[] { Convert.ToString(c.FavouriteListName), Convert.ToString(c.FavouriteListName) };



                    var totalcount = filteredFavorites.Count();// == 0 ? 0 : filteredFavorites.FirstOrDefault().TotalCount.Value;
                    var iTotalRecordsCount = filteredFavorites.Count();// == 0 ? 0 : filteredFavorites.FirstOrDefault().iTotalRecords.Value;

                    //  var totalcount = filteredEmployees.FirstOrDefault().TotalCount.Value;

                    // var iTotalRecordsCount = filteredEmployees.FirstOrDefault().iTotalRecords.Value.ToString();



                    if (totalcount == 0)// for search  with ZERO records as return case.
                    {
                        searchFavorites = _homeRepository.SelectFavouritesListByUserID(dataSetID, userID)
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

                /// <summary>
                /// Created by :Raghuveer
                /// Created on : 19 July 2017
                /// /// Fill the CorelationLead data along with Search and Sort
                /// <param name="param"></param>
                /// <param name="dataSetID"></param>
                /// <returns></returns>
                public ActionResult FillCorelationLeadLagByDataSetID(JQueryDataTableParamModel param, string dataSetID, int userID, Boolean IsFavoritesFlag, string SelFavName,
             string CategorySearch, string IndustryName, string SectorName, string SubSector1Name, string SubSector2Name, string SubSector3Name   )
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

                        IEnumerable<uspSelectLoadCorelationLeadLagByDataSetID_Result> filteredEmployees = _homeRepository.SelectLoadCorelationLeadLagByDataSetID(dataSetID, param.iDisplayLength, param.iDisplayStart, param.sSearch, sortColumnIndex, sortDirection, userID);
                        IEnumerable<uspSelectLoadCorelationLeadLagByDataSetID_Result> searchEmployess = null;


                        var result = from c in filteredEmployees
                                     select new[] { Convert.ToString(c.ASeriesName),Convert.ToString(c.CorrValue),Convert.ToString(c.lagValue),
                                     Convert.ToString(c.Company), Convert.ToString(c.Indicator), Convert.ToString(c.Description), Convert.ToString(c.FavouriteListName) ,Convert.ToString(c.Indicator)};



                        var totalcount = filteredEmployees.Count() == 0 ? 0 : filteredEmployees.FirstOrDefault().TotalCount.Value;
                        var iTotalRecordsCount = filteredEmployees.Count() == 0 ? 0 : filteredEmployees.FirstOrDefault().iTotalRecords.Value;

                        //  var totalcount = filteredEmployees.FirstOrDefault().TotalCount.Value;

                        // var iTotalRecordsCount = filteredEmployees.FirstOrDefault().iTotalRecords.Value.ToString();



                        if (totalcount == 0)// for search  with ZERO records as return case.
                        {
                            searchEmployess = _homeRepository.SelectLoadCorelationLeadLagByDataSetID(dataSetID, param.iDisplayLength, param.iDisplayStart, param.sSearch, sortColumnIndex, sortDirection, userID)
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
                /// Created by :Raghuveer
                /// Created on : 20 July 2017
                /// /// Fill the Favorites CorelationLead data along with Search and Sort by dataSetID and userid
                /// <param name="param"></param>
                /// <param name="dataSetID"></param>
                /// <returns></returns>
                public ActionResult FillFavoritesCorelationLeadLagByDataSetIDUesrID(JQueryDataTableParamModel param, string dataSetID, int userID, string SelFavName)
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

                    IEnumerable<uspSelectFavouriteListByUserID_Result> filteredFavorites = _homeRepository.SelectFavoritesLoadCorelationLeadLagByDataSetIDForPortal(dataSetID, userID, SelFavName, param.iDisplayLength, param.iDisplayStart, param.sSearch, sortColumnIndex, sortDirection);
                    IEnumerable<uspSelectFavouriteListByUserID_Result> searchFavorites = null;


                    var result = from c in filteredFavorites
                                 select new[] { Convert.ToString(c.ASeriesName),Convert.ToString(c.CorrValue),Convert.ToString(c.lagValue),
                                     Convert.ToString(c.Company), Convert.ToString(c.Indicator), Convert.ToString(c.Description), Convert.ToString(c.FavouriteListName) ,Convert.ToString(c.Indicator)};



                    var totalcount = filteredFavorites.Count() == 0 ? 0 : filteredFavorites.FirstOrDefault().TotalCount.Value;
                    var iTotalRecordsCount = filteredFavorites.Count() == 0 ? 0 : filteredFavorites.FirstOrDefault().iTotalRecords.Value;

                    //  var totalcount = filteredEmployees.FirstOrDefault().TotalCount.Value;

                    // var iTotalRecordsCount = filteredEmployees.FirstOrDefault().iTotalRecords.Value.ToString();



                    if (totalcount == 0)// for search  with ZERO records as return case.
                    {
                        searchFavorites = _homeRepository.SelectFavoritesLoadCorelationLeadLagByDataSetIDForPortal(dataSetID, userID, SelFavName, param.iDisplayLength, param.iDisplayStart, param.sSearch, sortColumnIndex, sortDirection)
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
                /// Created by Raghuveer  
                /// Created Date : July 20,2017
                /// Insering the Favorites values into db.
                /// </summary>
                /// <param name="model"></param>
                /// <returns></returns>
                [HttpPost]
                public ActionResult AddFavorites(FavoritesModel model)
                {
                    ActionResult response = null;

                    try
                    {
                        //for holding response value of DBCall
                        int intResult = 0;

                        if (Session["UserID"] != null && Session["UserID"].ToString() != string.Empty)
                        {
                            model.CreatedBY = Session["UserID"].ToString();
                            model.CreatedDate = DateTime.Now;

                        }

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
                /// Created by Raghuveer  
                /// Created Date : July 20,2017
                /// Update the Favorites values into db.
                /// </summary>
                /// <param name="model"></param>
                /// <returns></returns>
                [HttpPost]
                public ActionResult UpdateFavorites(FavoritesModel model)
                {
                    ActionResult response = null;

                    try
                    {
                        //for holding response value of DBCall
                        int intResult = 0;

                        if (Session["UserID"] != null && Session["UserID"].ToString() != string.Empty)
                        {
                            model.CreatedBY = Session["UserID"].ToString();
                            model.CreatedDate = DateTime.Now;
                            // model.UserID = Session["UserId"].ToString();

                        }



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
                /// Created Date : 20 July 2017
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
                /// Created Date : 20 July 2017
                /// For Getting Favorites List by Favorite ListName and Company ShortCode
                /// </summary>
                /// <param name="param"></param>
                /// <param name="dataSetID"></param>
                /// <param name="FavouriteListName"></param>
                /// <returns></returns>
                public ActionResult FillFavoritesCorelationLeadLagByDataSetIDByUserID(JQueryDataTableParamModel param, string dataSetID, string FavouriteListName)
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

                    IEnumerable<uspSelectFavouriteListByUserIDForDatacast_Result> filteredFavorites = _homeRepository.SelectFavoritesLoadCorelationLeadLagByDataSetID(dataSetID, Convert.ToInt32(Session["UserID"]), FavouriteListName, param.iDisplayLength, param.iDisplayStart, param.sSearch, sortColumnIndex, sortDirection);
                    IEnumerable<uspSelectFavouriteListByUserIDForDatacast_Result> searchFavorites = null;


                    var result = from c in filteredFavorites
                                 select new[] { Convert.ToString(c.ASeriesName),Convert.ToString(c.CorrValue),Convert.ToString(c.lagValue),
                                     Convert.ToString(c.Company), Convert.ToString(c.Indicator), Convert.ToString(c.Indicator)};



                    var totalcount = filteredFavorites.Count() == 0 ? 0 : filteredFavorites.FirstOrDefault().TotalCount.Value;
                    var iTotalRecordsCount = filteredFavorites.Count() == 0 ? 0 : filteredFavorites.FirstOrDefault().iTotalRecords.Value;

                    //  var totalcount = filteredEmployees.FirstOrDefault().TotalCount.Value;

                    // var iTotalRecordsCount = filteredEmployees.FirstOrDefault().iTotalRecords.Value.ToString();



                    if (totalcount == 0)// for search  with ZERO records as return case.
                    {
                        searchFavorites = _homeRepository.SelectFavoritesLoadCorelationLeadLagByDataSetID(dataSetID, Convert.ToInt32(Session["UserID"]), FavouriteListName, param.iDisplayLength, param.iDisplayStart, param.sSearch, sortColumnIndex, sortDirection)
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
                /// Created On : 21 July 2017
                /// For Getting All Results table count for a CustomerDataSetID
                /// </summary>
                /// <returns></returns>
                public JsonResult GetAllResultsCount(int CustomerDataSetID)
                {
                    int resultsCount = _homeRepository.GetAllResultsCount(CustomerDataSetID);
                    return Json(resultsCount, JsonRequestBehavior.AllowGet);
                }

        #endregion FavouritesSection Ends For Datacast 2.2


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
                public JsonResult GetSubSectors1BySector(string Industry, string Sector)
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

                        var dt1 = _homeRepository.GetSubSectors3BySubSectors2(Industry, Sector, SubSector1, SubSector2);
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

        #region ForDatacast 2.3

                /// <summary>
                /// Created by :Khajavali
                /// Created on : August 30 2017
                /// /// Fill the Favorites CorelationLead data along with  Category Search
                /// <param name="param"></param>
                /// <param name="dataSetID"></param>
                /// <returns></returns>
                public ActionResult FillFavoritesCorelationLeadLagByCategorySearch(JQueryDataTableParamModel param, string dataSetID, int userID, string IndustryName,
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

                    IEnumerable<uspSelectIndicatorListByCategorySearchPortal_Result> filteredFavorites = _homeRepository.GetIndcatorListByCategorySearch(dataSetID, Convert.ToInt32(userID), IndustryName, SectorName, SubSector1Name, SubSector2Name, SubSector3Name, param.iDisplayLength, param.iDisplayStart, param.sSearch, sortColumnIndex, sortDirection);
                    IEnumerable<uspSelectIndicatorListByCategorySearchPortal_Result> searchFavorites = null;


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


        #endregion datacast 2.3

           #region DASHBOARD
            /// <summary>
            /// Created By: Vijaya
            /// Created On : 04/03/2018
            /// Clears the Dataset's analysis...
            /// </summary>
            /// <param name="DatasetID"></param>
            /// <returns></returns>
            [HttpPost]
                public int ClearDataSetAnalysis(int DatasetID)
            {

                int finalResult = 0;

                try
                {
                    finalResult = _homeRepository.ClearDataSetAnalysis(DatasetID);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return finalResult;
            }
          #endregion

    }
}