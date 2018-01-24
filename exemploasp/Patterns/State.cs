using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace exemploasp.Patterns
{
	public class State : IState
	{
		protected DecisorCandidatura decisorCandidatura { get; }

		protected State(DecisorCandidatura adecisorCandidatura)
		{
			decisorCandidatura = adecisorCandidatura;
		}


		public virtual void Submeter()
		{ }

		public virtual void Aceitar()
		{ }

		public virtual void Rejeitar()
		{ }

		public virtual void PedirInformacao()
		{ }
	}
}