using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomHandlers.Models
{
    public class RSSFeed
    {
        public string Link { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public string PublishDate { get; set; }
    }
}