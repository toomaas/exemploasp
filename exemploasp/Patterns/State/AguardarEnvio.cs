using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using exemploasp.InteractDB;
using exemploasp.Models;

namespace exemploasp.Patterns
{
	public class AguardarEnvio : State
	{
		private StateDB db = StateDB.Instancia();

		public AguardarEnvio (DecisorCandidatura aDecisorCandidatura) : base(aDecisorCandidatura)
		{ }

        //passa do estado de aguardaenvio/rejeitada para submetido
		public override void Submeter(UserAccountExposicao userAccountExposicao)
		{
			db.SubmeterDb(userAccountExposicao);	
			decisorCandidatura.EstadoActual = decisorCandidatura.VerificarCandidatura;
		}

	}
}