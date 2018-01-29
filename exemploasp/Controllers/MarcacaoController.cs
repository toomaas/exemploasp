using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Web.Mvc;
using exemploasp.InteractDB;
using exemploasp.Models;
using Microsoft.Ajax.Utilities;

namespace exemploasp.Controllers
{
    public class MarcacaoController : Controller
    {
		OurDBContext db = new OurDBContext();
        MuseuInteractDB dbMuseu = new MuseuInteractDB();
        // GET: Marcacao
        public ActionResult Index()
        {
			var marcacoes = db.Marcacao.Include(a => a.UserAccount).Include(e => e.Exposicao);
            return View(marcacoes.ToList());
        }

        // GET: Marcacao/Create
        public ActionResult Create()
        {
	        ExposicaoDropdownList();
            return View();
        }

		private void UserAccountDropdownList(object userAccount = null)
		{
			ViewBag.UserAccountID = new SelectList(dbMuseu.Utilizadores(), "UserAccountID", "Nome", userAccount);
		}

		private void ExposicaoDropdownList(object Exposicao = null)
		{
            ViewBag.ExposicaoID = new SelectList(dbMuseu.ListaExposicao(), "ExposicaoID", "Nome", Exposicao);
	    }

		// POST: Marcacao/Create
		[HttpPost]
        public ActionResult Create([Bind(Include = "MarcacaoID, NomeRequerente, Idade, NumTelefoneRequerente, Data,HoraDeInicio,HoraDeFim,NumPessoas,ExposicaoID, UserAccountID")]Marcacao marcacao)
        {
            if (ModelState.IsValid)
            {
                    DateTime hoje = DateTime.Now;
                    var exp = db.Exposicao.FirstOrDefault(e => e.ExposicaoID == marcacao.ExposicaoID);
                    if (dbMuseu.DataExposicaoMarcacao(marcacao.Data, exp.DataInicial, exp.DataFinal))
                    {
                        TimeSpan dur = TimeSpan.Parse(exp.Duracao.Hour+":"+exp.Duracao.Minute);
                        marcacao.HoraDeFim = marcacao.HoraDeInicio.Add(dur);                       
                        db.Marcacao.Add(marcacao);
                        db.SaveChanges();
                        ViewBag.Message = "Marcação de " + marcacao.NomeRequerente + " criada com sucesso!";
                    }
                    else
                    {
                        ModelState.AddModelError("Data", "Esta Exposição occore de "+exp.DataInicial.ToShortDateString()+" a "+exp.DataFinal.ToShortDateString());
                        UserAccountDropdownList();
                        ExposicaoDropdownList();
                        return View();
                    }  
            }
            return RedirectToAction("Index");
        }

        // GET: Marcacao/Edit/5
        public ActionResult Edit(int id)
        {
            Marcacao marcacao = db.Marcacao.Find(id);
            if(marcacao == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            UserAccountDropdownList();
            return View(marcacao);
        }

        // POST: Marcacao/Edit/5
        [HttpPost]
        public ActionResult Edit([Bind(Include="MarcacaoID,NomeRequerente,Idade,NumTelefoneRequerente,Data,HoraDeInicio,HoraDeFim,NumPessoas,ExposicaoID,UserAccountID")] Marcacao marcacao)
        {
            if (ModelState.IsValid)
            {
                Exposicao exposicao = db.Exposicao.Find(marcacao.ExposicaoID);
                if (dbMuseu.DataExposicaoMarcacao(marcacao.Data, exposicao.DataInicial,exposicao.DataFinal))
                {
                    TimeSpan dur = TimeSpan.Parse(exposicao.Duracao.Hour + ":" + exposicao.Duracao.Minute);
                    marcacao.HoraDeFim = marcacao.HoraDeInicio.Add(dur);
                    db.Entry(marcacao).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                    ModelState.AddModelError("Data", "Esta Exposição occore de " + exposicao.DataInicial.ToShortDateString() + " a " + exposicao.DataFinal.ToShortDateString());
                    UserAccountDropdownList();
                    return View(marcacao);
            }
            UserAccountDropdownList();
            return View(marcacao);
        }

        // GET: Marcacao/Delete/5
        public ActionResult Delete(int id)
        {
            var marcacaoToDelete = db.Marcacao.SingleOrDefault(m => m.MarcacaoID == id);
            if (marcacaoToDelete != null)
            {
                db.Marcacao.Remove(marcacaoToDelete);
                db.SaveChanges();
                TempData["Message"] = "Marcação "+marcacaoToDelete.MarcacaoID + " removida com sucesso";
            }
            else
            {
                TempData["Message"] = "Erro ao remover Marcação";
            }
            return RedirectToAction("Index");
        }
    }
}
