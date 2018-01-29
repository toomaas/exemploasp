using System;
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
using exemploasp.ViewModels;

namespace exemploasp.Controllers
{
    public class ExposicaoController : Controller
    {
        OurDBContext db = new OurDBContext();
		MuseuInteractDB museuDB = new MuseuInteractDB();
        // GET: Exposicao
        public ActionResult Index()
        {
	        using (OurDBContext db = new OurDBContext())
	        {
	            if (TempData["Message"] != null) ViewBag.Message = TempData["Message"].ToString();
                return View(db.Exposicao.ToList());
	        }
        }

        // GET: Exposicao/Create
        public ActionResult Create()
        {
            var exposicao = new Exposicao();
            exposicao.Temas = new List<Tema>();
            ViewBag.Temas = museuDB.PopulateAssignedTemaData(exposicao);
            return View();
        }

        // POST: Exposicao/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "Nome,DataInicial,DataFinal,Duracao,NrItens")]Exposicao exposicao, string[] selectedTemas)
        {
	        if (ModelState.IsValid)
	        {
		        using (OurDBContext db = new OurDBContext())
		        {
		            if (exposicao.DataInicial >= DateTime.Now && exposicao.DataInicial < exposicao.DataFinal)
		            {
		                if (selectedTemas != null)
		                {
		                    exposicao.Temas = new List<Tema>();
		                    foreach (var tema in selectedTemas)
		                    {
		                        var temaToAdd = db.Tema.Find(int.Parse(tema));
		                        exposicao.Temas.Add(temaToAdd);
		                    }
		                    db.Exposicao.Add(exposicao);
		                    db.SaveChanges();
		                }
		            }
		            else
		            {
		                exposicao.Temas = new List<Tema>();
		                ViewBag.Temas = museuDB.PopulateAssignedTemaData(exposicao);
                        ModelState.AddModelError("", "Datas inválidas");
		                return View();
		            }
		        }
				ModelState.Clear();
	            TempData["Message"] = exposicao.Nome + " adicionado com sucesso";
            }
            return RedirectToAction("Index");
        }

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


	    [HttpPost]
		public ActionResult Edit(int? id, string[] selectedTemas)
	    {
			if (id == null)
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
	        var exposicaoUpdate = db.Exposicao.Include(t => t.Temas).Single(e => e.ExposicaoID == id);
            if (TryUpdateModel(exposicaoUpdate, "",
			    new string[] {"Nome,DataInicial,DataFinal,Duracao,NrItens"}))
		    {
			    try
			    {
				    museuDB.UpdateTemas(selectedTemas, exposicaoUpdate, db);
				    db.Entry(exposicaoUpdate).State = EntityState.Modified;
				    db.SaveChanges();
				    return RedirectToAction("Index");
			    }
			    catch (RetryLimitExceededException /* dex */)
			    {
				    ModelState.AddModelError("", "Nao foi possivel atualizar a exposição");
			    }
			}
            return View(exposicaoUpdate);
	    }

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
    }
}
