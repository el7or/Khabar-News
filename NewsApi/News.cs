using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
namespace NewsApi
{
    public class NewsController : ApiController
    {
        khabar db = new khabar();

        [HttpPost]
        public IHttpActionResult Login()
        {
            try
            {
                var o = Request.Content;
                string jsonContent = o.ReadAsStringAsync().Result;
                var param = HttpUtility.ParseQueryString(jsonContent);
                string Email = param["Email"];
                string Password = param["Password"];

                var user =db.tbl_frontend_users.Where(x=>x.email==Email&&x.password==Password).FirstOrDefault();
                if (user == null)
                {
                    return Ok(ToMsg("Wrong Username & Password !",false));
                }
                else
                {
                    JsonSerializerSettings jSetting = new JsonSerializerSettings();
                    jSetting.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                    jSetting.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    return Ok(JsonConvert.DeserializeObject(JsonConvert.SerializeObject(user, jSetting)));
                }
            }
            catch (Exception ex)
            {
                return Ok(ToMsg(ex.Message,true));
            }
        }
        class Category
        {
           public int CatPK { get; set; }
            public string title { get; set; }
            public string info { get; set; }
            public string icon_url { get; set; }
            public byte? bdeleted { get; set; }
            public DateTime indate { get; set; }
            public int addedby { get; set; }
            public DateTime? edit_date { get; set; }
            public int? editedby { get; set; }
            public Guid guid { get; set; }
            public int count { get; set; }
        }


        class Source
        {
            public int SourcePK { get; set; }
            public string title { get; set; }
            public string info { get; set; }
            public string icon_url { get; set; }
            public string sCats { get; set; }
            public string kindlist { get; set; }
            public bool isnormal { get; set; }
            public bool isgolden { get; set; }
            public bool issuperuser { get; set; }
            public bool isArgent { get; set; }
            public bool isManual { get; set; }
            public bool bdeleted { get; set; }
            public DateTime indate { get; set; }
            public int addedby { get; set; }
            public DateTime? edit_date { get; set; }
            public int? editedby { get; set; }
            public Guid guid { get; set; }
            public long follower_count { get; set; }
        }

        [HttpGet]
        [HttpPost]
        public IHttpActionResult LoadCats()
        {
            try
            {
                var o = Request.Content;
                string jsonContent = o.ReadAsStringAsync().Result;
                var param = HttpUtility.ParseQueryString(jsonContent);
                string Email = param["Email"];
                string Password = param["Password"];
                
                var cats = db.tbl_category.Where(s => s.bdeleted == 0 || s.bdeleted == null)
                .Select(x => new Category
                {
                    CatPK = x.CatPK,
                    title = x.title,
                    info = x.info,
                    icon_url = x.icon_url,
                    bdeleted = x.bdeleted,
                    indate = x.indate,
                    addedby = x.addedby,
                    editedby = x.editedby,
                    edit_date = x.edit_date,
                    guid = x.guid,
                    count = 0
                }).ToList();//.Where(s => s.bdeleted==0)

                //var user = db.tbl_frontend_users.Where(x => x.email == Email && x.password == Password).FirstOrDefault();
                //if (user == null)
                //{
                //    return Ok(ToMsg("Wrong Username & Password !", false));
                //    //return Ok(JsonConvert.DeserializeObject(JsonConvert.SerializeObject("Wrong Username & Password !")));
                //    // return Ok("{\"Message\":\"Wrong Username & Password !\"}");
                //}
                //else
                //{

                var user = db.tbl_frontend_users.Where(x => x.email == Email && x.password == Password).FirstOrDefault();
                bool issuperuser = false;
                bool isgolden = false;
                bool isnormal = true; //(user.Kind == 1) ? true : false;
                if (user != null)
                {
                    isnormal = (user.Kind == 1) ? true : false;
                    issuperuser = (user.Kind == 2) ? true : false;
                    isgolden = (user.Kind == 3) ? true : false;
                }
                //List<tbl_source> sources;
                var sources = db.tbl_source.Where(s => !s.bdeleted);
                int catFK = 0;
                if (isnormal) sources = sources.Where(s => s.isnormal == isnormal);
                if (isgolden) sources = sources.Where(s => s.isgolden == isgolden);
                if (issuperuser) sources = sources.Where(s => s.issuperuser == issuperuser);

                foreach (var item in cats)
                {                    
                        item.count = sources.Where(s => (s.sCats+",").Contains(item.CatPK+",")).Count();
                }
                JsonSerializerSettings jSetting = new JsonSerializerSettings();
                    jSetting.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                    jSetting.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    return Ok(JsonConvert.DeserializeObject(JsonConvert.SerializeObject(cats, jSetting)));
                //}
            }
            catch (Exception ex)
            {
                return Ok(ToMsg(ex.Message, true));
            }
        }

        [HttpGet]
        [HttpPost]
        public IHttpActionResult LoadSources()
        {
            try
            {
                var o = Request.Content;
                string jsonContent = o.ReadAsStringAsync().Result;
                var param = HttpUtility.ParseQueryString(jsonContent);
                string Email = param["Email"];
                string Password = param["Password"];
                string sCatFK = param["CatFK"];

                var user = db.tbl_frontend_users.Where(x => x.email == Email && x.password == Password).FirstOrDefault();
                bool issuperuser = false;
                bool isgolden = false;
                bool isnormal = true; //(user.Kind == 1) ? true : false;
                string knd = "1";
                if (user != null)
                {
                    isnormal = (user.Kind == 1) ? true : false;
                    issuperuser = (user.Kind == 2) ? true : false;
                    isgolden = (user.Kind == 3) ? true : false;
                    knd = user.Kind.ToString();
                }
                knd = "," + knd + ",";
                //List<tbl_source> sources;
                var sources = db.tbl_source.Select(x => new Source
                {
                    SourcePK = x.SourcePK,
                    title = x.title,
                    info = x.info,
                    icon_url = x.icon_url,
                    kindlist=x.kindlist,
                    sCats=x.sCats,
                    bdeleted = x.bdeleted,
                    isArgent=x.isArgent,
                    isManual=x.isManual,
                    indate = x.indate,
                    addedby = x.addedby,
                    editedby = x.editedby,
                    edit_date = x.edit_date,
                    guid = x.guid,
                    follower_count = x.follower_count,
                    isgolden=x.isgolden,
                    isnormal=x.isnormal,
                    issuperuser=x.issuperuser
                }).Where(s => !s.bdeleted).Where(x => ("," + x.kindlist + ",").Contains(knd));
                int catFK = 0;
                if (!String.IsNullOrEmpty(sCatFK) && int.TryParse(sCatFK, out catFK))
                    sources = sources.Where(s => s.sCats.Contains("," + catFK + ","));
                //if (isnormal) sources = sources.Where(s => s.isnormal == isnormal);
                //if (isgolden) sources = sources.Where(s => s.isgolden == isgolden);
                //if (issuperuser) sources = sources.Where(s => s.issuperuser == issuperuser);
                List<Source> lstSources = sources.ToList();
                JsonSerializerSettings jSetting = new JsonSerializerSettings();
                jSetting.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                jSetting.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                return Ok(JsonConvert.DeserializeObject(JsonConvert.SerializeObject(lstSources, jSetting)));
            }
            catch (Exception ex)
            {
                return Ok(ToMsg(ex.Message, true));
            }
        }

        [HttpGet]
        [HttpPost]
        public IHttpActionResult LoadNews0()
        {
            string line = "176";
            try
            {
                var o = Request.Content;
                string jsonContent = o.ReadAsStringAsync().Result;
                var param = HttpUtility.ParseQueryString(jsonContent);
                string Email = param["Email"];
                string Password = param["Password"];
                string lstSources = param["lstSources"];

                int LastID = 0;
                int.TryParse(param["LastID"], out LastID);
                int count = 20;
                int.TryParse(param["count"], out count);
                if (count == 0) count = 20;
                line = "190";
                var user = db.tbl_frontend_users.Where(x => x.email == Email && x.password == Password).FirstOrDefault();
                bool issuperuser = false;
                bool isgolden = false;
                bool isnormal = true; //(user.Kind == 1) ? true : false;
                if (user != null)
                {
                    isnormal = (user.Kind == 1) ? true : false;
                    issuperuser = (user.Kind == 3) ? true : false;
                    isgolden = (user.Kind == 2) ? true : false;
                }
                if (LastID == 0) LastID = 2147483647;
                var sources = db.tbl_source.Where(s => s.isnormal == isnormal || s.isgolden == isgolden || s.issuperuser == issuperuser);//.ToList();
                var news = db.tbl_today_news.Where(n => n.NewsPK < LastID  && sources.Any(s => s.SourcePK == n.SourceFK))
                    .OrderByDescending(n=>n.NewsPK);
                line = "204";
                if (!string.IsNullOrEmpty(lstSources)){
                    lstSources += ",";
                    line = "207";
                    //sources = sources.Where(s => lstSources.Contains( s.SourcePK+","));
                    //int pk = 0;
                    //sources = new List<NewsApi.tbl_source>();
                    //foreach (string item in lstSources.Split(','))
                    //{
                    //    if (int.TryParse(item, out pk))
                    //    { tbl_source s = new tbl_source(); s.SourcePK = pk; sources.Add(s); }
                    //}
                    //news = db.tbl_today_news.Where(n => n.NewsPK > LastID && sources.Any(s => s.SourcePK == n.SourceFK))
                    //    .OrderByDescending(n=>n.NewsPK).Take(count).ToList();
                    news = news.Where(n => lstSources.Contains(n.SourceFK + ","))
                        .OrderByDescending(n => n.NewsPK);//.Take(count);
                line = "220";
                }
                var newsList = news.Take(count).Select(x => new
                {
                    x.NewsPK,
                    x.guid,
                    x.SourceFK,
                    x.title,
                    x.pubdate,
                    x.spubdate,
                    x.sContent,
                    x.image_url,
                    x.video_url,
                    x.external_url,
                    x.kind,
                    x.authorisedby,
                    x.indate,
                    x.addedby,
                    x.editedby,
                    x.edit_date,
                    x.seencount
                }).ToList();

                line = "223";
                JsonSerializerSettings jSetting = new JsonSerializerSettings();
                jSetting.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                jSetting.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                return Ok(JsonConvert.DeserializeObject(JsonConvert.SerializeObject(newsList, jSetting)));
            }
            catch (Exception ex)
            {
                return Ok(ToMsg(line + " , " + ex.Message, true));
            }
        }

        [HttpGet]
        [HttpPost]
        public IHttpActionResult LoadNews()
        {
            try
            {
                var o = Request.Content;
                string jsonContent = o.ReadAsStringAsync().Result;
                var param = HttpUtility.ParseQueryString(jsonContent);
                string Email = param["Email"];
                string Password = param["Password"];
                string lstSources = param["lstSources"];
                string news_day = param["news_day"];
                string news_title = param["news_title"];
                string news_content = param["news_content"];

                int LastID = 0;
                int.TryParse(param["LastID"], out LastID);
                int count = 20;
                int.TryParse(param["count"], out count);
                if (count == 0) count = 20;

                var user = db.tbl_frontend_users.Where(x => x.email == Email && x.password == Password).FirstOrDefault();
                bool issuperuser = false;
                bool isgolden = false;
                bool isnormal = true; //(user.Kind == 1) ? true : false;
                string knd = "1";
                if (user != null)
                {
                    isnormal = (user.Kind == 1) ? true : false;
                    issuperuser = (user.Kind == 2) ? true : false;
                    isgolden = (user.Kind == 3) ? true : false;
                    knd = user.Kind.ToString();
                }
                if (LastID == 0) LastID = 2147483647;
                knd = "," + knd + ",";

                var sources = db.tbl_source.Where(s => !s.bdeleted).Where(x => ("," + x.kindlist + ",").Contains(knd));
                //var slst = sources.ToList();
                if (isnormal) sources = sources.Where(s => s.isnormal == isnormal);
                if (isgolden) sources = sources.Where(s => s.isgolden == isgolden);
                if (issuperuser) sources = sources.Where(s => s.issuperuser == issuperuser);
                //slst = sources.ToList();
                if (!string.IsNullOrEmpty(lstSources)) 
                {
                    lstSources = "," + lstSources + ",";
                    sources = sources.Where(s => lstSources.Contains("," + s.SourcePK + ","));
                }
                //slst = sources.ToList();
                var news = db.tbl_today_news.Where(n => n.NewsPK < LastID && sources.Any(s => s.SourcePK == n.SourceFK));
                   // .OrderByDescending(n=>n.NewsPK);

                //var newslst = news.ToList();

                //if (!string.IsNullOrEmpty(lstSources)){
                //    lstSources += ",";

                //    //sources = sources.Where(s => lstSources.Contains( s.SourcePK+","));
                //    //int pk = 0;
                //    //sources = new List<NewsApi.tbl_source>();
                //    //foreach (string item in lstSources.Split(','))
                //    //{
                //    //    if (int.TryParse(item, out pk))
                //    //    { tbl_source s = new tbl_source(); s.SourcePK = pk; sources.Add(s); }
                //    //}
                //    //news = db.tbl_today_news.Where(n => n.NewsPK > LastID && sources.Any(s => s.SourcePK == n.SourceFK))
                //    //    .OrderByDescending(n=>n.NewsPK).Take(count).ToList();
                //    news = news.Where(n => lstSources.Contains(n.SourceFK + ","))
                //        .OrderByDescending(n => n.NewsPK);//.Take(count);

                //}
                int nDays = 0;
                if (!string.IsNullOrEmpty(news_day) && int.TryParse(news_day,out nDays))
                {
                    DateTime dt = DateTime.Now.AddDays(-1 * nDays).Date;
                    news = news.Where(n => EntityFunctions.TruncateTime(n.pubdate.Value) == dt);//.OrderByDescending(n => n.NewsPK);
                }
                if (!string.IsNullOrEmpty(news_title))
                {
                    news = news.Where(n => n.title.Contains(news_title));//.OrderByDescending(n => n.NewsPK);
                }

                if (!string.IsNullOrEmpty(news_content))
                {
                    news = news.Where(n => n.sContent.Contains(news_content));//.OrderByDescending(n => n.NewsPK);
                }
                //newslst = news.ToList();
                //var newsList = news.OrderByDescending(n => n.NewsPK).Take(count).Select(x => new
                var newsList = news.Where(n=>n.pubdate.Value<=DateTime.UtcNow).OrderByDescending(n => n.pubdate).Take(count).Select(x => new
                {
                    x.NewsPK,
                    x.guid,
                    x.SourceFK,
                    x.title,
                    x.pubdate,
                    x.spubdate,
                    x.sContent,
                    x.image_url,
                    x.video_url,
                    x.external_url,
                    x.kind,
                    x.authorisedby,
                    x.indate,
                    x.addedby,
                    x.editedby,
                    x.edit_date,
                    x.seencount
                }).ToList();


                JsonSerializerSettings jSetting = new JsonSerializerSettings();
                jSetting.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                jSetting.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                return Ok(JsonConvert.DeserializeObject(JsonConvert.SerializeObject(newsList, jSetting)));
            }
            catch (Exception ex)
            {
                return Ok(ToMsg( ex.Message, true));
            }
        }

        [HttpPost]
        public IHttpActionResult NewsSeen()
        {
            try
            {
                var o = Request.Content;
                string jsonContent = o.ReadAsStringAsync().Result;
                var param = HttpUtility.ParseQueryString(jsonContent);
                string Email = param["Email"];
                string Password = param["Password"];
                string news_id = param["news_id"];

                //var user = db.tbl_frontend_users.Where(x => x.email == Email && x.password == Password).FirstOrDefault();
                 
                int nID = 0;
                if (!string.IsNullOrEmpty(news_id) && int.TryParse(news_id, out nID))
                {
                    tbl_today_news news = db.tbl_today_news.Find(nID);
                    if (news != null)
                    {                        
                        news.seencount++;
                        db.Entry(news).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        return Ok(ToMsg("done", false));
                    }
                    else
                        return Ok(ToMsg("not found", false));
                }
                else
                    return Ok(ToMsg("not valid id", false)); 

                //JsonSerializerSettings jSetting = new JsonSerializerSettings();
                //jSetting.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                //jSetting.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //return Ok(JsonConvert.DeserializeObject(JsonConvert.SerializeObject(newsList, jSetting)));

            }
            catch (Exception ex)
            {
                return Ok(ToMsg(ex.Message, true));
            }
        }

        [HttpPost]
        public IHttpActionResult SourceFollow()
        {
            try
            {
                var o = Request.Content;
                string jsonContent = o.ReadAsStringAsync().Result;
                var param = HttpUtility.ParseQueryString(jsonContent);
                string Email = param["Email"];
                string Password = param["Password"];
                string source_id = param["source_id"];
                bool following = param["following"] == "1";

                var user = db.tbl_frontend_users.Where(x => x.email == Email && x.password == Password).FirstOrDefault();
                if (user == null) return Ok(ToMsg("not valid user", false));

                if (user.following_sources == null) user.following_sources = "";
                string srcLst = "," + user.following_sources + ",";
                if (following && !srcLst.Contains("," + source_id + ","))
                {
                    user.following_sources += source_id + ",";
                    db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                if (!following && srcLst.Contains("," + source_id + ","))
                {
                    user.following_sources = user.following_sources.Replace(source_id + ",", "");
                    db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                

                return Ok(ToMsg("done", false));
            }
            catch (Exception ex)
            {
                return Ok(ToMsg(ex.Message, true));
            }
        }

        object ToMsg(String msg,bool err)
        {
            List<Messag> res = new List<Messag>();
            res.Add(new Messag(msg, err));
            return JsonConvert.DeserializeObject(JsonConvert.SerializeObject(res));            
        }
    }
    class Messag
    {
        public string Message;
        public bool isError;
        public Messag(string msg, bool err)
        {
            Message = msg;
            isError = err;
        }
    }
}