using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Runtime.ExceptionServices;

namespace exemploasp.Models
{
	public class Exposicao
	{
		[Key]
		public int ExposicaoID { get; set; }

		[Required(ErrorMessage = "O nome é óbrigatório")]
		public string Nome { get; set; }

		[Display(Name = "Data Inicio da exposição")]
		[Required(ErrorMessage = "Introduza uma data de inicio da exposição")]
		[DataType(DataType.Date)]
		public DateTime DataInicial { get; set; }

		[Display(Name = "Data Fim da exposição")]
		[Required(ErrorMessage = "Introduza uma data de fim da exposição")]
		[DataType(DataType.Date)]
		public DateTime DataFinal { get; set; }

		[Display(Name = "Duração")]
		[Required(ErrorMessage = "A duração é obrigatoria")]
		public int Duracao { get; set; }

		[Display(Name = "Numero de Itens")]
		[Required(ErrorMessage = "Introduza o número de itens")]
		public int NrItens { get; set; }


		public virtual ICollection<Tema> Temas { get; set; }


	}
}