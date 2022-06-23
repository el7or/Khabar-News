using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace Kabar_admin
{
    public partial class TestRssFeed : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string url =(string.IsNullOrEmpty(urlinput.Value)) ?Request.QueryString["url"]: urlinput.Value;
                if (!string.IsNullOrEmpty(url))
                {
                    urlinput.Value=url;
                    string s = ParseRss(url);
                    Rssfeed.InnerHtml = s;
                }
            }
            catch (Exception ex)
            {
                Rssfeed.InnerHtml = "<a href='/Sources'><h2 >Url Not Vaild</h2></a>";
            }
        }

        private string ParseRss(string url)
        {
            XmlDocument rssXmlDoc = new XmlDocument();

            // Load the RSS file from the RSS URL
            rssXmlDoc.Load(url);

            // Parse the Items in the RSS file
            XmlNodeList rssNodes = rssXmlDoc.SelectNodes("rss/channel/item");

            StringBuilder rssContent = new StringBuilder();
            
            // Iterate through the items in the RSS file
            foreach (XmlNode rssNode in rssNodes)
            {
                XmlNode rssSubNode = rssNode.SelectSingleNode("title");
                string title = rssSubNode != null ? rssSubNode.InnerText : "";
                //
                rssSubNode = rssNode.SelectSingleNode("link");
                string link = rssSubNode != null ? rssSubNode.InnerText : "";
                //
                rssSubNode = rssNode.SelectSingleNode("guid");
                string guid = rssSubNode != null ? rssSubNode.InnerText : "";
                //
                rssSubNode = rssNode.SelectSingleNode("pubDate");
                string date = rssSubNode != null ? rssSubNode.InnerText : "";
                //
                string imageurl = "";

                imageurl= findimagenode(rssNode.ChildNodes);
                string description = "";
                //
                rssSubNode = rssNode.SelectSingleNode("description");
                if (rssSubNode != null && !string.IsNullOrEmpty(rssSubNode.InnerText))
                    description = rssSubNode != null ? rssSubNode.InnerText : "";
                else
                {
                    rssSubNode = rssSubNode.NextSibling;
                    if (rssSubNode != null && rssSubNode.Name == "content:encoded")
                        description = rssSubNode != null ? rssSubNode.InnerText : "";
                }
               
                string newsformat = "";
                if (!string.IsNullOrEmpty(imageurl))
                    newsformat = "<br><a href='{0}'>{1}</a><br>{2} <br>{3} <br><img width='400' height='400' src='{4}'/><br>";
                else
                    newsformat = "<br><a href='{0}'>{1}</a><br>{2} <br>{3} <br>";


                rssContent.Append(string.Format(newsformat, link, title, date, description, imageurl));
            }

            // Return the string that contain the RSS items
            return rssContent.ToString();
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
            }catch(Exception ex)
            { }
            return url;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                string url = urlinput.Value;
                if (!string.IsNullOrEmpty(url))
                {
                    string s = ParseRss(url);
                    Rssfeed.InnerHtml = s;
                }
            }
            catch (Exception ex)
            {
                Rssfeed.InnerHtml = "<h2 >Url Not Vaild or Content</h2>";
            }
        }
    }
}