using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Kabar_admin
{
    public partial class SiteMaster : MasterPage
    {
        public tbl_frontend_users loginsession = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Session["loginsession"] ==null)
            {
                Response.Redirect("Auth");
            }
            //Get Session 
            else
            {
                loginsession = (tbl_frontend_users)Session["loginsession"];
                Labelusername.Text = loginsession.displayname;
                loginsession.password = "";
                loginsession.verifcationcode = Guid.Empty;
                loginsession.email = "";
            }
        }
    }
}