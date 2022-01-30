using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Forum.Data;
using Forum.Models;
using Forum.Services;

using Microsoft.AspNet.Identity;

namespace Forum.Controllers
{
    public class ThreadController : Controller
    {
        private ThreadService _threadService = null;

        public ThreadController()
        {
            _threadService = new ThreadService();
        }

        // GET: Thread
        public ActionResult Index(string topicId)
        {
            if (topicId == null || topicId == "")
            {
                return RedirectToAction("Topic", "Topic");
            }

            List<ThreadViewModel> threads = _threadService.GetThreadsByTopicId(topicId);

            if (threads != null && threads.Count > 0)
            {
                ViewBag.TopicTitle = threads[0].TopicTitle;
            }
            else
            {
                ViewBag.TopicTitle = TopicService.GetTopicTitleById(topicId);
            }

            ViewBag.TopicId = topicId;

            return View(threads);
        }

        [Authorize]
        [Authorize(Roles = Defines.Role_Admin)]
        public ActionResult AddEdit(string id, string topicId)
        {
            ThreadViewModel threadViewModel = null;

            if (id == null || id == "")
            {
                threadViewModel = new ThreadViewModel();

                threadViewModel.UserId = User.Identity.GetUserId();
                threadViewModel.TopicId = topicId;
            }
            else
            {
                threadViewModel = _threadService.GetThread(id);
            }

            return View("_addEdit", threadViewModel);
        }

        [HttpPost]
        [Authorize]
        [Authorize(Roles = Defines.Role_Admin)]
        public ActionResult Save(ThreadViewModel thread)
        {
            if (ModelState.IsValid)
            {
                if (!_threadService.Save(thread))
                {
                    return View("~/Views/Shared/Error.cshtml");
                }

                return RedirectToAction("Index", "Thread", new { threadId = thread.TopicId });
            }

            return View("_addEdit", thread);
        }

        [HttpGet]
        [Authorize]
        [Authorize(Roles = Defines.Role_Admin)]
        public ActionResult Delete(string id)
        {
            if (ModelState.IsValid)
            {
                if (!_threadService.Delete(id))
                {
                    return View("~/Views/Shared/Error.cshtml");
                }
            }

            return RedirectToAction("Index");
        }
    }
}