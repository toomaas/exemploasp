using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using exemploasp.Models;

namespace exemploasp.Patterns
{
	public class AguardarEnvio : State
	{

		public AguardarEnvio (DecisorCandidatura aDecisorCandidatura) : base(aDecisorCandidatura)
		{ }

		public override void Submeter(UserAccountExposicao userAccountExposicao)
		{

			var submeterCandidatura = db.UserAccountExposicao
				.Where(u => u.UserAccountID == userAccountExposicao.UserAccountID).SingleOrDefault(u => u.ExposicaoID == userAccountExposicao.ExposicaoID);

			if (submeterCandidatura != null)
			{
				submeterCandidatura.Assigned = 2;
				db.Entry(submeterCandidatura).State = EntityState.Modified;
				db.SaveChanges();
			}
			else
			{
				userAccountExposicao.Assigned = 2;
				db.UserAccountExposicao.Add(userAccountExposicao);
				db.SaveChanges();
			}
			decisorCandidatura.EstadoActual = decisorCandidatura.VerificarCandidatura;
		}

	}
}