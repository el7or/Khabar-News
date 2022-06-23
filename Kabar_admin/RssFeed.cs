using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace Kabar_admin
{
    public class RssFeed
    {
        public  static fetchResult ParseRss(tbl_source s)
        {

            fetchResult res = new fetchResult();
            try
            {
                XmlDocument rssXmlDoc = new XmlDocument();

                // Load the RSS file from the RSS URL
                rssXmlDoc.Load(s.feed_url);
                List<tbl_today_news> fetchednews = GetNewsFromXML(rssXmlDoc, s);
                res.foundCount += fetchednews.Count;

              res.addcount=savenews(fetchednews, s);
            }
            catch(Exception ex)
            { }
            // Return the string that contain the RSS items
            return res;
        }
        public async static Task<fetchResult> ascParseRss(tbl_source s)
        {

            fetchResult res = new fetchResult();
            try
            {
                XmlDocument rssXmlDoc = new XmlDocument();

                // Load the RSS file from the RSS URL
                rssXmlDoc.Load(s.feed_url);
                List<tbl_today_news> fetchednews = GetNewsFromXML(rssXmlDoc, s);
                res.foundCount += fetchednews.Count;

                res.addcount = savenews(fetchednews, s);
            }
            catch (Exception ex)
            { }
            // Return the string that contain the RSS items
            return res;
        }
        private static List<tbl_today_news> GetNewsFromXML(XmlDocument rssXmlDoc,tbl_source s)
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
                            news.pubdate = tempdate;
                        else if (DateTime.TryParseExact(news.spubdate, CultureInfo.CurrentCulture.DateTimeFormat.RFC1123Pattern, CultureInfo.CreateSpecificCulture("en-GB"), DateTimeStyles.None, out tempdate))
                            news.pubdate = tempdate;
                        else if (DateTime.TryParseExact(news.spubdate, CultureInfo.CurrentCulture.DateTimeFormat.RFC1123Pattern, CultureInfo.CreateSpecificCulture("ar-EG"), DateTimeStyles.None, out tempdate))
                            news.pubdate = tempdate;
                        else
                            news.pubdate = DateTime.Parse(news.spubdate);
                    }
                    catch (Exception ex) { news.pubdate = DateTime.Now; }
                    news.indate = DateTime.UtcNow;
                    news.edit_date = DateTime.UtcNow;
                    //
                    news.image_url = findimagenode(rssNode.ChildNodes);
                    //
                    rssSubNode = rssNode.SelectSingleNode("description");
                    if (rssSubNode != null && !string.IsNullOrEmpty(rssSubNode.InnerText))
                        news.sContent = rssSubNode != null ? rssSubNode.InnerText : "";
                    else
                    {
                        rssSubNode = rssSubNode.NextSibling;
                        if (rssSubNode != null && rssSubNode.Name == "content:encoded")
                            news.sContent = rssSubNode != null ? rssSubNode.InnerText : "";
                    }


                    if (string.IsNullOrEmpty(news.sContent)) continue;
                    ///
                    news.SourceFK = s.SourcePK;
                    //
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
                { }
            }

            return fetchednews;
        }
        private static int savenews(List<tbl_today_news> lnews, tbl_source s)
        {
            int newcount = 0;
            try
            {
                khabrEntities context = new khabrEntities();
                DateTime dt = DateTime.Today.Subtract(TimeSpan.FromDays(2));
                List<tbl_today_news> todaynews = context.tbl_today_news.Where(n => n.indate >= dt).ToList();
                List<tbl_today_news> oldnews = new List<tbl_today_news>();
                
                for (int i = lnews.Count - 1; i >= 0; i--)
                {
                    string title = lnews[i].title;
                    var old = todaynews.Where(n => n.SourceFK == s.SourcePK && n.title == title).FirstOrDefault();
                    if (old != null)
                    {
                        //save old news
                        oldnews.Add(old);
                        continue;
                    }
                    newcount++;
                    context.tbl_today_news.Add(lnews[i]);
                }
                context.SaveChanges();
                foreach (tbl_today_news news in lnews)
                {
                    //skip old news 
                    if (oldnews.Where(o => o.title == news.title).FirstOrDefault() != null) continue;
                    string[] cats = s.sCats.Split(',');
                    foreach (string cat in cats)
                    {
                        int catPK = 0;
                        if (int.TryParse(cat, out catPK))
                        {
                            tbl_news_cat ncat = new tbl_news_cat();
                            ncat.NewsFK = news.NewsPK;
                            ncat.CatFK = catPK;
                            ncat.indate = DateTime.Now;
                            ncat.bdeleted = 0;
                            ncat.addedby = 0;
                            context.tbl_news_cat.Add(ncat);
                        }
                    }
                }
                context.SaveChanges();
            }catch(Exception ex)
            {

            }
            return newcount;
        }

        private static string findimagenode(XmlNodeList nodes)
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

    }

    public class fetchResult
    {
        public int foundCount = 0;
        public int addcount = 0;
    }
}