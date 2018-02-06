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
        //exposicao é usado para que seja possivel gerir o objeto
        private Exposicao exposicao { get; set; }

        //valida se a exposição em questão é válida
        public override string Validar()
        {
            if (exposicao.DataInicial >= DateTime.Now && exposicao.DataInicial < exposicao.DataFinal)
            {
                return null;
            }
            return "Datas inválidas";
        }

        //guarda na em bd.exposicao a exposicao em questão
        public override void SalvarBd(OurDBContext db)
        {
            db.Exposicao.Add(exposicao);
            db.SaveChanges();
        }

        //construtor para converter o objeto passado num do tipo exposição
        public ObjExposicao(object objeto)
        {
            exposicao = (Exposicao)objeto;
        }
    }
}