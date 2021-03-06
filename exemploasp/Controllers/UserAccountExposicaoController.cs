﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using exemploasp.Models;
using exemploasp.Patterns;

namespace exemploasp.Controllers
{
    public class UserAccountExposicaoController : Controller
    {
        OurDBContext db = new OurDBContext();

        //retorna uma lista com todas as exposições que o utilizador pode se candidatar. é verificado se ainda nao se candidatou e se domina todos os temas da exposição
        private List<Exposicao> ExposicoesUtilizador(int idUser)
        {
            var user = db.UserAccount.Single(u => u.UserAccountID == idUser);
            var exposicoes = db.Exposicao.ToList();
            List<Exposicao> exposicoesAUsar = new List<Exposicao>();
            foreach (var exp in exposicoes)
            {
                List<bool> temasExp = new List<bool>();
                foreach (var temaExp in exp.Temas)
                {
                    var existeTema = false;
                    foreach (var temaUser in user.Temas)
                    {
                        if (temaExp.TemaID == temaUser.TemaID)
                            existeTema = true;
                    }
                    temasExp.Add(existeTema);
                }
                if (!temasExp.Contains(false))
                {
                    if (db.UserAccountExposicao.Where(u => u.UserAccountID == idUser).Where(u => u.Assigned == 1).SingleOrDefault(u => u.ExposicaoID == exp.ExposicaoID) != null || db.UserAccountExposicao.Where(u => u.UserAccountID == idUser).SingleOrDefault(u => u.ExposicaoID == exp.ExposicaoID) == null)
                    {
                        if (exp.DataFinal >= DateTime.Now)
                        {
                            exposicoesAUsar.Add(exp);
                        }
                        
                    }
 
                }
            }
            List<Exposicao> newList = new List<Exposicao>();
            foreach (var member in exposicoesAUsar)
                newList.Add(new Exposicao
                {
                    ExposicaoID = member.ExposicaoID,
                    Nome = member.Nome,
					DataInicial = member.DataInicial,
					DataFinal = member.DataFinal,
					Duracao = member.Duracao,
					Temas = member.Temas
                });
            return newList;
        }

        //GET: UserAccountExposicao/Candidatura/id
        //manda para a view uma lista de exposições que o utilizador pode candidatar-se e ainda todas as candidaturas ativas (aquelas que ainda não foram aceites ou que já terminaram)
        [Authorize]
        public ActionResult Candidatura()
        {
            int id = Convert.ToInt32(Session["UserAccountID"]);
            List<Exposicao> listaExposicoes = ExposicoesUtilizador(id);
            if (listaExposicoes.Count != 0)
            {
                ViewBag.Exposicoes = listaExposicoes;
            }
            ViewBag.UserID = id.ToString();
            return View(db.UserAccountExposicao.Where(u => u.UserAccountID == id).Where(u => u.Exposicao.DataFinal >= DateTime.Now).Where(u => u.Assigned != 4).Include(u => u.Exposicao).Include(u => u.UserAccount).ToList());
        }

		// POST: UserAccountExposicao/Candidatura
        //submete a candidatura escolhida pelo user. o padrão state trata do resto
		[HttpPost]
        public ActionResult Candidatura(string UserID, string ExposicaoID)
        {
            UserAccountExposicao userAccountExposicao = new UserAccountExposicao();
            if (ExposicaoID != "")
            {
                userAccountExposicao.ExposicaoID = Int32.Parse(ExposicaoID);
                userAccountExposicao.UserAccountID = Int32.Parse(UserID);
                DecisorCandidatura decisorCandidatura = new DecisorCandidatura(userAccountExposicao);
                decisorCandidatura.EstadoActual = decisorCandidatura.BuscarEstadoAtual();
                decisorCandidatura.Submeter();
            }
            return RedirectToAction("Candidatura");
        }

		// POST: UserAccountExposicao/Index/..extrainfo
        //adiciona informação extra à candidatura que necessitava de informação extra.
		[Authorize]
		[HttpPost]
		public ActionResult ExtraInfo(string UserID, int ExposicaoID, string InformacaoExtra)
        {
            int uID = Int32.Parse(UserID);
            
            UserAccountExposicao userAccountExposicaoToUpdate = db.UserAccountExposicao.Find(uID,ExposicaoID);
            if (userAccountExposicaoToUpdate != null)
            {
                userAccountExposicaoToUpdate.InformacaoExtra = InformacaoExtra;
                db.Entry(userAccountExposicaoToUpdate).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Candidatura","UserAccountExposicao");
        }

        //envia para a view todas as candidaturas
	    [Authorize(Users = "Administrador")]
		public ActionResult GestaoCandidaturas()
        {
            return View(db.UserAccountExposicao.Include(u => u.Exposicao).Include(u => u.UserAccount).ToList());
        }

        //envia para o padrao state o evento a efetuar na exposição selecionada(aceitar, rejeitar ou pedir mais info)
	    [Authorize(Users = "Administrador")]
		[HttpPost]
        public ActionResult GestaoCandidaturas(int UserID, int ExposicaoID, string Evento )
        {
            UserAccountExposicao userAccountExposicaoToUpdate = db.UserAccountExposicao.Find(UserID, ExposicaoID);
            if (userAccountExposicaoToUpdate != null)
            {
                DecisorCandidatura decisorCandidatura = new DecisorCandidatura(userAccountExposicaoToUpdate);
                decisorCandidatura.EstadoActual = decisorCandidatura.BuscarEstadoAtual();
                if (Evento == "Aceitar")
                {
                    decisorCandidatura.Aceitar();
                }
                else if (Evento == "Rejeitar")
                {
                    decisorCandidatura.Rejeitar();
                }
                else
                {
                    decisorCandidatura.PedirInformacao();
                }
            }
            return RedirectToAction("GestaoCandidaturas", "UserAccountExposicao");
        }

        //manda para a view todas as candidaturas aceites
	    [Authorize]
		public ActionResult CandidaturasAceites()
        {
            return View(db.UserAccountExposicao.Where(ue => ue.Assigned == 4).Include(u => u.UserAccount).Include(e => e.Exposicao).ToList());
        }

        //manda para a view todas as candidaturas rejeitadas. o utilizador pode voltar a fazer um pedido de candidatura para uma que já tenha sido rejeitada
	    [Authorize]
		public ActionResult CandidaturasRejeitadas()
        {
            return View(db.UserAccountExposicao.Where(ue => ue.Assigned == 1).Include(u => u.UserAccount).Include(e => e.Exposicao).ToList());
        }
    }
}
