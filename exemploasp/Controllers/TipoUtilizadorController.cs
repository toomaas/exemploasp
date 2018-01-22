﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using exemploasp.Models;

namespace exemploasp.Controllers
{
	public class TipoUtilizadorController : Controller
	{

		OurDBContext db = new OurDBContext();

		// GET: TipoUtilizador
		public ActionResult Index()
		{
			using (OurDBContext db = new OurDBContext())
			{
				return View(db.TipoUtilizador.ToList());
			}
		}


		// GET: TipoUtilizador/Create
		public ActionResult Create()
		{
			
			return View();
		}



		// POST: TipoUtilizador/Create
		[HttpPost]
		public ActionResult Create(TipoUtilizador tipoUtilizador)
		{
			try
			{
				if (!db.TipoUtilizador.Any(n => n.Tipo == tipoUtilizador.Tipo))
				{
						db.TipoUtilizador.Add(tipoUtilizador);
						db.SaveChanges();
				}
					
				ModelState.Clear();


				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}


// GET: TipoUtilizador/Edit/5
        public ActionResult Funcao()
        {

			UserAccountDropdownList();
	        TipoUtilizadorDropdownList();
			var utilizadores = db.TipoUtilizador.Include(u=>u.UserAccounts);
		        return View(utilizadores.ToList());
	        
		}

		private void UserAccountDropdownList(object userAccount = null)
		{

			var utilizadoresQuery = from u in db.UserAccount
				where u.TipoUtilizadorID ==1
				orderby u.Nome
				select u;
			ViewBag.UserAccountID = new SelectList(utilizadoresQuery, "UserAccountID", "Nome", userAccount);
		}

		private void TipoUtilizadorDropdownList(object tipoUtilizador = null)
		{

			var TiposUtilizadoresQuery = from u in db.TipoUtilizador
			    where u.TipoUtilizadorID !=1
				orderby u.Tipo
				select u;
			ViewBag.TipoUtilizadorID = new SelectList(TiposUtilizadoresQuery, "TipoUtilizadorID", "Tipo", tipoUtilizador);
		}

		// POST: TipoUtilizador/Edit/5
		[HttpPost]
        public ActionResult Funcao(int UserAccountID, int TipoUtilizadorID)
        {
            try
            {       



                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


		// GET: TipoUtilizador/Delete/5

		public ActionResult Delete(int id)
        {

	        TipoUtilizador tipoUtilizador = db.TipoUtilizador.Find(id);

	        if (tipoUtilizador == null)
	        {
		        return HttpNotFound();
	        }

	        return View(tipoUtilizador);
        }

        // POST: TipoUtilizador/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
		        TipoUtilizador tipo = db.TipoUtilizador.Find(id);
		        db.TipoUtilizador.Remove(tipo);
				db.SaveChanges();

			return RedirectToAction("Index");
		}
    }
}
