using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using exemploasp.Models;

namespace exemploasp.Patterns.Strategy
{
	

	public interface IStrategy
	{
		OurDBContext db { get; set; }

		void Execute(int UserID, int ExposicaoID);

	}
}
