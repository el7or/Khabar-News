using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Kabar_admin
{
    public class NewsApiController : ApiController
    {
        khabrEntities context = new khabrEntities();
        [HttpPost]
        public IHttpActionResult Login()
        {
            try
            {
                var o = Request.Content;
                string jsonContent = o.ReadAsStringAsync().Result;
                var param = HttpUtility.ParseQueryString(jsonContent);
                string email = param["Email"];
                string pass = param["Password"];

                tbl_frontend_users user = context.tbl_frontend_users.Where(u => u.email == email && u.password == pass).FirstOrDefault();
                if (user == null)
                {
                        return Ok("Wrong Email or Password !");
                }
                else
                {
                    JsonSerializerSettings jSetting = new JsonSerializerSettings();
                    jSetting.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                    jSetting.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    return Ok(JsonConvert.DeserializeObject(JsonConvert.SerializeObject(user, jSetting)));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        public IHttpActionResult LoadSources()
        {
            try
            {
                var o = Request.Content;
                string jsonContent = o.ReadAsStringAsync().Result;
                var param = HttpUtility.ParseQueryString(jsonContent);
                string email = param["Email"];
                string pass = param["Password"];

                tbl_frontend_users user = context.tbl_frontend_users.Where(u => u.email == email && u.password == pass).FirstOrDefault();
                if (user == null)
                {
                    return Ok("Wrong Email or Password !");
                }
                else
                {
                    var sources = context.tbl_source.Where(s => s.bdeleted == false).ToList();
                   
                    return Ok(sources);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        public IHttpActionResult LoadNews()
        {
            try
            {
                var o = Request.Content;
                string jsonContent = o.ReadAsStringAsync().Result;
                var param = HttpUtility.ParseQueryString(jsonContent);
                string email = param["Email"];
                string pass = param["Password"];
                int NewsID = 0;
                int.TryParse(param["NewsID"],out NewsID);
                tbl_frontend_users user = context.tbl_frontend_users.Where(u => u.email == email && u.password == pass).FirstOrDefault();
                if (user == null)
                {
                    return Ok("Wrong Email or Password !");
                }
                else
                {
                    var News = context.tbl_today_news.Where(n => n.NewsPK >NewsID).Take(25).ToList();
                  
                    return Ok(News);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}