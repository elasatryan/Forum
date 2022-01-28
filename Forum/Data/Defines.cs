using Forum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.SessionState;

namespace Forum.Data
{
    public class Defines
    {
        public const string KEY_CurrentAppManager = "CU";
        public const string KEY_DefaultConnection = "DefaultConnection";

        public static string DefaultConnection
        {
            get
            {
                return WebConfigurationManager.ConnectionStrings[KEY_DefaultConnection].ConnectionString;
            }
        }
        public static ApplicationUser CurrentAppManager { get; set; }
    }
}