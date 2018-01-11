using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using exemploasp.Models;

namespace exemploasp.Controllers
{
    public class TemaController : Controller
    {
        // GET: Tema
        public ActionResult Index()
        {
            using (OurDBContext db = new OurDBContext())
            {
                return View(db.Tema.ToList());
            }
        }

        // GET: Tema/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Tema/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tema/Create
        [HttpPost]
        public ActionResult Create(Tema tema)
        {
            try
            {
                // TODO: Add insert logic here
                using (OurDBContext db = new OurDBContext())
                {
                    db.Tema.Add(tema);
                    db.SaveChanges();
                }
                ModelState.Clear();
                ViewBag.Message = tema.Nome + "criado com sucesso";

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Tema/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Tema/Edit/5
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

        // GET: Tema/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Tema/Delete/5
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
