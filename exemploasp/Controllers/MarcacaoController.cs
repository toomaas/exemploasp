using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Data.Entity.Infrastructure;
using System.Web.Mvc;
using exemploasp.Models;

namespace exemploasp.Controllers
{
    public class MarcacaoController : Controller
    {
		OurDBContext db = new OurDBContext();

        // GET: Marcacao
        public ActionResult Index()
        {
            using (OurDBContext db = new OurDBContext())
            {
                var marcacoes = db.Marcacao.Include(u => u.UserAccount).Include(e => e.Exposicao);
                return View(marcacoes.ToList());
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
	        UserAccountDropdownList();
	        ExposicaoDropdownList();

			return View();
        }

	    private void UserAccountDropdownList(object userAccount = null)
	    {

			    var utilizadoresQuery = from u in db.userAccount
				    orderby u.Nome
				    select u;

			    ViewBag.UserAccountID = new SelectList(utilizadoresQuery, "UserID", "Nome", userAccount);
	    }

	    private void ExposicaoDropdownList(object Exposicao = null)
	    {

		        var exposicoesQuery = from e in db.Exposicao
			        orderby e.Nome
			        select e;

		        ViewBag.ExposicaoID = new SelectList(exposicoesQuery, "ExposicaoID", "Nome", Exposicao);
	    }




		// POST: Marcacao/Create
		[HttpPost]
        public ActionResult Create([Bind(Include = "MarcacaoID, NomeRequerente, Idade, NumTelefoneRequerente, Data,HoraDeInicio,HoraDeFim,NumPessoas,ExposicaoID, UserAccount_UserID,UserAccountID")]Marcacao marcacao)
        {
            if (ModelState.IsValid)
            {
                using (OurDBContext db = new OurDBContext())
                {
                    db.Marcacao.Add(marcacao);
	                UserAccountDropdownList(marcacao.UserAccountID);

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
