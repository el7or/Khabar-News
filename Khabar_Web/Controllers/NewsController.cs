using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Khabar_Web.Models;

namespace Khabar_Web.Controllers
{
    public class NewsController : Controller
    {
        private KhabarDBContext db = new KhabarDBContext();

        // GET: News
        public ActionResult Index(int? id, ListNewsType listNewsType)
        {
            try
            {
                string kind = "1";
                if (Request.Cookies["UserName"] != null)
                {
                    string email = Request.Cookies["UserName"].Value.ToString();
                    kind = db.tbl_frontend_users.Where(e => e.email == email).FirstOrDefault().Kind.ToString();
                }
                List<NewsVM> ListNews = null;
                if (listNewsType == ListNewsType.CategoryNews)
                {
                    // ListNews = db.tbl_today_news.Where(s => ("," + s.kindlist).Contains("," + kind + ",") && ("," + s.CatList).Contains("," + id + ",")).OrderByDescending(d => d.pubdate).Select(n => new NewsVM()
                    ListNews = db.tbl_today_news.Where(s => ("," + s.CatList).Contains("," + id + ",")).OrderByDescending(d => d.pubdate).Select(n => new NewsVM()

                    {
                        NewsID = n.NewsPK,
                        Title = n.title,
                        Content = n.sContent,
                        Image = (n.image_url == null || n.image_url == "" ? "/images/icons_news2.jpg" : n.image_url),
                        SourceTitle = n.tbl_source.title,
                        SeenCount = n.seencount,
                        ExternalURL = n.external_url,
                        Timing = n.pubdate
                    }).Take(20).ToList();
                }
                if (listNewsType == ListNewsType.SourceNews)
                {
                    ListNews = db.tbl_today_news.Where(s => s.SourceFK == id).OrderByDescending(d => d.pubdate).Select(n => new NewsVM()
                    {
                        NewsID = n.NewsPK,
                        Title = n.title,
                        Content = n.sContent,
                        Image = (n.image_url == null || n.image_url == "" ? "/images/icons_news2.jpg" : n.image_url),
                        SourceTitle = n.tbl_source.title,
                        SeenCount = n.seencount,
                        ExternalURL = n.external_url,
                        Timing = n.pubdate
                    }).Take(20).ToList();
                }
                return View(ListNews);
            }
            catch (Exception ex)
            {
                TempData["ex"] = ex.Message;
                TempData["exInfo"] = ex.InnerException.Message;
                Global.Log.Error(ex);
                return View("Error");
            }
        }

        // GET: More News
        public ActionResult GetMoreNews(int? id, ListNewsType listNewsType, string lastNewsDate)
        {
            try
            {
                //ListNewsType emListNewsType = (ListNewsType)Enum.Parse(typeof(ListNewsType), listNewsType, true);
                DateTime lastDate = DateTime.Parse(lastNewsDate);
                string kind = "1";
                if (Request.Cookies["UserName"] != null)
                {
                    string email = Request.Cookies["UserName"].Value.ToString();
                    kind = db.tbl_frontend_users.Where(e => e.email == email).FirstOrDefault().Kind.ToString();
                }
                List<NewsVM> ListNews = null;
                if (listNewsType == ListNewsType.CategoryNews)
                {
                    ListNews = db.tbl_today_news.Where(s => ("," + s.CatList).Contains("," + id + ",") && s.pubdate < lastDate).OrderByDescending(d => d.pubdate).Select(n => new NewsVM()
                    {
                        NewsID = n.NewsPK,
                        Title = n.title,
                        Content = n.sContent,
                        Image = (n.image_url == null || n.image_url == "" ? "/images/icons_news2.jpg" : n.image_url),
                        SourceTitle = n.tbl_source.title,
                        SeenCount = n.seencount,
                        ExternalURL = n.external_url,
                        Timing = n.pubdate
                    }).Take(10).ToList();
                }
                if (listNewsType == ListNewsType.SourceNews)
                {
                    ListNews = db.tbl_today_news.Where(s => s.SourceFK == id && s.pubdate < lastDate).OrderByDescending(d => d.pubdate).Select(n => new NewsVM()
                    {
                        NewsID = n.NewsPK,
                        Title = n.title,
                        Content = n.sContent,
                        Image = (n.image_url == null || n.image_url == "" ? "/images/icons_news2.jpg" : n.image_url),
                        SourceTitle = n.tbl_source.title,
                        SeenCount = n.seencount,
                        ExternalURL = n.external_url,
                        Timing = n.pubdate
                    }).Take(10).ToList();
                }
                return PartialView("~/Views/Shared/_MoreNewsPartial.cshtml", ListNews);
            }
            catch (Exception ex)
            {
                TempData["ex"] = ex.Message;
                TempData["exInfo"] = ex.InnerException.Message;
                Global.Log.Error(ex);
                return View("Error");
            }
        }

        // GET: News/Details/5
        public ActionResult Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                tbl_today_news n = db.tbl_today_news.Find(id);
                if (n == null)
                {
                    return HttpNotFound();
                }
                n.seencount += 1;
                db.Entry(n).State = EntityState.Modified;
                db.SaveChanges();

                var newsDetails = new NewsVM
                {
                    NewsID = n.NewsPK,
                    Title = n.title,
                    SourceTitle = n.tbl_source.title,
                    Image = (n.image_url == null || n.image_url == "" ? "/images/icons_news2.jpg" : n.image_url),
                    Content = n.sContent,
                    ExternalURL = n.external_url,
                    SeenCount = n.seencount,
                    Timing = n.pubdate
                };

                //ViewBag.RelatedNews = db.tbl_today_news.Where(s => s.kindlist == n.kindlist && s.SourceFK == n.SourceFK && s.NewsPK != n.NewsPK).OrderByDescending(d => d.pubdate).Select(r => new NewsVM()
                ViewBag.RelatedNews = db.tbl_today_news.Where(s => s.SourceFK == n.SourceFK && s.NewsPK != n.NewsPK).OrderByDescending(d => d.pubdate).Select(r => new NewsVM()
                {
                    NewsID = r.NewsPK,
                    Title = r.title,
                    Content = r.sContent,
                    Image = (r.image_url == null || r.image_url == "" ? "/images/icons_news2.jpg" : r.image_url),
                    SourceTitle = n.tbl_source.title,
                    Timing = n.pubdate
                }).Take(4).ToList();
                return View(newsDetails);
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
        public ActionResult Search(string search)
        {
            try
            {
                string kind = "1";
                if (Request.Cookies["UserName"] != null)
                {
                    string email = Request.Cookies["UserName"].Value.ToString();
                    kind = db.tbl_frontend_users.Where(e => e.email == email).FirstOrDefault().Kind.ToString();
                }
                //var ListNews = db.tbl_today_news.Where(s => ("," + s.kindlist).Contains("," + kind + ",") && (s.sContent.Contains(search) || s.title.Contains(search))).OrderByDescending(d => d.pubdate).Select(n => new NewsVM()
                var ListNews = db.tbl_today_news.Where(s => (s.sContent.Contains(search) || s.title.Contains(search))).OrderByDescending(d => d.pubdate).Select(n => new NewsVM()
                {
                    NewsID = n.NewsPK,
                    Title = n.title,
                    Content = n.sContent,
                    Image = (n.image_url == null || n.image_url == "" ? "/images/icons_news2.jpg" : n.image_url),
                    SourceTitle = n.tbl_source.title,
                    SeenCount = n.seencount,
                    ExternalURL = n.external_url,
                    Timing = n.pubdate
                }).ToList();
                ViewBag.txtSearch = search;
                if (ListNews.Count() > 0)
                {
                    if (ListNews.Count() < 50)
                    {
                        return View(ListNews.ToList());
                    }
                    else return View(ListNews.Take(50).ToList());
                }
                else return View("NoResult");
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
