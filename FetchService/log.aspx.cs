using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FetchService
{
    public partial class log : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string filename = string.Format("{0}-{1}-{2}.log", DateTime.UtcNow.Day, DateTime.UtcNow.Month, DateTime.UtcNow.Year);
                string fullpath = HostingEnvironment.MapPath("~").Trim('\\') + "\\log\\" + filename;
                string line = "";
                if (System.IO.File.Exists(fullpath))
                {
                    System.IO.StreamReader file = new System.IO.StreamReader(fullpath);
                    while ((line = file.ReadLine()) != null)
                    {
                        Response.Write(line + "</br>");

                    }

                    file.Close();
                }
            }catch(Exception ex)
            {
                Response.Write(ex.Message + "</br>");

            }
        }
    }
}