using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace exemploasp.Models
{
    public class OurDBContext : DbContext
    {
        public DbSet<UserAccount> userAccount { get; set; }

		public DbSet<Exposicao> Exposicao { get; set; }

        public DbSet<Tema> Tema { get; set; }

        public DbSet<Marcacao> Marcacao { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
	    {
		    modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
	    }
    }
}