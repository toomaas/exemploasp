﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using exemploasp.Models;
using System.Data.Entity;
using System.Web.Services.Description;

namespace exemploasp.Controllers
{
    public class TemaController : Controller
    {
        private OurDBContext db = new OurDBContext();
		// GET: Tema

		public ActionResult Index()
        {
            using (OurDBContext db = new OurDBContext())
            {
                if (TempData["Message"] != null) ViewBag.Message = TempData["Message"].ToString();
                return View(db.Tema.ToList());
            }
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
	                if (!db.Tema.Any(n => n.Nome == tema.Nome))
	                {
						db.Tema.Add(tema);
		                db.SaveChanges();

	                    TempData["Message"] = tema.Nome + " criado com sucesso";
	                }
	                else
	                {
	                    TempData["Message"] = tema.Nome + " já existente";
                    }
;
                }
                ModelState.Clear();


	            return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

		// GET: Tema/Delete/5

		public ActionResult Delete(int? id)
          {
              var temaToDelete = db.Tema.SingleOrDefault(t => t.TemaID == id);
              if (temaToDelete != null)
              {
                  db.Tema.Remove(temaToDelete);
                  db.SaveChanges();
                  TempData["Message"] = temaToDelete.Nome + " removido com sucesso";
              }
              else
              {
                  TempData["Message"] = "erro ao remover o tema";
              }
              return RedirectToAction("Index");
        }
    }
}
