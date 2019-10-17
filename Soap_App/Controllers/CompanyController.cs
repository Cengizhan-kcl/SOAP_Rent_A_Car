using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using RestApiDenemeleri;
using SpiceApp.Models.Entities;
namespace Soap_App.Controllers
{
    public class CompanyController : Controller
    {
        static string p = "", k = "";
        // GET: Company
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Rezdetay()
        {
            if (Session["giris"] != null && (bool)Session["giris"] == true)
            {
                return View("Rezdetay");
            }
            else
            {
                return RedirectToAction("Login", "Connect");
            }
        }
        public void teslim(string puani, string mesafe)
        {
            if (puani != "0")
                p = puani;
            else
                p = "1";
            if (mesafe != "")
                k = mesafe;
            else
                k = "0";
        }
        public ActionResult Teslimal()
        {
            //RentDetailServiceSoapClient rdc = new RentDetailServiceSoapClient();
            //ResponseOfRentDetail rd = rdc.ReturnCarToCompany(Convert.ToInt32(Session["id"].ToString()), Convert.ToInt32(k), Convert.ToInt32(p));
            //rdc.Close();
            string degis = JsonConvert.SerializeObject(new { RentID = Session["id"].ToString(), KmInfo=k , Score=p });
            var content = new StringContent(degis.ToString(), Encoding.UTF8, "application/json");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            Response<RentDetail> rd = Request<RentDetail>.PostAsync("http://localhost:51643/api/Rent", content);
            if (rd.isSuccess)
                return RedirectToAction("Kiralılar", "User");
            else
                return View("Error");
        }
    }
}