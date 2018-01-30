using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Runtime.ExceptionServices;

namespace exemploasp.Models
{
    public class Disponibilidade
    {

        [Key]
        public int DisponibilidadeID { get; set; }

        [Display(Name = "Data")]
        [Required (ErrorMessage = "Dia Disponível")]
        [DataType(DataType.Date)]
        public DateTime DataDisponibilidade { get; set; }

        public int ExposicaoID { get; set; }

        public int UserAccountID { get; set; }

        public virtual Exposicao Exposicao { get; set; }
        public virtual UserAccount UserAccount { get; set; } 
    }
}