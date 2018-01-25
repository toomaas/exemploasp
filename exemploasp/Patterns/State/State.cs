using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using exemploasp.Models;

namespace exemploasp.Patterns
{
	public class State : IState
	{
		protected DecisorCandidatura decisorCandidatura { get; }
		public OurDBContext db = new OurDBContext();

		protected State(DecisorCandidatura adecisorCandidatura)
		{
			decisorCandidatura = adecisorCandidatura;
		}

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