using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace exemploasp.Patterns
{
	public class DecisorCandidatura
	{
		public IState AguardarEnvio;
		public IState CandidaturaAceite;
		public IState EsperaInformacaoAdicional;
		public IState VerificarCandidatura;
		public IState EstadoActual;


		public DecisorCandidatura()
		{
			EstadoActual = AguardarEnvio;

			AguardarEnvio = new AguardarEnvio(this);
			CandidaturaAceite = new CandidaturaAceite(this);
			EsperaInformacaoAdicional = new EsperaInformacaoAdicional(this);
			VerificarCandidatura = new VerificarCandidatura(this);


		}

		public void Submeter()
		{
			EstadoActual.Submeter();
		}

		public void Rejeitar()
		{
			EstadoActual.Rejeitar();
		}

		public void PedirInformacao()
		{
			EstadoActual.PedirInformacao();
		}

		public void Aceitar()
		{
			EstadoActual.Aceitar();
		}


	}
}