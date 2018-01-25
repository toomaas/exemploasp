using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using exemploasp.Models;

namespace exemploasp.Patterns.Strategy
{
	public class SubmeterCandidatura : IStrategy
	{
		public OurDBContext db { get; set; }
		public void Execute(int UserID, int ExposicaoID)
		{
			var submeterCandidatura = db.UserAccountExposicao
				.Where(u => u.UserAccountID == UserID).SingleOrDefault(u => u.ExposicaoID == ExposicaoID);

			if (submeterCandidatura != null)
			{
				submeterCandidatura.Assigned = 2;
				db.Entry(submeterCandidatura).State = EntityState.Modified;
				db.SaveChanges();
			}
			else
			{
				
				submeterCandidatura = new UserAccountExposicao();
				submeterCandidatura.Assigned = 2;
				submeterCandidatura.ExposicaoID = ExposicaoID;
				submeterCandidatura.UserAccountID = UserID;
				db.UserAccountExposicao.Add(submeterCandidatura);
				db.SaveChanges();
			}
		}
	}
}
