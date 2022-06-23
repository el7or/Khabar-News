using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kabar_admin
{
    public class NewsFilter
    {
        public int creator = -1;
        public int SourcePK = -1;
        public int hasImage = -1;
        public int catPK = -1;
        public int day = 0;
        public int interval = -1;
        public string title = string.Empty;
         public string body = string.Empty;
    }
}