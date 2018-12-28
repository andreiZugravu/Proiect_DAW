﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proiect_DAW.Models;

namespace Proiect_DAW.Controllers
{
    public class SubjectsController : Controller
    {
        private SubjectDBContext db = new SubjectDBContext();

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

        public ActionResult Show(int id)
        {
            Subject subject = db.Subjects.Find(id);
            ViewBag.Subject = subject;
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
            return View();
        }

        [HttpPut]
        public ActionResult Edit(int id, Subject requestSubject)
        {
            try
            {
                Subject subject = db.Subjects.Find(id);
                if (TryUpdateModel(subject))
                {
                    subject.Title = requestSubject.Title;
                    subject.Content = requestSubject.Content;
                    subject.Data = System.DateTime.Now.ToString();
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
        public ActionResult Delete(int id)
        {
            Subject subject = db.Subjects.Find(id);
            db.Subjects.Remove(subject);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}