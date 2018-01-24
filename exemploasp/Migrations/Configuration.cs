using System.Collections.Generic;

namespace exemploasp.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
	using exemploasp.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<exemploasp.Models.OurDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "exemploasp.Models.OurDBContext";
        }

        protected override void Seed(exemploasp.Models.OurDBContext context)
        {

	        context.TipoUtilizador.AddOrUpdate(t => t.TipoUtilizadorID,
		        new TipoUtilizador() {TipoUtilizadorID = 1, Tipo = "Utilizador"},
		        new TipoUtilizador() {TipoUtilizadorID = 2, Tipo = "Administrador"}
	        );


			context.UserAccount.AddOrUpdate(u=>u.UserAccountID,
		        new UserAccount()
		        {
			        Nome= "Utilizador",Morada="Universidade da Madeira", Idade = DateTime.Parse("12-12-1998"),
			        Sexo = "Masculino", NumTelefone = 961234567, Email = "utilizador@email.com", Password = "?e?Y B/?A~Hg??O??J???~??????z?",
			        ConfirmPassword = "?e?Y B/?A~Hg??O??J???~??????z?", TipoUtilizadorID = 1
		        },

		        new UserAccount()
		        {
			        Nome= "Administrador",Morada="Universidade da Madeira", Idade = DateTime.Parse("12-8-1998"),
			        Sexo = "Masculino", NumTelefone = 961234569, Email = "administrador@email.com", Password = "?e?Y B/?A~Hg??O??J???~??????z?",
			        ConfirmPassword = "?e?Y B/?A~Hg??O??J???~??????z?", TipoUtilizadorID = 2
		        }
			);
	
		}
    }
}
