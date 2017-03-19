using DEEntities;
using GedOff.Models;
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
    public class CadastroController : Controller
    {
        //
        // GET: /Cadastro/

        public ActionResult Novo()
        {
            Entidade entidade = Session["entidade"] as Entidade;            
            return View(entidade);
        }

        [HttpPost]
        public ActionResult Novo(string NmEntidade, string NmUser, string NmEmail, string selTipo)
        {
            Entidade entidade = Session["entidade"] as Entidade;            
            Entidade entidadedestino = new Entidade()
            {
                NmEntidade = NmEntidade,
                NmUser = NmUser,
                NmEmail = NmEmail,
                NmPass = "123456",
                Tipo = new TipoEntidade()
                {
                    Codigo = int.Parse(selTipo)
                }
            };


            BLLEntidades.BLLEntidade.GravarEntidade(entidade, entidadedestino);

            return View(entidade);
        }

        [ErrorHandler]
        [HttpPost]
        public JsonResult NovoRegistro(string NmEntidade, string NmUser, string NmEmail, string selTipo, string Codigo)
        {
            Entidade entidade = Session["entidade"] as Entidade;
            if (entidade == null)
            {
                var erro = new
                {
                    msg = "Logon expirado"
                };
                return Json(erro);
            }
            try
            {
                Entidade entidadedestino = new Entidade()
                    {
                        NmEntidade = NmEntidade,
                        NmUser = NmUser,
                        NmEmail = NmEmail,
                        NmPass = "123456",
                        Tipo = new TipoEntidade()
                        {
                            Codigo = int.Parse(selTipo)
                        }
                    };

                if (Codigo != "")
                {
                    entidadedestino.Codigo = int.Parse(Codigo);
                    BLLEntidades.BLLEntidade.AtualizarEntidade(entidadedestino);
                }
                else
                {
                    BLLEntidades.BLLEntidade.GravarEntidade(entidade, entidadedestino);
                }
            }
            catch (Exception ex)
            {
                var erro = new
                {
                    msg = ex.Message
                };
                return Json(erro);
            }
            return Json(entidade);
        }

        [ErrorHandler]
        [HttpPost]
        public JsonResult ResetSenha(string id)
        {
            var ret = BLLEntidades.BLLEntidade.AtualizarEntidadePass(new Entidade()
            {
                Codigo = int.Parse(id),
                NmPass = "123456"
            });
            return Json(ret);
        }

        [ErrorHandler]
        [HttpPost]
        public JsonResult DeletarRegistro(string id)
        {
            Entidade entidade_ = Session["entidade"] as Entidade;
            bool ret = false;
            if (entidade_ == null)
            {
                var erro = new
                {
                    msg = "Logon expirado"
                };
                return Json(erro);
            }
            Entidade entidade = new Entidade() { Codigo = int.Parse(id) };

            try
            {
                ret = BLLEntidades.BLLEntidade.DeletarEntidade(entidade);
            }
            catch (Exception ex)
            {
                string error = "";
                if (ex.Message.IndexOf("Cannot delete or update a parent row: a foreign key constraint fails") > -1)
                {
                    error = "Não é possivel deletar esse registro pois há registros atrelados a ele, porém é possível bloquea-lo";
                }
                else
                {
                    error = ex.Message;
                }

                var erro = new
                {
                    msg = error
                };
                return Json(erro);
            }

            return Json(ret);
        }


        [HttpPost]
        public JsonResult BloquearRegistro(string id)
        {
            Entidade entidade_ = Session["entidade"] as Entidade;
            bool ret = false;
            if (entidade_ == null)
            {
                var erro = new
                {
                    msg = "Logon expirado"
                };
                return Json(erro);
            }
            Entidade entidade = new Entidade() { Codigo = int.Parse(id) };

            try
            {
                ret = BLLEntidades.BLLEntidade.MarkAsDeleted(entidade);
            }
            catch (Exception ex)
            {
                string error = "";

                error = ex.Message;

                var erro = new
                {
                    msg = error
                };
                return Json(erro);
            }

            return Json(ret);
        }


        [ErrorHandler]
        [HttpPost]
        public JsonResult AtualizarPassword(string iduser, string novopass, string passconfirma)
        {
            bool ret = false;
            Entidade ent = new Entidade();

            if (novopass != passconfirma)
            {
                var erro = new
                {
                    msg = "Password confirma, não confere"
                };
                return Json(erro);
            }

            ent.Codigo = Convert.ToInt32(iduser);
            ent.NmPass = novopass;

            ret = BLLEntidades.BLLEntidade.AtualizarEntidadePass(ent);

            return Json(ret);
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

            return Json(BLLEntidades.BLLEntidade.RetornarListaEntidade(), JsonRequestBehavior.AllowGet);
        }


        public ActionResult UsuariosBloqueados()
        {
            Entidade entidade_ = Session["entidade"] as Entidade;            
            var ret = BLLEntidades.BLLEntidade.RetornarListaEntidadeMarcadosDeletado();
            return View(ret);
        }

        [HttpPost]
        public ActionResult DesbloquearUsuario(string dadosid, string dadosnome)
        {
            Entidade entidade_ = Session["entidade"] as Entidade;            
            BLLEntidades.BLLEntidade.UnMarkUser(new Entidade() { Codigo = int.Parse(dadosid) });
            var ret = BLLEntidades.BLLEntidade.RetornarListaEntidadeMarcadosDeletado();
            return View("UsuariosBloqueados", ret);
        }
        
    }
}
