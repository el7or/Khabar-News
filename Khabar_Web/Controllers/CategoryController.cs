using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Khabar_Web.Models;

namespace Khabar_Web.Controllers
{
    public class CategoryController : Controller
    {
        private KhabarDBContext db = new KhabarDBContext();

        // GET: Category
        public ActionResult Index()
        {
            try
            {
                string kind = "1";
                if (Request.Cookies["UserName"] != null)
                {
                    string email = Request.Cookies["UserName"].Value.ToString();
                    kind = db.tbl_frontend_users.Where(e => e.email == email).FirstOrDefault().Kind.ToString();
                }
                var cats = db.tbl_category.Where(s => s.bdeleted == 0 || s.bdeleted == null).Select(c => new CategoryVM()
                {
                    CatID = c.CatPK,
                    Title = c.title,
                    Info = c.info,
                    Image = (c.icon_url == null ? "/images/icons_cats_2.png" : "http://admin.khabr.news/images/" + c.icon_url),
                    SourceCount = db.tbl_source.Where(s => ("," + s.kindlist).Contains(","+kind+ ",") && ("," + s.sCats ).Contains("," + c.CatPK + ",") && s.bdeleted == false).Count()
                }).Where(c => c.SourceCount > 0).ToList();
                return View(cats);
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
