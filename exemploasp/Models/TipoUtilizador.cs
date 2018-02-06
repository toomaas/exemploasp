using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Policy;
using System.Linq;
using System.Web;

namespace exemploasp.Models
{
	public class TipoUtilizador
	{
		[Key]
		public int TipoUtilizadorID { get; set; }

		[Display(Name = "Tipo de utilizador")]
		[Required(ErrorMessage = "Introduza um tipo de utilizador")]
		public string Tipo { get; set; }

		public virtual ICollection<UserAccount> UserAccounts { get; set; }
	}
}