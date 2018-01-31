using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Runtime.ExceptionServices;

namespace exemploasp.Models
{
	public class UserAccount : ITabelas
	{

		[Key]
		public int UserAccountID { get; set; }


		[Required(ErrorMessage = "O nome é óbrigatório")]
		[RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Nome inválido. Utilize apenas letras.")]
        public string Nome { get; set; }

		[Required(ErrorMessage = "A morada é óbrigatória")]
		public string Morada { get; set; }

		[Display(Name = "Data de Nascimento")]
		[Required(ErrorMessage = "A idade é óbrigatória")]
		[DataType(DataType.Date)]
		public DateTime Idade { get; set; }
        //public int Idade { get; set; }

		[Required(ErrorMessage = "O Sexo é óbrigatório")]
		public string Sexo { get; set; }

		[Display(Name = "Numero de telefone")]
		[RegularExpression(@"^[0-9]{9}$", ErrorMessage = "Numero de telefone invalido")]
		[Required(ErrorMessage = "O NumTelefone é óbrigatório")]
		public int NumTelefone { get; set; }

		[Required(ErrorMessage = "O email é óbrigatório")]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }

		[Required(ErrorMessage = "A Password é óbrigatório")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Display(Name = "Confirmar password")]
		[Compare("Password", ErrorMessage = "A confirmação de password é óbrigatório")]
		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; }

		public int TipoUtilizadorID { get; set; }

		public UserAccount()
		{
			TipoUtilizadorID = 1;
		}


		public virtual TipoUtilizador TipoUtilizador { get; set; }

		public virtual ICollection <Tema> Temas { get; set; }

		public virtual ICollection<Disponibilidade> Disponibilidades { get; set; }

		public virtual ICollection<UserAccountExposicao> UserAccountExposicaos { get; set; }

		public virtual ICollection<Marcacao> Marcacaos { get; set; }


	}
}