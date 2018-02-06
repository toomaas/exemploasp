using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace exemploasp.ViewModels
{
    //viewmodel DataExposicao que leva dados dos modelos desde o controller para a view
    public class DataExposicao
    {
        public DateTime Data { get; set; }
        public string DiaSemana { get; set; }
    }
}