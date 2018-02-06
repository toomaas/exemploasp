using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using exemploasp.Models;

namespace exemploasp.InteractDB
{
	public class StateDB
	{
		private static StateDB _instanciaStateDb;

		protected StateDB()
		{			
		}
        //singleton
		public static StateDB Instancia()
		{
			if (_instanciaStateDb == null)
			{
				_instanciaStateDb = new StateDB(); 
			}
			return _instanciaStateDb;
		}

		OurDBContext db = new OurDBContext();

		//Alterar o estado da candidatura na base de dados para 2 (VerificarCandidatura) ou cria uma nova com o estado a 2		
		public void SubmeterDb(UserAccountExposicao userAccountExposicao)
		{
			var submeterCandidatura = db.UserAccountExposicao.Where(u => u.UserAccountID == userAccountExposicao.UserAccountID).SingleOrDefault(u => u.ExposicaoID == userAccountExposicao.ExposicaoID);
			if (submeterCandidatura != null)
			{
				submeterCandidatura.Assigned = 2;
			    submeterCandidatura.InformacaoExtra = null;
				db.Entry(submeterCandidatura).State = EntityState.Modified;
				db.SaveChanges();
			}
			else
			{
				userAccountExposicao.Assigned = 2;
				db.UserAccountExposicao.Add(userAccountExposicao);
				db.SaveChanges();
			}
		}

		//Alterar o estado da candidatura na base de dados para 4 (CandidaturaAceite)
		public void AceitarDb(UserAccountExposicao userAccountExposicao)
		{
			var candidaturaAceitar = db.UserAccountExposicao.Where(u => u.UserAccountID == userAccountExposicao.UserAccountID).SingleOrDefault(u => u.ExposicaoID == userAccountExposicao.ExposicaoID);
			if (candidaturaAceitar != null)
			{
				candidaturaAceitar.Assigned = 4;
				db.Entry(candidaturaAceitar).State = EntityState.Modified;
				db.SaveChanges();
			}
		}

		//Alterar o estado da candidatura na base de dados para 1 (AguardarEnvio)
		public void RejeitarDb(UserAccountExposicao userAccountExposicao)
		{
			var rejeitarCandidatura = db.UserAccountExposicao.Where(u => u.UserAccountID == userAccountExposicao.UserAccountID).SingleOrDefault(u => u.ExposicaoID == userAccountExposicao.ExposicaoID);
			if (rejeitarCandidatura != null)
			{
				rejeitarCandidatura.Assigned = 1;
				db.Entry(rejeitarCandidatura).State = EntityState.Modified;
				db.SaveChanges();
			}
		}

		//Alterar o estado da candidatura na base de dados para 3 (EsperaInformacaoAdicional)
		public void PedirInformacaoDb(UserAccountExposicao userAccountExposicao)
		{
			var pedirInformacao = db.UserAccountExposicao
				.Where(u => u.UserAccountID == userAccountExposicao.UserAccountID).SingleOrDefault(u => u.ExposicaoID == userAccountExposicao.ExposicaoID);

			if (pedirInformacao != null)
			{
				pedirInformacao.Assigned = 3;
				db.Entry(pedirInformacao).State = EntityState.Modified;
				db.SaveChanges();
			}
		}

	}
}