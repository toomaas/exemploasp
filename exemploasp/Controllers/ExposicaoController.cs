﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Resources;
using System.Security.Cryptography.Pkcs;
using System.Web;
using System.Web.Mvc;
using exemploasp.InteractDB;
using exemploasp.Models;
using exemploasp.Patterns;
using exemploasp.Patterns.TemplateMethod;
using exemploasp.ViewModels;
using Exposicao = exemploasp.Models.Exposicao;

namespace exemploasp.Controllers
{
    public class ExposicaoController : Controller
    {
        OurDBContext db = new OurDBContext();
		MuseuInteractDb museuDB = new MuseuInteractDb();

        // GET: Exposicao
        //Lista de todas as exposições
        [Authorize(Users = "Administrador")]
		public ActionResult Index()
        {
	            if (TempData["Message"] != null) ViewBag.Message = TempData["Message"].ToString();
                return View(db.Exposicao.ToList());
        }

		//Lista exposições de acordo com o user
	    [Authorize]
		public ActionResult Lista()
        {
            int id = Convert.ToInt32(Session["UserAccountID"]);
            UserAccount userAccount = db.UserAccount.Where(u => u.UserAccountID == id).Include(u => u.Temas).Include(u => u.UserAccountExposicaos).SingleOrDefault();
            ViewBag.UserAccount = userAccount;
            return View(db.Exposicao.Include(e => e.Temas).OrderByDescending(e => e.ExposicaoID).ToList());
        }

		// GET: Exposicao/Create
	    [Authorize(Users = "Administrador")]
		public ActionResult Create()
        {
            var exposicao = new Exposicao();
            exposicao.Temas = new List<Tema>();
            ViewBag.Temas = museuDB.PopulateAssignedTemaData(exposicao);
            return View();
        }

		// POST: Exposicao/Create
        //verifica se os dados introduzidos são válidos e de seguida salva em bd.exposicao a nova exposição.É utilizado o template method
		[Authorize(Users = "Administrador")]
		[HttpPost]
        public ActionResult Create([Bind(Include = "Nome,DataInicial,DataFinal,Duracao,NrItens")]Exposicao exposicao, string[] selectedTemas)
        {
	        if (ModelState.IsValid)
	        {
                ObjetoMuseu oExposicao = new ObjExposicao(exposicao);
	            if (oExposicao.Validar() == null)
	            {
	                oExposicao.SalvarBd(db);
	                var exposicaoUpdate = db.Exposicao.Include(t => t.Temas).Single(e => e.ExposicaoID == exposicao.ExposicaoID);
                    museuDB.UpdateTemas(selectedTemas, exposicaoUpdate, db);
                }
	            else
	            {
	                exposicao.Temas = new List<Tema>();
	                ViewBag.Temas = museuDB.PopulateAssignedTemaData(exposicao);
	                ModelState.AddModelError("", oExposicao.Validar());
	                return View();

                }
	            ModelState.Clear();
	            TempData["Message"] = exposicao.Nome + " adicionado com sucesso"; 
            }
            return RedirectToAction("Index");
        }

        //manda para a view os dados necessários para editar a exposição selecionada
		[Authorize(Users = "Administrador")]
		public ActionResult Edit(int? id)
	    {
		    Exposicao exposicao = db.Exposicao.Include(t => t.Temas).SingleOrDefault(u => u.ExposicaoID == id);
		    if (exposicao == null || id == null)
		    {
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
	        ViewBag.Temas = museuDB.PopulateAssignedTemaData(exposicao);
            return View(exposicao);
	    }

        //atualiza os dados da exposição selecionada
		[Authorize(Users = "Administrador")]
		[HttpPost]
		public ActionResult Edit(int? id,string Nome, DateTime DataInicial, DateTime DataFinal, DateTime Duracao, int NrItens, string[] selectedTemas)
	    {
			if (id == null)
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
	        var exposicaoUpdate = db.Exposicao.Include(t => t.Temas).Single(e => e.ExposicaoID == id);
            if (TryUpdateModel(exposicaoUpdate, "",
			    new string[] {"Nome,DataInicial,DataFinal,Duracao,NrItens"}))
		    {
			    try
			    {
			        exposicaoUpdate.Nome = Nome;
			        exposicaoUpdate.DataInicial = DataInicial;
			        exposicaoUpdate.DataFinal = DataFinal;
			        exposicaoUpdate.Duracao = Duracao;
			        exposicaoUpdate.NrItens = NrItens;
				    museuDB.UpdateTemas(selectedTemas, exposicaoUpdate, db);
				    return RedirectToAction("Index");
			    }
			    catch (RetryLimitExceededException /* dex */)
			    {
				    ModelState.AddModelError("", "Nao foi possivel atualizar a exposição");
			    }
			}
            return View(exposicaoUpdate);
	    }

        //remove a exposição selecionada
	    [Authorize(Users="Administrador")]
		public ActionResult Delete(int? id)
	    {
		    var exposicaoToDelete = db.Exposicao.SingleOrDefault(e => e.ExposicaoID == id);
		    if (exposicaoToDelete != null)
		    {
			    db.Exposicao.Remove(exposicaoToDelete);
			    db.SaveChanges();
			    TempData["Message"] = exposicaoToDelete.Nome + " removido com sucesso";
			}
		    else
		    {
			    TempData["Message"] = "Erro ao remover Exposição";
		    }
		    return RedirectToAction("Index");
		}

        //envia para a view todas as exposições que já terminaram
        public ActionResult Arquivo()
        {
            List<Exposicao> exposicoes = db.Exposicao.Where(e => e.DataFinal < DateTime.Today).ToList();
            return View(exposicoes);
        }
    }
}
