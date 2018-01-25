using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using exemploasp.Models;

namespace exemploasp.Patterns.Strategy
{
	public class PedirInformacao : IStrategy
	{
		public OurDBContext db { get; set; }
		public void Execute(int UserID, int ExposicaoID)
		{
			var pedirInformacao = db.UserAccountExposicao
				.Where(u => u.UserAccountID == UserID).SingleOrDefault(u => u.ExposicaoID == ExposicaoID);

			if (pedirInformacao != null)
			{
				pedirInformacao.Assigned = 3;
				db.Entry(pedirInformacao).State = EntityState.Modified;
				db.SaveChanges();
			}
		}
	}
}