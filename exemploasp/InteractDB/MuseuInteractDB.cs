using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Query.Dynamic;
using exemploasp.Controllers;
using exemploasp.Models;
using exemploasp.ViewModels;

namespace exemploasp.InteractDB
{
	public class MuseuInteractDB
	{

		OurDBContext db = new OurDBContext();

		//CONTROLADOR MARCACAO

		//query para obter os utilizadores
		public IOrderedQueryable<UserAccount> Utilizadores()
		{
			return from u in db.UserAccount
				orderby u.Nome
				select u;
		}

		// Lista de exposiçoes
		public List<Exposicao> ListaExposicao()
		{

			var exposicoesQuery = from e in db.Exposicao
				orderby e.Nome
				select e;

			List<Exposicao> newList = new List<Exposicao>();
			foreach (var member in exposicoesQuery)
				newList.Add(new Exposicao
				{
					ExposicaoID = member.ExposicaoID,
					Nome = member.Nome + " de " + member.DataInicial.ToShortDateString() + " a " + member.DataFinal.ToShortDateString() + " DUR: " + member.Duracao.ToShortTimeString()
				});

			return newList;
		}


		public bool DataExposicaoMarcacao(DateTime dataMarcacao, DateTime dataInicialExposicao,
			DateTime dataFinalExposicao)
		{
			if (dataMarcacao >= dataInicialExposicao && dataMarcacao <= dataFinalExposicao)
			{
				return true;
			}
			return false;
		}


		//CONTROLADOR Account

		//
		public IOrderedQueryable<TipoUtilizador> TiposUtilizadores()
		{
			return from u in db.TipoUtilizador
				where u.TipoUtilizadorID != 1
				orderby u.Tipo
				select u;
		}

		public UserAccount userAccountUpdate(int userAccountID, int tipoUtilizadorID)
		{
			var userAccountToUpdate = db.UserAccount.Single(u => u.UserAccountID == userAccountID);
			userAccountToUpdate.TipoUtilizadorID = tipoUtilizadorID;

			return userAccountToUpdate;
		}

	    public void UpdateTemas(string[] selectedTemas, ITabelas tabela, OurDBContext dbContext)
	    {
	        var userAccountTemas = new HashSet<int>(tabela.Temas.Select(t => t.TemaID));
            if (selectedTemas == null)
	        {
	            if (tabela.Temas.Count != 0)
	            {
	                foreach (var tema in dbContext.Tema)
	                {
                        if(tabela.Temas.Contains(tema))
	                    tabela.Temas.Remove(tema);
	                }
	            }
	            dbContext.Entry(tabela).State = EntityState.Modified;
	            dbContext.SaveChanges();
                return;
	        }
	        var selectedTemasHS = new HashSet<string>(selectedTemas);     
	        foreach (var tema in dbContext.Tema)
	        {
	            if (selectedTemasHS.Contains(tema.TemaID.ToString()))
	            {
	                if (!userAccountTemas.Contains(tema.TemaID))
	                {
	                    tabela.Temas.Add(tema);

	                }
	            }
	            else
	            {
	                if (userAccountTemas.Contains(tema.TemaID))
	                {
	                    tabela.Temas.Remove(tema);
	                }
	            }

	        }
	        dbContext.Entry(tabela).State = EntityState.Modified;
	        dbContext.SaveChanges();
        }


        public UserAccount EditUser(UserAccount userAccountToUpdate, string nome, string morada, int numTelefone)
		{
			//var userAccountToUpdate = db.UserAccount.Include(u => u.Temas).Single(u => u.UserAccountID == id);
			userAccountToUpdate.Morada = morada;
			userAccountToUpdate.NumTelefone = numTelefone;
			userAccountToUpdate.Nome = nome;


			return userAccountToUpdate;
		}

		public string Encrypt(string passwordPlainText)
		{
			if (passwordPlainText == null) throw new ArgumentNullException("passwordPlainText");

			//encrypt data
			byte[] data = System.Text.Encoding.ASCII.GetBytes(passwordPlainText);
			data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
			String hash = System.Text.Encoding.ASCII.GetString(data);
			//var data = Encoding.Unicode.GetBytes(passwordPlainText);
			//byte[] encrypted = ProtectedData.Protect(data, null, Scope);

			return hash; //System.Text.Encoding.UTF8.GetString(encrypted);//Convert.ToBase64String(encrypted);

		}

	    public List<AssignedTemaData> PopulateAssignedTemaData(ITabelas tabela)
	    {
	        var allTemas = db.Tema;
	        var tabelaTemas = new HashSet<int>(tabela.Temas.Select(t => t.TemaID));
	        var viewModel = new List<AssignedTemaData>();
	        foreach (var tema in allTemas)
	        {
	            viewModel.Add(new AssignedTemaData { TemaID = tema.TemaID, Nome = tema.Nome, Assigned = tabelaTemas.Contains(tema.TemaID) });
	        }
	        return viewModel;
	    }
    }
}