using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using exemploasp.Models;

namespace exemploasp.Patterns.Strategy
{
	public class AceitarCandidatura : IStrategy
	{
		public OurDBContext db { get; set; }

		public void Execute(int UserID,int ExposicaoID)
		{

			var candidaturaAceitar = db.UserAccountExposicao
				.Where(u => u.UserAccountID == UserID).SingleOrDefault(u => u.ExposicaoID == ExposicaoID);

			if (candidaturaAceitar != null)
			{
				candidaturaAceitar.Assigned = 4;
				db.Entry(candidaturaAceitar).State = EntityState.Modified;
				db.SaveChanges();
			}
		}
	}
}