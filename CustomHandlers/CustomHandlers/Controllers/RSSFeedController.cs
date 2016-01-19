using CustomHandlers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;

namespace CustomHandlers.Controllers
{
    public class RSSFeedController : Controller
    {
        // GET: RSSFeed
        public ActionResult Index()
        {
            return View(ParseRssXmlFile());
        }
        
        private List<RSSFeed> ParseRssXmlFile()
        {
            string RssFeedUrl = "http://timesofindia.feedsportal.com/c/33039/f/533965/index.rss";
            List<RSSFeed> feeds = new List<RSSFeed>();
            try
            {
                XDocument xDoc = new XDocument();
                xDoc = XDocument.Load(RssFeedUrl);
                var items = (from x in xDoc.Descendants("item")
                             select new
                             {
                                 title = x.Element("title").Value,
                                 link = x.Element("link").Value,
                                 pubDate = x.Element("pubDate").Value,
                                 description = x.Element("description").Value
                             });

                if (items != null)
                {
                    foreach (var i in items)
                    {
                        RSSFeed f = new RSSFeed
                        {
                            Title = i.title,
                            Link = i.link,
                            PublishDate = i.pubDate,
                            Description = i.description
                        };

                        feeds.Add(f);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return feeds;
        }
    }
}