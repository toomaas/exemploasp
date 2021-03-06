﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using exemploasp.InteractDB;
using exemploasp.Models;

namespace exemploasp.Patterns.TemplateMethod
{
    class ObjMarcacao : ObjetoMuseu
    {
        private Marcacao marcacao { get; set; }

        private Exposicao exposicao { get; set; }

        //verifica se a marcação é válida
        public override string Validar()
        {
            OurDBContext db = new OurDBContext();
            MuseuInteractDb dbMuseu = new MuseuInteractDb();
            if (exposicao != null)
            {
                //a data escolhida tem de estar entre a data de inicio e de fim da exposicao
                if (dbMuseu.DataExposicaoMarcacao(marcacao.Data, exposicao.DataInicial, exposicao.DataFinal))
                {
                    return null;
                }
                return "Esta Exposição occore de " + exposicao.DataInicial.ToShortDateString() + " a " + exposicao.DataFinal.ToShortDateString();
            }
            return "Exposicao não existente";
        }

        //guarda na bd a marcação em questão
        public override void SalvarBd(OurDBContext db)
        {
            TimeSpan dur = TimeSpan.Parse(exposicao.Duracao.Hour + ":" + exposicao.Duracao.Minute);
            marcacao.HoraDeFim = marcacao.HoraDeInicio.Add(dur);
            db.Marcacao.Add(marcacao);
            db.SaveChanges();
        }

        public ObjMarcacao(object objeto)
        {
            OurDBContext dbC = new OurDBContext();
            marcacao = (Marcacao)objeto;
            exposicao = dbC.Exposicao.FirstOrDefault(e => e.ExposicaoID == marcacao.ExposicaoID);
        }
    }
}