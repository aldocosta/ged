using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GedOff.Security
{
    public class CustomSecurity : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //List<UsuarioViewModel> bd = new List<UsuarioViewModel>();
            //bd.Add(new UsuarioViewModel() { Name = "cc1", Pass = "123", Roles = new[] { "usuario" } });
            //bd.Add(new UsuarioViewModel() { Name = "cc2", Pass = "123", Roles = new[] { "admin", "usuario" } });
            //bd.Add(new UsuarioViewModel() { Name = "cc3", Pass = "123", Roles = new[] { "super", "usuario" } });



            if (filterContext == null)
            {

            }

            string name = filterContext.HttpContext.User.Identity.Name;
            var user = System.Web.HttpContext.Current.Session["entidade"];


            if (string.IsNullOrEmpty(name))
            {
                this.HandleUnauthorizedRequest(filterContext);
            }

            //else
            //{
            //    var atroles = this.Roles.Split(',');
            //    var userroles = user.Roles;

            //    var retono = user.Roles.Where(s => this.Roles.IndexOf(s) > -1).FirstOrDefault() != null;
            //    if (!retono)
            //    {
            //        this.HandleUnauthorizedRequest(filterContext);
            //    }
            //}
        }
    }
}




