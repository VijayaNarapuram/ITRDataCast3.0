using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ITR.DataRepository;
using ITR.Models;
using Newtonsoft.Json;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace ITR.Controllers
{
    public class DatacastDBWidgetController : Controller
    {
        private DatacastDBWidgetIDataRepository _homeRepository;

        //Dependency Injection resolver using the interface
        public DatacastDBWidgetController(DatacastDBWidgetIDataRepository _repo)
        {
            _homeRepository = _repo;

        }

        //
        // GET: /DatacastDBWidget/
        public ActionResult Index()
        {
            return View();
        }

           /// <summary>
        /// CreatedBy: Vijaya
        /// Created Date: 2018/04/18
        /// TO Select Phase values of Dashboard Widgets By CompanySchortCode
        /// </summary>
        /// <returns></returns>
        public JsonResult SelectPhaseValuesOfDBWidgetsList(int UserId, string CompanyShortCode)
        {
            string finalResult = "";
            var DBWidgetsListByCompanyId = _homeRepository.SelectDBWidgetsListByCompanyID(UserId, CompanyShortCode);

            //var PhaseValuesOfDBWidgetsList = _homeRepository.SelectPhaseValuesOfDBWidgetsByCompanyId(CompanyShortCode);
            var PhaseValuesOfDBWidgetsList = _homeRepository.SelectPhaseValuesOfDBWidgetsByCompanyIdNew(CompanyShortCode);
            var TimingValuesOfDBWidgetsList = _homeRepository.SelectTimingValuesOfDBWidgetsByCompanyId(CompanyShortCode);

            var DistinctPhaseValuesOfDBWidgetsList = PhaseValuesOfDBWidgetsList.Select(e => new {e.DashboardWidgetsListId, e.Indicator })
                                .Distinct();

            string currentYear = DateTime.Now.Year.ToString().Substring(DateTime.Now.Year.ToString().Length - 2);
            string CurrentMonth = String.Format("{0:MMMM}", DateTime.Now);
            //CurrentMonth = "January";

            DateTime currentDate = DateTime.Now;
            string nextYear = currentDate.AddYears(1).Year.ToString();
            string NNYear = currentDate.AddYears(2).Year.ToString();
            //int CurrentMonthtiming = 0;

            DataTable dt = new DataTable();
            dt.Columns.Add("MonthYear");
            dt.Columns.Add("Phase");
            DataRow tempRow = dt.NewRow();
            foreach(var item in PhaseValuesOfDBWidgetsList)
            {
                tempRow = dt.NewRow();
                tempRow["MonthYear"] = item.MonthYear;
                tempRow["Phase"] = item.Phases;
                dt.Rows.Add(tempRow);
            }
            //DataTable transposedTable = GenerateTransposedTable(dt);

            finalResult =
                   "<div class='table-responsive'>" +                      
                       "<table class='table table-striped table-bordered' cellspacing='0' width='100%'>" +
                       "<thead> " +
                       "  <tr> " + "<th style='background-color:lightgray;display:none;' colspan='1'> Dashboard Widgets ListId </th>" +
                                   "<th style='background-color:lightgray;' colspan='1'> Indicator </th>" +
                                    "<th style='background-color:lightgray;' colspan='1'> Current Month </th>" +
                                   "<th style='background-color:lightgray;' colspan='1'> Mar-" + currentYear + "</th>" +
                                   "<th style='background-color:lightgray;' colspan='1'> Jun-" + currentYear + "</th>" +
                                   "<th style='background-color:lightgray;' colspan='1'> Sep-" + currentYear + "</th>" +
                                   "<th style='background-color:lightgray;' colspan='1'> Dec-" + currentYear + "</th>" +
                                   "<th style='background-color:lightgray;' colspan='1'> Year-End " + nextYear + "</th>" +
                                   "<th style='background-color:lightgray;' colspan='1'> Year-End " + NNYear + "</th>" +                                  
                                   "<th style='background-color:lightgray;' colspan='1'> Timing </th>" +
                                   "<th style='background-color:lightgray;' colspan='1'> Delete </th>" +
                           "</tr>";
            finalResult += "</thead>";
            finalResult += "<tbody>";    

            string[] monthsArray = new string[] { CurrentMonth, "mar", "jun", "sep", "dec", "dec_" + nextYear.Substring(nextYear.Length - 2), "dec_" + NNYear.Substring(NNYear.Length - 2) };
            
            //foreach (var item in DistinctPhaseValuesOfDBWidgetsList)
            if (DBWidgetsListByCompanyId.Count > 0)
            {
                foreach (var item in DBWidgetsListByCompanyId)
                {
                    finalResult += "<tr>";
                    finalResult += "<td style='background-color:lightgray;display:none;'>" + item.DashboardWidgetsListId + "</td>";
                    finalResult += "<td>" + item.Indicator + "</td>";

                    foreach (var AI in monthsArray)
                    {
                        if (AI.Contains('_')) //NEXT YEAR, NEXTNEXT YEAR...
                        {
                            var YearValue = AI.Split('_')[1];
                            var indicatorValuesList = PhaseValuesOfDBWidgetsList.Select(e => new { e.DashboardWidgetsListId, e.Indicator, e.SHORTCODE, e.MY, e.Phases }).
                                                                            Where(e => e.Indicator == item.Indicator &&
                                                                                       AI.Split('_')[0].ToLower().Contains(e.MY.ToString().Split(' ')[1].ToLower()) && //MONTH                                                                                    
                                                                                      e.MY.ToString().Split(' ')[2] == YearValue
                                                                            );

                            //var ExistsCount = indicatorValuesList.Count(a => (a.DashboardWidgetsListId == item.DashboardWidgetsListId)
                            //                                               && a.MY.ToString().Split(' ')[2] == AI.Split('_')[1]
                            //                                               && a.MY.ToString().Split(' ')[1].ToString().ToLower() == "dec"
                            //                                               ) > 0;

                            if (indicatorValuesList.Count() > 0)
                            {
                                string PhaseVal = string.Empty;

                                foreach (var value in indicatorValuesList)
                                {
                                    //if (value.MY.ToString().Split(' ')[2].ToString().ToLower() == "dec" && value.MY.ToString().Split(' ')[1] == nextYear.Substring(nextYear.Length - 2))
                                    //{
                                        PhaseVal = value.Phases;
                                   // }                                          
                                }

                                finalResult += "<td>" + PhaseVal + "</td>";         
                            }
                            else
                            {
                                finalResult += "<td>" + "N/A" + "</td>";
                            }
                        }
                        else //CURRENT YEAR...
                        {
                            var indicatorValuesList = PhaseValuesOfDBWidgetsList.Select(e => new { e.DashboardWidgetsListId, e.Indicator, e.SHORTCODE, e.MY, e.Phases }).
                                                                                                    Where(e => e.Indicator == item.Indicator
                                                                                                               && AI.ToLower().Contains(e.MY.ToString().Split(' ')[1].ToLower()) //MONTH
                                                                                                               && e.MY.ToString().Split(' ')[2] == currentYear //YEAR
                                                                                                        
                                                                                                  );

                            //var ExistsCount = indicatorValuesList.Count(a => a.DashboardWidgetsListId == item.DashboardWidgetsListId && 
                            //                                                 a.MY.ToString().Split(' ')[2] == currentYear      
                            //                                           ) > 0;

                            //var IndicatorsCount = indicatorValuesList.Count();

                            if (indicatorValuesList.Count() > 0)
                            {
                                //string PhaseValue = string.Empty;
                                foreach (var value in indicatorValuesList)
                                {
                                    if (value.MY.ToString().Split(' ')[1].ToString().ToLower() == AI.ToString().ToLower())
                                    {
                                        finalResult += "<td>" + value.Phases + "</td>";
                                    }
                                }                               
                            }
                            else
                            {
                                finalResult += "<td>" + "N/A" + "</td>";
                            }
                        }
                    }

                    //ADD TIMING COLUMN...
                    //finalResult += "<td>" + CurrentMonthtiming + "</td>";
                    Double IndicatorTiming = 0;
                    foreach (var item1 in TimingValuesOfDBWidgetsList)
                    {
                        if (item1.IndicatorShortCode == item.IndicatorShortCode)
                        {                           
                            IndicatorTiming = Convert.ToDouble(item1.Timing);
                        }                                        
                    }
                    finalResult += "<td>" + IndicatorTiming + "</td>";       

                    finalResult += "<td><a href='#' id='btnDelDbWidget_" + item.DashboardWidgetsListId + "' class='pull-left btnDelDbWidget'  title='Click to delete dashboard widgets'>Delete</a></td>";
                    finalResult += "</tr>";
                }
            }
            else
            {
                finalResult += "<tr><td><span>No widgets selected for this company</span></td></tr>";
            }

            finalResult += "</tbody>";
            return Json(finalResult, JsonRequestBehavior.AllowGet);
        }

        //[AcceptVerbs(HttpVerbs.Get)]
        //public JsonResult SelectDBWidgetsListByCompanyID(string CompanyShortCode, int UserId)
        //{
        //    var listInfo = _homeRepository.SelectDBWidgetsListByCompanyID(UserId, CompanyShortCode);
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    serializer.MaxJsonLength = int.MaxValue;
        //    string rettval = serializer.Serialize(listInfo);
        //    return Json(rettval, JsonRequestBehavior.AllowGet);
        //}
               
        public ActionResult SelectDBWidgetsListByCompanyID(string CompanyShortCode, int UserId)
        {
            ActionResult response = null;
            IList<uspSelectDBWidgetsListByCompanyID_Result> resultList = _homeRepository.SelectDBWidgetsListByCompanyID(UserId, CompanyShortCode);

            string strIndicatorsOfCompany = string.Empty;

            try
            {
                if (resultList.Count > 0)
                {
                    foreach (var item in resultList)
                    {
                        strIndicatorsOfCompany = strIndicatorsOfCompany + item.IndicatorShortCode + '_' + item.Indicator + '~';
                    }

                    //strIndicatorsOfCompany = Convert.ToString(resultList[0].IndicatorShortCode);

                    response = Json(new { result = "Success", strIndicatorsOfCompany = strIndicatorsOfCompany });
                }
                else
                {
                    response = Json(new { result = "notSuccess", strIndicatorsOfCompany = strIndicatorsOfCompany });
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
        /// To Delete a DASHBOARD WIDGET...
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteDBWidgetIndicator(int DashboardWidgetsListId, string CompanyShortCode, int UserID)
        {
            ActionResult response = null;

            try
            {
                //for holding response value of DBCall
                int intResult = 0;

                //inserts the details to table
                intResult = _homeRepository.DeleteDBWidgetIndicator(DashboardWidgetsListId, CompanyShortCode, UserID);

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
