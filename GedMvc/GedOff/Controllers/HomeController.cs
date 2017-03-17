using BLLEntidades;
using DEEntities;
using GedOff.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utilities;

namespace GedOff.Controllers
{
    public class HomeController : Controller
    {
        string _Path = "/Inicio";
        public ActionResult Inicio()
        {
            var ret = Session["entidade"] as Entidade;

            if (ret != null)
            {
                return View(ret);
            }

            return View();
        }

        public ActionResult Login()
        {
            //string path = @"c:\gedrepositorio\config.bin";

            //var con = Util.Deserializar(path, "");
            //string PATH = @"\currentuser\SOFTWARE\gedsys\guidkey";
            ////var guid = Util.retornarRegitro(PATH, "guidkey");

            //if (con != null)
            //{
            ///*    if (con.GuidValue != guid)
            //    {
            //        Session["licensaInvalida"] = "Este arquivo de licensa não está válido, contate o administrador!";
            //        return RedirectToAction("/licensa");
            //    }
            // */ 
            //}
            ////Session["configuracao"] = con;

            //var dias = (con.DataFimLicensa - DateTime.Now);

            //if (con.DataFimLicensa < DateTime.Now)
            //{
            //    return RedirectToAction("/licensa");
            //}

            //Session.Clear();
            return View();
        }


        [HttpPost]
        public ActionResult GedLogin(string inputLogin, string inputPassword3)
        {
            try
            {
                var ret = BLLEntidades.BLLEntidade.Logar(inputLogin, inputPassword3);

                if (ret == null)
                {
                    //return RedirectToAction("Login");
                    //Response.Redirect("/home/Login?msg=Usuário ou senha inválidos");
                    return RedirectToAction("/login", new { msg = "Usuário ou senha inválidos" });
                }
                else
                {
                    Session["entidade"] = ret;
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("/login", new { msg = ex.Message });
                //Response.Redirect("/home/Login?msg=" + ex.Message);

                //return Content("<script language='javascript' type='text/javascript'>window.location.href =  myApp.retornarAppName+'/home/login?msg=erro'</script>");
            }

            return RedirectToAction(_Path);
        }

        public ActionResult Logout()
        {
            var session = Session["entidade"] = null;
            Response.Redirect(ConfigurationManager.AppSettings["appname"] + "/Home/Login");

            return View();
        }

        private void VerificarSessao(object obj)
        {
            if (obj == null)
            {
                Response.Redirect(ConfigurationManager.AppSettings["appname"] + "/Home/Login");
                return;
            }
        }

        public ActionResult AlterarSenha()
        {
            return View();
        }

        public ActionResult Licensa()
        {
            string path = @"c:\gedrepositorio\config.bin";
            var con = Util.Deserializar(path, "");
            return View(con);
        }

    }
}
