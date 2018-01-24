using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace exemploasp.Patterns
{
	public class AguardarEnvio : State
	{

		public AguardarEnvio (DecisorCandidatura aDecisorCandidatura) : base(aDecisorCandidatura)
		{ }

		public override void Submeter()
		{
			decisorCandidatura.EstadoActual = decisorCandidatura.VerificarCandidatura;
		}

	}
}