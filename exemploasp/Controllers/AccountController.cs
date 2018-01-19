using System;
using System.Collections.Generic;
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

namespace exemploasp.Controllers
{
	public class AccountController : Controller
	{
		// GET: Account
		public ActionResult Index()
		{
			using (OurDBContext db = new OurDBContext())
			{
				return View(db.UserAccount.ToList());
			}
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
					var encrypt = Encrypt(account.Password);
					account.Password = encrypt;
				    encrypt = Encrypt(account.ConfirmPassword);
				    account.ConfirmPassword = encrypt;
                    db.UserAccount.Add(account);
					db.SaveChanges();
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
                UserAccount user = db.UserAccount.Find(id);
                if (id == null || user == null)
				{
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
				}
				
				ViewData["Utilizador"] = user;
			}
			return View();
		}
	}
}