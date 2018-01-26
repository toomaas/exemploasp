﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using exemploasp.Models;
using exemploasp.Patterns;

namespace exemploasp.Controllers
{
    public class UserAccountExposicaoController : Controller
    {
        OurDBContext db = new OurDBContext();

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
                    exposicoesAUsar.Add(exp);
                }
            }
            List<Exposicao> newList = new List<Exposicao>();
            foreach (var member in exposicoesAUsar)
                newList.Add(new Exposicao
                {
                    ExposicaoID = member.ExposicaoID,
                    Nome = member.Nome + " de " + member.DataInicial.ToShortDateString() + " a " + member.DataFinal.ToShortDateString() + " DUR: " + member.Duracao.ToShortTimeString()
                });
            return newList;
        }

        //GET: UserAccountExposicao/Index/id
        public ActionResult Index(int id)
        {
            List<Exposicao> listaExposicoes = ExposicoesUtilizador(id);
            ViewBag.ExposicaoID = new SelectList(listaExposicoes, "ExposicaoID", "Nome");
            ViewBag.UserID = id.ToString();
            return View(db.UserAccountExposicao.Where(u => u.UserAccountID == id)/*.Where(u => u.Assigned == 2)*/.Include(u => u.Exposicao).Include(u => u.UserAccount).ToList());
        }

        // POST: UserAccountExposicao/Index
        [HttpPost]
        public ActionResult Index(string UserID, string ExposicaoID)
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
            List<Exposicao> listaExposicoes = ExposicoesUtilizador(Int32.Parse(UserID));
            ViewBag.ExposicaoID = new SelectList(listaExposicoes, "ExposicaoID", "Nome");
            ViewBag.UserID = UserID;
            int id = Int32.Parse(UserID);
            return View(db.UserAccountExposicao.Where(u => u.UserAccountID == id).Where(u => u.Assigned == 2).Include(u => u.Exposicao).Include(u => u.UserAccount).ToList());
        }
        /* POST: UserAccountExposicao/Index/..extrainfo
        [HttpPost]
        public ActionResult Index(string UserID, string ExposicaoID, string InformacaoExtra)
        {
           // UserAccountExposicao userAccountExposicao = db.UserAccountExposicao.Find(UserID,ExposicaoID);
            List<Exposicao> listaExposicoes = ExposicoesUtilizador(Int32.Parse(UserID));
            ViewBag.ExposicaoID = new SelectList(listaExposicoes, "ExposicaoID", "Nome");
            ViewBag.UserID = UserID;
            int id = Int32.Parse(UserID);
            return View(db.UserAccountExposicao.Where(u => u.UserAccountID == id).Where(u => u.Assigned == 2).Include(u => u.Exposicao).Include(u => u.UserAccount).ToList());
        }*/
    }
}