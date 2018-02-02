﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using exemploasp.Models;

namespace exemploasp.Controllers
{
    public class HomeController : Controller
    {
        OurDBContext db = new OurDBContext();
	    [Authorize]
		public ActionResult Index()
        {
            int id = Int32.Parse(Session["UserAccountID"].ToString());
            List<Marcacao> marcacaos = db.Marcacao.Where(m => m.UserAccountID == id).ToList();
            return View(marcacaos);
        }
	}
}