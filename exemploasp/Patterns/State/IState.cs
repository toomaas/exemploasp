using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using exemploasp.Models;

namespace exemploasp.Patterns
{
	public interface IState
	{
		void Submeter(UserAccountExposicao userAccountExposicao);
		void Aceitar(UserAccountExposicao userAccountExposicao);
		void Rejeitar(UserAccountExposicao userAccountExposicao);
		void PedirInformacao(UserAccountExposicao userAccountExposicao);
	}
}
