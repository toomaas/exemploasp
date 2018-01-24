using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.IdentityModel;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Mvc;
using exemploasp.Models;
using exemploasp.ViewModels;

namespace exemploasp.Controllers
{
	public class AccountController : Controller
	{
	    OurDBContext db = new OurDBContext();
        // GET: Account
        public ActionResult Index()
		{
			    var user = db.UserAccount.Include(t => t.TipoUtilizador).Include(u => u.Temas);
                return View(user.ToList());
		}

	    private void PopulateAssignedTemaData(UserAccount userAccount)
	    {
	        var allTemas = db.Tema;
	        var userAccountTemas = new HashSet<int>(userAccount.Temas.Select(t => t.TemaID));
	        var viewModel = new List<AssignedTemaData>();
	        foreach (var tema in allTemas)
	        {
	            viewModel.Add(new AssignedTemaData { TemaID = tema.TemaID, Nome = tema.Nome, Assigned = userAccountTemas.Contains(tema.TemaID) });
	        }
	        ViewBag.Temas = viewModel;
	    }

        public ActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Register(UserAccount account)
		{
			if (ModelState.IsValid)
			{
				using (OurDBContext db = new OurDBContext())
				{
				    DateTime data18 = DateTime.Now;
				    data18 = data18.AddYears(-18);
				    if (account.Idade < data18)
                    {
                        if (!db.UserAccount.Any(n => n.Email == account.Email))
                        {
                            var encrypt = Encrypt(account.Password);
                            account.Password = encrypt;
                            encrypt = Encrypt(account.ConfirmPassword);
                            account.ConfirmPassword = encrypt;
                            db.UserAccount.Add(account);
                            db.SaveChanges();
                        }
                        else
                        {
                            ModelState.AddModelError("Email", "Email já existente.");
                            return View();
                        }
                    }
				    else
				    {
				        ModelState.AddModelError("Idade", "Idade não permitida.");
				        return View();
				    }
                }
				ModelState.Clear();
				ViewBag.Message = account.Nome + " registado";
				return RedirectToAction("Login");
			}
			return View();
		}

		public string Encrypt(string passwordPlainText)
		{
			if(passwordPlainText == null) throw new ArgumentNullException("passwordPlainText");

            //encrypt data
		    byte[] data = System.Text.Encoding.ASCII.GetBytes(passwordPlainText);
		    data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
		    String hash = System.Text.Encoding.ASCII.GetString(data);
            //var data = Encoding.Unicode.GetBytes(passwordPlainText);
			//byte[] encrypted = ProtectedData.Protect(data, null, Scope);

		    return hash; //System.Text.Encoding.UTF8.GetString(encrypted);//Convert.ToBase64String(encrypted);

		}

		public DataProtectionScope Scope { get; set; }


		//Login
		public ActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Login(UserAccount user)
		{
			using (OurDBContext db = new OurDBContext())
			{
			    var encrypt = Encrypt(user.Password);
                var usr = db.UserAccount.Where(u => u.Email == user.Email && u.Password == encrypt).FirstOrDefault();
				if (usr != null)
				{
					Session["UserAccountID"] = usr.UserAccountID.ToString();
					Session["Username"] = usr.Nome.ToString();
					Session["TipoUtilizador"] = usr.TipoUtilizador.Tipo.ToString();
					return RedirectToAction("LoggedIn");
				}
				else
				{
					ModelState.AddModelError("", "username ou a pass estao mal");
				}
			}
			return View();
		}

		public ActionResult LoggedIn()
		{
			if (Session["UserAccountID"] != null)
			{
				return View();
			}
			else
			{
				return RedirectToAction("Login");
			}
		}

		public ActionResult Logout()
		{
			if (Session["UserAccountID"] != null)
			{
				Session["UserAccountID"] = null;


			}
			return RedirectToAction("LoggedIn");
		}

	    //"int? id" signfica que o parametro id pode ter um valor inteiro ou pode receber um valor null
        public ActionResult PerfilUser(int? id)
		{

		    using (OurDBContext db = new OurDBContext())
		    {
		        UserAccount user = db.UserAccount.Include(t => t.TipoUtilizador).Include(u => u.Temas).SingleOrDefault(u => u.UserAccountID == id);
		        if (id == null || user == null)
		        {
		            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
		        }

		        if(TempData["Message"] != null) ViewBag.Message = TempData["Message"].ToString();
		        PopulateAssignedTemaData(user);
		        return View(user);
		    }
		}

        [HttpPost]
	    public ActionResult PerfilUser(int? id, string[] selectedTemas)
	    {
            if(id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
	        var userAccountToUpdate = db.UserAccount
	            .Include(u => u.Temas).Single(u => u.UserAccountID == id);	        
            if (TryUpdateModel(userAccountToUpdate, "",
	            new string[] {"Nome,Morada,Idade,Sexo,NumTelefone,Email,Password,ConfirmPassword,TipoUtilizadorID"}))
	        {
	            try
	            {
	                UpdateUserAccountTemas(selectedTemas, userAccountToUpdate);
	                db.Entry(userAccountToUpdate).State = EntityState.Modified;
	                db.SaveChanges();
	                return RedirectToAction("PerfilUser");
                }
	            catch (RetryLimitExceededException /* dex */)
                {
                    ModelState.AddModelError("", "Nao foi possivel atualizar o user");
                }
	        }
            PopulateAssignedTemaData(userAccountToUpdate);
	        return View(userAccountToUpdate);
        }

	    public ActionResult Edit(int? id)
	    {
	        UserAccount user = db.UserAccount.Include(t => t.TipoUtilizador).Include(u => u.Temas).SingleOrDefault(u => u.UserAccountID == id);
	        if (id == null || user == null)
	        {
	            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
	        }
	        PopulateAssignedTemaData(user);
	        return View(user);   
	    }

	    [HttpPost]
	    public ActionResult Edit(int? id, string nome, string morada, int numTelefone)
	    {
	        if (id == null)
	            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
	        var userAccountToUpdate = db.UserAccount
	            .Include(u => u.Temas).Single(u => u.UserAccountID == id);
	        userAccountToUpdate.Morada = morada;
	        userAccountToUpdate.NumTelefone = numTelefone;
	        userAccountToUpdate.Nome = nome;
	        if (TryUpdateModel(userAccountToUpdate, "",
	            new string[] { "Nome,Morada,Idade,Sexo,NumTelefone,Email,Password,ConfirmPassword,TipoUtilizadorID" }))
	        {
	            try
	            {
	                db.Entry(userAccountToUpdate).State = EntityState.Modified;
	                db.SaveChanges();
	                return RedirectToAction("PerfilUser", new {  id });
	            }
	            catch (RetryLimitExceededException /* dex */)
	            {
	                ModelState.AddModelError("", "Nao foi possivel atualizar o user");
	            }
	        }
	        return View(userAccountToUpdate);
	    }

	    public ActionResult AlterarPassword(int? id)
	    {
	        var userAccountToUpdate = db.UserAccount.SingleOrDefault(u => u.UserAccountID == id);
            if (id == null || userAccountToUpdate == null)
	        {
	            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
	        }
	        PopulateAssignedTemaData(userAccountToUpdate);
	        return View(userAccountToUpdate);
	    }

        [HttpPost]
	    public ActionResult AlterarPassword(int?id,string pwAntiga, string Password, string confpw)
	    {
	        UserAccount user = db.UserAccount.SingleOrDefault(u => u.UserAccountID == id);
	        if (id == null || user == null)
	        {
	            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
	        }
	        if (user.Password == Encrypt(pwAntiga))
	        {
	            user.Password = Encrypt(Password);
	            user.ConfirmPassword = Encrypt(Password);
	            db.Entry(user).State = EntityState.Modified;
	            db.SaveChanges();
	            TempData["Message"] = "Password alterada com sucesso";
                return RedirectToAction("PerfilUser", new { id });
            }
	        ModelState.AddModelError("", "Nao foi possivel atualizar a password");
            PopulateAssignedTemaData(user);
	        return View(user);
	    }

        private void UpdateUserAccountTemas(string[] selectedTemas, UserAccount userAccountToUpdate)
	    {
	        if (selectedTemas == null)
	        {
                userAccountToUpdate.Temas = new List<Tema>();
	            return;
	        }
	        var selectedTemasHS = new HashSet<string>(selectedTemas);
	        var userAccountTemas = new HashSet<int>(userAccountToUpdate.Temas.Select(t => t.TemaID));
	        foreach (var tema in db.Tema)
	        {
	            if (selectedTemasHS.Contains(tema.TemaID.ToString()))
	            {
	                if (!userAccountTemas.Contains(tema.TemaID))
	                {
	                    userAccountToUpdate.Temas.Add(tema);
	                }
	            }
	            else
	            {
	                if (userAccountTemas.Contains(tema.TemaID))
	                {
	                    userAccountToUpdate.Temas.Remove(tema);
	                }
	            }
	        }
	    }



		public ActionResult Funcao()
		{

			UserAccountDropdownList();
			TipoUtilizadorDropdownList();

			var users = db.UserAccount.Include(t => t.TipoUtilizador);
			return View(users.ToList());
		}

		[HttpPost]
		public ActionResult Funcao(int userAccountID, int tipoUtilizadorID)
        { 
		    var userAccountToUpdate = db.UserAccount.Single(u => u.UserAccountID == userAccountID);
            userAccountToUpdate.TipoUtilizadorID = tipoUtilizadorID;
            if (ModelState.IsValid)
            {
                     db.Entry(userAccountToUpdate).State = EntityState.Modified;
                     db.SaveChanges();
                     return RedirectToAction("Index");
            }

            UserAccountDropdownList();
            TipoUtilizadorDropdownList();

            var users = db.UserAccount.Include(t => t.TipoUtilizador);
            ModelState.AddModelError("", "Erro ao alterar função");
            return View(users.ToList());

        }


		private void UserAccountDropdownList(object userAccount = null)
		{

			var utilizadoresQuery = from u in db.UserAccount
				//where u.TipoUtilizadorID == 1
				orderby u.Nome
				select u;
			ViewBag.UserAccountID = new SelectList(utilizadoresQuery, "UserAccountID", "Nome", userAccount);
		}

		private void TipoUtilizadorDropdownList(object tipoUtilizador = null)
		{

			var TiposUtilizadoresQuery = from u in db.TipoUtilizador
				where u.TipoUtilizadorID !=1
				orderby u.Tipo
				select u;
			ViewBag.TipoUtilizadorID = new SelectList(TiposUtilizadoresQuery, "TipoUtilizadorID", "Tipo", tipoUtilizador);
		}

	}
}