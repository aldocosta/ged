﻿using BLLEntidades;
using DEEntities;
using GedOff.Models;
using GedOff.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GedOff.Controllers
{
    [CustomSecurity]
    public class PastaDeptoController : Controller
    {
        //DAL.DalPasta dalpasta = new DAL.DalPasta();
        DAL.DALProcesso dalprocesso = new DAL.DALProcesso();
        //
        // GET: /PastaDepto/
        public Depto RetornarDepto { get; set; }

        /// <summary>
        /// Gerencia as pastas do depto
        /// </summary>
        /// <returns></returns>
        public ActionResult GerenciarPastas()
        {
            Session["idpasta"] = null;
            var id = int.Parse(Request.QueryString["iddepto"].Replace("#", ""));
            Session["iddepto"] = id;

            Session["depto"] = BLLEntidades.BLLPasta.RetornarDepto(id);

            var ret = BLLEntidades.BLLPasta.RetornarPastasDepto(id).Where(p => p.CodigoPai == 0);

            return View(ret);
        }

        public ActionResult GerenciarPastasDaPasta()
        {
            var idpasta = int.Parse(Request.QueryString["idpasta"]);

            var pasta = BLLPasta.RetornarPasta(idpasta);
            Session["pasta"] = pasta;

            int id = pasta.CodigoDepto;

            RetornarDepto = BLLEntidades.BLLPasta.RetornarDepto(id);

            Session["depto"] = RetornarDepto;


            List<PastaProcesso> pastas = new List<PastaProcesso>();

            //retornando pastas
            BLLPasta.RetornarPastasDepto(pasta.CodigoDepto).Where(p => p.CodigoPai == idpasta).ToList().ForEach(delegate(Pasta p)
                         {
                             pastas.Add(new PastaProcesso()
                             {
                                 Codigo = p.Codigo,
                                 CodigoDepto = p.CodigoDepto,
                                 CodigoPai = p.CodigoPai,
                                 DsPasta = p.DsPasta,
                                 DtCriacao = p.DtCriacao,
                                 NmPasta = p.NmPasta,
                                 Tipo = "Pasta",
                                 Owner = p.Owner
                             });
                         });

            //retornando processos
            BLLProcessos.RetornarListaProcesso().Where(p => p.IdPai == idpasta).ToList().ForEach(delegate(Processos p)            
            {
                pastas.Add(new PastaProcesso()
                {
                    Codigo = p.Id,
                    DsPasta = p.Descricao,
                    NmPasta = p.NomeProcesso,
                    Tipo = "Processo",
                    Owner = p.Owner
                });
            });


            return View(pastas);
        }

        public ActionResult GerenciarProcessos()
        {            
            int id = 0;
            try
            {
                id = int.Parse(Request.QueryString["idprocesso"]);
            }
            catch
            {
            }

            Session["idpasta"] = id;

            var ret = BLLEntidades.BLLProcessos.RetornarProcesso(id);

            return View(ret);
        }

        [ErrorHandler]
        [HttpPost]
        public JsonResult GravarPasta(string NmPasta, string DsPasta, string idpastaform)
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

            var id = int.Parse(Session["iddepto"].ToString());
            int idpastapai = 0;
            if (Session["idpasta"] != null)
            {
                idpastapai = int.Parse(Session["idpasta"].ToString());
            }

            Pasta p = new Pasta()
            {
                CodigoPai = idpastapai,
                NmPasta = NmPasta,
                DsPasta = DsPasta,
                DtCriacao = DateTime.Now,
                CodigoDepto = id,
                CdEntidade = entidade_.Codigo
            };

            if (string.IsNullOrEmpty(idpastaform))
            {
                //p.Codigo = dalpasta.Save(p);
                p.Codigo = BLLPasta.SalvarPasta(p);
            }
            else
            {
                p.Codigo = int.Parse(idpastaform);
                BLLPasta.AtualizarPasta(p);
            }

            p.Owner = BLLEntidade.RetornarEntidade(p.CdEntidade);
            return Json(p);
        }

        [ErrorHandler]
        [HttpPost]
        public JsonResult GravarPastaFilha(string NmPasta, string DsPasta, string idpastapai)
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

            var id = int.Parse(Session["iddepto"].ToString());
            idpastapai = idpastapai.Replace("#", "");
            Pasta p = new Pasta()
            {
                CodigoPai = int.Parse(idpastapai),
                NmPasta = NmPasta,
                DsPasta = DsPasta,
                DtCriacao = DateTime.Now,
                CodigoDepto = id,
                CdEntidade = entidade_.Codigo
            };

            //p.Codigo = dalpasta.Save(p);
            p.Codigo = BLLPasta.SalvarPasta(p);
            p.Owner = BLLEntidade.RetornarEntidade(p.CdEntidade);

            return Json(p);
        }


        [ErrorHandler]
        [HttpPost]
        public JsonResult ApagarPasta(string id)
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

            bool ret = false;
            try
            {
                ret = BLLPasta.DeletarPasta(new Pasta()
                    //ret = dalpasta.Delete(new Pasta()
                    {
                        Codigo = int.Parse(id)
                    });
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf("Cannot delete or update a parent row") > -1)
                {
                    var erro = new
                    {
                        msgerro = "Atenção, a pasta não pode ser deletada por conter pastas/processos nela. Favor deletar os registros filhos desta pasta."
                    };

                    return Json(erro);
                }
                else
                {
                    throw ex;
                }
            }
            return Json(ret);
        }

        [ErrorHandler]
        [HttpPost]
        public JsonResult ApagarProcesso(string id)
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

            try
            {
                ret = dalprocesso.Delete(new Processos()
                    {
                        Id = int.Parse(id)
                    });
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf("Cannot delete or update a parent row") > -1)
                {
                    var erro = new
                    {
                        msg = "Atenção: O registro não pode ser deletado por conter registros filhos atrelados."
                    };
                    return Json(erro);
                }
            }

            return Json(ret);
        }

        [ErrorHandler]
        [HttpPost]
        public JsonResult GravarProcesso(string NmProcesso, string DsProcesso, string idpasta, string idprocesso)
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

            var id = Convert.ToInt32(Session["iddepto"].ToString());

            Processos proc = new Processos()
            {
                IdPai = int.Parse(idpasta),
                NomeProcesso = NmProcesso,
                Descricao = DsProcesso,
                CdEntidade = entidade_.Codigo,
                Owner = entidade_
            };

            if (idprocesso == "")
            {
                proc.Id = dalprocesso.Save(proc);
            }
            else if (Convert.ToInt32(idprocesso) > 0)
            {
                proc.Id = Convert.ToInt32(idprocesso);
                dalprocesso.Update(proc);
            }
            return Json(proc);
        }

        public ActionResult MenuNavegar()
        {
            return View();
        }



        [HttpPost]
        public ActionResult Upload(FormCollection fc, HttpPostedFileBase upload, HttpPostedFileBase upload2, HttpPostedFileBase upload3, HttpPostedFileBase upload4,
            string idprocesso, string uptext1, string uptext2, string uptext3, string uptext4)
        {
            List<HttpPostedFileBase> controls = new List<HttpPostedFileBase>() { upload, upload2, upload3, upload4 };
            Entidade entidade = Session["entidade"] as Entidade;
            
            StatusOperacao so = null;
            int count = 0;
            foreach (var item in controls)
            {
                string filepath = "C:\\gedrepositorio\\";

                if (item != null)
                {
                    try
                    {
                        Repositorio repositorio = new Repositorio();
                        repositorio.CdProcesso = int.Parse(idprocesso);
                        repositorio.CdEntidade = entidade.Codigo;

                        if (item.FileName.Split('\\').Length > 0)
                        {
                            repositorio.NomeArquivo = item.FileName.Split('\\')[item.FileName.Split('\\').Length - 1].ToLower();
                        }
                        else
                        {
                            repositorio.NomeArquivo = item.FileName.ToLower();
                        }

                        //repositorio.Caminho = filepath + repositorio.NomeArquivo.ToLower();

                        if (count == 0)
                        {
                            repositorio.Caminho = uptext1;
                        }
                        if (count == 1)
                        {
                            repositorio.Caminho = uptext2;
                        }
                        if (count == 2)
                        {
                            repositorio.Caminho = uptext3;
                        }
                        if (count == 3)
                        {
                            repositorio.Caminho = uptext4;
                        }
                        count++;


                        repositorio.Tamanho = (int)item.InputStream.Length;
                        repositorio.Extensao = item.FileName.Split('.')[1].ToLower();

                        so = BLLRepositorio.SalvarArquivoValidando(repositorio);
                        if (so.Arquivo == null)
                        {
                            throw new Exception(so.Msg);
                        }
                        repositorio.CdRepositorio = so.Arquivo.CdRepositorio;

                        repositorio.NomeFisicoArquivo = (filepath.Trim() + repositorio.CdRepositorio.ToString().Trim() + idprocesso.Trim() + "." + repositorio.Extensao.Trim()).Trim();

                        if (so.Msg == null)
                        {
                            item.SaveAs(repositorio.NomeFisicoArquivo);
                        }
                        else
                        {
                            return RedirectToAction("GerenciarProcessos", "pastadepto", new { idprocesso = idprocesso, erro = so.Msg });
                        }
                    }
                    catch (Exception e)
                    {
                        return RedirectToAction("GerenciarProcessos", "pastadepto", new { idprocesso = idprocesso, erro = e.Message });
                    }
                }
            }
            return RedirectToAction("GerenciarProcessos", "pastadepto", new { idprocesso = idprocesso });
        }

        public ActionResult PesquisarRegistros()
        {            
            return View();
        }

        [HttpPost]
        public ActionResult PesquisarRegistros(string iduser, string key)
        {   
            var ret = BLLRepositorio.RetornarListaPorDeptoUsuario(iduser, key);
            if (ret.Count() == 0)
            {
                key = key.ToLower();
                var retpastas = BLLPasta.RetornarPastas().Where(p => p.NmPasta.ToLower().IndexOf(key) > -1
                    || p.DsPasta.ToLower().IndexOf(key) > -1
                    || p.Owner.NmEntidade.ToLower().IndexOf(key) > -1);
                return View("PesquisarPastas", retpastas);
            }
            return View(ret);
        }

        public ActionResult PesquisarPastas()
        {   
            return View();
        }

        [HttpPost]
        public ActionResult PesquisarPastas(string key)
        {
            key = key.ToLower();            
         
            var ret = BLLPasta.RetornarPastas().Where(p => p.NmPasta.ToLower().IndexOf(key) > -1 || p.DsPasta.ToLower().IndexOf(key) > -1 || p.Owner.NmEntidade.IndexOf(key) > -1);
            return View(ret);
        }


        public ActionResult DeletarArquivo(string id, string idprocesso, string nome)
        {
            try
            {
                if (!BLLRepositorio.DeletarArquivo(Convert.ToInt32(id), nome))
                {
                    return RedirectToAction("GerenciarProcessos", new { idprocesso = idprocesso, erro = "Não foi possível deletar o registro, contate o administrador do sistema!" });
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("GerenciarProcessos", new { idprocesso = idprocesso, erro = ex.Message });
            }

            return RedirectToAction("GerenciarProcessos", new { idprocesso = idprocesso });
        }
        public FileResult Download(string arquivo, string extensao, string nome)
        {
            string nomeArquivo = "c:\\gedrepositorio\\" + nome;

            string contentType = Utilities.Util.GetMimeType(nome.Split('.')[1]);

            return File(nomeArquivo, contentType, arquivo);
        }

        private string GetMimeType(string arquivo)
        {
            string ret = "";

            switch (arquivo)
            {
                case "txt":
                    ret = "application/txt";
                    break;
                case "doc":
                    ret = "application/msword";
                    break;
                case "pdf":
                    ret = "application/pdf";
                    break;
            }

            return ret;
        }



    }

    public class Arquivos
    {
        public int arquivoID { get; set; }
        public string arquivoNome { get; set; }
        public string arquivoCaminho { get; set; }
    }
}
