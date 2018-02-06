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
	public class MuseuInteractDb
	{
	    private readonly OurDBContext _db = new OurDBContext();

		//query para obter os utilizadores e a sua função
		public List<UserAccount> Utilizadores()
		{
			var ulist =  from u in _db.UserAccount
				orderby u.Nome
				select u;

		    List<UserAccount> newList = new List<UserAccount>();
		    foreach (var member in ulist)
		        newList.Add(new UserAccount
		        {
		            UserAccountID = member.UserAccountID,
		            Nome = member.Nome + " : " + member.TipoUtilizador.Tipo
		        });

		    return newList;
        }

	    //query para obter os utilizadores que têm disponibilidade para realizar uma certa masrcação
	    public IOrderedQueryable<UserAccount> UtilizadoresMarcacao(int exposicaoId, int marcacaoId)
	    {
	        Marcacao marcacao = _db.Marcacao.Find(marcacaoId);
	        return from u in _db.UserAccount
                where u.UserAccountExposicaos.FirstOrDefault(m => m.ExposicaoID == exposicaoId).Assigned == 4
                   where u.Disponibilidades.Where(d => d.ExposicaoID == exposicaoId).Any(d => d.DataDisponibilidade == marcacao.Data)
	            orderby u.Nome
	            select u;
	    }

        // Lista de exposiçoes não expiradas
        public List<Exposicao> ListaExposicao()
		{
			var exposicoesQuery = from e in _db.Exposicao
                where e.DataFinal > DateTime.Now
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

        //verifica se a data escolhida para a marcação ocorre enquanto que a exposição está ativa
		public bool DataExposicaoMarcacao(DateTime dataMarcacao, DateTime dataInicialExposicao, DateTime dataFinalExposicao)
		{
			if (dataMarcacao >= dataInicialExposicao && dataMarcacao <= dataFinalExposicao)
			{
				return true;
			}
			return false;
		}

        //retorna os tipos de utilizador existentes
		public IOrderedQueryable<TipoUtilizador> TiposUtilizadores()
		{
			return from u in _db.TipoUtilizador
				where u.TipoUtilizadorID != 1
				orderby u.Tipo
				select u;
		}

        //método que remove ou adiciona temas a uma exposição ou a um utilizador. ITabelas pode ser um utilizador ou uma exposição
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
	        var selectedTemasHs = new HashSet<string>(selectedTemas);     
	        foreach (var tema in dbContext.Tema)
	        {
	            if (selectedTemasHs.Contains(tema.TemaID.ToString()))
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

        //altera os dados do utilizador
        public UserAccount EditUser(UserAccount userAccountToUpdate, string nome, string morada, int numTelefone)
		{
			userAccountToUpdate.Morada = morada;
			userAccountToUpdate.NumTelefone = numTelefone;
			userAccountToUpdate.Nome = nome;
			return userAccountToUpdate;
		}

        //função que encripta a password.
		public string Encrypt(string passwordPlainText)
		{
			if (passwordPlainText == null) throw new ArgumentNullException("passwordPlainText");
			byte[] data = System.Text.Encoding.ASCII.GetBytes(passwordPlainText);
			data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
			String hash = System.Text.Encoding.ASCII.GetString(data);
			return hash;
		}

        //
	    public List<AssignedTemaData> PopulateAssignedTemaData(ITabelas tabela)
	    {
	        var allTemas = _db.Tema;
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