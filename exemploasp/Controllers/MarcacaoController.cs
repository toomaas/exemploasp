using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using exemploasp.Models;

namespace exemploasp.Controllers
{
    public class MarcacaoController : Controller
    {
        // GET: Marcacao
        public ActionResult Index()
        {
            using (OurDBContext db = new OurDBContext())
            {
                return View(db.Marcacao.ToList());
            }
          
        }

        // GET: Marcacao/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Marcacao/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Marcacao/Create
        [HttpPost]
        public ActionResult Create(Marcacao marcacao)
        {
            if (ModelState.IsValid)
            {
                using (OurDBContext db = new OurDBContext())
                {
                    db.Marcacao.Add(marcacao);
                    db.SaveChanges();
                }
                ModelState.Clear();
                ViewBag.Message = "Marcação de "+marcacao.NomeRequerente + " criada com sucesso!";
            }

            return RedirectToAction("Index");
        }

        // GET: Marcacao/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Marcacao/Edit/5
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

        // GET: Marcacao/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Marcacao/Delete/5
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
