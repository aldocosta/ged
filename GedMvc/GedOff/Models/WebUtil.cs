using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GedOff.Models
{
    public class WebUtil
    {
        private static System.Web.UI.Page Page = new System.Web.UI.Page();
        public static void RedirectToLogin()
        {
            Page.Response.Redirect("/home/login");
        }

        public static void ValidarSessao()
        {
            if (Page.Session["entidade"] == null)
            {
                RedirectToLogin();
            }
        }
    }
}