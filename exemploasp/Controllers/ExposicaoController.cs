using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using exemploasp.Models;
using exemploasp.ViewModels;

namespace exemploasp.Controllers
{
    public class ExposicaoController : Controller
    {
        OurDBContext db = new OurDBContext();

        // GET: Exposicao
        public ActionResult Index()
        {
	        using (OurDBContext db = new OurDBContext())
	        {
		        return View(db.Exposicao.ToList());
	        }
        }

        // GET: Exposicao/Details/5
        public ActionResult Details(int id)
        {
            return View();
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


               // return View();
            }

        // GET: Exposicao/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Exposicao/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Exposicao/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Exposicao/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
