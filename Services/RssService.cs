﻿using System;
using Prism.Data;
using Prism.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;
using Prism.Helpers;

namespace Prism.Services
{
    public interface IRssService
    {
        void ReadAllAndStore();
        List<NewsArticle> Read(NewsSource newsSource);
    }
    
    public class RssService: IRssService
    {
        private readonly DatabaseCtx context;

        public RssService(DatabaseCtx context)
        {
            this.context = context;
        }

        public List<NewsArticle> Read(NewsSource newsSource)
        {       
            XmlReader reader = XmlReader.Create(newsSource.Link);
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

        public void ReadAllAndStore()
        {
            List<NewsSource> newsSources = context.NewsSources.ToList();
            //databaseCtx.NewsArticles.RemoveRange(databaseCtx.NewsArticles);
            List<NewsArticle> newsArticles = new List<NewsArticle>();
            // read from all sources
            for (int i = 0; i < newsSources.Count; i++)
            {
                try
                {
                    NewsSource source = newsSources[i];
                    List<NewsArticle> articles = Read(source);
                    newsArticles.AddRange(articles);
                }
                catch (Exception)
                {
                    Console.WriteLine("Cannot read from source" + newsSources[i].Name);
                }
                
            }
            
            // last group number
            int lastGroup = context.NewsArticles.Select(x => (int?)x.Group).Max() ?? 0;
            
            for (int rep = 0; rep < 31; rep++) // 32 is number of bits of the SimHash fingerprint
            {
                // sort
                newsArticles.Sort((x, y) => x.SimHash.CompareTo(y.SimHash));
                // search for possible groups
                for (int i = 1; i < newsArticles.Count; i++)
                {
                    var previous = newsArticles[i - 1];
                    var current = newsArticles[i];
                    if (HammingDistance(previous.SimHash, current.SimHash) <= 3)
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

            context.AddRange(newsArticles);
            context.SaveChanges();
            Console.WriteLine("News read from " + newsSources.Count.ToString() + " sources");
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
        
        public static uint RotateLeft(uint value, int count)
        {
            return (value << count) | (value >> (32 - count));
        }
    }
}
