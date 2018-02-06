using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace exemploasp.Models
{
    public class OurDBContext : DbContext
    {
        public DbSet<UserAccount> UserAccount { get; set; }

		public DbSet<Exposicao> Exposicao { get; set; }

        public DbSet<Tema> Tema { get; set; }

        public DbSet<Marcacao> Marcacao { get; set; }

		public DbSet<TipoUtilizador> TipoUtilizador { get; set; }

        public DbSet<UserAccountExposicao> UserAccountExposicao { get; set; }

        public System.Data.Entity.DbSet<exemploasp.Models.Disponibilidade> Disponibilidade { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
	    {
		    modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
	    }
    }
}