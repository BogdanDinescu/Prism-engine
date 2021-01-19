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
        public static List<NewsArticle> Read(string link, NewsSource newsSource)
        {       
            XmlReader reader = XmlReader.Create(link);
            SyndicationFeed feed = SyndicationFeed.Load(reader);
            reader.Close();

            List<NewsArticle> articles = new List<NewsArticle>();

            foreach (SyndicationItem item in feed.Items)
            {
                articles.Add(new NewsArticle {
                    Title = item.Title.Text,
                    Source = feed.Links.Count >= 1 ? feed.Links[0].Uri.ToString():"",
                    Link = item.Links.Count >= 1 ? item.Links[0].Uri.ToString(): "",
                    Content = item.Summary.Text,
                    ImageUrl = item.Links.Count >= 2 ? item.Links[1].Uri.ToString():"",
                    NewsSource = newsSource
                });
            }
            return articles;
        }

        public static void ReadAllAndStore(DatabaseCtx databaseCtx)
        {
            List<NewsSource> newsSources = databaseCtx.NewsSources.ToList();
            databaseCtx.NewsArticles.RemoveRange(databaseCtx.NewsArticles);
            List<List<NewsArticle>> newsArticles = new List<List<NewsArticle>>();
            int maxim = 0;
            for (int i = 0; i<newsSources.Count; i++)
            {
                NewsSource source = newsSources[i];
                List<NewsArticle> articles = Read(source.Link,source);
                newsArticles.Add(articles);
                if (articles.Count > maxim)
                    maxim = articles.Count;
            }
            for (int j = 0; j < maxim; j++)
            {
                for (int i = 0; i < newsArticles.Count; i++)
                {
                    if (j < newsArticles[i].Count)
                    {
                        databaseCtx.NewsArticles.Add(newsArticles[i][j]);
                    }
                }
            }
            databaseCtx.SaveChanges();
            Debug.WriteLine("News read from " + newsSources.Count.ToString() + " sources");
        }
    }
}
