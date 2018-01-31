using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using exemploasp.Models;

namespace exemploasp.Patterns.TemplateMethod
{
    abstract class ObjetoMuseu
    {
        public abstract string Validar();

        public abstract void SalvarBd(OurDBContext db);
    }

}