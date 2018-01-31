using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Security.Permissions;
using System.Web.Mvc;
using exemploasp.InteractDB;
using exemploasp.Models;
using exemploasp.Patterns.TemplateMethod;
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

        private void UserAccountDropdownListMarcacao(int exposicaoID,int marcacaoID, UserAccount userAccount = null)
        {
            if (userAccount == null)
            {
                UserAccount userAccountEmpty = new UserAccount();
                ViewBag.UserAccountID = new SelectList(dbMuseu.UtilizadoresMarcacao(exposicaoID, marcacaoID), "UserAccountID", "Nome", userAccountEmpty.UserAccountID);
                return;
            }
            ViewBag.UserAccountID = new SelectList(dbMuseu.UtilizadoresMarcacao(exposicaoID, marcacaoID), "UserAccountID", "Nome", userAccount.UserAccountID);
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
                ObjetoMuseu oMarcacao = new ObjMarcacao(marcacao);
                if (oMarcacao.Validar() == null)
                {
                    oMarcacao.SalvarBd(db);
                    ViewBag.Message = "Marcação de " + marcacao.NomeRequerente + " criada com sucesso!";
                }
                else
                {
                    var exp = db.Exposicao.FirstOrDefault(e => e.ExposicaoID == marcacao.ExposicaoID);
                    if(exp != null)
                        ModelState.AddModelError("Data", oMarcacao.Validar());
                    UserAccountDropdownList();
                    ExposicaoDropdownList();
                    return View();
                }
                ModelState.Clear();
            }
            return RedirectToAction("Index");
        }

		// GET: Marcacao/Edit/5
		public ActionResult Edit(int id)
        {
            Marcacao marcacao = db.Marcacao.SingleOrDefault(m => m.MarcacaoID == id);
            if(marcacao == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            UserAccountDropdownListMarcacao(marcacao.ExposicaoID, id, marcacao.UserAccount);
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
                    return RedirectToAction("Edit", "Marcacao", new {id = marcacao.MarcacaoID});
                }
                    ModelState.AddModelError("Data", "Esta Exposição occore de " + exposicao.DataInicial.ToShortDateString() + " a " + exposicao.DataFinal.ToShortDateString());
            }
            UserAccountDropdownListMarcacao(marcacao.ExposicaoID, marcacao.MarcacaoID, marcacao.UserAccount);
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

        public ActionResult Arquivo()
        {
            List<Marcacao> marcacaos = db.Marcacao.Where(m => m.Data < DateTime.Now).ToList();
            return View(marcacaos);
        }
    }
}
