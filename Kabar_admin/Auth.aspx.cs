using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Kabar_admin
{
    public partial class Auth : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void loginbtn_Click(object sender, EventArgs e)
        {
            string email = inputEmail.Value.Trim();
            string password = inputPassword.Value.Trim();
            if ( !String.IsNullOrEmpty(email) && !String.IsNullOrEmpty(password))
            {
                khabrEntities context = new khabrEntities();
                var currentuser = context.tbl_frontend_users.Where(u => u.email == email && u.password == password).FirstOrDefault();
                if (currentuser != null&&currentuser.isAdmin)
                {
                    Session.Add("loginsession", currentuser);
                    if (currentuser.viewnews)
                        Response.Redirect("News");
                    else if (currentuser.viewsources)
                        Response.Redirect("Sources");
                    else if (currentuser.viewcats)
                        Response.Redirect("Categories");
                    else if (currentuser.viewusers)
                        Response.Redirect("Accounts");
                    else
                        Response.Redirect("NoRights");

                }
                else
                {
                    error.Visible = true;
                }

            }
            else
            {
                error.Visible = true;
            }
        }
    }
}