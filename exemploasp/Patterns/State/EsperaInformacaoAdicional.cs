using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using exemploasp.Models;

namespace exemploasp.Patterns
{
	public class EsperaInformacaoAdicional : State
	{
		public EsperaInformacaoAdicional(DecisorCandidatura adecisorCandidatura) : base(adecisorCandidatura)
		{ }

		public override void Rejeitar(UserAccountExposicao userAccountExposicao)
		{
			var rejeitarCandidatura = db.UserAccountExposicao
				.Where(u => u.UserAccountID == userAccountExposicao.UserAccountID).SingleOrDefault(u => u.ExposicaoID == userAccountExposicao.ExposicaoID);

			if (rejeitarCandidatura != null)
			{
				rejeitarCandidatura.Assigned = 1;
				db.Entry(rejeitarCandidatura).State = EntityState.Modified;
				db.SaveChanges();
			}

			decisorCandidatura.EstadoActual = decisorCandidatura.AguardarEnvio;
		}

		public override void Aceitar(UserAccountExposicao userAccountExposicao)
		{
			var candidaturaAceitar = db.UserAccountExposicao
				.Where(u => u.UserAccountID == userAccountExposicao.UserAccountID).SingleOrDefault(u => u.ExposicaoID == userAccountExposicao.ExposicaoID);

			if (candidaturaAceitar != null)
			{
				candidaturaAceitar.Assigned = 4;
				db.Entry(candidaturaAceitar).State = EntityState.Modified;
				db.SaveChanges();
			}


			decisorCandidatura.EstadoActual = decisorCandidatura.CandidaturaAceite;
		}
	}
}