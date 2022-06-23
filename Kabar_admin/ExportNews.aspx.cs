using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Kabar_admin
{
    public partial class ExportNews : System.Web.UI.Page
    {
        NewsFilter nfilter = null;
        public khabrEntities context;
        public IQueryable<tbl_today_news> filterlist;
        protected void Page_Load(object sender, EventArgs e)
        {
            context = new khabrEntities();
            Applyfilter();
            if (Request.QueryString["download"] != null && Request.QueryString["download"] == "T")
            {
                Response.ContentType = "text/html";
                Response.AppendHeader("Content-Disposition", "attachment; filename=FilteredNews.html");
                //Response.ContentEncoding = Encoding.Unicode;
            }
            string header = System.IO.File.ReadAllText(MapPath("~") + "newsheadr.html");
            string footer = System.IO.File.ReadAllText(MapPath("~") + "newsfooter.html");
            string format = System.IO.File.ReadAllText(MapPath("~") + "newstemplate.html");
            Response.Write(header);
            Response.Write("<div style='max-width:800px;text-align:center;margin-right:auto;margin-left:auto;'>");
            foreach (tbl_today_news news in filterlist.ToList())
            {
                //string newsformat = @"<div style='width:100%;min-width:700px;min-height:250px;text-align:right;margin-right:auto;margin-left:auto;display:block;'>
                //                    <br>{4} :-  <a href='{0}'>{1}</a><br>{2} <br>{3} <br>
                //                            </div>";
               // if(!string.IsNullOrEmpty(nfilter.title)|| !string.IsNullOrEmpty(nfilter.body))
              //  Response.Write(string.Format(format, news.external_url, news.title.Replace(nfilter.title,string.Format("<div style='border-radius:10px;border:1px solid gray;'>{0}</div>", nfilter.title)), news.spubdate, news.sContent.Replace(nfilter.body, string.Format("<div style='border-radius:10px;border:1px solid gray;'>{0}</div>", nfilter.body)), news.tbl_source.title));
              //  else
                    Response.Write(string.Format(format, news.external_url, news.title, news.spubdate, news.sContent, news.tbl_source.title));
            }
            Response.Write("</div> ");
            Response.Write(footer);
            // Response.ContentType = "";
        }
        protected void Applyfilter()
        {
            IQueryable<tbl_today_news> filterResult = null;
            try
            {
                if (Session["loginsession"] == null)
                    Response.Redirect("Auth");
                if (Session["newsfilter"] != null)
                {
                    nfilter = (NewsFilter)Session["newsfilter"];
                    filterbyday(nfilter, ref filterResult);
                    filterbyinterval(nfilter, ref filterResult);
                    filterbycreator(nfilter, ref filterResult);
                    filterbysource(nfilter, ref filterResult);
                    filterbyimage(nfilter, ref filterResult);
                    filterbycategory(nfilter, ref filterResult);
                    filterbytitle(nfilter, ref filterResult);
                    filterbybody(nfilter, ref filterResult);
                }
            }
            catch(Exception ex)
            { }
            finally { filterlist = (filterResult != null) ? filterResult : context.tbl_today_news; }
        }

        private void filterbyday(NewsFilter f, ref IQueryable<tbl_today_news> filterlist)
        {

            if (f.day > -1)
            {
                DateTime dt = (DateTime.Now.Subtract(TimeSpan.FromDays(f.day)));
                if (filterlist == null)
                    filterlist = context.tbl_today_news.Where(n => n.pubdate.Value.Day == dt.Day && n.pubdate.Value.Month == dt.Month && n.pubdate.Value.Year == dt.Year);
                else
                    filterlist = filterlist.Where(n => n.pubdate.Value.Day == dt.Day && n.pubdate.Value.Month == dt.Month && n.pubdate.Value.Year == dt.Year);
            }

        }
        private void filterbyinterval(NewsFilter f, ref IQueryable<tbl_today_news> filterlist)
        {

            if (f.interval > -1)
            {
                DateTime dt = (DateTime.Now.Subtract(TimeSpan.FromDays(f.interval)));
                if (filterlist == null)
                    filterlist = context.tbl_today_news.Where(n => n.pubdate.Value.Day >= dt.Day && n.pubdate.Value.Month >= dt.Month && n.pubdate.Value.Year >= dt.Year);
                else
                    filterlist = filterlist.Where(n => n.pubdate.Value.Day >= dt.Day && n.pubdate.Value.Month >= dt.Month && n.pubdate.Value.Year >= dt.Year);
            }

        }
        private void filterbycreator(NewsFilter f, ref IQueryable<tbl_today_news> filterlist)
        {

            if (f.creator > -1)
            {
                if (filterlist == null)
                    filterlist = context.tbl_today_news.Where(n => n.addedby == f.creator);
                else
                    filterlist = filterlist.Where(n => n.addedby == f.creator);
            }

        }
        private void filterbysource(NewsFilter f, ref IQueryable<tbl_today_news> filterlist)
        {
            if (f.SourcePK > -1)
            {
                if (filterlist == null)
                    filterlist = context.tbl_today_news.Where(n => n.SourceFK == f.SourcePK);
                else
                    filterlist = filterlist.Where(n => n.SourceFK == f.SourcePK);
            }
        }
        private void filterbycategory(NewsFilter f, ref IQueryable<tbl_today_news> filterlist)
        {
            if (f.catPK > -1)
            {
                if (filterlist == null)
                    filterlist = context.tbl_today_news.Where(n => n.tbl_news_cat.Any(nc => nc.CatFK == f.catPK));
                else
                    filterlist = filterlist.Where(n => n.tbl_news_cat.Any(nc => nc.CatFK == f.catPK));
            }

        }
        private void filterbyimage(NewsFilter f, ref IQueryable<tbl_today_news> filterlist)
        {
            if (f.hasImage == 0)
            {
                if (filterlist == null)
                    filterlist = context.tbl_today_news.Where(n => n.image_url == null);
                else
                    filterlist = filterlist.Where(n => n.image_url == null);
            }
            else if (f.hasImage == 1)
            {
                if (filterlist == null)
                    filterlist = context.tbl_today_news.Where(n => n.image_url != null);
                else
                    filterlist = filterlist.Where(n => n.image_url != null);

            }

        }
        private void filterbytitle(NewsFilter f, ref IQueryable<tbl_today_news> filterlist)
        {
            if (!string.IsNullOrEmpty(f.title))
            {
                if (filterlist == null)
                    filterlist = context.tbl_today_news.Where(n => n.title.Contains(f.title));
                else
                    filterlist = filterlist.Where(n => n.title.Contains(f.title));
            }

        }
        private void filterbybody(NewsFilter f, ref IQueryable<tbl_today_news> filterlist)
        {
            if (!string.IsNullOrEmpty(f.body))
            {
                if (filterlist == null)
                    filterlist = context.tbl_today_news.Where(n => n.title.Contains(f.body));
                else
                    filterlist = filterlist.Where(n => n.title.Contains(f.body));
            }

        }
    }
}