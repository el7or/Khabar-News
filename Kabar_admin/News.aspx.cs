using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Kabar_admin
{
    public partial class News : System.Web.UI.Page
    {
        public khabrEntities context;
        public IQueryable<tbl_today_news> filterlist;
        public int currentID = 0;
        private tbl_frontend_users loginsession = null;
        public string result;
        protected void Page_Load(object sender, EventArgs e)
        {
            //string s = Request.Url.Host;
            context = new khabrEntities();
            result = "";
            
            if (Request.QueryString["id"] != null && int.TryParse(Request.QueryString["id"].ToString(), out currentID))
            {
                ButtonDelete.Enabled = true;
                ButtonUpdate.Enabled = true;
                ButtonImage.Enabled = true;
                openurl.Attributes["class"] = "btn  btn-outline-light  m-1 rounded  ";
            }
            else
            {
                ButtonDelete.Enabled = false;
                ButtonUpdate.Enabled = false;
                //ButtonImage.Enabled = false;
                openurl.Attributes["class"] = "btn  btn-outline-light  m-1 rounded disabled  ";
                //Buttonsavephoto.Visible = false;
                Buttondeletephoto.Visible = false;
            }
            //Get Session 
            if (Session["loginsession"] != null)
            {
                loginsession = (tbl_frontend_users)Session["loginsession"];
                ApplyRights();
            }
            //
            Applyfilter();
            //
            if (!IsPostBack)
            {
                Pop_Cats();
                Pop_Sources();
                pop_Kinds();
                load_current();
                Pop_usersfilter();
            }
        }
        protected void Applyfilter()
        {
            IQueryable<tbl_today_news> filterResult = null;
            try
            {
                if (Session["loginsession"] == null)
                    return;
                //
                NewsFilter filter = null;

                if (Session["newsfilter"] == null)
                    filter = new NewsFilter();
                else
                    filter = (NewsFilter)Session["newsfilter"];
                //
                loadfilter(filter);
                filterbyday(filter, ref filterResult);
                filterbyinterval(filter, ref filterResult);
                filterbycreator(filter, ref filterResult);
                filterbysource(filter, ref filterResult);
                filterbyimage(filter, ref filterResult);
                filterbycategory(filter, ref filterResult);
                filterbytitle(filter, ref filterResult);
                filterbybody(filter, ref filterResult);
            }
            catch (Exception ex)
            { }
            finally { filterlist = (filterResult != null) ? filterResult : context.tbl_today_news; }
        }
        private void loadfilter(NewsFilter filter)
        {
         ListItem  sourceitem = combosourceFilter.Items.FindByValue(filter.SourcePK.ToString());
            ListItem catitem = catcombofilter.Items.FindByValue(filter.catPK.ToString());
            ListItem dayitem = Selectoneday.Items.FindByValue(filter.day.ToString());
            ListItem intervalitem = Selectdateinterval.Items.FindByValue(filter.interval.ToString());
            ListItem creatoritem = SelectCreatorFilter.Items.FindByValue(filter.creator.ToString());
            if (sourceitem != null)
            {
                combosourceFilter.SelectedIndex = -1;
                sourceitem.Selected = true;
            }
            if (catitem != null)
            {
                catcombofilter.SelectedIndex = -1;
                catitem.Selected = true;
            }
            if (dayitem != null)
            {
                Selectoneday.SelectedIndex = -1;
                dayitem.Selected = true;
            }
            if (intervalitem != null)
            {
                Selectdateinterval.SelectedIndex = -1;
                intervalitem.Selected = true;
            }
            if (creatoritem != null)
            {
                SelectCreatorFilter.SelectedIndex = -1;
                creatoritem.Selected = true;
            }
            inputtitleFilter.Value = filter.title;
            inputbodyFilter.Value = filter.body;
            hasimagecheck.Checked = (filter.hasImage == 1) ? true : false;
            noimagecheck.Checked = (filter.hasImage == 0) ? true : false;
            allcheck.Checked = (filter.hasImage == -1) ? true : false;
        }
        private void filterbyday(NewsFilter f, ref IQueryable<tbl_today_news> filterlist)
        {
           
            if (f.day > -1)
            {
                DateTime dt = (DateTime.Now.Subtract(TimeSpan.FromDays(f.day)));
                if (filterlist==null )
                    filterlist = context.tbl_today_news.Where(n => n.pubdate.Value.Day ==dt.Day &&n.pubdate.Value.Month==dt.Month&&n.pubdate.Value.Year==dt.Year );
                else
                    filterlist = filterlist.Where(n => n.pubdate.Value.Day == dt.Day && n.pubdate.Value.Month == dt.Month && n.pubdate.Value.Year == dt.Year);
            }
            
        }
        private void filterbyinterval(NewsFilter f,ref IQueryable<tbl_today_news> filterlist)
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
                if (filterlist == null )
                    filterlist = context.tbl_today_news.Where(n => n.addedby==f.creator);
                else
                    filterlist = filterlist.Where(n => n.addedby == f.creator);
            }
           
        }
        private void filterbysource(NewsFilter f, ref IQueryable<tbl_today_news> filterlist)
        {
            if (f.SourcePK > -1)
            {
                if (filterlist == null )
                    filterlist = context.tbl_today_news.Where(n => n.SourceFK == f.SourcePK);
                else
                    filterlist = filterlist.Where(n => n.SourceFK == f.SourcePK);
            }
        }
        private void filterbycategory(NewsFilter f, ref IQueryable<tbl_today_news> filterlist)
        {
            if (f.catPK > -1)
            {
                if (filterlist == null )
                    filterlist = context.tbl_today_news.Where(n => n.tbl_news_cat.Any(nc=>nc.CatFK == f.catPK));
                else
                    filterlist = filterlist.Where(n => n.tbl_news_cat.Any(nc => nc.CatFK == f.catPK));
            }
            
        }
        private void filterbyimage(NewsFilter f, ref IQueryable<tbl_today_news> filterlist)
        {
            if (f.hasImage ==0)
            {
                if (filterlist == null )
                    filterlist = context.tbl_today_news.Where(n => n.image_url==null);
                else
                    filterlist = filterlist.Where(n => n.image_url == null);
            }else if(f.hasImage==1)
            {
                if (filterlist == null )
                    filterlist = context.tbl_today_news.Where(n => n.image_url != null);
                else
                    filterlist = filterlist.Where(n => n.image_url != null);

            }
            
        }
        private void filterbytitle(NewsFilter f, ref IQueryable<tbl_today_news> filterlist)
        {
            if (!string.IsNullOrEmpty( f.title))
            {
                if (filterlist == null )
                    filterlist = context.tbl_today_news.Where(n =>n.title.Contains(f.title));
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
        private void ApplyRights()
        {
            if (!loginsession.viewnews)
                Response.Redirect("NoRights");
            ButtonReset.Visible = ButtonAdd.Visible = loginsession.addnews;
            ButtonUpdate.Visible=Buttonsavephoto.Visible/*=Buttondeletephoto.Visible*/ = loginsession.editnews;
            ButtonDelete.Visible = loginsession.delnews;
        }
        private void Pop_usersfilter()
        {

            foreach (tbl_frontend_users user in context.tbl_frontend_users.ToList())
            {
                ListItem item = new ListItem(user.displayname, user.FUserPK.ToString());

                SelectCreatorFilter.Items.Add(item);
            }
        }
        private void Pop_Cats()
        {

            foreach (tbl_category cat in context.tbl_category.ToList())
            {
                ListItem item = new ListItem(cat.title, cat.CatPK.ToString());
                ListItem itemfilter = new ListItem(cat.title, cat.CatPK.ToString());
                CheckBoxListcategories.Items.Add(item);
                catcombofilter.Items.Add(itemfilter);
            }
        }
        private void Pop_Sources()
        {

            foreach (tbl_source sour in context.tbl_source.OrderBy(s=>s.title).ToList())
            {
                ListItem item = new ListItem(sour.title, sour.SourcePK.ToString());
                ListItem itemfilter = new ListItem(sour.title, sour.SourcePK.ToString());
                if (sour.bdeleted)
                    item.Attributes["disabled"] = "disabled";
                else
                {
                    combosourceFilter.Items.Add(itemfilter);
                    SourcesCombo.Items.Add(item);
                }
                //
               

                // CheckBoxListcategories.Items.Add(item);
            }
        }
        private void pop_Kinds()
        {

            foreach (tbl_kinds kind in context.tbl_kinds.ToList())
            {
                ListItem item = new ListItem(kind.kindName, kind.kindID.ToString());
                // CategoriesCombo.Items.Add(item);
                CheckBoxListkinds.Items.Add(item);

            }
        }
        private void load_current()
        {
            try
            {

                if (currentID > 0)
                {
                    tbl_today_news currentNews = (tbl_today_news)(context.tbl_today_news.Where(n => n.NewsPK == currentID).FirstOrDefault());
                    //
                    load_Currenturls(currentNews);
                    //
                    HiddenFieldnewsID.Value = currentNews.NewsPK.ToString();
                    sourTitle.Value = currentNews.title;
                    sourContent.Value = currentNews.sContent;
                    contenthtml.InnerHtml = currentNews.sContent;
                    SourVideoUrl.Value = currentNews.video_url;
                    SourExternalUrl.Value = currentNews.external_url;
                    openurl.HRef = currentNews.external_url;
                    Textdate.Value = currentNews.spubdate;
                    if (!string.IsNullOrEmpty(currentNews.image_url) &&(!currentNews.image_url.StartsWith("http")|| checkimageExist(currentNews.image_url)))
                    {
                        NewsImage.Visible = true;
                        FileUploadNewsphoto.Visible = false;
                        HiddenFieldimagename.Value = currentNews.image_url;
                        if (currentNews.image_url.StartsWith("http"))
                            NewsImage.Src = currentNews.image_url;
                        else
                            NewsImage.Src = "~\\images\\" + currentNews.image_url;
                        Buttondeletephoto.Visible = true;
                        Buttonsavephoto.Visible = false;
                    }
                    else
                    {
                        HiddenFieldimagename.Value = "";
                        NewsImage.Src = "";
                        NewsImage.Visible = false;
                        FileUploadNewsphoto.Visible = true;
                        Buttondeletephoto.Visible = false;
                        Buttonsavephoto.Visible = true;
                    }
                    if (SourcesCombo.Items.Contains(new ListItem(currentNews.tbl_source.title, currentNews.tbl_source.SourcePK.ToString())))
                    {
                        SourcesCombo.Items.FindByValue(currentNews.SourceFK.ToString()).Selected = true;
                    }
                    string[] kinds = currentNews.kindlist.Split(',');
                    foreach (string kind in kinds)
                    {
                        ListItem Kitem = CheckBoxListkinds.Items.FindByValue(kind);
                        if (Kitem != null)
                            Kitem.Selected = true;

                    }
                    string[] cats = currentNews.CatList.Split(',');
                    // foreach (tbl_news_cat link in currentNews.tbl_news_cat.ToList())
                    foreach (string cat in cats)
                    {
                        //ListItem item = CheckBoxListcategories.Items.FindByValue(link.CatFK.ToString());
                        ListItem item = CheckBoxListcategories.Items.FindByValue(cat);
                        if (item != null)
                            item.Selected = true;

                    }
                }
            }
            catch (Exception ex)
            {


            }

        }
        private void load_Currenturls(tbl_today_news currentNews)
        {
            externalurl1.HRef = currentNews.external_url;
            SourVideoUrl1.HRef = currentNews.video_url;
        }
        private bool checkimageExist(string url)
        {
            bool exists=false;
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.Method = "HEAD";

                request.GetResponse();
                exists = true;
            }
            catch
            {
             
            }
            return exists;
        }
        protected void showResult(string result)
        {
            if (!string.IsNullOrEmpty(result))
            {
                string func = string.Format("showResult('{0}')", result);
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", func, true);
            }
        }
        protected void populateImageAlert()
        {
            string func = "error_image()";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", func, true);
        }
        protected void populateImageuploadAlert(string error)
        {

            string func = string.Format("error_upload('{0}')", error);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", func, true);
        }
        protected void populateImagedeleteAlert()
        {
            string func = "error_delete()";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", func, true);
        }
        protected void gotodownload()
        {
            string func = "go()";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", func, true);
        }
        protected void ButtonAdd_Click(object sender, EventArgs e)
        {
            try
            {
                AddNews();
            }catch(Exception ex)
            { }
            finally
            {
                showResult(result);
                Result();
            }
        }
        private bool AddNews()
        {
            bool sucess = false;
            try
            {
                int sourID = 0;
                //byte kind = 0;
                tbl_today_news news = new tbl_today_news();
                if (!string.IsNullOrEmpty(sourTitle.Value) && !string.IsNullOrEmpty(sourContent.Value))
                {
                    news.title = sourTitle.Value;
                    news.sContent = sourContent.Value;
                    news.external_url = SourExternalUrl.Value;
                    news.video_url = SourVideoUrl.Value;
                    int.TryParse(SourcesCombo.Value, out sourID);
                    //byte.TryParse(SelectKind.Value, out kind);

                    news.indate = DateTime.Now;
                    news.spubdate = DateTime.Now.ToString();
                    news.pubdate = DateTime.Now;
                    news.addedby = loginsession.FUserPK;
                    news.edit_date = DateTime.Now;
                    news.SourceFK = sourID;
                    //news.kind = kind;
                    string kinds = "";
                    foreach (ListItem k in CheckBoxListkinds.Items)
                    {
                        if (k.Selected)
                        {
                            kinds += k.Value + ",";
                        }
                    }
                    kinds = kinds.Trim(',');
                    news.kindlist = kinds;
                    //
                    //context.tbl_today_news.Add(news);
                   // context.SaveChanges();
                    //related source is not loaded automatically 
                    var source = context.tbl_source.Where(s => s.SourcePK == news.SourceFK).FirstOrDefault();
                    if (source != null)
                        news.isArgent = source.isArgent;
                  
                    string scats = "";
                    foreach (ListItem i in CheckBoxListcategories.Items)
                    {
                        if (i.Selected)
                        {
                            scats += i.Value +",";
                            //tbl_news_cat link = new tbl_news_cat();
                            //link.CatFK = int.Parse(i.Value);
                            //link.NewsFK = news.NewsPK;
                            //link.addedby = loginsession.FUserPK;
                            //link.indate = DateTime.Now;
                            //news.tbl_news_cat.Add(link);
                        }
                    }
                    scats=scats.Trim(',');
                    news.CatList = scats;
                    context.tbl_today_news.Add(news);
                    context.SaveChanges();
                    currentID = news.NewsPK;//to be added refresh link
                    HiddenFieldnewsID.Value = news.NewsPK.ToString();
                    result = "News has been added.";
                    sucess = true;
                }
               
            }
            catch (Exception ex)
            {
                result = "Error has been occurred";
            }
           
            return sucess;
        }

        protected void ButtonUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                int sourID = 0;
                // byte kind = 0;
                int newsID;
                if (!string.IsNullOrEmpty(HiddenFieldnewsID.Value) && int.TryParse(HiddenFieldnewsID.Value, out newsID))
                {
                    tbl_today_news currentNews = (tbl_today_news)(context.tbl_today_news.Where(n => n.NewsPK == newsID).FirstOrDefault());
                    if (currentNews != null)
                    {
                        currentNews.title = sourTitle.Value;
                        currentNews.sContent = sourContent.Value;
                        currentNews.external_url = SourExternalUrl.Value;
                        currentNews.video_url = SourVideoUrl.Value;
                        int.TryParse(SourcesCombo.Value, out sourID);
                        //byte.TryParse(SelectKind.Value, out kind);
                        currentNews.edit_date = DateTime.Now;
                        currentNews.editedby = loginsession.FUserPK;
                        currentNews.isArgent = currentNews.tbl_source.isArgent;
                        currentNews.SourceFK = sourID;
                        //currentNews.kind = kind;
                        string kinds = "";
                        foreach (ListItem k in CheckBoxListkinds.Items)
                        {
                            if (k.Selected)
                            {
                                kinds += k.Value + ",";
                            }
                        }
                        kinds = kinds.Trim(',');
                        currentNews.kindlist = kinds;
                       // context.SaveChanges();
                        result = "News has been updated.";
                        string scats = "";
                        foreach (ListItem i in CheckBoxListcategories.Items)
                        {
                            try
                            {
                                int cid = int.Parse(i.Value);
                               // tbl_news_cat existing = (tbl_news_cat)(context.tbl_news_cat.Where(n => n.CatFK == cid && n.NewsFK == newsID).FirstOrDefault());
                                if (i.Selected /*&& existing == null*/)
                                {
                                    scats += i.Value + ",";
                                    //tbl_news_cat link = new tbl_news_cat();
                                    //link.CatFK = int.Parse(i.Value);
                                    //link.NewsFK = currentNews.NewsPK;
                                    //link.addedby = loginsession.FUserPK;
                                    //link.indate = DateTime.Now;
                                    //currentNews.tbl_news_cat.Add(link);

                                }
                                //else if (!i.Selected && existing != null)
                                //{
                                //    context.tbl_news_cat.Remove(existing);
                                //}
                                //else
                                //{
                                //    //do nothing
                                //}

                            }
                            catch (Exception ex)
                            { }
                        }
                        scats = scats.Trim(',');
                        currentNews.CatList = scats;
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                result = "Error has been occurred";
            }
            finally
            {
                showResult(result);
                Result();
            }
        }

        protected void ButtonNewsdelete_Click(object sender, EventArgs e)
        {
            try
            {
                int newsID;
                if (!string.IsNullOrEmpty(HiddenFieldnewsID.Value) && int.TryParse(HiddenFieldnewsID.Value, out newsID))
                {
                    var currentNews = context.tbl_today_news.Where(n => n.NewsPK == newsID).FirstOrDefault();
                    if (currentNews != null)
                    {
                        if (!string.IsNullOrEmpty(currentNews.image_url))
                        {
                            //string imagename = currentNews.image_url;
                            string imagepath = "http://" + Request.Url.Host + "/images/";
                            string imagename = currentNews.image_url.Replace(imagepath, "");
                            System.IO.File.Delete(MapPath("~").TrimEnd('\\') + "\\images\\" + imagename);
                        }
                        //
                        context.tbl_today_news.Remove(currentNews);
                        //List<tbl_news_cat> newsCats = context.tbl_news_cat.Where(nc => nc.NewsFK == newsID).ToList();
                        //foreach (tbl_news_cat newscat in newsCats)
                        //{
                        //    context.tbl_news_cat.Remove(newscat);
                        //}
                        context.SaveChanges();
                       
                      
                        result = "News has been deleted.";
                    }
                }
            }
            catch (Exception ex)
            {
                result = "Error has been occurred";
            }
            finally
            {

                showResult(result);
                Result(true);
            }
        }
        protected void Buttonsavephoto_Click(object sender, EventArgs e)
        {
            bool error = true;
            try
            {
                //string ftpUsername = ConfigurationManager.AppSettings["ftpusername"];
                //string ftpPassword = ConfigurationManager.AppSettings["ftppassword"];
                //string ftpserver = ConfigurationManager.AppSettings["ftpserver"];
                //string imagefolder_local = ConfigurationManager.AppSettings["imagefolder_local"];
                //string imagelocalpath = ConfigurationManager.AppSettings["imagelocalpath"];
                string imagename = Guid.NewGuid().ToString() + ".png";
                if (!FileUploadNewsphoto.HasFile || (!FileUploadNewsphoto.FileName.Contains(".png") && !FileUploadNewsphoto.FileName.Contains(".jpeg") && !FileUploadNewsphoto.FileName.Contains(".jpg")))
                {
                    populateImageAlert();
                    error = true;
                    return;
                }
                int newsID;
                if (string.IsNullOrEmpty(HiddenFieldnewsID.Value) || !int.TryParse(HiddenFieldnewsID.Value, out newsID))
                    AddNews();

               if (!string.IsNullOrEmpty(HiddenFieldnewsID.Value) && int.TryParse(HiddenFieldnewsID.Value, out newsID) && FileUploadNewsphoto.FileBytes.Length > 0)
                {
                    FileUploadNewsphoto.PostedFile.SaveAs(MapPath("~").TrimEnd('\\') + "\\images\\" + imagename);
                    var currentNews = context.tbl_today_news.Where(n => n.NewsPK == newsID).FirstOrDefault();
                    if (currentNews != null)
                    {
                        string imagepath ="http://"+ Request.Url.Host+"/images/";
                        currentNews.image_url = imagepath.TrimEnd('/')+"/"+ imagename;
                        currentNews.edit_date = DateTime.Now;
                        currentNews.addedby = loginsession.FUserPK;
                        context.SaveChanges();
                        result = "Image has been Saved.";
                    }
                }
            }
            catch (Exception ex)
            {
                result = "Error has been occurred";
            }
            finally
            {
                if (!error)
                {
                    showResult(result);
                    //Result();
                }
                else
                {
                    Result();
                }
            }
        }
        protected void buttonImagedelete_click(object sender, EventArgs e)
        {
            try
            {
                int newsID;
                if (!string.IsNullOrEmpty(HiddenFieldnewsID.Value) && int.TryParse(HiddenFieldnewsID.Value, out newsID))
                {

                    var currentNews = context.tbl_today_news.Where(n => n.NewsPK == newsID).FirstOrDefault();
                    if (currentNews != null)
                    {
                        string imagepath = "http://" + Request.Url.Host + "/images/";
                        string imagename = currentNews.image_url.Replace(imagepath, "");
                        currentNews.image_url = "";
                        context.SaveChanges();
                        System.IO.File.Delete(MapPath("~").TrimEnd('\\') + "\\images\\" + imagename);
                        result = "Image has been deleted.";
                    }
                }
            }
            catch (Exception ex)
            {
                result = "Error has been occurred";
            }
            finally
            {

                showResult(result);
                Result();
            }
        }
        protected void Result(bool reset = false)
        {
            if (currentID > 0 && !reset)
                Response.Redirect(string.Format("News?id={0}", currentID));
            else
                Response.Redirect("News");
        }

        protected void Buttonreset_Click(object sender, EventArgs e)
        {
            try
            {
                HiddenFieldnewsID.Value = "";
                //SourcesCombo.SelectedIndex = 0;
                sourTitle.Value = "";
                sourContent.Value = "";
                SourVideoUrl.Value = "";
                SourExternalUrl.Value = "";
                CheckBoxListcategories.ClearSelection();
                CheckBoxListkinds.ClearSelection();
                // SelectKind.SelectedIndex = 0;
                contenthtml.InnerHtml = "";
                externalurl1.HRef = "";
                SourVideoUrl1.HRef = "";
                Textdate.Value = "";
            }
            catch (Exception ex)
            { }
        }
        protected void ButtonApplyFilterClick(object sender, EventArgs e)
        {
            try
            {
                NewsFilter filter = new NewsFilter();
                filter.SourcePK = int.Parse(combosourceFilter.Value);
                filter.creator = int.Parse(SelectCreatorFilter.Value);
                filter.catPK = int.Parse(catcombofilter.Value);
                filter.day = int.Parse(Selectoneday.Value);
                filter.interval = int.Parse(Selectdateinterval.Value);
                filter.title = inputtitleFilter.Value;
                filter.body = inputbodyFilter.Value;
                if (hasimagecheck.Checked)
                    filter.hasImage = int.Parse(hasimagecheck.Value);
                else if (noimagecheck.Checked)
                    filter.hasImage = int.Parse(noimagecheck.Value);
                else
                    filter.hasImage = int.Parse(allcheck.Value);

                if (Session["newsfilter"] == null)
                    Session.Add("newsfilter", filter);
                else
                    Session["newsfilter"] = filter;
               
            }
            catch (Exception ex) { }
            finally { Response.Redirect("News"); }
        }
        protected void ButtonCancelFilterClick(object sender, EventArgs e)
        {
            try
            {
                NewsFilter filter = new NewsFilter();
                filter.SourcePK = -1;
                filter.creator = -1;
                filter.catPK = -1;
                filter.day = 0;
                filter.interval = -1;
                filter.hasImage = -1;
                if (Session["newsfilter"] == null)
                    Session.Add("newsfilter", filter);
                else
                    Session["newsfilter"] = filter;

                
            }
            catch (Exception ex) { }
            finally { Response.Redirect("News"); }
        }
        protected void ButtondownloadClick(object sender, EventArgs e)
        {
            try
            {
                NewsFilter filter = new NewsFilter();
                filter.SourcePK = int.Parse(combosourceFilter.Value);
                filter.creator = int.Parse(SelectCreatorFilter.Value);
                filter.catPK = int.Parse(catcombofilter.Value);
                filter.day = int.Parse(Selectoneday.Value);
                filter.interval = int.Parse(Selectdateinterval.Value);
                if (hasimagecheck.Checked)
                    filter.hasImage = int.Parse(hasimagecheck.Value);
                else if (noimagecheck.Checked)
                    filter.hasImage = int.Parse(noimagecheck.Value);
                else
                    filter.hasImage = int.Parse(allcheck.Value);

                if (Session["newsfilter"] == null)
                    Session.Add("newsfilter", filter);
                else
                    Session["newsfilter"] = filter;

            }
            catch (Exception ex) { }
            finally { Response.Redirect("News?ex=1"); }
        }
    }
}