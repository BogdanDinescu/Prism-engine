using System;
using Prism.Data;
using Prism.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;

namespace Prism.Helpers
{
    public static class RSSReader
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
                    CreateDate = DateTime.Today,
                    Group = 0,
                    SimHash = SimHash.SimHashOfString(item.Title.Text),
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
            //databaseCtx.NewsArticles.RemoveRange(databaseCtx.NewsArticles);
            List<NewsArticle> newsArticles = new List<NewsArticle>();
            // read from all sources
            for (int i = 0; i < newsSources.Count; i++)
            {
                try
                {
                    NewsSource source = newsSources[i];
                    List<NewsArticle> articles = Read(source.Link,source);
                    newsArticles.AddRange(articles);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                
            }
            
            // last group number
            int lastGroup = databaseCtx.NewsArticles.Select(x => (int?)x.Group).Max() ?? 0;
            
            for (int rep = 0; rep < 31; rep++) // 32 is number of bits of the SimHash fingerprint
            {
                // sort
                newsArticles.Sort((x, y) => x.SimHash.CompareTo(y.SimHash));
                // search for possible groups
                for (int i = 1; i < newsArticles.Count; i++)
                {
                    var previous = newsArticles[i - 1];
                    var current = newsArticles[i];
                    if (HammingDistance(previous.SimHash, current.SimHash) <= 4)
                    {
                        if (previous.Group == 0)
                        {
                            previous.Group = current.Group = ++lastGroup;
                        }
                        else
                        {
                            current.Group = previous.Group;
                        }
                    }
                }

                // rotate
                foreach (var article in newsArticles)
                {
                    article.SimHash = RotateLeft(article.SimHash, 1);
                }
            }

            databaseCtx.AddRange(newsArticles);
            databaseCtx.SaveChanges();
            Debug.WriteLine("News read from " + newsSources.Count.ToString() + " sources");
        }
        
        public static uint HammingDistance(uint n1, uint n2)
        {
            uint x = n1 ^ n2;
            uint setBits = 0;

            while (x > 0) 
            {
                setBits += x & 1;
                x >>= 1;
            }
            return setBits;
        }
        
        public static uint RotateLeft(this uint value, int count)
        {
            return (value << count) | (value >> (32 - count));
        }
    }
}
