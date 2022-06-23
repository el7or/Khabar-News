using Khabar_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Khabar_Web
{
    public class Global : System.Web.HttpApplication
    {
        private KhabarDBContext db = new KhabarDBContext();
        public static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(Global));
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            log4net.Config.XmlConfigurator.Configure();
            Log.Info("Application_Start");
        }
        protected void Session_Start(Object sender, EventArgs e)
        {
            var Top5Cats = db.tbl_category.Where(s => s.bdeleted == 0 || s.bdeleted == null).Select(c => new CategoryVM()
            {
                CatID = c.CatPK,
                Title = c.title,
                SourceCount = db.tbl_source.Where(s => ("," + s.sCats).Contains("," + c.CatPK + ",") && s.bdeleted == false).Count()
            }).OrderByDescending(c => c.SourceCount).Take(5).ToList();
            Session["Top5Cats"] = Top5Cats;

            var Top5Srcs = db.tbl_source.Where(s => s.bdeleted == false).Select(c => new SourceVM()
            {
                SrcID = c.SourcePK,
                Title = c.title,
                NewsCount = c.tbl_today_news.Count()
            }).OrderByDescending(c => c.NewsCount).Take(5).ToList();
            Session["Top5Srcs"] = Top5Srcs;
        }
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            if (ex != null)// is ThreadAbortException)
                return;
            Log.Error("Application_Error", ex);
            //Logger.Error(LoggerType.Global, ex, "Exception");
        }
    }
}
