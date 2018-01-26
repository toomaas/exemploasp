using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using exemploasp.InteractDB;
using exemploasp.Models;

namespace exemploasp.Patterns
{
	public class EsperaInformacaoAdicional : State
	{
		public StateDB stateDb = StateDB.Instancia();

		public EsperaInformacaoAdicional(DecisorCandidatura adecisorCandidatura) : base(adecisorCandidatura)
		{ }

		public override void Rejeitar(UserAccountExposicao userAccountExposicao)
		{

			stateDb.RejeitarDb(userAccountExposicao);
			decisorCandidatura.EstadoActual = decisorCandidatura.AguardarEnvio;
		}

		public override void Aceitar(UserAccountExposicao userAccountExposicao)
		{
			stateDb.AceitarDb(userAccountExposicao);
			decisorCandidatura.EstadoActual = decisorCandidatura.CandidaturaAceite;
		}
	}
}