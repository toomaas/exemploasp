using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using exemploasp.Models;

namespace exemploasp.Controllers
{
    public class ExposicaoController : Controller
    {
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
            return View();
        }

        // POST: Exposicao/Create
        [HttpPost]
        public ActionResult Create(Exposicao exposicao)
        {
  
	            if (ModelState.IsValid)
	            {
		            using (OurDBContext db = new OurDBContext())
		            {
			            db.Exposicao.Add(exposicao);
			            db.SaveChanges();
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
