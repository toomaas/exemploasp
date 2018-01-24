using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using exemploasp.Models;

namespace exemploasp.Patterns
{
	public class VerificarCandidatura : State
	{

		public VerificarCandidatura(DecisorCandidatura adecisorCandidatura) : base(adecisorCandidatura)
		{ }

		public override void PedirInformacao()
		{
			decisorCandidatura.EstadoActual = decisorCandidatura.EsperaInformacaoAdicional;
		}

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