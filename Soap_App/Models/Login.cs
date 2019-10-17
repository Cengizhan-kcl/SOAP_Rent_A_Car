using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Soap_App.Models
{
    public class Login
    {
        public string Adi { get; set; }
        public string SoyAdi { get; set; }
        public string email { get; set; }
        public string adres { get; set; }
        public string tel { get; set; }
        public DateTime ehliyetTarihi { get; set; }
        public DateTime dogumTarihi { get; set; }
        public string kullaniciAdi { get; set; }
        public string password { get; set; }
    }
}