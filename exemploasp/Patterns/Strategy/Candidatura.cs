using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using exemploasp.Models;

namespace exemploasp.Patterns.Strategy
{
	public class Candidatura
	{
		OurDBContext db = new OurDBContext();


		public void DecideCandidatura(int UserID, int ExposicaoID, string decisao)
		{
			var estadoCandidatura = ObterEstado(UserID, ExposicaoID);
			var strategy = EscolherEstrategia(estadoCandidatura);
			strategy.Execute(UserID,ExposicaoID);
		}


		private IStrategy EscolherEstrategia(EstadosCandidatura estado)
		{
			if (estado == EstadosCandidatura.AguardaEnvio)
			{
				return new SubmeterCandidatura(); 				
			}

			if (estado == EstadosCandidatura.VerificaCandidatura)
			{

				return new DecidirCandidatura();
			}

			if (estado == EstadosCandidatura.EsperaInformacao)
			{
				return new DecidirCandidatura();
			}
			else return null;

		}




		private EstadosCandidatura ObterEstado(int UserID, int ExposicaoID)
		{

			var obterEstado = db.UserAccountExposicao
				.Where(u => u.UserAccountID == UserID).SingleOrDefault(u => u.ExposicaoID == ExposicaoID);

			if (obterEstado != null)
			{

				switch (obterEstado.Assigned)
				{
					case 1:
					{
						return EstadosCandidatura.AguardaEnvio;
					}

					case 2:
					{
						return EstadosCandidatura.VerificaCandidatura;
					}

					case 3:
					{
						return EstadosCandidatura.EsperaInformacao;
					}
					case 4:
					{
						return EstadosCandidatura.CandidaturaAceite;
					}



				}
			}
			return EstadosCandidatura.AguardaEnvio;
		}

	}
}