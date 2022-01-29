using Forum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Forum.Controllers
{
    public class HomeController : Controller
    {
        //public ActionResult Index()
        //{
        //    Dictionary<string, object> mainObjects = new Dictionary<string, object>();
        //    TopicContext topicContext = new TopicContext();
        //    mainObjects.Add("topics", topicContext.Topics.ToList());
        //    return View(mainObjects);
        //}
        
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}