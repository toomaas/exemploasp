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
using System.Web.ApplicationServices;
using System.Web.Mvc;
using exemploasp.InteractDB;
using exemploasp.Models;
using exemploasp.Patterns.TemplateMethod;
using exemploasp.ViewModels;

namespace exemploasp.Controllers
{
	public class AccountController : Controller
	{
	    OurDBContext db = new OurDBContext();
		MuseuInteractDB museuDB = new MuseuInteractDB();

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
			Session.Remove("UserAccountID");
			Session.Remove("Username");
			return View();
		}

		[HttpPost]
		public ActionResult Register(UserAccount account)
		{
		    //var firstWord = s.Substring(0, s.IndexOf(" "));
            ObjetoMuseu oUserAccount = new ObjUserAccount(account);
            if (ModelState.IsValid)
			{
			    if (oUserAccount.Validar() == null)
			    {
			        oUserAccount.SalvarBd(db);
			    }
			    else
			    {
                    var firstWord = oUserAccount.Validar().Substring(0, oUserAccount.Validar().IndexOf(" "));
                    ModelState.AddModelError(firstWord, oUserAccount.Validar());
			        return View();
                }
			    ModelState.Clear();
			    ViewBag.Message = account.Nome + " registado";
			    return RedirectToAction("Login");
            }
			return View();
		}

		//Login
		public ActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Login(UserAccount user)
		{
			var encrypt = museuDB.Encrypt(user.Password);
            var usr = db.UserAccount.FirstOrDefault(u => u.Email == user.Email && u.Password == encrypt);
			if (usr != null)
			{
				Session["UserAccountID"] = usr.UserAccountID.ToString();
				Session["Username"] = usr.Nome.ToString();
				Session["TipoUtilizador"] = usr.TipoUtilizador.Tipo.ToString();
				
				return RedirectToAction("Index","Home");
			}
			ModelState.AddModelError("", "username ou a pass estao mal");
			return View();
		}

		public ActionResult LoggedIn()
		{
			if (Session["UserAccountID"] != null)
			{
				return RedirectToAction("Index","Home");
			}
			return RedirectToAction("Login");
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
		    UserAccount user = db.UserAccount.Include(t => t.TipoUtilizador).Include(u => u.Temas).Include(u => u.Disponibilidades).Include(a => a.UserAccountExposicaos.Select(e=>e.Exposicao)).SingleOrDefault(u => u.UserAccountID == id);
			var exp = db.Exposicao.Where(a=>a.UserAccountExposicaos.Any(c=>c.UserAccountID ==id && c.Assigned==4)).ToList();
			if (id == null || user == null)
		    {
		        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
		    }
		    if(TempData["Message"] != null) ViewBag.Message = TempData["Message"].ToString();
		    ViewBag.Temas = museuDB.PopulateAssignedTemaData(user);
            return View(user);
		}

		[HttpPost]
	    public ActionResult PerfilUser(int? id, string[] selectedTemas)
	    {
            if(id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
	        var userAccountToUpdate = db.UserAccount.Include(u => u.Temas).Single(u => u.UserAccountID == id);
            if (TryUpdateModel(userAccountToUpdate, "",
	            new string[] {"Nome,Morada,Idade,Sexo,NumTelefone,Email,Password,ConfirmPassword,TipoUtilizadorID"}))
	        {
	            try
	            {
	                museuDB.UpdateTemas(selectedTemas, userAccountToUpdate,db);
	                return RedirectToAction("PerfilUser");
                }
	            catch (RetryLimitExceededException /* dex */)
                {
                    ModelState.AddModelError("", "Nao foi possivel atualizar o user");
                }
	        }       
            ViewBag.Temas = museuDB.PopulateAssignedTemaData(userAccountToUpdate);
            return View(userAccountToUpdate);
        }

		public ActionResult Edit(int? id)
	    {
	        UserAccount user = db.UserAccount.Include(t => t.TipoUtilizador).Include(u => u.Temas).SingleOrDefault(u => u.UserAccountID == id);
	        if (id == null || user == null)
	        {
	            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
	        }
	        ViewBag.Temas = museuDB.PopulateAssignedTemaData(user);
            return View(user);   
	    }

		[HttpPost]
	    public ActionResult Edit(int? id, string nome, string morada, int numTelefone)
	    {
	        if (id == null)
	            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
	        var userAccountToUpdate = db.UserAccount.Include(u => u.Temas).SingleOrDefault(u => u.UserAccountID == id);
            if (TryUpdateModel(museuDB.EditUser(userAccountToUpdate, nome, morada, numTelefone), "",
	            new string[] { "Nome,Morada,Idade,Sexo,NumTelefone,Email,Password,ConfirmPassword,TipoUtilizadorID" }))
	        {
	            try
	            {
                    db.Entry(museuDB.EditUser(userAccountToUpdate, nome, morada, numTelefone)).State = EntityState.Modified;
	                db.SaveChanges();
	                return RedirectToAction("PerfilUser", new {  id });
	            }
	            catch (RetryLimitExceededException /* dex */)
	            {
	                ModelState.AddModelError("", "Nao foi possivel atualizar o user");
	            }
	        }
	        return View(museuDB.EditUser(userAccountToUpdate, nome, morada, numTelefone));
	    }

	    public ActionResult AlterarPassword(int? id)
	    {
	        var userAccountToUpdate = db.UserAccount.SingleOrDefault(u => u.UserAccountID == id);
            if (id == null || userAccountToUpdate == null)
	        {
	            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
	        }
	        ViewBag.Temas = museuDB.PopulateAssignedTemaData(userAccountToUpdate);
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
	        if (user.Password == museuDB.Encrypt(pwAntiga))
	        {
	            user.Password = museuDB.Encrypt(Password);
	            user.ConfirmPassword = museuDB.Encrypt(Password);
	            db.Entry(user).State = EntityState.Modified;
	            db.SaveChanges();
	            TempData["Message"] = "Password alterada com sucesso";
                return RedirectToAction("PerfilUser", new { id });
            }
	        ModelState.AddModelError("", "Nao foi possivel atualizar a password");
	        ViewBag.Temas = museuDB.PopulateAssignedTemaData(user);
            return View(user);
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
			userAccountUpdate(userAccountID, tipoUtilizadorID);
			if (ModelState.IsValid)
            {
                db.Entry(userAccountUpdate(userAccountID, tipoUtilizadorID)).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            UserAccountDropdownList();
            TipoUtilizadorDropdownList();
            var users = db.UserAccount.Include(t => t.TipoUtilizador);
            ModelState.AddModelError("", "Erro ao alterar função");
            return View(users.ToList());
        }

		public UserAccount userAccountUpdate(int userAccountID, int tipoUtilizadorID)
		{
			var userAccountToUpdate = db.UserAccount.Single(u => u.UserAccountID == userAccountID);
			userAccountToUpdate.TipoUtilizadorID = tipoUtilizadorID;

			return userAccountToUpdate;
		}

		private void UserAccountDropdownList(object userAccount = null)
		{
			ViewBag.UserAccountID = new SelectList(museuDB.Utilizadores(), "UserAccountID", "Nome", userAccount);
		}

		private void TipoUtilizadorDropdownList(object tipoUtilizador = null)
		{
			ViewBag.TipoUtilizadorID = new SelectList(museuDB.TiposUtilizadores(), "TipoUtilizadorID", "Tipo", tipoUtilizador);
		}
	}
}