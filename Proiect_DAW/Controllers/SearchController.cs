using Proiect_DAW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

namespace Proiect_DAW.Controllers
{
    public class SearchController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Search
        public ActionResult Index(string SearchStr, int? pageSubject, int? pageReply)
        {
            string SearchStrLower = SearchStr.ToLower();
            var subjects = from subject in db.Subjects
                           where (subject.Title.ToLower().Contains(SearchStrLower))
                           || (subject.Content.ToLower().Contains(SearchStrLower))
                           orderby subject.Title
                           select subject;
            var replies = from reply in db.Replies
                           where reply.Content.ToLower().Contains(SearchStrLower)
                           orderby reply.Content
                           select reply;

            int pageSize = 5;
            int pageIndex = pageSubject.HasValue ? Convert.ToInt32(pageSubject) : 1;
            PagedList<Subject> subjectsPaged = new PagedList<Subject>(subjects, pageIndex, pageSize);
            ViewBag.Subjects = subjectsPaged;

            pageIndex = pageReply.HasValue ? Convert.ToInt32(pageReply) : 1;
            PagedList<Reply> repliesPaged = new PagedList<Reply>(replies, pageIndex, pageSize);
            ViewBag.Replies = repliesPaged;

            ViewBag.SearchStr = SearchStr;

            return View();
        }
    }
}