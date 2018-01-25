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

		public IState AguardarEnvio { get; }
		public IState CandidaturaAceite { get; }
		public IState EsperaInformacaoAdicional { get; }
		public IState VerificarCandidatura { get; }
	    public IState EstadoActual { get; set; }

	    public UserAccountExposicao userAccountExposicao;

		public DecisorCandidatura(UserAccountExposicao userAccountExposicao)
		{
		    this.userAccountExposicao = userAccountExposicao;
		    EstadoActual = BuscarEstadoAtual();

			AguardarEnvio = new AguardarEnvio(this);
			CandidaturaAceite = new CandidaturaAceite(this);
			EsperaInformacaoAdicional = new EsperaInformacaoAdicional(this);
			VerificarCandidatura = new VerificarCandidatura(this);
		}

		public IState BuscarEstadoAtual()
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

		public void Submeter()
		{

			EstadoActual.Submeter(userAccountExposicao);
			
		}

		public void Rejeitar()
		{
			EstadoActual.Rejeitar(userAccountExposicao);
		}

		public void PedirInformacao()
		{
			EstadoActual.PedirInformacao(userAccountExposicao);
		}

		public void Aceitar()
		{
			EstadoActual.Aceitar(userAccountExposicao);
		}


	}
}