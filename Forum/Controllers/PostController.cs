using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Forum.Models;
using Forum.Services;

namespace Forum.Controllers
{
    public class PostController : Controller
    {
        private PostService _postService = null;

        public PostController()
        {
            _postService = new PostService();
        }

        // GET: Post
        public ActionResult Index(string threadId)
        {
            if (threadId == null || threadId == "")
            {
                return RedirectToAction("Topic", "Topic");
            }

            List<PostViewModel> posts = _postService.GetPostsByThreadId(threadId);
            
            ThreadViewModel threadViewModel = ThreadService.GetThreadInfoById(threadId);

            ViewBag.ThreadTitle = "Re: " + threadViewModel.Title;
            ViewBag.Inactive = threadViewModel.Inactive;
            ViewBag.ThreadId = threadId;

            return View(posts);
        }

        public ActionResult Post(PostViewModel postViewModel)
        {
            if (ModelState.IsValid)
            {
                if (_postService.Post(postViewModel))
                {
                    return RedirectToAction("Index", new { threadId = postViewModel.ThreadId });
                }
            }

            return RedirectToAction("Index");
        }
    }
}