using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proiect_DAW.Models;
using Microsoft.AspNet.Identity;
using PagedList;
using PagedList.Mvc;
using System.Web.Security;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Proiect_DAW.Controllers
{
    public class SubjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            /*//de creat rapid prima data, ca sa creeze tabelul
            Categorie categorie = new Categorie();
            categorie.Nume = "Test";
            db.Categorii.Add(categorie);
            db.SaveChanges();
            */
            var subjects = from subject in db.Subjects
                           orderby subject.Title
                           select subject;
            ViewBag.Subjects = subjects;
            return View();
        }

        public ActionResult Show(int id, int? page)
        {
            Subject subject = db.Subjects.Find(id);
            if (TryUpdateModel(subject))
            {
                subject.NumberOfViews = subject.NumberOfViews + 1;
                db.SaveChanges();
            }
            ViewBag.Subject = subject;

            int pageSize = 5;
            int pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            PagedList<Reply> replies = new PagedList<Reply>(subject.Replies, pageIndex, pageSize);
            ViewBag.Replies = replies;

            return View();
        }

        [Authorize(Roles = "User,Editor,Administrator")]
        public ActionResult New(int? CategoryId)
        {
            ViewBag.CategoriesIds = db.Categories;
            ViewBag.CategoryId = CategoryId;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "User,Editor,Administrator")]
        public ActionResult New(Subject subject)
        {
            try
            {
                subject.Data = System.DateTime.Now.ToString();
                subject.UserId = User.Identity.GetUserId();
                subject.NumberOfViews = 0;
                db.Subjects.Add(subject);
                db.SaveChanges();
                return RedirectToAction("Show", new { id = subject.SubjectId });
            }
            catch (Exception e)
            {
                return View();
            }
        }

        [HttpPost]
        [Authorize(Roles = "User,Editor,Administrator")]
        public ActionResult Reply_New(Reply model)
        {
            try
            {
                Reply reply = new Reply();
                reply.Content = model.Content;
                reply.Data = System.DateTime.Now.ToString();
                reply.SubjectId = model.SubjectId;
                reply.UserId = User.Identity.GetUserId();
                db.Replies.Add(reply);
                db.SaveChanges();
                return RedirectToAction("Show", new { id = model.SubjectId });
            }
            catch (Exception e)
            {
                return View();
            }
        }

        [Authorize(Roles = "User,Editor,Administrator")]
        public ActionResult Edit(int id)
        {
            Subject subject = db.Subjects.Find(id);
            ViewBag.Subject = subject;
            ViewBag.CategoriesIds = db.Categories;

            if (subject.UserId == User.Identity.GetUserId() || User.IsInRole("Editor") || User.IsInRole("Administrator"))
            {
                return View();
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unui articol care nu va apartine!";
                return RedirectToAction("Show", new { id });
            }
        }

        [HttpPut]
        [Authorize(Roles = "User,Editor,Administrator")]
        public ActionResult Edit(int id, Subject requestSubject)
        {
            try
            {
                Subject subject = db.Subjects.Find(id);
                if (subject.UserId == User.Identity.GetUserId() || User.IsInRole("Editor") || User.IsInRole("Administrator"))
                {
                    if (TryUpdateModel(subject))
                    {
                        subject.Title = requestSubject.Title;
                        subject.Content = requestSubject.Content;
                        subject.Data = System.DateTime.Now.ToString();
                        db.SaveChanges();
                    }
                    return RedirectToAction("Show", "Categories", new { id = subject.CategoryId });
                }
                else
                {
                    TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unui articol care nu va apartine!";
                    return RedirectToAction("Show", "Categories", new { id = subject.CategoryId });
                }
            }
            catch (Exception e)
            {
                return View();
            }
        }

        [Authorize(Roles = "User,Editor,Administrator")]
        public ActionResult Reply_Edit(int id)
        {
            Reply reply = db.Replies.Find(id);
            ViewBag.Reply = reply;

            if (reply.UserId == User.Identity.GetUserId() || User.IsInRole("Editor") || User.IsInRole("Administrator"))
            {
                return View();
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unui articol care nu va apartine!";
                return RedirectToAction("Show", new { id = reply.Subject.SubjectId });
            }
        }

        [HttpPut]
        [Authorize(Roles = "User,Editor,Administrator")]
        public ActionResult Reply_Edit(int id, Reply requestReply)
        {
            try
            {
                Reply reply = db.Replies.Find(id);
                if (reply.UserId == User.Identity.GetUserId() || User.IsInRole("Editor") || User.IsInRole("Administrator"))
                {
                    if (TryUpdateModel(reply))
                    {
                        reply.Content = requestReply.Content;
                        reply.Data = System.DateTime.Now.ToString();
                        db.SaveChanges();
                    }
                    return RedirectToAction("Show", new { id = reply.SubjectId });
                }
                else
                {
                    TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unui articol care nu va apartine!";
                    return RedirectToAction("Show", new { id = reply.SubjectId });
                }
            }
            catch (Exception e)
            {
                return View();
            }
        }

        [HttpDelete]
        [Authorize(Roles = "User,Editor,Administrator")]
        public ActionResult Delete(int id)
        {
            Subject subject = db.Subjects.Find(id);
            if (subject.UserId == User.Identity.GetUserId() || User.IsInRole("Editor") || User.IsInRole("Administrator"))
            {
                db.Subjects.Remove(subject);
                db.SaveChanges();
                return RedirectToAction("Show", "Categories", new { id = subject.CategoryId });
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unui articol care nu va apartine!";
                return RedirectToAction("Show", "Categories", new { id = subject.CategoryId });
            }
        }

        [HttpDelete]
        [Authorize(Roles = "User,Editor,Administrator")]
        public ActionResult Reply_Delete(int id)
        {
            Reply reply = db.Replies.Find(id);
            if (reply.UserId == User.Identity.GetUserId() || User.IsInRole("Editor") || User.IsInRole("Administrator"))
            {
                db.Replies.Remove(reply);
                db.SaveChanges();
                return RedirectToAction("Show", new { id = reply.SubjectId });
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unui articol care nu va apartine!";
                return RedirectToAction("Show", new { id = reply.SubjectId });
            }
        }
    }
}