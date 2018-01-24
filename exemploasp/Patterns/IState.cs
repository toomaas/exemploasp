using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace exemploasp.Patterns
{
	public interface IState
	{
		void Submeter();
		void Aceitar();
		void Rejeitar();
		void PedirInformacao();
	}
}
