using Prism.Data;
using Prism.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;

namespace Prism.Helpers
{
    public class RSSReader
    {
        public static IEnumerable<NewsArticle> Read(string link, NewsSource newsSource)
        {       
            XmlReader reader = XmlReader.Create(link);
            SyndicationFeed feed = SyndicationFeed.Load(reader);
            reader.Close();

            List<NewsArticle> articles = new List<NewsArticle>();

            foreach (SyndicationItem item in feed.Items)
            {
                articles.Add(new NewsArticle {
                    Title = item.Title.Text,
                    Source = feed.Links[0].Uri.ToString(),
                    Link = item.Links[0].Uri.ToString(),
                    Content = item.Summary.Text,
                    ImageUrl = item.Links[1].Uri.ToString(),
                    NewsSource = newsSource
                });
            }
            return articles;
        }

        public static void ReadAllAndStore(DatabaseCtx databaseCtx)
        {
            List<NewsSource> newsSources = databaseCtx.NewsSources.ToList();
            databaseCtx.NewsArticles.RemoveRange(databaseCtx.NewsArticles);
            foreach (NewsSource source in newsSources)
            {
                IEnumerable<NewsArticle> articles = Read(source.Link,source);
                databaseCtx.NewsArticles.AddRange(articles);         
            }
            databaseCtx.SaveChanges();
            Debug.WriteLine("News read from " + newsSources.Count.ToString() + " sources");
        }
    }
}
