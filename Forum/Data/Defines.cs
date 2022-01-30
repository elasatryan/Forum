using Forum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.SessionState;

namespace Forum.Data
{
    public static class Defines
    {
        static Defines()
        {
            DefaultConnection = WebConfigurationManager.ConnectionStrings[KEY_DefaultConnection].ConnectionString;
        }

        public const string KEY_CurrentAppManager = "CU";
        public const string KEY_DefaultConnection = "DefaultConnection";

        public static string DefaultConnection { get; set; }

        public static ApplicationUser CurrentAppManager { get; set; }

        public const string Role_Admin = "admin";
        public const string Role_User = "user";
    }
}