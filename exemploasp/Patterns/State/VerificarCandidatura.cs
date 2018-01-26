using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using exemploasp.InteractDB;
using exemploasp.Models;

namespace exemploasp.Patterns
{
	public class VerificarCandidatura : State
	{
		private StateDB db = StateDB.Instancia();

		public VerificarCandidatura(DecisorCandidatura adecisorCandidatura) : base(adecisorCandidatura)
		{ }

		public override void PedirInformacao(UserAccountExposicao userAccountExposicao)
		{
			db.PedirInformacaoDb(userAccountExposicao);
			decisorCandidatura.EstadoActual = decisorCandidatura.EsperaInformacaoAdicional;
		}

		public override void Rejeitar(UserAccountExposicao userAccountExposicao)
		{
			db.RejeitarDb(userAccountExposicao);
			decisorCandidatura.EstadoActual = decisorCandidatura.AguardarEnvio;
		}

		public override void Aceitar(UserAccountExposicao userAccountExposicao)
		{

			db.AceitarDb(userAccountExposicao);
			decisorCandidatura.EstadoActual = decisorCandidatura.CandidaturaAceite;
		}
	}
}