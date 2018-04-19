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
        public JsonResult SelectPhaseValuesOfDBWidgetsList(string CompanyShortCode)
        {
            string finalResult = "";
            var PhaseValuesOfDBWidgetsList = _homeRepository.SelectPhaseValuesOfDBWidgetsByCompanyId(CompanyShortCode);


            var DistinctPhaseValuesOfDBWidgetsList = PhaseValuesOfDBWidgetsList.Select(e => new {e.DashboardWidgetsListId, e.Indicator })
                                .Distinct();

            string currentYear = DateTime.Now.Year.ToString();
            DateTime currentDate = DateTime.Now;
            string nextYear = currentDate.AddYears(1).Year.ToString();
            string NNYear = currentDate.AddYears(2).Year.ToString();

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
                       "<table class='table table-th-block table-dark' id='Phasesdatatable-example'>" +
                       "<thead> " +
                       "  <tr> " + "<th style='background-color:lightgray;display:none;' colspan='1'> Dashboard Widgets ListId </th>" +
                                   "<th style='background-color:lightgray;' colspan='1'> Indicator </th>" +
                                   "<th style='background-color:lightgray;' colspan='1'> Mar-" + currentYear + "</th>" +
                                   "<th style='background-color:lightgray;' colspan='1'> Jun-" + currentYear + "</th>" +
                                   "<th style='background-color:lightgray;' colspan='1'> Sep-" + currentYear + "</th>" +
                                   "<th style='background-color:lightgray;' colspan='1'> Dec-" + currentYear + "</th>" +
                                   "<th style='background-color:lightgray;' colspan='1'> Year-End " + nextYear + "</th>" +
                                  "<th style='background-color:lightgray;' colspan='1'> Year-End " + NNYear + "</th>" +
                                  "<th style='background-color:lightgray;' colspan='1'> Delete </th>" +
                                   "<th style='background-color:lightgray;' colspan='1'> Timing </th>" +
                           "</tr>";
            finalResult += "</thead>";
            finalResult += "<tbody>";
            
            foreach (var item in DistinctPhaseValuesOfDBWidgetsList)
            {
                finalResult += "<tr>";
                finalResult += "<td style='background-color:lightgray;display:none;'>" + item.DashboardWidgetsListId + "</td>";
                finalResult += "<td>" + item.Indicator + "</td>";
                finalResult += "<td>" + "N/A" + "</td>";
                finalResult += "<td>" + "N/A" + "</td>";
                finalResult += "<td>" + "N/A" + "</td>";
                finalResult += "<td>" + "N/A" + "</td>";
                finalResult += "<td>" + "N/A" + "</td>";
                finalResult += "<td>" + "N/A" + "</td>";
                finalResult += "<td>" + "N/A" + "</td>"; //TIMING
                finalResult += "<td><button id='btnAddInline_" + item.DashboardWidgetsListId + "' type='button' class='btn btn-success pull-left saveBtn btndelInline' style='display: inline-block;'>Delete</button></td>";
                finalResult += "</tr>";
            }

            finalResult += "</tbody>";
            return Json(finalResult, JsonRequestBehavior.AllowGet);
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