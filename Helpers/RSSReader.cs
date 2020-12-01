using Prism.Models;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Xml;

namespace Prism.Helpers
{

    public interface IRSSReader
    {
        public IEnumerable<NewsArticle> Read(string link);
    }

    public class RSSReader
    {
        public static IEnumerable<NewsArticle> Read(string link)
        {
            XmlReader reader = XmlReader.Create(link);
            SyndicationFeed feed = SyndicationFeed.Load(reader);
            reader.Close();

            List<NewsArticle> articles = new List<NewsArticle>();

            foreach (SyndicationItem item in feed.Items)
            {
                articles.Add(new NewsArticle {
                    Title = item.Title.Text,
                    Link = item.Links[0].Uri.ToString(),
                    Content = item.Summary.Text,
                    ImageUrl = item.Links[1].Uri.ToString()
                });
            }
            return articles;
        }
    }
}
