using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using exemploasp.Models;

namespace exemploasp.Patterns
{
	public class DecisorCandidatura
	{
		OurDBContext db = new OurDBContext();

		public IState AguardarEnvio;
		public IState CandidaturaAceite;
		public IState EsperaInformacaoAdicional;
		public IState VerificarCandidatura;
		public IState EstadoActual;



		public DecisorCandidatura(UserAccountExposicao userAccountExposicao)
		{
			EstadoActual = BuscarEstadoAtual(userAccountExposicao);

			AguardarEnvio = new AguardarEnvio(this);
			CandidaturaAceite = new CandidaturaAceite(this);
			EsperaInformacaoAdicional = new EsperaInformacaoAdicional(this);
			VerificarCandidatura = new VerificarCandidatura(this);


		}

		public IState BuscarEstadoAtual(UserAccountExposicao userAccountExposicao)
		{
			var obterEstado = db.UserAccountExposicao
				.Where(u => u.UserAccountID == userAccountExposicao.UserAccountID).SingleOrDefault(u => u.ExposicaoID == userAccountExposicao.ExposicaoID);

			if (obterEstado != null)
			{

				switch (obterEstado.Assigned)
				{
					case 1:
					{
						return AguardarEnvio;
					}

					case 2:
					{
						return VerificarCandidatura;
					}

					case 3:
					{
						return EsperaInformacaoAdicional;
					}
					case 4:
					{
						return CandidaturaAceite;
					}



				}
			}
			return AguardarEnvio;
		}


		OurDBContext bd = new OurDBContext();

		public void Submeter(UserAccountExposicao userAccountExposicao)
		{

			EstadoActual.Submeter(userAccountExposicao);
			
		}

		public void Rejeitar(UserAccountExposicao userAccountExposicao)
		{
			EstadoActual.Rejeitar(userAccountExposicao);
		}

		public void PedirInformacao(UserAccountExposicao userAccountExposicao)
		{
			EstadoActual.PedirInformacao(userAccountExposicao);
		}

		public void Aceitar(UserAccountExposicao userAccountExposicao)
		{
			EstadoActual.Aceitar(userAccountExposicao);
		}


	}
}