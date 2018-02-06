using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using exemploasp.Models;

namespace exemploasp.Patterns.TemplateMethod
{
    abstract class ObjetoMuseu
    {
        //metodos a implementar por todas as classes que implementam ObjetoMuseu

        //verifica se o objeto em questão é válido para ser introduzido na bd
        public abstract string Validar();

        //guarda na BD o objeto em questão no seu lugar correspondente
        public abstract void SalvarBd(OurDBContext db);
    }
}