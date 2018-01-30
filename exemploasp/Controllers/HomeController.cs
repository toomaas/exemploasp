using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using exemploasp.Models;

namespace exemploasp.Controllers
{
    public class HomeController : Controller
    {
        OurDBContext db = new OurDBContext();
        public ActionResult Index()
        {
            int id = Int32.Parse(Session["UserAccountID"].ToString());
            List<Marcacao> marcacaos = db.Marcacao.Where(m => m.UserAccountID == id).ToList();
            return View(marcacaos);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}