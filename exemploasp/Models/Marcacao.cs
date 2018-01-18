using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Runtime.ExceptionServices;

namespace exemploasp.Models
{
    public class Marcacao
    {
        [Key]
        public int MarcacaoID { get; set; }

        [Required(ErrorMessage = "O nome do requerente é obrigatório")]
        public string NomeRequerente { get; set; }

        [Required(ErrorMessage = "A idade é óbrigatória")]
        public int Idade { get; set; }

        [Display(Name = "Numero de telefone do requerente")]
        [Required(ErrorMessage = "O numero de telefone do requerente é óbrigatório")]
        public int NumTelefoneRequerente { get; set; }

        [Display(Name = "Data da visita")]
        [Required(ErrorMessage = "Introduza uma data para a visita")]
        [DataType(DataType.Date)]
        public DateTime Data { get; set; }

        [Display(Name = "Hora de inicio da visita")]
        [Required(ErrorMessage = "Introduza uma data de fim da exposição")]
        [DataType(DataType.Time)]
        public DateTime HoraDeInicio { get; set; }

        [Display(Name = "Hora de fim da visita")]
        [Required(ErrorMessage = "Introduza uma data de fim da exposição")]
        [DataType(DataType.Time)]
        public DateTime HoraDeFim { get; set; }

        [Display(Name = "Nº de pessoas")]
        [Required(ErrorMessage = "É necessário introduzir o número de pessoas")]
        public int NumPessoas { get; set; }

		public int ExposicaoID { get; set; }
		public int UserAccountID { get; set; }

        public virtual Exposicao Exposicao { get; set; }

        public virtual UserAccount UserAccount { get; set; }

	    public Marcacao()
	    {
		    ExposicaoID = 1;
	    }
    }
}