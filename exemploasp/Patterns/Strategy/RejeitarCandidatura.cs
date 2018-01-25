using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using exemploasp.Models;

namespace exemploasp.Patterns.Strategy
{
	public class RejeitarCandidatura : IStrategy
	{
		public OurDBContext db { get; set; }
		public void Execute(int UserID, int ExposicaoID)
		{

			var rejeitarCandidatura = db.UserAccountExposicao
				.Where(u => u.UserAccountID == UserID).SingleOrDefault(u => u.ExposicaoID == ExposicaoID);

			if (rejeitarCandidatura != null)
			{
				rejeitarCandidatura.Assigned = 1;
				db.Entry(rejeitarCandidatura).State = EntityState.Modified;
				db.SaveChanges();
			}
		}
	}
}