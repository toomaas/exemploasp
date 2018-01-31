using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using exemploasp.Models;

namespace exemploasp.Patterns.TemplateMethod
{
    class ObjExposicao : ObjetoMuseu
    {
        private Exposicao exposicao { get; set; }

        public override string Validar()
        {
            if (exposicao.DataInicial >= DateTime.Now && exposicao.DataInicial < exposicao.DataFinal)
            {
                return null;
            }
            return "Datas inválidas";
        }

        public override void SalvarBd(OurDBContext db)
        {
            db.Exposicao.Add(exposicao);
            db.SaveChanges();
        }

        public ObjExposicao(object objeto)
        {
            exposicao = (Exposicao)objeto;
        }
    }
}