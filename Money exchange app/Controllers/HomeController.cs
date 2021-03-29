using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Money_exchange_app.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Money_exchange_app.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        List<string> currencys;
        HistoryDB historyDB;

        public HomeController(ILogger<HomeController> logger,HistoryDB dB)
        {
            historyDB = dB;
            _logger = logger;
            currencys = WebAdapter.GetCurrencys();
        }

        public IActionResult Index()
        {
            ViewBag.Currencys = new SelectList(currencys);
            return View();
        }
        [HttpPost]
        public ActionResult<string> Index(string fromData, string toData, string amount)
        {
            try
            {
                double fromAmount, toAmount;
                if (!double.TryParse(amount,out fromAmount))
                {
                    return Problem("Bad incoming value");
                }else
                {                    
                    string cost = WebAdapter.GetCostOf(fromData, toData);
                    toAmount = double.Parse(cost)*fromAmount;
                    History operation = new History { FromCurrency = fromData, FromAmount = fromAmount, ToCurrency = toData, ToAmount = toAmount, Date = DateTime.Now.ToString("dd.MM.yyyy HH:mm") };
                    historyDB.History.Add(operation);
                    historyDB.SaveChanges();
                    return Ok(cost);
                }
                
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        public IActionResult History(int? Id,string FromCurrency,string FromAmount,string ToCurrency,string ToAmount,string Date)
        {
            ViewBag.Currencys = new SelectList(currencys);

            IQueryable<History> historyData=historyDB.Set<History>();
            if(Id!=null)
            {
                historyData= historyData.Where(x => x.Id.ToString().StartsWith(Id.ToString()));
            }
            if (FromCurrency!="All"&&!String.IsNullOrEmpty(FromCurrency))
            {
                historyData = historyData.Where(x => x.FromCurrency.StartsWith(FromCurrency));
            }
            if (!String.IsNullOrEmpty(FromAmount))
            {
                historyData = historyData.Where(x => x.FromAmount.ToString().StartsWith(FromAmount));
            }
            if (ToCurrency != "All" && !String.IsNullOrEmpty(FromCurrency))
            {
                historyData = historyData.Where(x => x.ToCurrency.StartsWith(ToCurrency));
            }
            if (!String.IsNullOrEmpty(ToAmount))
            {
                historyData = historyData.Where(x => x.ToAmount.ToString().StartsWith(ToAmount));
            }
            if (!String.IsNullOrEmpty(Date))
            {
                historyData = historyData.Where(x => x.Date.StartsWith(Date));
            }


            ViewBag.HistoryData = historyData.ToList();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
