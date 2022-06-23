using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Khabar_Web.Models;

namespace Khabar_Web.Controllers
{
    public class UserController : Controller
    {
        private KhabarDBContext db = new KhabarDBContext();

        // GET: User/Login
        public ActionResult Login()
        {
            try
            {                
                Response.Cookies["UserName"].Expires = DateTime.Now.AddDays(-1);
                return View();
            }
            catch (Exception ex)
            {
                TempData["ex"] = ex.Message;
                TempData["exInfo"] = ex.InnerException.Message;
                Global.Log.Error(ex);
                return View("Error");
            }
        }

        // POST: User/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "email,password", Exclude = "passwordConfirm")] UserVM userVM)
        {
            try
            {
                ModelState.Remove("passwordConfirm");
                if (ModelState.IsValid)
                {
                    var loginUser = db.tbl_frontend_users.Where(u => u.email == userVM.email.Trim() && u.password == userVM.password.Trim()).FirstOrDefault();

                    if (loginUser == null)
                    {
                        ViewBag.WrongLogin = 1;
                        return View(userVM);
                    }
                    else
                    {
                        ViewBag.WrongLogin = 0;
                        Response.Cookies["UserName"].Value = userVM.email.Trim();
                        Session["userID"] = loginUser.FUserPK;
                        TempData["login"] = "Successfully login";
                    }
                }

                return View(userVM);
            }
            catch (Exception ex)
            {
                TempData["ex"] = ex.Message;
                TempData["exInfo"] = ex.InnerException.Message;
                Global.Log.Error(ex);
                return View("Error");
            }
        }

        // GET: User/Create
        public ActionResult Create()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                TempData["ex"] = ex.Message;
                TempData["exInfo"] = ex.InnerException.Message;
                Global.Log.Error(ex);
                return View("Error");
            }
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FUserPK,guid,displayname,email,password,passwordConfirm")] UserVM userVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int checkEmail = db.tbl_frontend_users.Where(e => e.email == userVM.email).Count();
                    if (checkEmail > 0)
                    {
                        TempData["email"] = "Wrong email";
                    }
                    else
                    {
                        var newUser = new tbl_frontend_users()
                        {
                            displayname = userVM.email,
                            email = userVM.email,
                            password = userVM.password,
                            Kind = 1,
                            indate = DateTime.Now,
                            addedby = 0
                        };
                        db.tbl_frontend_users.Add(newUser);
                        db.SaveChanges();

                        Response.Cookies["UserName"].Value = userVM.email.Trim();
                        Session["userID"] = newUser.FUserPK;
                        TempData["sign"] = "Successfully registered";
                    }
                }
                return View(userVM);
            }
            catch (Exception ex)
            {
                TempData["ex"] = ex.Message;
                TempData["exInfo"] = ex.InnerException.Message;
                Global.Log.Error(ex);
                return View("Error");
            }
        }

        // GET: User/SignOut
        public ActionResult SignOut()
        {
            try
            {
                Response.Cookies["UserName"].Expires = DateTime.Now.AddDays(-1);
                Session["userID"] = null;
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                TempData["ex"] = ex.Message;
                TempData["exInfo"] = ex.InnerException.Message;
                Global.Log.Error(ex);
                return View("Error");
            }
        }

        // GET: User/ForgetPass
        public ActionResult ForgetPass()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                TempData["ex"] = ex.Message;
                TempData["exInfo"] = ex.InnerException.Message;
                Global.Log.Error(ex);
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgetPass([Bind(Include = "email", Exclude = "password,passwordConfirm")] UserVM userVM)
        {
            try
            {
                ModelState.Remove("password");
                ModelState.Remove("passwordConfirm");
                if (ModelState.IsValid)
                {
                    var loginUser = db.tbl_frontend_users.Where(u => u.email == userVM.email.Trim()).FirstOrDefault();

                    if (loginUser == null)
                    {
                        TempData["emailWrong"] = "Wrong email";
                        return View(userVM);
                    }
                    else
                    {
                        string loginLink = "http://khabr.news/User/login";
                        SmtpClient ss = new SmtpClient("mail.khabr.news", 8889);
                        //ss.EnableSsl = false;
                        ss.Timeout = 10000;
                        ss.DeliveryMethod = SmtpDeliveryMethod.Network;
                        ss.UseDefaultCredentials = false;
                        ss.Credentials = new NetworkCredential("mail@khabr.news", "khabar@123");

                        MailMessage mm = new MailMessage("mail@khabr.news", loginUser.email, "بيانات الحساب - موقع خبر", "<div style=\"text-align:right;direction:rtl;font-size:larger;\">مرحبا بك في موقع خبر، الآن يمكنك تسجيل الدخول باستخدام البيانات التالية:<br /><b>البريد الإلكتروني: </b><br />" + loginUser.email + "<br /><b>كلمة المرور: </b><br />" + loginUser.password + "<br />" + "<a href=\"" + loginLink + "\" target=\"_blank\">" + loginLink + "</a></div>");
                        mm.IsBodyHtml = true;
                        mm.BodyEncoding = Encoding.UTF8;
                        mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                        ss.Send(mm);

                        TempData["forget"] = "Successfully Send";
                    }
                }
                return View(userVM);
            }
            catch (Exception ex)
            {
                TempData["ex"] = ex.Message;
                TempData["exInfo"] = ex.InnerException.Message;
                Global.Log.Error(ex);
                return View("Error");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
