using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Kabar_admin
{
    public partial class Sources : System.Web.UI.Page
    {
       public khabrEntities context;
        public int currentID = 0;
        private tbl_frontend_users loginsession = null;
        public string result;
        protected void Page_Load(object sender, EventArgs e)
        {
            context = new khabrEntities();
            result = "";
            if (Request.QueryString["id"] != null && int.TryParse(Request.QueryString["id"].ToString(), out currentID))
            {
                ButtonDelete.Enabled = true;
                ButtonUpdate.Enabled = true;
                ButtonImage.Enabled = true;
                Buttonfetch.Enabled = true;
            }else
            {
                    ButtonDelete.Enabled = false;
                    ButtonUpdate.Enabled = false;
                    ButtonImage.Enabled = false;
                Buttonfetch.Enabled = false;
            }
            //Get Session 
            if (Session["loginsession"] != null)
            {
                loginsession = (tbl_frontend_users)Session["loginsession"];
                ApplyRights();
            }
            if (!IsPostBack)
            {
                Pop_Categories();
                pop_Kinds();
                load_current();
            }
        }
        private void ApplyRights()
        {
            if (!loginsession.viewsources)
                Response.Redirect("NoRights");

            ButtonReset.Visible = ButtonAdd.Visible = loginsession.addsources;
            ButtonUpdate.Visible=Buttonsavephoto.Visible=Buttondeletephoto.Visible = loginsession.editsources;
            Buttonfetch.Visible = loginsession.addnews;
            ButtonDelete.Visible = loginsession.delsources;
        }
        private void Pop_Categories()
        {

            foreach (tbl_category cat in context.tbl_category.ToList())
            {
                ListItem item = new ListItem(cat.title, cat.CatPK.ToString());
               // CategoriesCombo.Items.Add(item);
                CheckBoxListcategories.Items.Add(item);
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
               
                if (currentID>0)
                {
                    tbl_source current = (tbl_source)(context.tbl_source.Where(s => s.SourcePK == currentID&&!s.bdeleted).FirstOrDefault());
                    HiddenFieldsourID.Value = current.SourcePK.ToString();
                    sourTitle.Value = current.title;
                    sourinfo.Value = current.info;
                    sourfeedurl.Value = current.feed_url;
                    //if (CategoriesCombo.Items.FindByValue(current.CatFK.ToString()) != null)
                    //{
                    //    CategoriesCombo.Items.FindByValue(current.CatFK.ToString()).Selected = true;
                    //}
                    CheckboxisArgent.Checked = current.isArgent;
                    CheckboxisManual.Checked = current.isManual;
                    //checkNormal.Checked = current.isnormal;
                    //checksuperuser.Checked = current.issuperuser;
                    //checkgolden.Checked = current.isgolden;
                    if (!string.IsNullOrEmpty(current.icon_url))
                    {
                        CatIcon.Visible = true;
                        FileUploadCatIcon.Visible = false;
                        string imagepath = ConfigurationManager.AppSettings["imageurl"];
                        if (current.icon_url.Contains("http"))
                            CatIcon.Src = current.icon_url;
                        else
                            CatIcon.Src = "~\\images\\" + current.icon_url;
                        Buttondeletephoto.Visible = true;
                        Buttonsavephoto.Visible = false;
                    }
                    else
                    {
                        CatIcon.Src = "";
                        CatIcon.Visible = false;
                        FileUploadCatIcon.Visible = true;
                        Buttondeletephoto.Visible = false;
                        Buttonsavephoto.Visible = true;
                    }
                    string[] cats = current.sCats.Split(',');
                    foreach (string cat in cats)
                    {
                        ListItem item = CheckBoxListcategories.Items.FindByValue(cat);
                        if (item != null)
                            item.Selected = true;

                    }
                    string[] kinds = current.kindlist.Split(',');
                    foreach (string kind in kinds)
                    {
                        ListItem Kitem = CheckBoxListkinds.Items.FindByValue(kind);
                        if (Kitem != null)
                            Kitem.Selected = true;

                    }
                }
            }
            catch (Exception ex )
            {

              
            }

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
        protected void ButtonAdd_Click(object sender, EventArgs e)
        {
            try
            {
                int catid = 0;
                int newsourceID = 0;
                tbl_source sour = new tbl_source();
                tbl_source dublicat = context.tbl_source.Where(s => s.title == sourTitle.Value && s.feed_url == sourfeedurl.Value).FirstOrDefault();
                if (dublicat == null)
                {
                    sour.title = sourTitle.Value;
                    sour.info = sourinfo.Value;
                    sour.feed_url = sourfeedurl.Value;
                    //sour.isnormal = checknormal.checked;
                    //sour.issuperuser = checksuperuser.checked;
                    //sour.isgolden = checkgolden.checked;
                    sour.isArgent = CheckboxisArgent.Checked;
                    sour.isManual = CheckboxisManual.Checked;
                    //  int.TryParse(CategoriesCombo.Value, out catid);
                    sour.CatFK = catid;
                    sour.indate = DateTime.Now;
                    sour.edit_date = DateTime.Now;
                    sour.addedby = loginsession.FUserPK;
                    //               
                    string sCats = "";
                    foreach (ListItem i in CheckBoxListcategories.Items)
                    {
                        if (i.Selected)
                        {
                            sCats += i.Value + ",";
                        }
                    }
                    sCats = sCats.Trim(',');
                    sour.sCats = sCats;
                    //
                    string kinds = "";
                    foreach (ListItem k in CheckBoxListkinds.Items)
                    {
                        if (k.Selected)
                        {
                            kinds += k.Value + ",";
                        }
                    }
                    kinds = kinds.Trim(',');
                    sour.kindlist = kinds;
                    //
                    context.tbl_source.Add(sour);
                    context.SaveChanges();
                    result = "Source has been added.";
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

        protected void ButtonUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                int catid = 0;
                int sourID;
                if (!string.IsNullOrEmpty(HiddenFieldsourID.Value) && int.TryParse(HiddenFieldsourID.Value, out sourID))
                {
                    tbl_source sour = (tbl_source)(context.tbl_source.Where(s => s.SourcePK == sourID).FirstOrDefault());
                    tbl_source dublicat = context.tbl_source.Where(s => s.SourcePK != sourID&& s.title == sourTitle.Value && s.feed_url == sourfeedurl.Value).FirstOrDefault();
                    if (sour != null&& dublicat==null)
                    {
                        sour.title = sourTitle.Value;
                        sour.info = sourinfo.Value;
                        sour.feed_url = sourfeedurl.Value;
                        //sour.isnormal = checkNormal.Checked;
                        //sour.issuperuser = checksuperuser.Checked;
                        //sour.isgolden = checkgolden.Checked;
                        sour.isArgent = CheckboxisArgent.Checked;
                        sour.isManual = CheckboxisManual.Checked;
                        //int.TryParse(CategoriesCombo.Value, out catid);
                        sour.CatFK = catid;
                        sour.edit_date = DateTime.Now;
                        sour.editedby = loginsession.FUserPK;
                        string sCats = "";
                        foreach (ListItem i in CheckBoxListcategories.Items)
                        {
                            if (i.Selected)
                            {
                                sCats += i.Value + ",";
                            }
                        }
                        sCats = sCats.Trim(',');
                        sour.sCats = sCats;
                        //
                        string kinds = "";
                        foreach (ListItem k in CheckBoxListkinds.Items)
                        {
                            if (k.Selected)
                            {
                                kinds += k.Value + ",";
                            }
                        }
                        kinds = kinds.Trim(',');
                        sour.kindlist = kinds;
                        //
                        context.SaveChanges();
                        result = "Source has been updated.";
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
        protected void Buttonfetch_Click(object sender, EventArgs e)
        {
            try
            {
                int sourID;
                if (!string.IsNullOrEmpty(HiddenFieldsourID.Value) && int.TryParse(HiddenFieldsourID.Value, out sourID))
                {
                    var current = context.tbl_source.Where(s => s.SourcePK == sourID).FirstOrDefault();
                    if (current != null&&current.isManual==false)
                    {
                        fetchResult res = RssFeed.ParseRss(current);
                       if ( res.foundCount>0)
                            result =string.Format("Source has been fetched. {0} found  {1} added",res.foundCount,res.addcount);
                       else
                            result = "Fetching source has failed check url.";
                    }
                }
            }
            catch (Exception ex)
            { result = "Error has been occurred"; }
            finally
            {
                showResult(result);
            }
        }
        protected void ButtonSourcesdelete_Click(object sender, EventArgs e)
        {
            try
            {
                int sourID;
                if (!string.IsNullOrEmpty(HiddenFieldsourID.Value) && int.TryParse(HiddenFieldsourID.Value, out sourID))
                {
                    var current = context.tbl_source.Where(s => s.SourcePK == sourID).FirstOrDefault();
                    if (current != null)
                    {
                        //string iconname = current.icon_url;
                        string imagepath = "http://" + Request.Url.Host + "/images/";
                        
                        string iconname =(!string.IsNullOrEmpty(current.icon_url))? current.icon_url.Replace(imagepath, ""):"";
                        current.bdeleted = true;
                        //context.tbl_source.Remove(current);
                        //List<tbl_news_cat> newsCats = context.tbl_news_cat.Where(nc => nc.CatFK == sourID).ToList();
                        //foreach (tbl_news_cat newscat in newsCats)
                        //{
                        //    context.tbl_news_cat.Remove(newscat);
                        //}
                        context.SaveChanges();
                        if(!string.IsNullOrEmpty(iconname))
                        System.IO.File.Delete(MapPath("~").TrimEnd('\\') + "\\images\\" + iconname);
                        result = "Source has been deleted.";
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
                string iconname = Guid.NewGuid().ToString() + ".png";
                if (!FileUploadCatIcon.HasFile || (!FileUploadCatIcon.FileName.Contains(".png") && !FileUploadCatIcon.FileName.Contains(".jpeg") && !FileUploadCatIcon.FileName.Contains(".jpg")))
                {
                    populateImageAlert();
                    error = true;
                    return;
                }
                int sourID;
                if (!string.IsNullOrEmpty(HiddenFieldsourID.Value) && int.TryParse(HiddenFieldsourID.Value, out sourID) && FileUploadCatIcon.FileBytes.Length > 0)
                {
                    FileUploadCatIcon.PostedFile.SaveAs(MapPath("~").TrimEnd('\\') + "\\images\\"  + iconname);
                    var current = context.tbl_source.Where(s => s.SourcePK == sourID).FirstOrDefault();
                    if (current != null)
                    {
                        string imagepath = "http://" + Request.Url.Host + "/images/";
                        current.icon_url = imagepath.TrimEnd('/') + "/" + iconname;
                        current.edit_date = DateTime.Now;
                        current.editedby = loginsession.FUserPK;
                        context.SaveChanges();
                        result = "Icon has been Saved.";
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
                }else
                {
                    Result();
                }
            }
        }
        protected void buttonImagedelete_click(object sender, EventArgs e)
        {
            try
            {
                int sourID;
                if (!string.IsNullOrEmpty(HiddenFieldsourID.Value) && int.TryParse(HiddenFieldsourID.Value, out sourID))
                {

                    var current = context.tbl_source.Where(s => s.SourcePK == sourID).FirstOrDefault();
                    if (current != null)
                    {
                        string imagepath = "http://" + Request.Url.Host + "/images/";
                        string iconname = current.icon_url.Replace(imagepath, "");
                        current.icon_url = "";
                        context.SaveChanges();
                        System.IO.File.Delete(MapPath("~").TrimEnd('\\') + "\\images\\" + iconname);
                        result = "Icon has been deleted.";
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
        protected void Result(bool reset=false)
        {
            if(currentID>0&&!reset)
            Response.Redirect(string.Format("Sources?id={0}", currentID));
            else
                Response.Redirect("Sources");
        }

        protected void Buttonreset_Click(object sender, EventArgs e)
        {
            try
            {
              
                sourTitle.Value = "";
                sourinfo.Value = "";
                sourfeedurl.Value= "";
                //checkNormal.Checked = false;
                //checksuperuser.Checked = false;
                //checkgolden.Checked = false;
                //CategoriesCombo.SelectedIndex = 0;
                CheckBoxListcategories.ClearSelection();
                CheckBoxListkinds.ClearSelection();
            }
            catch (Exception ex)
            { }
        }
    }
}