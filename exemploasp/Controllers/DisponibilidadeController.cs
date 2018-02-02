using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using exemploasp.Models;
using exemploasp.ViewModels;

namespace exemploasp.Controllers
{
    public class DisponibilidadeController : Controller
    {
        OurDBContext db = new OurDBContext();

		//GET: Disponibilidade/Definir/id?exp=exp
		[Authorize]
		public ActionResult Definir(int id, int exp)
        {
            UserAccountExposicao userAccountExposicao = db.UserAccountExposicao.Find(id, exp);  
            if (userAccountExposicao != null)
            {
                Exposicao exposicao = db.Exposicao.Find(userAccountExposicao.ExposicaoID);
                ViewBag.Datas = PopulateDatasExposicao(exposicao);
                return View(userAccountExposicao);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
	    [Authorize]
		[HttpPost]
        public ActionResult Definir(int id, int exp, DateTime[] selectedDatas)
        {
            UserAccountExposicao userAccountExposicao = db.UserAccountExposicao.Find(id, exp);
            if (userAccountExposicao != null)
            {
                if (selectedDatas != null)
                {
                    foreach (var data in selectedDatas)
                    {
                        Disponibilidade disponibilidade = new Disponibilidade
                        {
                            DataDisponibilidade = data,
                            ExposicaoID = exp,
                            UserAccountID = id
                        };
                        db.Disponibilidade.Add(disponibilidade);
                    }
                    db.SaveChanges();
                    return RedirectToAction("PerfilUser", "Account", new {id = id});
                }
                return RedirectToAction("Definir", new { id = id, exp=exp });
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

		//GET: Disponibilidade/Definir/id?exp=exp
		[Authorize]
		public ActionResult Ver(int id, int exp)
        {
            Exposicao exposicao = db.Exposicao.Find(exp);
            List<Disponibilidade> disponibilidades = db.Disponibilidade.Where(d => d.ExposicaoID == exp).Where(d => d.UserAccountID == id).Include(d => d.Exposicao).Include(d => d.UserAccount).ToList();
            if (exposicao != null)
            {
                ViewBag.NomeExposicao = exposicao.Nome;
                
            }
            return View(disponibilidades);
        }

        public List<DataExposicao> PopulateDatasExposicao(Exposicao exposicao)
        {
            var viewModel = new List<DataExposicao>();
            var culture = new System.Globalization.CultureInfo("pt-PT");
            foreach (DateTime day in CadaDia(exposicao.DataInicial, exposicao.DataFinal))
            {
                viewModel.Add(new DataExposicao() {Data = day.Date, DiaSemana = culture.DateTimeFormat.GetDayName(day.Date.DayOfWeek)});
            }
            return viewModel;
        }

        public IEnumerable<DateTime> CadaDia(DateTime desde, DateTime ate)
        {
            for (var dia = desde.Date; dia.Date <= ate.Date; dia = dia.AddDays(1))
                yield return dia;
        }

        
    }
}
