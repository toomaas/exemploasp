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

        //passa do estado de "espera por resposta"/submetido para o estado onde o user tem de introduzir mais informação para voltar a ser avaliado
		public override void PedirInformacao(UserAccountExposicao userAccountExposicao)
		{
			db.PedirInformacaoDb(userAccountExposicao);
			decisorCandidatura.EstadoActual = decisorCandidatura.EsperaInformacaoAdicional;
		}

        //passa do estado de submetido para rejeitado
		public override void Rejeitar(UserAccountExposicao userAccountExposicao)
		{
			db.RejeitarDb(userAccountExposicao);
			decisorCandidatura.EstadoActual = decisorCandidatura.AguardarEnvio;
		}

        //passa do estado de submetido para aceite
		public override void Aceitar(UserAccountExposicao userAccountExposicao)
		{
			db.AceitarDb(userAccountExposicao);
			decisorCandidatura.EstadoActual = decisorCandidatura.CandidaturaAceite;
		}
	}
}