using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Proiect_DAW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Proiect_DAW.Controllers
{
    [Authorize(Roles = "User,Administrator")]
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        // GET: Admin
        public ActionResult Index()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            IdentityRole editorRole = roleManager.FindByName("Editor");
            IdentityRole adminRole = roleManager.FindByName("Administrator");
            var editorUsers = new List<ApplicationUser>();
            var notEditorUsers = new List<ApplicationUser>();
            foreach (ApplicationUser user in db.Users)
            {
                var isAdmin = false;
                foreach (IdentityUserRole role in user.Roles)
                {
                    if (role.RoleId.Equals(adminRole.Id)) //e admin, nu il includ
                    {
                        isAdmin = true;
                        break;
                    }
                }

                if (isAdmin == false)
                {
                    var isEditor = false;
                    foreach (IdentityUserRole role in user.Roles)
                    {
                        if (role.RoleId.Equals(editorRole.Id)) //e editor
                        {
                            editorUsers.Add(user);
                            isEditor = true;
                            break;
                        }
                    }

                    if (isEditor == false)
                    {
                        notEditorUsers.Add(user);
                    }
                }
            }

            ViewBag.editorUsers = editorUsers;
            ViewBag.notEditorUsers = notEditorUsers;
            ViewBag.allUsers = db.Users;

            return View();
        }

        [HttpPost]
        public ActionResult New_editor(string UserName)
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var user = UserManager.FindById(UserName);
            UserManager.AddToRole(user.Id, "Editor");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete_editor(string UserName)
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var user = UserManager.FindById(UserName);
            UserManager.RemoveFromRole(user.Id, "Editor");
            return RedirectToAction("Index");
        }
    }
}