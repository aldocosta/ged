using DEEntities;
using GedOff.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GedOff.Controllers
{
    public class DepartamentoController : Controller
    {
        //
        // GET: /Departamento/

        public ActionResult Novo()
        {
            Entidade entidade = Session["entidade"] as Entidade;
            VerificarSessao(entidade);
            return View();
        }

        [ErrorHandler]
        [HttpPost]
        public JsonResult DeletarRegistro(string id)
        {
            Entidade entidade_ = Session["entidade"] as Entidade;
            bool del = false;
            if (entidade_ == null)
            {
                var erro = new
                {
                    msg = "Logon expirado"
                };
                return Json(erro);
            }
            try
            {
                del = BLLEntidades.BLLDepto.DeletarDepto(int.Parse(id));
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf("Cannot delete or update a parent row") > -1)
                {
                    var erro = new
                    {
                        msg = "Não é possível deletar este registro, pois o mesmo contem registros dependentes"
                    };
                    return Json(erro);
                }
            }
            return Json(del);
        }

        [ErrorHandler]
        [HttpPost]
        public JsonResult NovoDepto(string NmDepto, string id)
        {
            Entidade entidade_ = Session["entidade"] as Entidade;

            if (entidade_ == null)
            {
                var erro = new
                {
                    msg = "Logon expirado"
                };
                return Json(erro);
            }
            if (id != "" && id != null)
            {
                return Json(BLLEntidades.BLLDepto.Atualizar(id, NmDepto));
            }

            return Json(BLLEntidades.BLLDepto.GravarDepto(new DEEntities.Depto()
            {
                DataCriacao = DateTime.Now,
                NomeDepto = NmDepto,
                CdEntidade = entidade_.Codigo
            }));
        }

        [ErrorHandler]
        [HttpPost]
        public JsonResult CriarGrid()
        {
            Entidade entidade_ = Session["entidade"] as Entidade;

            if (entidade_ == null)
            {
                var erro = new
                {
                    msg = "Logon expirado"
                };
                return Json(erro);
            }
            var ret = from alias in BLLEntidades.BLLDepto.RetornarDeptos()
                      select new
                      {
                          Id = alias.Id,
                          NomeDepto = alias.NomeDepto,
                          DataCriacao = alias.DataCriacao.ToShortDateString(),
                          Count = BLLEntidades.BLLDepto.RetornarDeptos().Count,
                          Owner = alias.OwnerName
                      };
            return Json(ret, JsonRequestBehavior.AllowGet);
        }

        private void VerificarSessao(object obj)
        {
            if (obj == null)
            {
                Response.Redirect(ConfigurationManager.AppSettings["appname"] + "/Home/Login");
                return;
            }
        }

    }
}
