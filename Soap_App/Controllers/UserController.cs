using Newtonsoft.Json;
using RestApiDenemeleri;
using Soap_App.Models;
using SpiceApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;
namespace Soap_App.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            if (Session["giris"] != null && (bool)Session["giris"] == true)
            {
                Session["rezler"] = null;
                //ReservationServiceSoapClient rsc = new ReservationServiceSoapClient();
                //ResponseOfReservation rr = rsc.FetchAllById(Ches.Stack.user.UserID);
                Response<Reservation> rr = Request<Reservation>.GetAsync("http://localhost:51643/api/Reservation/"+Ches.Stack.user.UserID.ToString());
                if (rr.isSuccess)
                    Session["rezler"] = rr.Data.ToList();
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Connect");
            }
        }
        [HttpPost]
        public void Index(string t1, string t2)
        {
            recTar model = new recTar();
            model.tar1 = Convert.ToDateTime(t1);
            model.tar2 = Convert.ToDateTime(t2);
            Session["araclar"] = null;
            //ReservationServiceSoapClient rsc = new ReservationServiceSoapClient();
            Response<Car> rc;
            if (Session["Proid"].ToString() != "1")
                rc = Request<Car>.GetAsync("http://localhost:51643/api/Reservation/Available/" + Ches.Stack.user.UserID.ToString() + "/" + model.tar1.Ticks + "/" + model.tar2.Ticks);
            //rc = rsc.FetchAvailableCarsForResv(Ches.Stack.user.UserID, model.tar1, model.tar2);
            else
                rc = Request<Car>.GetAsync("http://localhost:51643/api/Reservation/Available/" + Session["id"].ToString() + "/" + model.tar1.Ticks + "/" + model.tar2.Ticks);
                //rc = rsc.FetchAvailableCarsForResv(Convert.ToInt32(Session["id"]), model.tar1, model.tar2);
            if (rc.isSuccess)
                Session["araclar"] = rc.Data.ToList();
            Ches.Stack.tarihler = model;
        }


        public ActionResult Detayver()
        {
            if (Session["giris"] != null && (bool)Session["giris"] == true)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Connect");
            }

        }

        public void Reziptal(string id)
        {
            //ReservationServiceSoapClient rsc = new ReservationServiceSoapClient();
            //rsc.CancelReservation(Convert.ToInt32(id));
            //rsc.Close();
            Request<Reservation>.DeleteAsync("http://localhost:51643/api/Reservation/"+id);
        }
        public void Rezkabul(string id)
        {
            //RentDetailServiceSoapClient rsc = new RentDetailServiceSoapClient();
            //rsc.CompleteReservation(Convert.ToInt32(id));
            //rsc.Close();
            Request<RentDetail>.GetAsync("http://localhost:51643/api/Rent/CompleteRez/"+id);
        }
        public ActionResult Kiralılar()
        {
            if (Session["giris"] != null && (bool)Session["giris"] == true)
            {
                Session["kiralar"] = null;
                //RentDetailServiceSoapClient rds = new RentDetailServiceSoapClient();
                //ResponseOfRentDetail rd = rds.FetchAllRentDetailById(Ches.Stack.user.UserID);
                Response<RentDetail> rd = Request<RentDetail>.GetAsync("http://localhost:51643/api/Rent/GetByCst/" + Ches.Stack.user.UserID.ToString());   
                if (rd.isSuccess)
                    Session["kiralar"] = rd.Data.ToList();
                //rds.Close();
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Connect");
            }
        }
        public ActionResult Profil()
        {
            if (Session["giris"] != null && (bool)Session["giris"] == true)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Connect");
            }

        }
        [HttpPost]
        public ActionResult Profil(User model)
        {
            Response<User> ru2 = new Response<User>();
            //UserServiceSoapClient usc = new UserServiceSoapClient();
            if (model.Person.Address != null)
                Ches.Stack.user.Person.Address = model.Person.Address;
            if (model.Person.Email != null)
                Ches.Stack.user.Person.Email = model.Person.Email;
            if (model.Person.Phone != null)
                Ches.Stack.user.Person.Phone = model.Person.Phone;
            if (model.Password != null)
            {
                //ru2 = usc.ChangePassword(Ches.Stack.user.UserID, model.Password);
                string degis = JsonConvert.SerializeObject(new User() { UserID = Ches.Stack.user.UserID, Password = model.Password});
                var con = new StringContent(degis.ToString(), Encoding.UTF8, "application/json");
                con.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                ru2 = Request<User>.PostAsync("http://localhost:51643/api/User/ChangePassword",con);
            }
            string body = JsonConvert.SerializeObject(Ches.Stack.user);
            var content = new StringContent(body.ToString(), Encoding.UTF8, "application/json");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            Response<User> ru = Request<User>.PutAsync("http://localhost:51643/api/User", content);
            if (ru.isSuccess || ru2.isSuccess)
                return RedirectToAction("Index", "User");
            else
                return View("Error");
        }
        public void Rezdet(string id)
        {
            Session["id"] = null;
            Session["id"] = id;
        }

    }
}