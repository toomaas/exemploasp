using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Data.Entity.Infrastructure;
using System.Net;
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
                var marcacoes = db.Marcacao.Include(a => a.UserAccount).Include(e => e.Exposicao);
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
	        //UserAccountDropdownList();
	        ExposicaoDropdownList();

			return View();
        }

	    private void UserAccountDropdownList(object userAccount = null)
	    {

			    var utilizadoresQuery = from u in db.UserAccount
				    orderby u.Nome
				    select u;

			    ViewBag.UserAccountID = new SelectList(utilizadoresQuery, "UserAccountID", "Nome", userAccount);
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
        public ActionResult Create([Bind(Include = "MarcacaoID, NomeRequerente, Idade, NumTelefoneRequerente, Data,HoraDeInicio,HoraDeFim,NumPessoas,ExposicaoID, UserAccountID")]Marcacao marcacao)
        {
            if (ModelState.IsValid)
            {
                using (OurDBContext db = new OurDBContext())
                {
                    DateTime hoje = DateTime.Now;
                    var exp = db.Exposicao.FirstOrDefault(e => e.ExposicaoID == marcacao.ExposicaoID);
                    if (marcacao.Data >= exp.DataInicial && marcacao.Data < exp.DataFinal)
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
                //ModelState.Clear();
                
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
                db.Entry(marcacao).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
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
