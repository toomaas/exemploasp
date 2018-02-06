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
        MuseuInteractDb dbMuseu = new MuseuInteractDb();

		// GET: Marcacao
        //apresenta todas as marcações
		[Authorize(Users = "Administrador")]
		public ActionResult Index()
        {
			var marcacoes = db.Marcacao.Include(a => a.UserAccount).Include(e => e.Exposicao);
            return View(marcacoes.ToList());
        }

		// GET: Marcacao/Create
        //manda para a view os dados das exposições existentes para que seja possivel ligar a marcação a uma exposição
	    [Authorize(Users = "Administrador")]
		public ActionResult Create()
        {
	        ExposicaoDropdownList();
            return View();
        }

        //envia para a viewbag todos os utilizadores que são capazes de fazer a exposição da marcação e que têem disponibilidade para tal
        private void UserAccountDropdownListMarcacao(int exposicaoID,int marcacaoID, UserAccount userAccount = null)
        {
            IOrderedQueryable<UserAccount> users = dbMuseu.UtilizadoresMarcacao(exposicaoID, marcacaoID);
            List<UserAccount> usersAUsar = new List<UserAccount>();
            Marcacao marcacaoAtual = db.Marcacao.Find(marcacaoID);
            foreach (var user in users)
            {
                List<Marcacao> marcacoesUser = db.Marcacao.Where(m => m.UserAccountID == user.UserAccountID).ToList();
                List<bool> marcacaoSobreposto = new List<bool>();
                foreach (var marcacao in marcacoesUser)
                {
                    if (marcacao.MarcacaoID != marcacaoAtual.MarcacaoID)
                    {
                        DateTime dataInicioMarcacaoAtual = new DateTime(marcacaoAtual.Data.Year,
                            marcacaoAtual.Data.Month, marcacaoAtual.Data.Day, marcacaoAtual.HoraDeInicio.Hour,
                            marcacaoAtual.HoraDeInicio.Minute, marcacaoAtual.HoraDeInicio.Second);
                        DateTime dataFinalMarcacaoAtual = new DateTime(marcacaoAtual.Data.Year,
                            marcacaoAtual.Data.Month, marcacaoAtual.Data.Day, marcacaoAtual.HoraDeFim.Hour,
                            marcacaoAtual.HoraDeFim.Minute, marcacaoAtual.HoraDeFim.Second);

                        DateTime dataInicioMarcacao = new DateTime(marcacao.Data.Year, marcacao.Data.Month,
                            marcacao.Data.Day, marcacao.HoraDeInicio.Hour, marcacao.HoraDeInicio.Minute,
                            marcacao.HoraDeInicio.Second);
                        DateTime dataFinalMarcacao = new DateTime(marcacao.Data.Year, marcacao.Data.Month,
                            marcacao.Data.Day, marcacao.HoraDeFim.Hour, marcacao.HoraDeFim.Minute,
                            marcacao.HoraDeFim.Second);

                        bool verificacao = dataInicioMarcacao < dataInicioMarcacaoAtual && dataFinalMarcacao < dataInicioMarcacaoAtual || dataInicioMarcacao > dataFinalMarcacaoAtual && dataFinalMarcacao > dataFinalMarcacaoAtual;
                        marcacaoSobreposto.Add(verificacao);
                    }
                }
                //se existe um false, isto significa que já há uma marcação sobreposta para aquele dia e hora, logo este utilizador não pode fazer a visita guiada da marcação em questão
                if (!marcacaoSobreposto.Contains(false))
                {
                    usersAUsar.Add(user);
                }
            }
            if (userAccount == null)
            {
                UserAccount userAccountEmpty = new UserAccount();
                

                ViewBag.UserAccountID = new SelectList(usersAUsar, "UserAccountID", "Nome", userAccountEmpty.UserAccountID);
                return;
            }
            ViewBag.UserAccountID = new SelectList(usersAUsar, "UserAccountID", "Nome", userAccount.UserAccountID);
        }

        //envia para a view todas as exposições
        private void ExposicaoDropdownList(object Exposicao = null)
		{
            ViewBag.ExposicaoID = new SelectList(dbMuseu.ListaExposicao(), "ExposicaoID", "Nome", Exposicao);
	    }

		// POST: Marcacao/Create
        //valida se os dados introduzidos são válidos e de seguida guarda em bd.marcacao a nova marcação (ainda sem guia escolhido)
	    [Authorize(Users = "Administrador")]
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
                    ExposicaoDropdownList();
                    return View();
                }
                ModelState.Clear();
            }
            return RedirectToAction("Index");
        }

		// GET: Marcacao/Edit/5
        //manda para a view os dados necessários para poder editar a marcação escolhida
	    [Authorize(Users = "Administrador")]
		public ActionResult Edit(int id)
        {
            Marcacao marcacao = db.Marcacao.SingleOrDefault(m => m.MarcacaoID == id);
            if(marcacao == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            UserAccountDropdownListMarcacao(marcacao.ExposicaoID, id, marcacao.UserAccount);
            return View(marcacao);
        }

		// POST: Marcacao/Edit/5
        //valida os dados editados e atualiza na base de dados a marcação selecionada
	    [Authorize(Users = "Administrador")]
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
        //remove de bd.marcacao a marcação escolhida
	    [Authorize(Users = "Administrador")]
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

        //manda para a view todas as marcações que já terminaram
        public ActionResult Arquivo()
        {
            List<Marcacao> marcacaos = db.Marcacao.Where(m => m.Data < DateTime.Now).ToList();
            return View(marcacaos);
        }
    }
}
