﻿using System;
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
using System.Web.Security;
using exemploasp.InteractDB;
using exemploasp.Models;
using exemploasp.Patterns.TemplateMethod;
using exemploasp.ViewModels;

namespace exemploasp.Controllers
{
	public class AccountController : Controller
	{
	    OurDBContext db = new OurDBContext();
		MuseuInteractDb museuDB = new MuseuInteractDb();

        // GET: Account mostra todos os utilizadores
		[Authorize(Users = "Administrador")]
        public ActionResult Index()
		{
			var user = db.UserAccount.Include(t => t.TipoUtilizador).Include(u => u.Temas);
			return View(user.ToList());
		}

	    public ActionResult Register()
	    {
	        Session.Remove("UserAccountID");
	        Session.Remove("Username");
	        FormsAuthentication.SignOut();
            return View();
	    }

        //valida e salva na base de dados um novo utilizador, se válido. utiliza o template method
		[HttpPost]
		public ActionResult Register(UserAccount account)
		{
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

		[AllowAnonymous]
		//pagina de login
		public ActionResult Login()
		{
		    Session.Remove("UserAccountID");
		    Session.Remove("Username");
            FormsAuthentication.SignOut();
            return View();
		}

        //verifica se existe um utilizador na bd com o nome e password e faz a sua autenticação
		[AllowAnonymous]
		[HttpPost]
		public ActionResult Login(UserAccount user)
		{
			var encrypt = museuDB.Encrypt(user.Password);
            var usr = db.UserAccount.FirstOrDefault(u => u.Email == user.Email && u.Password == encrypt);
			if (usr != null)
			{			
				FormsAuthentication.SetAuthCookie(usr.TipoUtilizador.Tipo, true);
				Session["UserAccountID"] = usr.UserAccountID.ToString();
				Session["Username"] = usr.Nome;
				Session["TipoUtilizador"] = usr.TipoUtilizador.Tipo;				
				return RedirectToAction("Index","Home");
			}
			ModelState.AddModelError("", "username ou a pass estao mal");
			return View();
		}

        //redireciona para dentro do site ou para o login, se está autenticado ou não, respetivamente
		public ActionResult LoggedIn()
		{
			if (Session["UserAccountID"] != null)
			{
				return RedirectToAction("Index","Home");
			}
			return RedirectToAction("Login");
		}

        //remove a autenticação do utilizador
		public ActionResult Logout()
		{
			if (Session["UserAccountID"] != null)
			{
				FormsAuthentication.SignOut();
				Session["UserAccountID"] = null;
			}
			return RedirectToAction("LoggedIn");
		}

		//"int? id" signfica que o parametro id pode ter um valor inteiro ou pode receber um valor null
        //envia para a view os dados do utilizador e temas dominados pelo utilizador e também as exposições aceites
		[Authorize]
		public ActionResult PerfilUser()
		{
		    int id = Convert.ToInt32(Session["UserAccountID"]);
		    UserAccount user = db.UserAccount.Include(t => t.TipoUtilizador).Include(u => u.Temas).Include(u => u.Disponibilidades).Include(a => a.UserAccountExposicaos.Select(e=>e.Exposicao)).SingleOrDefault(u => u.UserAccountID == id);
			var exp = db.Exposicao.Where(a=>a.UserAccountExposicaos.Any(c=>c.UserAccountID ==id && c.Assigned==4)).ToList();
			if (user == null)
		    {
		        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
		    }
		    if(TempData["Message"] != null) ViewBag.Message = TempData["Message"].ToString();
		    ViewBag.Temas = museuDB.PopulateAssignedTemaData(user);
            return View(user);
		}

        //atualiza os temas dominados pelo utilizador
		[Authorize]
		[HttpPost]
	    public ActionResult PerfilUser(string[] selectedTemas)
		{
		    int id = Convert.ToInt32(Session["UserAccountID"]);
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

        //manda para a view os dados do utilizador, para que possam ser editados
		[Authorize]
		public ActionResult Edit(int? id)
		{
            if(id == null)
	          id = Convert.ToInt32(Session["UserAccountID"]);
            UserAccount user = db.UserAccount.Include(t => t.TipoUtilizador).Include(u => u.Temas).SingleOrDefault(u => u.UserAccountID == id);
	        if (user == null)
	        {
	            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
	        }
	        ViewBag.Temas = museuDB.PopulateAssignedTemaData(user);
            return View(user);   
	    }

        //atualiza os dados do utilizador
		[Authorize]
		[HttpPost]
	    public ActionResult Edit(int UserAccountID,string nome, string morada, int numTelefone)
		{
		    int id = UserAccountID;//Convert.ToInt32(Session["UserAccountID"]);
	        var userAccountToUpdate = db.UserAccount.Include(u => u.Temas).SingleOrDefault(u => u.UserAccountID == id);
            if (TryUpdateModel(museuDB.EditUser(userAccountToUpdate, nome, morada, numTelefone), "",
	            new string[] { "Nome,Morada,Idade,Sexo,NumTelefone,Email,Password,ConfirmPassword,TipoUtilizadorID" }))
	        {
	            try
	            {
                    db.Entry(museuDB.EditUser(userAccountToUpdate, nome, morada, numTelefone)).State = EntityState.Modified;
	                db.SaveChanges();
	                return RedirectToAction("PerfilUser");
	            }
	            catch (RetryLimitExceededException /* dex */)
	            {
	                ModelState.AddModelError("", "Nao foi possivel atualizar o user");
	            }
	        }
	        return View(museuDB.EditUser(userAccountToUpdate, nome, morada, numTelefone));
	    }

        //envia para a view dados do utilizador que quer alterar a password
		[Authorize]
		public ActionResult AlterarPassword()
	    {
	        int id = Convert.ToInt32(Session["UserAccountID"]);
            var userAccountToUpdate = db.UserAccount.SingleOrDefault(u => u.UserAccountID == id);
            if (userAccountToUpdate == null)
	        {
	            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
	        }
	        ViewBag.Temas = museuDB.PopulateAssignedTemaData(userAccountToUpdate);
            return View(userAccountToUpdate);
	    }

        //se as verificações de password estão válidas então altera para a nova password
		[Authorize]
		[HttpPost]
	    public ActionResult AlterarPassword(string pwAntiga, string Password, string confpw)
	    {
	        int id = Convert.ToInt32(Session["UserAccountID"]);
            UserAccount user = db.UserAccount.SingleOrDefault(u => u.UserAccountID == id);
	        if (user == null)
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
                return RedirectToAction("PerfilUser");
            }
	        ModelState.AddModelError("", "Nao foi possivel atualizar a password");
	        ViewBag.Temas = museuDB.PopulateAssignedTemaData(user);
            return View(user);
	    }

        //envia para a view dados para criar duas seleclists para selecionar o utilizador e o seu novo tipo de utilizador
		[Authorize(Users = "Administrador")]
		public ActionResult Funcao()
		{
            UserAccountDropdownList();
			TipoUtilizadorDropdownList();
			var users = db.UserAccount.Include(t => t.TipoUtilizador);
			return View(users.ToList());
		}

        //altera o tipo de utilizador do utilizador selecionado
		[Authorize]
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

        //retorna a conta com o novo tipo de utilizador
		public UserAccount userAccountUpdate(int userAccountID, int tipoUtilizadorID)
		{
			var userAccountToUpdate = db.UserAccount.Single(u => u.UserAccountID == userAccountID);
			userAccountToUpdate.TipoUtilizadorID = tipoUtilizadorID;
			return userAccountToUpdate;
		}

        //envia para a view todos os utilizadores
		private void UserAccountDropdownList(object userAccount = null)
		{
			ViewBag.UserAccountID = new SelectList(museuDB.Utilizadores(), "UserAccountID", "Nome", userAccount);
		}

        //envia para a view todos os tipos de utilizadores
		private void TipoUtilizadorDropdownList(object tipoUtilizador = null)
		{
			ViewBag.TipoUtilizadorID = new SelectList(museuDB.TiposUtilizadores(), "TipoUtilizadorID", "Tipo", tipoUtilizador);
		}
	}
}