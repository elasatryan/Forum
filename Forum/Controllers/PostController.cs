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
            ViewBag.Description = threadViewModel.Description;
            ViewBag.ThreadId = threadId;

            return View(posts);
        }

        public ActionResult GetPost(string postId)
        {
            return View("_editView", _postService.GetPostById(postId));
        }

        [Authorize]
        public ActionResult Delete(string id)
        {
            if (ModelState.IsValid)
            {
                if (!_postService.Delete(id))
                {
                    return View("~/Views/Shared/Error.cshtml");
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult ViewPost(string id)
        {
            return View("_editView", _postService.GetPostById(id));
        }

        [Authorize]
        public ActionResult Edit(string id)
        {
            if (id != null && id != "")
            {
                PostViewModel postViewModel = _postService.GetPostById(id);
                if (ModelState.IsValid)
                {
                    if (!_postService.EditPost(postViewModel))
                    {
                        return View("~/Views/Shared/Error.cshtml");
                    }
                    return View("_edit", postViewModel);
                }
            }

            return View("_edit");
        }

        [Authorize]
        public ActionResult Save(PostViewModel postViewModel)
        {
            if (ModelState.IsValid)
            {
                if (!_postService.EditPost(postViewModel))
                {
                    return View("~/Views/Shared/Error.cshtml");
                }
            }

            return RedirectToAction("Index", new { threadId = postViewModel.ThreadId });
        }

        [Authorize]
        public ActionResult Post(PostViewModel postViewModel)
        {
            if (ModelState.IsValid)
            {
                if (!_postService.Post(postViewModel))
                {
                    return View("~/Views/Shared/Error.cshtml");
                }
            }

            return RedirectToAction("Index", new { threadId = postViewModel.ThreadId });
        }
    }
}