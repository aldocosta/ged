using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace GedOff
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        //public void Application_Error(object sender, EventArgs e)
        //{
        //    HttpContext context = HttpContext.Current;
        //    Exception exception = context.Server.GetLastError();

        //    //if (exception.Message != "The controller for path '/Scripts/jquery.min.map' was not found or does not implement IController.")
        //    //{
        //    //    Response.Redirect("../erro.html");
        //    //    //((SqlException)exception). // aqui você acessa as propriedades de uma SqlException
        //    //}
        //}

    }
}