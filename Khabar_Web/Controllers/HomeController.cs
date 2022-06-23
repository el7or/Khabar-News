using Khabar_Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace Khabar_Web.Controllers
{
    public class HomeController : Controller
    {
        private KhabarDBContext db = new KhabarDBContext();
        //public static readonly Regex YoutubeVideoRegex = new Regex(@"youtu(?:\.be|be\.com)/(?:.*v(?:/|=)|(?:.*/)?)([a-zA-Z0-9-_]+)", RegexOptions.IgnoreCase);
        public ActionResult Index()
        {
            try
            {
                /* Check if user login or not */
                string followingSrc = "";
                if (Request.Cookies["UserName"] != null)
                {
                    string email = Request.Cookies["UserName"].Value.ToString();
                    var user = db.tbl_frontend_users.Where(e => e.email == email).FirstOrDefault();
                    if (user.following_sources != null || user.following_sources != "")
                    {
                        followingSrc = "," + user.following_sources;
                    }
                }
                else
                {
                    if (Request.Cookies["UserSrc"] != null && Request.Cookies["UserSrc"].Value.ToString() != "")
                    {
                        followingSrc = "," + Request.Cookies["UserSrc"].Value.ToString();
                    }
                }

                /* Get 5 variety news to top slider */
                // ViewBag.SliderNews = await db.tbl_today_news.Where(i => (followingSrc.Length<15?true: followingSrc.Contains("," + i.SourceFK.ToString() + ",")) && i.image_url != null && i.image_url != "")
                ViewBag.SliderNews = db.tbl_today_news.OrderByDescending(n => n.NewsPK).Take(10000).Where(i => i.image_url != null && i.image_url != "")
                      .GroupBy(s => s.SourceFK)
                      .SelectMany(d => d.OrderByDescending(p => p.pubdate).Take(1))
                      .OrderByDescending(d => d.pubdate).Take(5).Select(n => new NewsVM
                      {
                          NewsID = n.NewsPK,
                          Title = n.title,
                          Image = n.image_url,
                          SourceTitle = n.tbl_source.title,
                          Timing = n.pubdate
                      }).ToList();

                /* Get 4 news to section: الأحدث عالميا */
                int recentNewsCount = db.tbl_today_news.OrderByDescending(n => n.NewsPK).Take(10000).Where(i => followingSrc.Contains("," + i.SourceFK.ToString() + ",") && i.image_url != null && i.image_url != "" && ("," + i.CatList).Contains("," + 2 + ",")).Count();
                if (recentNewsCount >= 4)
                {
                    ViewBag.RecentNews = db.tbl_today_news.OrderByDescending(n=>n.NewsPK).Take(10000).Where(i => followingSrc.Contains("," + i.SourceFK.ToString() + ",") && i.image_url != null && i.image_url != "" && ("," + i.CatList).Contains("," + 2 + ","))
                    .OrderByDescending(d => d.pubdate).Take(4).Select(n => new NewsVM
                    {
                        NewsID = n.NewsPK,
                        Title = n.title,
                        Content = n.sContent,
                        Image = n.image_url,
                        SourceTitle = n.tbl_source.title,
                        SeenCount = n.seencount,
                        ExternalURL = n.external_url,
                        Timing = n.pubdate
                    }).ToList();
                }
                else if (recentNewsCount == 0)
                {
                    ViewBag.RecentNews = db.tbl_today_news.OrderByDescending(n => n.NewsPK).Take(10000).Where(i => i.image_url != null && i.image_url != "" && ("," + i.CatList).Contains("," + 2 + ","))
                        .OrderByDescending(d => d.pubdate).Take(4).Select(n => new NewsVM
                        {
                            NewsID = n.NewsPK,
                            Title = n.title,
                            Content = n.sContent,
                            Image = n.image_url,
                            SourceTitle = n.tbl_source.title,
                            SeenCount = n.seencount,
                            ExternalURL = n.external_url,
                            Timing = n.pubdate
                        }).ToList();
                }
                else
                {
                    var recentNews = db.tbl_today_news.OrderByDescending(n => n.NewsPK).Take(10000).Where(i => followingSrc.Contains("," + i.SourceFK.ToString() + ",") && i.image_url != null && i.image_url != "" && ("," + i.CatList).Contains("," + 2 + ","))
                    .OrderByDescending(d => d.pubdate).Select(n => new NewsVM
                    {
                        NewsID = n.NewsPK,
                        Title = n.title,
                        Content = n.sContent,
                        Image = n.image_url,
                        SourceTitle = n.tbl_source.title,
                        SeenCount = n.seencount,
                        ExternalURL = n.external_url,
                        Timing = n.pubdate
                    }).ToList();
                    recentNews.AddRange(db.tbl_today_news.OrderByDescending(n => n.NewsPK).Take(10000).Where(i => i.image_url != null && i.image_url != "" && ("," + i.CatList).Contains("," + 2 + ","))
                    .OrderByDescending(d => d.pubdate).Take(4 - recentNewsCount).Select(n => new NewsVM
                    {
                        NewsID = n.NewsPK,
                        Title = n.title,
                        Content = n.sContent,
                        Image = n.image_url,
                        SourceTitle = n.tbl_source.title,
                        SeenCount = n.seencount,
                        ExternalURL = n.external_url,
                        Timing = n.pubdate
                    }));
                    ViewBag.RecentNews = recentNews.ToList();
                }

                /* Get 4 news to section: الأكثر مشاهدة */
                int seenNewsCount = db.tbl_today_news.OrderByDescending(n => n.NewsPK).Take(10000).Where(i => followingSrc.Contains("," + i.SourceFK.ToString() + ",") && i.image_url != null && i.image_url != "").Count();
                if (seenNewsCount >= 4)
                {
                    ViewBag.SeenNews = db.tbl_today_news.OrderByDescending(n => n.NewsPK).Take(10000).Where(i => followingSrc.Contains("," + i.SourceFK.ToString() + ",") && i.image_url != null && i.image_url != "")
                        .OrderByDescending(s => s.seencount).Take(4).Select(n => new NewsVM
                        {
                            NewsID = n.NewsPK,
                            Title = n.title,
                            Content = n.sContent,
                            Image = n.image_url,
                            SourceTitle = n.tbl_source.title,
                            SeenCount = n.seencount,
                            ExternalURL = n.external_url,
                            Timing = n.pubdate
                        }).ToList();
                }
                else if (seenNewsCount == 0)
                {
                    ViewBag.SeenNews = db.tbl_today_news.OrderByDescending(n => n.NewsPK).Take(10000).Where(i => i.image_url != null && i.image_url != "")
                        .OrderByDescending(s => s.seencount).Take(4).Select(n => new NewsVM
                        {
                            NewsID = n.NewsPK,
                            Title = n.title,
                            Content = n.sContent,
                            Image = n.image_url,
                            SourceTitle = n.tbl_source.title,
                            SeenCount = n.seencount,
                            ExternalURL = n.external_url,
                            Timing = n.pubdate
                        }).ToList();
                }
                else
                {
                    var seenNews = db.tbl_today_news.OrderByDescending(n => n.NewsPK).Take(10000).Where(i => followingSrc.Contains("," + i.SourceFK.ToString() + ",") && i.image_url != null && i.image_url != "")
                        .OrderByDescending(s => s.seencount).Select(n => new NewsVM
                        {
                            NewsID = n.NewsPK,
                            Title = n.title,
                            Content = n.sContent,
                            Image = n.image_url,
                            SourceTitle = n.tbl_source.title,
                            SeenCount = n.seencount,
                            ExternalURL = n.external_url,
                            Timing = n.pubdate
                        }).ToList();
                    seenNews.AddRange(db.tbl_today_news.OrderByDescending(n => n.NewsPK).Take(10000).Where(i => i.image_url != null && i.image_url != "")
                        .OrderByDescending(s => s.seencount).Take(4 - seenNewsCount).Select(n => new NewsVM
                        {
                            NewsID = n.NewsPK,
                            Title = n.title,
                            Content = n.sContent,
                            Image = n.image_url,
                            SourceTitle = n.tbl_source.title,
                            SeenCount = n.seencount,
                            ExternalURL = n.external_url,
                            Timing = n.pubdate
                        }));
                    ViewBag.SeenNews = seenNews.ToList();
                }

                /* Get 3 news to section: اقتصاد */
                int economyNewsCount = db.tbl_today_news.OrderByDescending(n => n.NewsPK).Take(10000).Where(i => followingSrc.Contains("," + i.SourceFK.ToString() + ",") && i.image_url != null && i.image_url != "" && ("," + i.CatList).Contains("," + 10 + ",")).Count();
                if (economyNewsCount >= 3)
                {
                    ViewBag.SportNews = db.tbl_today_news.OrderByDescending(n => n.NewsPK).Take(10000).Where(i => followingSrc.Contains("," + i.SourceFK.ToString() + ",") && i.image_url != null && i.image_url != "" && ("," + i.CatList).Contains("," + 10 + ","))
                        .OrderByDescending(d => d.pubdate).Take(3).Select(n => new NewsVM
                        {
                            NewsID = n.NewsPK,
                            Title = n.title,
                            Content = n.sContent,
                            Image = n.image_url,
                            SourceTitle = n.tbl_source.title,
                            SeenCount = n.seencount,
                            ExternalURL = n.external_url,
                            Timing = n.pubdate
                        }).ToList();
                }
                else if (economyNewsCount == 0)
                {
                    ViewBag.SportNews = db.tbl_today_news.OrderByDescending(n => n.NewsPK).Take(10000).Where(i => i.image_url != null && i.image_url != "" && ("," + i.CatList).Contains("," + 10 + ","))
                        .OrderByDescending(d => d.pubdate).Take(3).Select(n => new NewsVM
                        {
                            NewsID = n.NewsPK,
                            Title = n.title,
                            Content = n.sContent,
                            Image = n.image_url,
                            SourceTitle = n.tbl_source.title,
                            SeenCount = n.seencount,
                            ExternalURL = n.external_url,
                            Timing = n.pubdate
                        }).ToList();
                }
                else
                {
                    var sportNews = db.tbl_today_news.OrderByDescending(n => n.NewsPK).Take(10000).Where(i => followingSrc.Contains("," + i.SourceFK.ToString() + ",") && i.image_url != null && i.image_url != "" && ("," + i.CatList).Contains("," + 10 + ","))
                        .OrderByDescending(d => d.pubdate).Select(n => new NewsVM
                        {
                            NewsID = n.NewsPK,
                            Title = n.title,
                            Content = n.sContent,
                            Image = n.image_url,
                            SourceTitle = n.tbl_source.title,
                            SeenCount = n.seencount,
                            ExternalURL = n.external_url,
                            Timing = n.pubdate
                        }).ToList();
                    sportNews.AddRange(db.tbl_today_news.OrderByDescending(n => n.NewsPK).Take(10000).Where(i => i.image_url != null && i.image_url != "" && ("," + i.CatList).Contains("," + 10 + ","))
                        .OrderByDescending(d => d.pubdate).Take(3 - economyNewsCount).Select(n => new NewsVM
                        {
                            NewsID = n.NewsPK,
                            Title = n.title,
                            Content = n.sContent,
                            Image = n.image_url,
                            SourceTitle = n.tbl_source.title,
                            SeenCount = n.seencount,
                            ExternalURL = n.external_url,
                            Timing = n.pubdate
                        }));
                    ViewBag.SportNews = sportNews.ToList();
                }

                /* Get 2 news to section: الوطن العربي */
                int arabicNewsCount = db.tbl_today_news.OrderByDescending(n => n.NewsPK).Take(10000).Where(i => followingSrc.Contains("," + i.SourceFK.ToString() + ",") && i.image_url != null && i.image_url != "" && ("," + i.CatList).Contains("," + 13 + ",")).Count();
                if (arabicNewsCount >= 2)
                {
                    ViewBag.ArabicNews = db.tbl_today_news.OrderByDescending(n => n.NewsPK).Take(10000).Where(i => followingSrc.Contains("," + i.SourceFK.ToString() + ",") && i.image_url != null && i.image_url != "" && ("," + i.CatList).Contains("," + 13 + ","))
                        .OrderByDescending(d => d.pubdate).Take(2).Select(n => new NewsVM
                        {
                            NewsID = n.NewsPK,
                            Title = n.title,
                            Content = n.sContent,
                            Image = n.image_url,
                            SourceTitle = n.tbl_source.title,
                            SeenCount = n.seencount,
                            ExternalURL = n.external_url,
                            Timing = n.pubdate
                        }).ToList();
                }
                else if (arabicNewsCount == 0)
                {
                    ViewBag.ArabicNews = db.tbl_today_news.OrderByDescending(n => n.NewsPK).Take(10000).Where(i => i.image_url != null && i.image_url != "" && ("," + i.CatList).Contains("," + 13 + ","))
                        .OrderByDescending(d => d.pubdate).Take(2).Select(n => new NewsVM
                        {
                            NewsID = n.NewsPK,
                            Title = n.title,
                            Content = n.sContent,
                            Image = n.image_url,
                            SourceTitle = n.tbl_source.title,
                            SeenCount = n.seencount,
                            ExternalURL = n.external_url,
                            Timing = n.pubdate
                        }).ToList();
                }
                else
                {
                    var arabicNews = db.tbl_today_news.OrderByDescending(n => n.NewsPK).Take(10000).Where(i => followingSrc.Contains("," + i.SourceFK.ToString() + ",") && i.image_url != null && i.image_url != "" && ("," + i.CatList).Contains("," + 13 + ","))
                        .OrderByDescending(d => d.pubdate).Take(1).Select(n => new NewsVM
                        {
                            NewsID = n.NewsPK,
                            Title = n.title,
                            Content = n.sContent,
                            Image = n.image_url,
                            SourceTitle = n.tbl_source.title,
                            SeenCount = n.seencount,
                            ExternalURL = n.external_url,
                            Timing = n.pubdate
                        }).ToList();
                    arabicNews.AddRange(db.tbl_today_news.OrderByDescending(n => n.NewsPK).Take(10000).Where(i => i.image_url != null && i.image_url != "" && ("," + i.CatList).Contains("," + 13 + ","))
                        .OrderByDescending(d => d.pubdate).Take(1).Select(n => new NewsVM
                        {
                            NewsID = n.NewsPK,
                            Title = n.title,
                            Content = n.sContent,
                            Image = n.image_url,
                            SourceTitle = n.tbl_source.title,
                            SeenCount = n.seencount,
                            ExternalURL = n.external_url,
                            Timing = n.pubdate
                        }));
                    ViewBag.ArabicNews = arabicNews.ToList();
                }

                /* Get 5 news to section: خليجي */
                int khalijiNewsCount = db.tbl_today_news.OrderByDescending(n => n.NewsPK).Take(10000).Where(i => followingSrc.Contains("," + i.SourceFK.ToString() + ",") && i.image_url != null && i.image_url != "" && ("," + i.CatList).Contains("," + 12 + ",")).Count();
                if (khalijiNewsCount >= 5)
                {
                    ViewBag.KhalijiNews = db.tbl_today_news.OrderByDescending(n => n.NewsPK).Take(10000).Where(i => followingSrc.Contains("," + i.SourceFK.ToString() + ",") && i.image_url != null && i.image_url != "" && ("," + i.CatList).Contains("," + 12 + ","))
                        .OrderByDescending(d => d.pubdate).Take(5).Select(n => new NewsVM
                        {
                            NewsID = n.NewsPK,
                            Title = n.title,
                            Image = n.image_url,
                            SeenCount = n.seencount,
                            SourceTitle = n.tbl_source.title,
                            Timing = n.pubdate
                        }).ToList();
                }
                else if (khalijiNewsCount == 0)
                {
                    ViewBag.KhalijiNews = db.tbl_today_news.OrderByDescending(n => n.NewsPK).Take(10000).Where(i => i.image_url != null && i.image_url != "" && ("," + i.CatList).Contains("," + 12 + ","))
                        .OrderByDescending(d => d.pubdate).Take(5).Select(n => new NewsVM
                        {
                            NewsID = n.NewsPK,
                            Title = n.title,
                            Image = n.image_url,
                            SeenCount = n.seencount,
                            SourceTitle = n.tbl_source.title,
                            Timing = n.pubdate
                        }).ToList();
                }
                else
                {
                    var khalijiNews = db.tbl_today_news.OrderByDescending(n => n.NewsPK).Take(10000).Where(i => followingSrc.Contains("," + i.SourceFK.ToString() + ",") && i.image_url != null && i.image_url != "" && ("," + i.CatList).Contains("," + 12 + ","))
                        .OrderByDescending(d => d.pubdate).Select(n => new NewsVM
                        {
                            NewsID = n.NewsPK,
                            Title = n.title,
                            Image = n.image_url,
                            SeenCount = n.seencount,
                            SourceTitle = n.tbl_source.title,
                            Timing = n.pubdate
                        }).ToList();
                    khalijiNews.AddRange(db.tbl_today_news.OrderByDescending(n => n.NewsPK).Take(10000).Where(i => i.image_url != null && i.image_url != "" && ("," + i.CatList).Contains("," + 12 + ","))
                        .OrderByDescending(d => d.pubdate).Take(5 - khalijiNewsCount).Select(n => new NewsVM
                        {
                            NewsID = n.NewsPK,
                            Title = n.title,
                            Image = n.image_url,
                            SeenCount = n.seencount,
                            SourceTitle = n.tbl_source.title,
                            Timing = n.pubdate
                        }));
                    ViewBag.KhalijiNews = khalijiNews.ToList();
                }


                //ViewBag.VideoNews = await db.tbl_today_news.Where(i => i.video_url != null && i.video_url != "")
                //    .OrderByDescending(d => d.pubdate).Take(2).Select(n => new NewsVM
                //    {
                //        NewsID = n.NewsPK,
                //        Title = n.title,
                //        Image = n.video_url,
                //        SourceTitle = n.tbl_source.title,
                //        SeenCount = n.seencount,
                //        Content = n.sContent,
                //        Timing = n.pubdate
                //    }).ToListAsync();
                //foreach (var item in ViewBag.VideoNews)
                //{
                //    item.Image = "//www.youtube.com/embed/" + (YoutubeVideoRegex.Match(item.Image).Success ? YoutubeVideoRegex.Match(item.Image).Groups[1].Value : "");
                //}

                return View();
            }
            catch (Exception ex)
            {
                //if (ex.InnerException != null) msg += Environment.NewLine + ex.InnerException.Message;
                TempData["ex"] = ex.Message;
                TempData["exInfo"] = ex.InnerException.Message;
                Global.Log.Error(ex);
                return View("Error");
            }
        }

        /* Get top 5 Category to menu */
        [ChildActionOnly]
        public PartialViewResult GetTopCats()
        {
            try
            {
                var Top5Cats = Session["Top5Cats"] as List<CategoryVM>;
                return PartialView("~/Views/Shared/_TopCatsPartial.cshtml", Top5Cats);
            }
            catch (Exception ex)
            {
                TempData["ex"] = ex.Message;
                TempData["exInfo"] = ex.InnerException.Message;
                Global.Log.Error(ex);
                return PartialView("Error");
            }
        }

        /* Get top 5 Sources to menu */
        [ChildActionOnly]
        public PartialViewResult GetTopSrcs()
        {
            try
            {
                var Top5Srcs = Session["Top5Srcs"] as List<SourceVM>;
                return PartialView("~/Views/Shared/_TopSrcsPartial.cshtml", Top5Srcs);
            }
            catch (Exception ex)
            {
                TempData["ex"] = ex.Message;
                TempData["exInfo"] = ex.InnerException.Message;
                Global.Log.Error(ex);
                return PartialView("Error");
            }
        }

        public ActionResult About()
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

        public ActionResult Contact()
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
    }
}