using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace exemploasp.ViewModels
{
    //viewmodel assignedtemadata que leva dados dos modelos desde o controller para a view
    public class AssignedTemaData
    {
        public int TemaID { get; set; }
        public string Nome { get; set; }
        public bool Assigned { get; set; }
    }
}