using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using exemploasp.Models;

namespace exemploasp.Patterns.TemplateMethod
{
    class ObjTema : ObjetoMuseu
    {
        private Tema tema { get; set; }
        
        //verifica se o tema é válido
        public override string Validar()
        {
            OurDBContext db = new OurDBContext();
            //verifica se já existe algum tema
            if (!db.Tema.Any(n => n.Nome == tema.Nome))
            {
                return null;
            }
            return tema.Nome + " já existente";
        }

        //guarda na bd o novo tema
        public override void SalvarBd(OurDBContext db)
        {
            db.Tema.Add(tema);
            db.SaveChanges();
        }

        public ObjTema(object objeto)
        {
            tema = (Tema)objeto;
        }
    }
}