using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using exemploasp.Models;

namespace exemploasp.Patterns.Strategy
{
	public class DecidirCandidatura : IStrategy
	{
		public OurDBContext db { get; set; }
		public void Execute(int UserID, int ExposicaoID)
		{


		}
	}
}