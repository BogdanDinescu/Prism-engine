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
            List<NewsArticle> articles = new List<NewsArticle>();
            articles.Add(new NewsArticle {
                Title = "Articol senzational",
                Link = "https://getbootstrap.com/docs/4.0/utilities/flex/",
                Content = "Un articol senzational pe care il sciru gresit pentru ca nu imi pasa",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/b/b6/Image_created_with_a_mobile_phone.png/220px-Image_created_with_a_mobile_phone.png"
            });
            articles.Add(new NewsArticle
            {
                Title = "Articol senzational 2",
                Link = "https://getbootstrap.com/docs/4.0/utilities/flex/",
                Content = "Inca un articol senzational pe care il sciru gresit pentru ca nu imi pasa",
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/b/b6/Image_created_with_a_mobile_phone.png/220px-Image_created_with_a_mobile_phone.png"
            });
            /*
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
            }*/
            return articles;
        }
    }
}
