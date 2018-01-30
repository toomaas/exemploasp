using System;
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
				if (TempData["Message"] != null) ViewBag.Message = TempData["Message"].ToString();
				return View(db.TipoUtilizador.ToList());
			}
		}


		// GET: TipoUtilizador/Create
		[Authorize]
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

		// GET: TipoUtilizador/Delete/5

		public ActionResult Delete(int? id)
        {

	        TipoUtilizador tipoUtilizador = db.TipoUtilizador.Find(id);
			if (tipoUtilizador != null)
	        {
				db.TipoUtilizador.Remove(tipoUtilizador);
				db.SaveChanges();
		        TempData["Message"] = tipoUtilizador.Tipo + " removido com sucesso";
			}
			else
			{
				TempData["Message"] = "erro ao remover o tipo de utilizador";
			}

	        return RedirectToAction("Index");
        }

    }
}
