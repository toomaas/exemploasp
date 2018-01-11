﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Runtime.ExceptionServices;

namespace exemploasp.Models
{
	public class Tema
	{

		[Key]
		public int TemaID { get; set; }


		[Required(ErrorMessage = "O nome é óbrigatório")]
		public string Nome { get; set; }


		[Required(ErrorMessage = "A descrição é óbrigatória")]
		public string Descricao { get; set; }

		public virtual ICollection<Exposicao> Exposicoes { get; set; }



	}
}