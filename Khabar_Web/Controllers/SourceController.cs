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
    public class SourceController : Controller
    {
        private KhabarDBContext db = new KhabarDBContext();

        // GET: Source
        public ActionResult Index(int? id)
        {
            try
            {
                string followingSrc = "";
                string kind = "1";
                var lastday = DateTime.Now.Date.AddDays(-1);
                if (Request.Cookies["UserName"] == null)
                {
                    if (Request.Cookies["UserSrc"] != null && Request.Cookies["UserSrc"].Value.ToString() != "")
                    {
                        followingSrc = "," + Request.Cookies["UserSrc"].Value.ToString();
                    }
                }
                else
                {
                    string email = Request.Cookies["UserName"].Value.ToString();
                    var user = db.tbl_frontend_users.Where(e => e.email == email).FirstOrDefault();
                    followingSrc = "," + user.following_sources;
                    kind = user.Kind.ToString();
                }
                if (id == null)
                {
                    var allSrc = db.tbl_source.Where(s => ("," + s.kindlist).Contains(","+kind+ ",") && s.bdeleted == false).Select(s => new SourceVM()
                    {
                        SrcID = s.SourcePK,
                        Title = s.title,
                        Info = s.info,
                        Image = (s.icon_url == null ? "/images/icons_source.png" : "http://admin.khabr.news/images/" + s.icon_url),
                        NewsCount = s.tbl_today_news.Where(d => d.pubdate >= lastday).Count(),
                        isFollow = (followingSrc.Contains("," + s.SourcePK.ToString() + ",") ? true : false)
                    }).Where(n => n.NewsCount > 0).ToList();
                    return View(allSrc);
                }
                else
                {
                    var srcByCats = db.tbl_source.Where(s => ("," + s.kindlist).Contains("," + kind + ",") && ("," + s.sCats).Contains("," + id + ",") && s.bdeleted == false).Select(s => new SourceVM()
                    {
                        SrcID = s.SourcePK,
                        Title = s.title,
                        Info = s.info,
                        Image = (s.icon_url == null ? "/images/icons_source.png" : "http://admin.khabr.news/images/" + s.icon_url),
                        NewsCount = s.tbl_today_news.Where(d => d.pubdate >= lastday).Count(),
                        isFollow = (followingSrc.Contains("," + s.SourcePK.ToString() + ",") ? true : false)
                    }).Where(n => n.NewsCount > 0).ToList();
                    return View(srcByCats);
                }
            }
            catch (Exception ex)
            {
                TempData["ex"] = ex.Message;
                TempData["exInfo"] = ex.InnerException.Message;
                Global.Log.Error(ex);
                return View("Error");
            }
        }

        // Save sources to User
        public JsonResult SaveSource(string src, bool isAdd)
        {
            if (isAdd)
            {
                if (Request.Cookies["UserName"] == null)
                {
                    if (Request.Cookies["UserSrc"] == null || Request.Cookies["UserSrc"].Value.ToString() =="")
                    {
                        Response.Cookies["UserSrc"].Value = src + ",";
                    }
                    Response.Cookies["UserSrc"].Value =  Request.Cookies["UserSrc"].Value.ToString() + src + ",";
                }
                else
                {
                    string email = Request.Cookies["UserName"].Value.ToString();
                    var user = db.tbl_frontend_users.Where(e => e.email == email).FirstOrDefault();
                    user.following_sources += src + ",";
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            else
            {
                if (Request.Cookies["UserName"] == null)
                {
                    Response.Cookies["UserSrc"].Value = Request.Cookies["UserSrc"].Value.ToString().Replace(src + ",", "");
                }
                else
                {
                    string email = Request.Cookies["UserName"].Value.ToString();
                    var user = db.tbl_frontend_users.Where(e => e.email == email).FirstOrDefault();
                    user.following_sources = user.following_sources.Replace(src + ",", "");
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            return Json(JsonRequestBehavior.AllowGet);
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
