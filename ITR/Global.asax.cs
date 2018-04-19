using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ITR.Models;
using ITR.Controllers;
using System.Diagnostics;
using ITR.DataRepository;

namespace ITR
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            Bootstrapper.Initialise();
        }

        /// <summary>
        /// CreatedBy: Ram Mohan
        /// Created Date: 01-09-2016
        /// Storing Exception Details in db
        /// </summary>        
        protected void Application_Error(object sender, EventArgs e)
        {
            HomeRepository objExcpLog = new HomeRepository();

            Exception exception = Server.GetLastError();

            HttpContext con = HttpContext.Current;
            DateTime logdate = DateTime.Now;
            string url = con.Request.Url.ToString();
            string Message = exception.InnerException == null ? exception.Message : exception.InnerException.ToString();
            string Soruce = exception.Source;

            string CustomerId = string.Empty;                      

            if (Session.Count > 0)
            {
                if (Session["UserID"] != null && Session["UserID"].ToString() != string.Empty)
                {
                    CustomerId = Session["UserID"].ToString();
                }                

            }                  
            //Inserting exception details
            objExcpLog.InsertExceptionLogDetails(CustomerId, Message, url, CustomerId);


        }
    }
}
