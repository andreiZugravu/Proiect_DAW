using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proiect_DAW.Models;
using Microsoft.AspNet.Identity;
using PagedList;
using PagedList.Mvc;

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
            ViewBag.Subject = subject;

            int pageSize = 5;
            int pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            PagedList<Reply> replies = new PagedList<Reply>(subject.Replies, pageIndex, pageSize);
            ViewBag.Replies = replies;

            return View();
        }

        public ActionResult New()
        {
            ViewBag.CategoriesIds = db.Categories;
            return View();
        }

        [HttpPost]
        public ActionResult New(Subject subject)
        {
            try
            {
                subject.Data = System.DateTime.Now.ToString();
                subject.UserId = User.Identity.GetUserId();
                subject.NumberOfViews = 0;
                db.Subjects.Add(subject);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            Subject subject = db.Subjects.Find(id);
            ViewBag.Subject = subject;

            if (subject.UserId == User.Identity.GetUserId() || User.IsInRole("Editor") || User.IsInRole("Administrator"))
            {
                return View();
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unui articol care nu va apartine!";
                return RedirectToAction("Index"); //TODO
            }
        }

        [HttpPut]
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
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unui articol care nu va apartine!";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                return View();
            }
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Subject subject = db.Subjects.Find(id);
            if (subject.UserId == User.Identity.GetUserId() || User.IsInRole("Editor") || User.IsInRole("Administrator"))
            {
                db.Subjects.Remove(subject);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unui articol care nu va apartine!";
                return RedirectToAction("Index");
            }
        }
    }
}