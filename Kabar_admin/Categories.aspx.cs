using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Kabar_admin
{
    public partial class Categories : System.Web.UI.Page
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
            }else
            {
                    ButtonDelete.Enabled = false;
                    ButtonUpdate.Enabled = false;
                    ButtonImage.Enabled = false;
            }
            //Get Session 
            if (Session["loginsession"] != null)
            {
                loginsession = (tbl_frontend_users)Session["loginsession"];
                ApplyRights();
            }
            if (!IsPostBack)
            {
               
                load_current();
            }
        }
        private void ApplyRights()
        {
            if (!loginsession.viewcats)
                Response.Redirect("NoRights");
            ButtonReset.Visible = ButtonAdd.Visible = loginsession.addcats;
            ButtonUpdate.Visible = Buttonsavephoto.Visible = Buttondeletephoto.Visible = loginsession.editcats;
            ButtonDelete.Visible = loginsession.delcats;
        }
        private void load_current()
        {
            try
            {
               
                if (currentID>0)
                {
                    tbl_category current = (tbl_category)(context.tbl_category.Where(c => c.CatPK == currentID).FirstOrDefault());
                    HiddenFieldCatID.Value = current.CatPK.ToString();
                    catTitle.Value = current.title;
                    catinfo.Value = current.info;
                    CheckboxisCountry.Checked = current.isCountry;
                    if(!string.IsNullOrEmpty(current.icon_url))
                    {
                        CatIcon.Visible = true;
                        FileUploadCatIcon.Visible = false;
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
               
                tbl_category cat = new tbl_category();
                cat.title = catTitle.Value;
                cat.info = catinfo.Value;
                cat.isCountry = CheckboxisCountry.Checked;
                cat.indate = DateTime.Now;
                cat.edit_date = DateTime.Now;
                cat.addedby = loginsession.FUserPK;
                context.tbl_category.Add(cat);
                context.SaveChanges();
               
                result = "Category has been added.";
            }
            catch (Exception ex)
            {
                result = "Error has been occurred";
            } finally
            {
                showResult(result);
                Result();
            }
        }

        protected void ButtonUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                int catID;
                if (!string.IsNullOrEmpty(HiddenFieldCatID.Value) && int.TryParse(HiddenFieldCatID.Value, out catID))
                {
                    tbl_category cat = (tbl_category)(context.tbl_category.Where(n => n.CatPK == catID).FirstOrDefault());
                    if (cat != null)
                    {
                        cat.title = catTitle.Value;
                        cat.info = catinfo.Value;
                        cat.isCountry = CheckboxisCountry.Checked;
                        cat.edit_date = DateTime.Now;
                        cat.editedby = loginsession.FUserPK;
                        context.SaveChanges();
                        result = "Category has been updated.";
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

        protected void ButtonCatsdelete_Click(object sender, EventArgs e)
        {
            try
            {
                int catID;
                if (!string.IsNullOrEmpty(HiddenFieldCatID.Value) && int.TryParse(HiddenFieldCatID.Value, out catID))
                {
                    var current = context.tbl_category.Where(c => c.CatPK == catID).FirstOrDefault();
                    if (current != null)
                    {
                        string iconname = current.icon_url;
                        context.tbl_category.Remove(current);
                        List<tbl_news_cat> newsCats = context.tbl_news_cat.Where(nc => nc.CatFK == catID).ToList();
                        foreach (tbl_news_cat newscat in newsCats)
                        {
                            context.tbl_news_cat.Remove(newscat);
                        }
                        context.SaveChanges();
                        System.IO.File.Delete(MapPath("~").TrimEnd('\\') + "\\images\\" + iconname);
                        result = "Category has been deleted.";
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
                int catID;
                if (!string.IsNullOrEmpty(HiddenFieldCatID.Value) && int.TryParse(HiddenFieldCatID.Value, out catID) && FileUploadCatIcon.FileBytes.Length > 0)
                {
                    FileUploadCatIcon.PostedFile.SaveAs(MapPath("~").TrimEnd('\\') + "\\images\\"  + iconname);
                    var current = context.tbl_category.Where(n => n.CatPK == catID).FirstOrDefault();
                    if (current != null)
                    {
                        current.icon_url = iconname;
                        current.edit_date = DateTime.Now;
                        current.addedby = loginsession.FUserPK;
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
                int catID;
                if (!string.IsNullOrEmpty(HiddenFieldCatID.Value) && int.TryParse(HiddenFieldCatID.Value, out catID))
                {

                    var current = context.tbl_category.Where(c => c.CatPK == catID).FirstOrDefault();
                    if (current != null)
                    {
                        string iconname = current.icon_url;
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
            Response.Redirect(string.Format("Categories?id={0}", currentID));
            else
                Response.Redirect("Categories");
        }

        protected void Buttonreset_Click(object sender, EventArgs e)
        {
            try
            {
              
                catTitle.Value = "";
                catinfo.Value = "";
               
            }
            catch (Exception ex)
            { }
        }
    }
}