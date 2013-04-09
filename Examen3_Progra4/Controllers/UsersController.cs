using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Examen3_Progra4.Models;


namespace Examen3_Progra4.Controllers
{
    public class UsersController : Controller
    {
        private MyDBEntities db = new MyDBEntities();

        public ActionResult UserLogOn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UserLogOn(logOnModel model, string returnUrl)
        {
            System.Diagnostics.Debug.WriteLine("Si entro");
            if (ModelState.IsValid)
            {
                if (validateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("MyToDo", "Todo");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        public ActionResult UserLogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("UserLogOn", "Users");
        }

        public ActionResult UserRegister()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UserRegister(registerModel model)
        {
            if (ModelState.IsValid)
            {
                string passwordConvert = FormsAuthentication.HashPasswordForStoringInConfigFile(model.Password, "SHA1");
                user myUser = new user
                {
                    email = model.UserName,
                    password = passwordConvert
                };
                try
                {
                    db.users.AddObject(myUser);
                    db.SaveChanges();
                }catch{
                    ModelState.AddModelError("", "The User already exist, can't create the new user");
                    return View(model);
                }

                FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
                return RedirectToAction("MyToDo", "Todo");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
            
        }
        

        private bool validateUser(string user,string password){
            string passwordConvert;
            passwordConvert = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "SHA1");

            var myUsers = from u in db.users
                          where u.email.Equals(user) && u.password.Equals(passwordConvert)
                          select u;
            if (myUsers.Count() == 0)
                return false;

            return true;

        }
        
    }
}
