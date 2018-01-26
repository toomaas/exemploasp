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
		        return View(db.Exposicao.ToList());
	        }
        }

        // GET: Exposicao/Create
        public ActionResult Create()
        {
            var exposicao = new Exposicao();
            exposicao.Temas = new List<Tema>();
            PopulateAssignedTemaData(exposicao);
            return View();
        }

        private void PopulateAssignedTemaData(Exposicao exposicao)
        {
            var allTemas = db.Tema;
            var exposicaoTemas = new HashSet<int>(exposicao.Temas.Select(t => t.TemaID));
            var viewModel = new List<AssignedTemaData>();
            foreach (var tema in allTemas)
            {
                viewModel.Add(new AssignedTemaData { TemaID = tema.TemaID, Nome = tema.Nome, Assigned = exposicaoTemas.Contains(tema.TemaID) });
            }
            ViewBag.Temas = viewModel;
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
                        PopulateAssignedTemaData(exposicao);
                        ModelState.AddModelError("", "Datas inválidas");
		                return View();
		            }
		        }
				ModelState.Clear();
		        ViewBag.Message = exposicao.Nome+"Criada com sucesso";
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

		    PopulateAssignedTemaData(exposicao);
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

				    UpdateTemas(selectedTemas, exposicaoUpdate);
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


	    public void UpdateTemas(string[] selectedTemas, Exposicao exposicao)
	    {
		    if (selectedTemas == null)
		    {
			    exposicao.Temas = new List<Tema>();
			    return;
		    }
		    var selectedTemasHS = new HashSet<string>(selectedTemas);
		    var userAccountTemas = new HashSet<int>(exposicao.Temas.Select(t => t.TemaID));
		    foreach (var tema in db.Tema)
		    {
			    if (selectedTemasHS.Contains(tema.TemaID.ToString()))
			    {
				    if (!userAccountTemas.Contains(tema.TemaID))
				    {
					    exposicao.Temas.Add(tema);

				    }
			    }
			    else
			    {
				    if (userAccountTemas.Contains(tema.TemaID))
				    {
					    exposicao.Temas.Remove(tema);
				    }
			    }

		    }

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
