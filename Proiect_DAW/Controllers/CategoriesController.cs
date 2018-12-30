using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proiect_DAW.Models;
using PagedList;
using PagedList.Mvc;

namespace Proiect_DAW.Controllers
{
    public class CategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var categories = from category in db.Categories
                             orderby category.Name
                             select category;
            ViewBag.Categories = categories;

            var subjects = from subject in db.Subjects
                           orderby subject.CategoryId
                           select subject;
            ViewBag.Subjects = subjects;

            return View();
        }

        public ActionResult Show(int id, int? page, string Field, string Order)
        {
            Category category = db.Categories.Find(id);
            ViewBag.Category = category;

            int pageSize = 5;
            int pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            if (Field != null && Order != null)
            {
                if (Field == "Title" && Order == "Ascending")
                {
                    PagedList<Subject> subjects = new PagedList<Subject>(category.Subjects.OrderBy(m => m.Title), pageIndex, pageSize);
                    ViewBag.Subjects = subjects;
                }
                else if (Field == "Title" && Order == "Descending")
                {
                    PagedList<Subject> subjects = new PagedList<Subject>(category.Subjects.OrderByDescending(m => m.Title), pageIndex, pageSize);
                    ViewBag.Subjects = subjects;
                }
                else if (Field == "Data" && Order == "Ascending")
                {
                    PagedList<Subject> subjects = new PagedList<Subject>(category.Subjects.OrderBy(m => m.Data), pageIndex, pageSize);
                    ViewBag.Subjects = subjects;
                }
                else if (Field == "Data" && Order == "Descending")
                {
                    PagedList<Subject> subjects = new PagedList<Subject>(category.Subjects.OrderByDescending(m => m.Data), pageIndex, pageSize);
                    ViewBag.Subjects = subjects;
                }
                ViewBag.Field = Field;
                ViewBag.Order = Order;
            }
            else
            {
                PagedList<Subject> subjects = new PagedList<Subject>(category.Subjects, pageIndex, pageSize);
                ViewBag.Subjects = subjects;
            }

            return View();
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult New()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult New(Category category)
        {
            try
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View();
            }
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int id)
        {
            Category category = db.Categories.Find(id);
            ViewBag.Category = category;
            return View();
        }

        [HttpPut]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int id, Category requestCategory)
        {
            try
            {
                Category category = db.Categories.Find(id);
                if (TryUpdateModel(category))
                {
                    category.Name = requestCategory.Name;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View();
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}