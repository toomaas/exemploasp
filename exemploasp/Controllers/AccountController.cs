﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
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
				return View(db.userAccount.ToList());
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
					db.userAccount.Add(account);
					db.SaveChanges();
				}
				ModelState.Clear();
				ViewBag.Message = account.Nome + " registado";
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
			using (OurDBContext db = new OurDBContext())
			{
				var usr = db.userAccount.Where(u => u.Email == user.Email && u.Password == user.Password).FirstOrDefault();
				if (usr != null)
				{
					Session["UserID"] = usr.UserID.ToString();
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
			if (Session["UserId"] != null)
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
			if (Session["UserId"] != null)
			{
				Session["UserId"] = null;


			}
			return RedirectToAction("LoggedIn");
		}

		public ActionResult PerfilUser(int id)
		{
			using (OurDBContext db = new OurDBContext())
			{
				if (id == null)
				{
					return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
				}
				UserAccount user = db.userAccount.Find(id);
				ViewBag.Username = user.Nome;

				UserAccount username = new UserAccount
				{
					Nome = user.Nome,
					Email = user.Email,
					Morada = user.Morada,
					Sexo = user.Sexo,
					Idade = user.Idade,
					NumTelefone = user.NumTelefone
				};

				ViewData["Utilizador"] = username;

				

			}

			return View();
		}
	}
}