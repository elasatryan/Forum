using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Collections.Generic;

using Microsoft.AspNet.Identity;

using Forum.Data;
using Forum.Entities;
using Forum.Models;
using Forum.Services;
using System.Net;

namespace Forum.Controllers
{
    public class TopicController : Controller
    {
        private TopicService _topicService = null;

        public TopicController()
        {
            _topicService = new TopicService();
        }

        public ActionResult Topic()
        {
            List<TopicViewModel> topics = _topicService.GetTopics();

            return View(topics);
        }

        [HttpGet]
        [Authorize(Roles = Defines.Role_Admin)]
        public ActionResult AddEdit(string id)
        {
            TopicViewModel topicViewModel = null;

            if (id == null || id == "")
            {
                topicViewModel = new TopicViewModel();
                topicViewModel.UserId = User.Identity.GetUserId();
            }
            else
            {
                topicViewModel = _topicService.GetTopic(id);
            }

            return View("_addEdit", topicViewModel);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Save(TopicViewModel topic)
        {
            if (ModelState.IsValid)
            {
                if (!_topicService.Save(topic))
                {
                    return View("~/Views/Shared/Error.cshtml");
                }
            }

            return RedirectToAction("Topic");
        }

        [HttpGet]
        [Authorize]
        [Authorize(Roles = Defines.Role_Admin)]
        public ActionResult Delete(string id)
        {
            if (ModelState.IsValid)
            {
                if (!_topicService.Delete(id))
                {
                    return View("~/Views/Shared/Error.cshtml");
                }
            }

            return RedirectToAction("Topic");
        }
    }
}