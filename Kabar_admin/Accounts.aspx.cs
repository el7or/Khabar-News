using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Kabar_admin
{
    public partial class Accounts : System.Web.UI.Page
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
                Buttonchangepassword.Enabled = true;
            }
            else
            {
                ButtonDelete.Enabled = false;
                ButtonUpdate.Enabled = false;
                Buttonchangepassword.Enabled = false;
            }
            //Get Session 
            if(Session["loginsession"] !=null)
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
            if (!loginsession.viewusers)
                Response.Redirect("NoRights");
            //
            ButtonReset.Visible = ButtonAdd.Visible = loginsession.addusers;
            ButtonUpdate.Visible =Buttonchangepassword.Visible= loginsession.editusers;
            ButtonDelete.Visible = loginsession.delusers;

        }
        private void load_current()
        {
            try
            {
               
                if (currentID>0)
                {
                    tbl_frontend_users current = (tbl_frontend_users)(context.tbl_frontend_users.Where(u => u.FUserPK == currentID).FirstOrDefault());
                    HiddenFieldUserID.Value = current.FUserPK.ToString();
                    inputDisplayname.Value = current.displayname;
                    inuputEmail.Value = current.email;
                    ListItem item = SelectKind.Items.FindByValue(current.Kind.ToString());
                    if (item != null)
                        item.Selected = true;
                    //
                    CheckBoxisAdmin.Checked = current.isAdmin;
                    //
                    CheckBoxNewsView.Checked = current.viewnews;
                    CheckBoxNewsAdd.Checked = current.addnews;
                    CheckBoxNewsUpdate.Checked = current.editnews;
                    CheckBoxNewsDelete.Checked = current.delnews;
                    //
                    CheckBoxCategoriesView.Checked = current.viewcats;
                    CheckBoxCategoriesAdd.Checked = current.addcats;
                    CheckBoxCategoriesUpdate.Checked = current.editcats;
                    CheckBoxCategoriesDelete.Checked = current.delcats;
                    //
                    CheckBoxSourceView.Checked = current.viewsources;
                    CheckBoxSourceAdd.Checked = current.addsources;
                    CheckBoxSourceUpdate.Checked = current.editsources;
                    CheckBoxSourceDelete.Checked = current.delsources;
                    //
                    CheckBoxAccountView.Checked = current.viewusers;
                    CheckBoxAccountAdd.Checked = current.addusers;
                    CheckBoxAccountUpdate.Checked = current.editusers;
                    CheckBoxAccountDelete.Checked = current.delusers;
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
        protected string Randompassword(int length)
        {
            Random randpass = new Random();
            string password= randpass.Next(123456, 999999).ToString();
            try
            {
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                password = new string(Enumerable.Repeat(chars, length)
                  .Select(s => s[randpass.Next(s.Length)]).ToArray());
            }catch(Exception ex)
            { }
            return password;
        }
        protected void ButtonAdd_Click(object sender, EventArgs e)
        {
            bool success = true;
            try
            {
                LabelEmailused.Style["display"] = "none";
                string email = inuputEmail.Value.Trim();
                tbl_frontend_users checkuser = context.tbl_frontend_users.Where(f => f.email == email).FirstOrDefault();
                if (checkuser == null)
                {
                    byte kind = 1;
                    tbl_frontend_users user = new tbl_frontend_users();
                    user.displayname = inputDisplayname.Value;
                    user.email = inuputEmail.Value;
                    byte.TryParse(SelectKind.Value, out kind);
                    user.Kind = kind;
                    user.isAdmin = CheckBoxisAdmin.Checked;
                    user.password = Randompassword(10);
                    //
                    user.viewnews = CheckBoxNewsView.Checked;
                    user.addnews = CheckBoxNewsAdd.Checked;
                    user.editnews = CheckBoxNewsUpdate.Checked;
                    user.delnews = CheckBoxNewsDelete.Checked;
                    //
                    user.viewcats = CheckBoxCategoriesView.Checked;
                    user.addcats = CheckBoxCategoriesAdd.Checked;
                    user.editcats = CheckBoxCategoriesUpdate.Checked;
                    user.delcats = CheckBoxCategoriesDelete.Checked;
                    //
                    user.viewsources = CheckBoxSourceView.Checked;
                    user.addsources = CheckBoxSourceAdd.Checked;
                    user.editsources = CheckBoxSourceUpdate.Checked;
                    user.delsources = CheckBoxSourceDelete.Checked;
                    //
                    user.viewusers = CheckBoxAccountView.Checked;
                    user.addusers = CheckBoxAccountAdd.Checked;
                    user.editusers = CheckBoxAccountUpdate.Checked;
                    user.delusers = CheckBoxAccountDelete.Checked;
                    //
                    user.indate = DateTime.Now;
                    user.edit_date = DateTime.Now;
                    user.addedby = loginsession.FUserPK;
                    context.tbl_frontend_users.Add(user);
                    context.SaveChanges();
                    
                    result = "User has been added.";
                }
                else
                {
                    LabelEmailused.Style["display"] = "inline";
                    success = false;
                }
            }
            catch (Exception ex)
            {
                success = false;
                result = "Error has been occurred";
            }
            finally
            {
                showResult(result);
                if (success)
                    Result();
            }
        }

        protected void ButtonUpdate_Click(object sender, EventArgs e)
        {
           
            bool success = true;
            try
            {
                LabelEmailused.Style["display"] = "none";
                string email = inuputEmail.Value.Trim();
                byte kind = 1;
                int userID;
                if (!string.IsNullOrEmpty(HiddenFieldUserID.Value) && int.TryParse(HiddenFieldUserID.Value, out userID))
                {
                    tbl_frontend_users checkuser = context.tbl_frontend_users.Where(f => f.email == email&&f.FUserPK!= userID).FirstOrDefault();
                    if (checkuser == null)
                    {
                        tbl_frontend_users user = (tbl_frontend_users)(context.tbl_frontend_users.Where(u => u.FUserPK == userID).FirstOrDefault());
                        if (user != null)
                        {

                            user.displayname = inputDisplayname.Value;
                            user.email = email;
                            byte.TryParse(SelectKind.Value, out kind);
                            user.Kind = kind;
                            //
                            user.isAdmin = CheckBoxisAdmin.Checked;
                            //
                            user.viewnews = CheckBoxNewsView.Checked;
                            user.addnews = CheckBoxNewsAdd.Checked;
                            user.editnews = CheckBoxNewsUpdate.Checked;
                            user.delnews = CheckBoxNewsDelete.Checked;
                            //
                            user.viewcats = CheckBoxCategoriesView.Checked;
                            user.addcats = CheckBoxCategoriesAdd.Checked;
                            user.editcats = CheckBoxCategoriesUpdate.Checked;
                            user.delcats = CheckBoxCategoriesDelete.Checked;
                            //
                            user.viewsources = CheckBoxSourceView.Checked;
                            user.addsources = CheckBoxSourceAdd.Checked;
                            user.editsources = CheckBoxSourceUpdate.Checked;
                            user.delsources = CheckBoxSourceDelete.Checked;
                            //
                            user.viewusers = CheckBoxAccountView.Checked;
                            user.addusers = CheckBoxAccountAdd.Checked;
                            user.editusers = CheckBoxAccountUpdate.Checked;
                            user.delusers = CheckBoxAccountDelete.Checked;
                            //
                            user.edit_date = DateTime.Now;
                            user.editedby = loginsession.FUserPK;
                            context.SaveChanges();
                            result = "User has been updated.";
                        }
                    }else
                    {
                        LabelEmailused.Style["display"] = "inline";
                        success = false;
                    }
                }
            }
            catch (Exception ex)
            {
                success = false;
                result = "Error has been occurred";
            }
            finally
            {
                showResult(result);
                if (success)
                    Result();
            }
        }

        protected void ButtonAccountdelete_Click(object sender, EventArgs e)
        {
            try
            {
                int userID;
                if (!string.IsNullOrEmpty(HiddenFieldUserID.Value) && int.TryParse(HiddenFieldUserID.Value, out userID))
                {
                    var current = context.tbl_frontend_users.Where(u => u.FUserPK == userID).FirstOrDefault();
                    if (current != null)
                    {
                       
                        context.tbl_frontend_users.Remove(current);
                        context.SaveChanges();
                        result = "User has been deleted.";
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
        protected void ButtonupdatePassword_Click(object sender, EventArgs e)
        {
            try
            {
                int userID;
                if (!string.IsNullOrEmpty(HiddenFieldUserID.Value) && int.TryParse(HiddenFieldUserID.Value, out userID))
                {
                    var current = context.tbl_frontend_users.Where(u => u.FUserPK == userID).FirstOrDefault();
                    if (current != null)
                    {
                        current.password = TextBoxpassword.Text;
                        
                        context.SaveChanges();
                        result = "password has been updated.";
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
                Result(false);
            }
        }

        protected void Result(bool reset=false)
        {
            if(currentID>0&&!reset)
            Response.Redirect(string.Format("Accounts?id={0}", currentID));
            else
                Response.Redirect("Accounts");
        }

        protected void Buttonreset_Click(object sender, EventArgs e)
        {
            try
            {
                inputDisplayname.Value = "";
                inuputEmail.Value = "";
                //
                CheckBoxNewsView.Checked = false;
                CheckBoxNewsAdd.Checked = false;
                CheckBoxNewsUpdate.Checked = false;
                CheckBoxNewsDelete.Checked = false;
                //
                CheckBoxCategoriesView.Checked = false;
                CheckBoxCategoriesAdd.Checked = false;
                CheckBoxCategoriesUpdate.Checked = false;
                CheckBoxCategoriesDelete.Checked = false;
                //
                CheckBoxSourceView.Checked = false;
                CheckBoxSourceAdd.Checked = false;
                CheckBoxSourceUpdate.Checked = false;
                CheckBoxSourceDelete.Checked = false;
                //
                CheckBoxAccountView.Checked = false;
                CheckBoxAccountAdd.Checked = false;
                CheckBoxAccountUpdate.Checked = false;
                CheckBoxAccountDelete.Checked = false;
            }
            catch (Exception ex)
            { }
        }
    }
}