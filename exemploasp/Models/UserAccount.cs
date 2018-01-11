﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Runtime.ExceptionServices;

namespace exemploasp.Models
{
    public class UserAccount
    {
        [Key]
        public int UserID { get; set; }

        [Required(ErrorMessage = "O nome é óbrigatório")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "A morada é óbrigatória")]
        public string Morada { get; set; }

        [Required(ErrorMessage = "A idade é óbrigatória")]
        public int Idade { get; set; }

        [Required(ErrorMessage = "O Sexo é óbrigatório")]
        public string Sexo { get; set; }

        [Required(ErrorMessage = "O NumTelefone é óbrigatório")]
        public int NumTelefone { get; set; }

        [Required(ErrorMessage = "O TipoUtilizador é óbrigatório")]
        public int TipoUtilizador { get; set; }

        [Required(ErrorMessage = "O email é óbrigatório")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "A Password é óbrigatório")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "A confirmação de password é óbrigatório")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

	    public UserAccount()
	    {
		    TipoUtilizador = 1;
	    }
    }
}