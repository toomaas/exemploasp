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

        //passa do estado de ""ter informação e estar à espera de resposta" para o estado "rejeitado"
		public override void Rejeitar(UserAccountExposicao userAccountExposicao)
		{
			stateDb.RejeitarDb(userAccountExposicao);
			decisorCandidatura.EstadoActual = decisorCandidatura.AguardarEnvio;
		}

	    //passa do estado de ""ter informação e estar à espera de resposta" para o estado "aceite"
        public override void Aceitar(UserAccountExposicao userAccountExposicao)
		{
			stateDb.AceitarDb(userAccountExposicao);
			decisorCandidatura.EstadoActual = decisorCandidatura.CandidaturaAceite;
		}
	}
}