using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SpiceApp.Models.Templates;
using RestApiDenemeleri;
using Soap_App.Models;

namespace Soap_App.Controllers
{
    public class ManagerController : Controller
    {
        // GET: Manager
        public ActionResult Index()
        {
            Session["dailyKm"] = null;
            Session["dailyKm"] = Request<DailyKmInfo>.GetAsync("http://localhost:51643/api/Report/DailyKm/"+Ches.Stack.user.UserID.ToString()).Data;
            Session["comBalIn"] = null;
            Session["comBalIn"] = Request<CompanyBalanceInfo>.GetAsync("http://localhost:51643/api/Report/Balance/" + Ches.Stack.user.UserID.ToString()).Data;
            Session["ovKmIn"] = null;
            Session["ovKmIn"] = Request<OverKmInfo>.GetAsync("http://localhost:51643/api/Report/OverKm/" + Ches.Stack.user.UserID.ToString()).Data;
            Session["rentRate"] = null;
            if (Session["raporTar"] != null) {
                string ver = Session["raporTar"].ToString();
                Session["rentRate"] = Request<RentRate>.GetAsync("http://localhost:51643/api/Report/MonthlyRent/" + Ches.Stack.user.UserID.ToString() + "/" + DateTime.Parse(ver).Ticks).Data;
            }
            return View();
        }
        public void RentID(string id)
        {
            Session["rent_ID"] = null;
            Session["rent_ID"] = id;
        }
        [HttpPost]
        public void tar(string t1)
        {
            Session["raporTar"] = null;
            Session["raporTar"] = t1;
        }
        public ActionResult Rentdetay()
        {
            Session["rent_detay"] = null;
            Session["rent_detay"] = Request<DailyKmInfo>.GetAsync("http://localhost:51643/api/Report/DailyKmByRent/" + Ches.Stack.user.UserID.ToString() + "/" + Session["rent_ID"].ToString()).Data;
            return View();
        }
    }
}