using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using Soap_App.UserService;
using SpiceApp.Models.Entities;

namespace Soap_App.Models
{
    public class Ches
    {
        private static Ches stack = null;
        private Ches() { }

        public User user;
        public recTar tarihler;
        public static Ches Stack
        {
            get
            {
                if (stack == null)
                    stack = new Ches();
                return stack;
            }
        }
    }
}