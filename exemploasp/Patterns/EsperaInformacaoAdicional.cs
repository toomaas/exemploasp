using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace exemploasp.Patterns
{
	public class EsperaInformacaoAdicional : State
	{
		public EsperaInformacaoAdicional(DecisorCandidatura adecisorCandidatura) : base(adecisorCandidatura)
		{ }

		public override void Rejeitar()
		{
			decisorCandidatura.EstadoActual = decisorCandidatura.AguardarEnvio;
		}

		public override void Aceitar()
		{
			decisorCandidatura.EstadoActual = decisorCandidatura.CandidaturaAceite;
		}
	}
}