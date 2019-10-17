using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using RestApiDenemeleri;
using Soap_App.Models;
//using Soap_App.UserService;     //soap version
using SpiceApp.Models.Entities;   //rest version

namespace Soap_App.Controllers
{
    public class ConnectController : Controller
    {
        // GET: Connect
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        
        public ActionResult Logout()
        {
            Ches.Stack.user = null;
            Session["giris"] = false;
            Session["rezler"] = null;
            return RedirectToAction("Login", "Connect");
        }

        [HttpPost]
        public ActionResult Login(User model)
        {
            //UserServiceSoapClient usc = new UserServiceSoapClient();
            //ResponseOfUser ru= usc.Login(model.Username, model.Password);

            string body = JsonConvert.SerializeObject(model);
            var content = new StringContent(body.ToString(), Encoding.UTF8, "application/json");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            Response<User> task = Request<User>.PostAsync("http://localhost:51643/api/User/Login", content);
            var ru = task;
 
            if (ru.isSuccess)
            {
                Ches.Stack.user = ru.Data[0];
                Session["Proid"] = null;
                Session["Proid"] = ru.Data[0].Role.RoleID.ToString();
                Session["comid"] = null;
                Session["comid"] = ru.Data[0].Person.Company.CompanyID.ToString();
                Session["profil"] = null;
                Session["profil"] = ru.Data.ToList();
                Session["giris"] = ru.isSuccess;
                if(Session["Proid"].ToString() == "1" || Session["Proid"].ToString() == "1001")
                {
                    var url = "http://localhost:51643/api/User";
                    Response<User> result = Request<User>.GetAsync(url);
                    ru = result;
                    Session["costumer"] = null;
                    Session["costumer"] = ru.Data.ToList();
                }
                return RedirectToAction("Index", "User");
            }
            else
            {
                return View("Error");
            }

        }
        [HttpPost]
        public ActionResult Register(User model)
        {
            //UserServiceSoapClient usc = new UserServiceSoapClient();
            //ResponseOfUser ru = usc.RegisterUser(model);
            string obj = JsonConvert.SerializeObject(model);
            var url = "http://localhost:51643/api/User/Register";
            var content = new StringContent(obj.ToString(), Encoding.UTF8, "application/json");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            Response<User> response = Request<User>.PostAsync(url, content);
            if(response.isSuccess) return RedirectToAction("Login", "Connect");
            return View("Error");


        }
        
    }
}