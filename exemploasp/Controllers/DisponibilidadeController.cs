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
        readonly OurDBContext _db = new OurDBContext();

		//GET: Disponibilidade/Definir/id?exp=exp
        //envia para a view os dados necessários para apresentar as datas que o utilizador pode escolher para fazer visitas guiadas para a exposição selecionada
		[Authorize]
		public ActionResult Definir(int id, int exp)
        {
            UserAccountExposicao userAccountExposicao = _db.UserAccountExposicao.Find(id, exp);  
            if (userAccountExposicao != null)
            {
                Exposicao exposicao = _db.Exposicao.Find(userAccountExposicao.ExposicaoID);
                ViewBag.Datas = PopulateDatasExposicao(exposicao);
                return View(userAccountExposicao);
            }
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
        }

        //guarda na em bd.disponibilidade todos os dias escolhidos pelo utilizador para fazer visitas para a exposi~ção selecionada
	    [Authorize]
		[HttpPost]
        public ActionResult Definir(int id, int exp, DateTime[] selectedDatas)
        {
            UserAccountExposicao userAccountExposicao = _db.UserAccountExposicao.Find(id, exp);
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
                        _db.Disponibilidade.Add(disponibilidade);
                    }
                    _db.SaveChanges();
                    return RedirectToAction("PerfilUser", "Account", new {id = id});
                }
                return RedirectToAction("Definir", new { id = id, exp=exp });
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

		//GET: Disponibilidade/Definir/id?exp=exp
        //envia para a view os dados necessários para apresentar os dias escolhidos pelo utilizador para fazer visitas guiadas para a exposição selecionada
		[Authorize]
		public ActionResult Ver(int id, int exp)
        {
            Exposicao exposicao = _db.Exposicao.Find(exp);
            List<Disponibilidade> disponibilidades = _db.Disponibilidade.Where(d => d.ExposicaoID == exp).Where(d => d.UserAccountID == id).Include(d => d.Exposicao).Include(d => d.UserAccount).ToList();
            if (exposicao != null)
            {
                ViewBag.Exposicao = exposicao;
                
            }
            return View(disponibilidades);
        }

        //retorna uma lista com todos os dias de uma exposição
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

        //retorna um enumeravel com todos os dias entre duas datas
        public IEnumerable<DateTime> CadaDia(DateTime desde, DateTime ate)
        {
            for (var dia = desde.Date; dia.Date <= ate.Date; dia = dia.AddDays(1))
                yield return dia;
        }       
    }
}
