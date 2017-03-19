using DEEntities;
using GedOff.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GedOff.Controllers
{
    [CustomSecurity]
    public class DeptoUsuarioController : Controller
    {
        public ActionResult Relacionar()
        {
            var Entidades = BLLEntidades.BLLEntidade.RetornarListaEntidade();
            return View(Entidades);
        }

        [HttpPost]
        public JsonResult RetornarListaDeptos()
        {
            return Json(BLLEntidades.BLLDepto.RetornarDeptos());
        }

        [HttpPost]
        public JsonResult GravarRelacao(string iddepto, string identidade)
        {
            var deptos = iddepto.Split(',');

            foreach (string item in deptos)
            {
                try
                {

                    BLLEntidades.BLLEntidadeDepto.AtribuirEntidadeDepto(
                    new Entidade() { Codigo = int.Parse(identidade) }, new Depto() { Id = int.Parse(item) });
                }
                catch
                {
                }
            }

            return Json("");
        }

        [HttpPost]
        public JsonResult DeletarRelacao(string iddepto, string id)
        {
            return Json(BLLEntidades.BLLEntidadeDepto.DeletarRelacao(id, iddepto));
        }

        [HttpPost]
        public JsonResult RetornarDeptosEntidade(string identidade)
        {
            return Json(BLLEntidades.BLLEntidadeDepto.RetornarDepartamentosEntidade(new EntidadeDepto()
            {
                cdEntidade = int.Parse(identidade)
            }));
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
