using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.UI.WebControls;
using System.Xml;

namespace FetchService
{
    public partial class fetchRSSonebyone : System.Web.UI.Page
    {
        khabrEntities context = new khabrEntities();
        List<tbl_today_news> todaynews = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            int index = 0;
            fetchResult res;// =new fetchResult();
            try
            {
               
                tbl_source currentSource = null;

                //to prevent loop
                //if (Request.QueryString["index"] != null && , out index))
                    int.TryParse(Request.QueryString["index"], out index);
                if (index == 0)
                    log("===================================================================" + Environment.NewLine);
                else
                    log("--------------------------------------------------------------" + Environment.NewLine);
                //currentSource = context.tbl_source.Where(s =>s.bdeleted == false).OrderBy(s => s.SourcePK).Skip(CurrentsourceID).Take(1).FirstOrDefault();
                var sources = context.tbl_source.Where(s => s.bdeleted == false&&s.isManual==false).OrderBy(s=>s.SourcePK).ToList();
                if (index < sources.Count)
                {
                    currentSource = sources.ElementAt(index);
                    if (currentSource != null)
                    {

                        log(string.Format("Fetching source ID {0} -{1}-  start at {2} ", currentSource.SourcePK,currentSource.title, DateTime.Now));

                        DateTime dt = DateTime.Today.Subtract(TimeSpan.FromDays(3));
                        todaynews = context.tbl_today_news.Where(n => n.SourceFK == currentSource.SourcePK).ToList();

                        res = ParseRss(currentSource);

                        log(string.Format("Fetching done  at {2}  {0} found {1} added  " + Environment.NewLine, res.foundCount, res.addcount, DateTime.Now));
                        index++;
                    }
                   
                       

                }
                else
                    index = 0;
            }
            catch (Exception ex)
            {

                log(ex.Message);
                
            }
            finally
            {
               
                if (index != 0)
                    // Response.RedirectPermanent("fetchRSSonebyone.aspx?lastsourID=" + CurrentsourceID);
                    Server.Transfer("fetchRSSonebyone.aspx?index=" + index);
               
                    
            }
        }
        public fetchResult ParseRss(tbl_source s)
        {

            fetchResult res = new fetchResult();
            try
            {
                XmlDocument rssXmlDoc = new XmlDocument();

                // Load the RSS file from the RSS URL
                rssXmlDoc.Load(s.feed_url);
                //  Response.Write(string.Format("load xml doc from source {0} at {1} <br/>", s.title, DateTime.Now));
                List<tbl_today_news> fetchednews = GetNewsFromXML(rssXmlDoc, s);
                //  Response.Write(string.Format("GEt news from xml doc from source {0} at {1} <br/>", s.title, DateTime.Now));
                res.foundCount += fetchednews.Count;

                res.addcount = savenews(fetchednews, s);
                // Response.Write(string.Format("Save  news from xml doc from source {0} at {1} <br/>", s.title, DateTime.Now));
            }
            catch (Exception ex)
            { log(Environment.NewLine + ex.Message + Environment.NewLine); }
            // Return the string that contain the RSS items
             return res;
        }
        private  List<tbl_today_news> GetNewsFromXML(XmlDocument rssXmlDoc, tbl_source s)
        {
            List<tbl_today_news> fetchednews = new List<tbl_today_news>();
            // Parse the Items in the RSS file
            XmlNodeList rssNodes = rssXmlDoc.SelectNodes("rss/channel/item");

            StringBuilder rssContent = new StringBuilder();

            // Iterate through the items in the RSS file
            foreach (XmlNode rssNode in rssNodes)
            {
                try
                {
                    tbl_today_news news = new tbl_today_news();
                    XmlNode rssSubNode = rssNode.SelectSingleNode("title");
                    news.title = rssSubNode != null ? rssSubNode.InnerText : "";
                    if (string.IsNullOrEmpty(news.title)) continue;
                    //
                    rssSubNode = rssNode.SelectSingleNode("link");
                    news.external_url = rssSubNode != null ? rssSubNode.InnerText : "";
                    if (string.IsNullOrEmpty(news.external_url)) continue;
                    //
                    //rssSubNode = rssNode.SelectSingleNode("guid");
                    //news.guid = rssSubNode != null ? rssSubNode.InnerText : "";
                    //
                    rssSubNode = rssNode.SelectSingleNode("pubDate");
                    news.spubdate = rssSubNode != null ? rssSubNode.InnerText : "";
                    try
                    {
                        DateTime tempdate;
                        if (DateTime.TryParseExact(news.spubdate, CultureInfo.CurrentCulture.DateTimeFormat.RFC1123Pattern, CultureInfo.CreateSpecificCulture("en-US"), DateTimeStyles.None, out tempdate))
                            news.pubdate = tempdate.ToUniversalTime();
                        else if (DateTime.TryParseExact(news.spubdate, CultureInfo.CurrentCulture.DateTimeFormat.RFC1123Pattern, CultureInfo.CreateSpecificCulture("en-GB"), DateTimeStyles.None, out tempdate))
                            news.pubdate = tempdate.ToUniversalTime();
                        else if (DateTime.TryParseExact(news.spubdate, CultureInfo.CurrentCulture.DateTimeFormat.RFC1123Pattern, CultureInfo.CreateSpecificCulture("ar-EG"), DateTimeStyles.None, out tempdate))
                            news.pubdate = tempdate.ToUniversalTime();
                        else
                            news.pubdate =( DateTime.Parse(news.spubdate)).ToUniversalTime();
                    }
                    catch (Exception ex) { news.pubdate = DateTime.Now.ToUniversalTime(); }
                    news.indate = DateTime.Now;
                    news.edit_date = DateTime.Now;
                    //
                    news.image_url = findimagenode(rssNode.ChildNodes);
                    //
                    rssSubNode = rssNode.SelectSingleNode("description");
                    if(rssSubNode!=null&&!string.IsNullOrEmpty(rssSubNode.InnerText))
                    news.sContent = rssSubNode != null ? rssSubNode.InnerText : "";
                    else
                    {
                        rssSubNode = rssSubNode.NextSibling;
                        if(rssSubNode!=null&&rssSubNode.Name=="content:encoded")
                            news.sContent = rssSubNode != null ? rssSubNode.InnerText : "";
                    }

                    if (string.IsNullOrEmpty(news.sContent)) continue;
                    ///
                    news.SourceFK = s.SourcePK;
                    //
                    news.kindlist = s.kindlist;
                    news.CatList = s.sCats;
                    if (s.isnormal)
                        news.kind = 1;
                    else if (s.isgolden)
                        news.kind = 2;
                    else if (s.issuperuser)
                        news.kind = 3;
                    else
                        news.kind = 0;
                    //
                    fetchednews.Add(news);

                }
                catch (Exception ex)
                { log(Environment.NewLine + ex.Message + Environment.NewLine); }
            }

            return fetchednews;
        }
        private  int savenews(List<tbl_today_news> lnews, tbl_source s)
        {
            int newcount = 0;
            try
            {
                List<tbl_today_news> oldnews = new List<tbl_today_news>();
               
                for (int i = lnews.Count - 1; i >= 0; i--)
                {
                    string title = lnews[i].title;
                    lnews[i].isArgent = s.isArgent;
                    var old = todaynews.Where(n =>n.SourceFK==s.SourcePK&&n.title == title).FirstOrDefault();
                    if (old != null)
                    {
                        //save old news
                        oldnews.Add(old);
                        continue;
                    }
                   // Response.Write(string.Format("<br/> {0} ", lnews[i].title));
                    //newcount++;
                    context.tbl_today_news.Add(lnews[i]);
                }
                newcount=context.SaveChanges();

                //for (int j = lnews.Count - 1; j >= 0; j--)
                //{
                //    //skip old news 
                //    if (oldnews.Where(o => o.title == lnews[j].title).FirstOrDefault() != null) continue;
                //    string[] cats = s.sCats.Split(',');
                //    foreach (string cat in cats)
                //    {
                //        int catPK = 0;
                //        if (int.TryParse(cat, out catPK))
                //        {
                //            tbl_news_cat ncat = new tbl_news_cat();
                //            ncat.NewsFK = lnews[j].NewsPK;
                //            ncat.CatFK = catPK;
                //            ncat.indate = DateTime.Now;
                //            ncat.bdeleted = 0;
                //            ncat.addedby = 0;
                //            context.tbl_news_cat.Add(ncat);
                //        }
                //    }
                   
                //}
                //context.SaveChanges();
            }
            catch (Exception ex)
            {
                log(Environment.NewLine + ex.Message + Environment.NewLine);
            }
            return newcount;
        }

        private string findimagenode(XmlNodeList nodes)
        {
            string url = "";
            XmlNode node = null;
            try
            {
                foreach (XmlNode n in nodes)
                {
                    if (n.Name == "media:thumbnail" || n.Name == "enclosure" || n.Name == "media:group" || n.Name == "image")
                    {
                        if (n.Name == "media:group" && n.FirstChild != null)
                            node = n.FirstChild;
                        else if (n.Name == "image" && n.FirstChild != null)
                            node = n.FirstChild;
                        else
                            node = n;
                        break;
                    }
                }

                if (node != null && node.Attributes != null && node.Attributes["url"] != null)
                    url = node.Attributes["url"].InnerText;
                else if (node != null && (node.Name == "img" || node.Name == "#text"))
                {
                    if (node.Attributes != null && node.Attributes["src"] != null)
                    {
                        url = node.Attributes["src"].InnerText;
                    }
                    else
                    {
                        url = Regex.Match(node.InnerText, "<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;
                    }
                }
                else if (node != null)
                    url = node.InnerText;
                else
                    url = "";
            }
            catch (Exception ex)
            { }
            return url;
        }
        private static void log(string log)
        {
            string filename = string.Format("{0}-{1}-{2}.log", DateTime.UtcNow.Day, DateTime.UtcNow.Month, DateTime.UtcNow.Year);
            string fullpath = HostingEnvironment.MapPath("~").Trim('\\') + "\\log\\" + filename;
            //if (System.IO.File.Exists(fullpath))
            System.IO.File.AppendAllText(fullpath, log);
        }
    }
    //public class fetchResult
    //{
    //    public int foundCount = 0;
    //    public int addcount = 0;
    //}
}