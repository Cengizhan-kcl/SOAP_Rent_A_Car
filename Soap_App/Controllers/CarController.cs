using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SpiceApp.Models.Entities;
using Soap_App.Models;
using RestApiDenemeleri;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;

namespace Soap_App.Controllers

{
    public class CarController : Controller
    {
        // GET: Car
        public ActionResult Index()
        {
            if (Session["giris"] != null && (bool)Session["giris"] == true)
                return View();
            else
            {
                return RedirectToAction("Login", "Connect");
            }
        }
        public void Detayla(string id)
        {
            Session["id"] = null;
            Session["id"] = id;
        }
        public void Rezervle(string id)
        {
            //ReservationServiceSoapClient rsc = new ReservationServiceSoapClient();
            //ResponseOfReservation rr;
            Response<Reservation> rr;
            if (Session["Proid"].ToString() != "1")
            {
                string obj = JsonConvert.SerializeObject(new { CarID=id, UserID= Ches.Stack.user.UserID.ToString(), StartingDate= Ches.Stack.tarihler.tar1, EndDate= Ches.Stack.tarihler.tar2});
                var content = new StringContent(obj.ToString(), Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                rr = Request<Reservation>.PostAsync("http://localhost:51643/api/Reservation", content);
            }
            // rr = rsc.MakeReservation(Convert.ToInt32(id), Ches.Stack.user.UserID, Ches.Stack.tarihler.tar1, Ches.Stack.tarihler.tar2);
            else
            {
                string obj = JsonConvert.SerializeObject(new { CarID = id, UserID = Ches.Stack.user.UserID.ToString(), StartingDate = Ches.Stack.tarihler.tar1, EndDate = Ches.Stack.tarihler.tar2 });
                var content = new StringContent(obj.ToString(), Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                rr = Request<Reservation>.PostAsync("http://localhost:51643/api/Reservation", content);
            }
                //rr = rsc.MakeReservation(Convert.ToInt32(id), Convert.ToInt32(Session["id"]), Ches.Stack.tarihler.tar1, Ches.Stack.tarihler.tar2);
            //rsc.Close();
        }
        public ActionResult SirketArac()
        {
            //CarServiceSoapClient csc = new CarServiceSoapClient();
            //CarService.ResponseOfCar rc = csc.FetchAllCarsByCompanyId(Ches.Stack.user.Person.Company.CompanyID);
            //csc.Close();
            Response<Car> rc = Request<Car>.GetAsync("http://localhost:51643/api/Car/Company/"+ Ches.Stack.user.Person.Company.CompanyID.ToString());
            Session["sirketarac"] = null;
            if (rc.isSuccess)
                Session["sirketarac"] = rc.Data.ToList();
            return View();
        }
        public void Deaktifle()
        {
            //CarServiceSoapClient csc = new CarServiceSoapClient();
            //csc.DeleteCarById(Convert.ToInt32(Session["id"]));
            //csc.Close();
            Request<Car>.DeleteAsync("http://localhost:51643/api/Car/"+ Session["id"].ToString());
        }
        public void Aktifle()
        {
            //CarServiceSoapClient csc = new CarServiceSoapClient();
            //csc.ActivateCarById(Convert.ToInt32(Session["id"]));
            //csc.Close();
            Request<Car>.GetAsync("http://localhost:51643/api/Car/Activate/" + Session["id"].ToString());
        }
        public ActionResult Addnew()
        {
            //BrandServiceSoapClient bsc = new BrandServiceSoapClient();
            //ResponseOfBrand rb = bsc.FetchAllBrands();
            Response<Brand> rb = Request<Brand>.GetAsync("http://localhost:51643/api/Brand");
            Session["brand"] = null;
            Session["brand"] = rb.Data.ToList();
            //bsc.Close();
            return View();
        }
        [HttpPost]
        public ActionResult AddCar(Car model)
        {
            //CarServiceSoapClient csc = new CarServiceSoapClient();
            //CarService.ResponseOfCar rc = csc.AddNewCar(model);
            string obj = JsonConvert.SerializeObject(model);
            var content = new StringContent(obj.ToString(), Encoding.UTF8, "application/json");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            Response<Car> rc = Request<Car>.PostAsync("http://localhost:51643/api/Car",content);
            if (rc.isSuccess)
                return RedirectToAction("SirketArac", "Car");
            else
                return View("Error");
        }
    }
}