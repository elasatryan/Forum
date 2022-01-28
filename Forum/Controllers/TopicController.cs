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

namespace Forum.Controllers
{
    public class TopicController : Controller
    {
        private TopicService _topicService = null;

        public TopicController()
        {
            _topicService = new TopicService();
        }

        // GET: Topic
        public ActionResult Topic()
        {
            List<Topic> topics = _topicService.GetTopics();
            List<TopicViewModel> topicViews = new List<TopicViewModel>();
            //todo: use automapper
            topicViews.AddRange(topics.Select(h => new TopicViewModel { Id = h.Id, UserId=h.UserId, Title = h.Title }));
            return View(topicViews);
        }

        [HttpGet]
        public ActionResult Get(string id)
        {
            return View("_topic", _topicService.GetTopic(id));
        }

        [HttpGet]
        public ActionResult Add()
        {
            Topic topicViewModel = new Topic();
            topicViewModel.UserId = User.Identity.GetUserId();

            return View("_add", topicViewModel);
        }

        [HttpPost]
        public ActionResult Save(TopicViewModel topic)
        {
            _topicService.Save(topic);

            return RedirectToAction("Topic");
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            _topicService.Delete(id);

            return RedirectToAction("Topic");
        }
    }
}