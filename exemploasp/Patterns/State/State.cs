using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using exemploasp.InteractDB;
using exemploasp.Models;

namespace exemploasp.Patterns
{
	public class State : IState
	{
		protected DecisorCandidatura decisorCandidatura { get; }

		protected State(DecisorCandidatura adecisorCandidatura)
		{
			decisorCandidatura = adecisorCandidatura;
		}

        //metodos a implementar
		public virtual void Submeter(UserAccountExposicao userAccountExposicao)
		{ }

		public virtual void Aceitar(UserAccountExposicao userAccountExposicao)
		{ }

		public virtual void Rejeitar(UserAccountExposicao userAccountExposicao)
		{ }

		public virtual void PedirInformacao(UserAccountExposicao userAccountExposicao)
		{ }
	}
}