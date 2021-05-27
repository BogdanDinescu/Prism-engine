using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prism.Data;
using Prism.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Prism.Controllers
{
    [Authorize]
    [ApiController]
    [Route("news")]
    public class NewsController : ControllerBase
    {
        private readonly DatabaseCtx database;

        public NewsController(DatabaseCtx context)
        {
            this.database = context;
        }

        private int GetUserId()
        {
            try
            {
                return Int32.Parse(User.FindFirst(ClaimTypes.Name)?.Value);
            }
            catch (Exception)
            {
                throw new ApplicationException("UserId was not found");
            }
        }

        [HttpGet]
        // Get news
        public IActionResult Get([FromQuery] int page = 0)
        {
            int userId = GetUserId();
            List<NewsSource> sources = database.UserPreferences.Where(u => u.UserId == userId).SelectMany(u => u.NewsSources).ToList();

            return Ok(new
            {
                news = database.NewsArticles.Where(x => sources.Contains(x.NewsSource))
                    .OrderByDescending(x => x.CreateDate).ThenByDescending(x => x.Group).Select(x => new { x.Id, x.Title, x.Source, x.Content, x.ImageUrl, x.Link, x.CreateDate, x.Group})
                    .Skip(page*20).Take(20).ToList()
            });
        }

        [HttpGet]
        [Route("search")]
        public IActionResult Search([FromQuery] string searchQuery, [FromQuery] string date)
        {
            IQueryable<NewsArticle> query = database.NewsArticles.AsQueryable();;
            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = database.NewsArticles.Where(a => a.Title.Contains(searchQuery));
            }

            try
            {
                DateTime dateTime = DateTime.Parse(date);
                query = query.Where(a => a.CreateDate.Equals(dateTime)).OrderByDescending(x => x.Group);
            }
            catch (Exception)
            {
                query = query.OrderByDescending(x => x.CreateDate);
            }
            
            return Ok(new
            {
                news = query.Take(50).ToList()
            });
        }

        [HttpGet]
        [Route("sources")]
        // Get sources
        public IActionResult GetSources()
        {
            int userId = GetUserId();

            List<NewsSource> preferedNewsSources = database.UserPreferences.Where(u => u.UserId == userId).SelectMany(u => u.NewsSources).ToList();
            List<NewsSource> newsSources = database.NewsSources.ToList();
            List<object> returnedList = new List<object>();

            foreach (NewsSource newsSource in newsSources)
            {
                returnedList.Add(new
                {
                    id = newsSource.Id,
                    name = newsSource.Name,
                    link = newsSource.Link,
                    selected = preferedNewsSources.Contains(newsSource)
                });
            }
            return Ok(returnedList);
        }

        [HttpPost]
        [Route("sources")]
        [Authorize(Roles = Role.Admin)]
        public IActionResult AddSource([FromBody] NewsSource source)
        {
            database.NewsSources.Add(source);
            database.SaveChanges();
            return Ok();
        }

        [HttpPut]
        [Route("sources")]
        [Authorize(Roles = Role.Admin)]
        public IActionResult ModifySource([FromBody] NewsSource source)
        {

            var s = database.NewsSources.Find(source.Id);

            if (s == null)
                return NotFound("Source not found");

            database.Entry(s).CurrentValues.SetValues(source);
            database.SaveChanges();
            return Ok(source);
        }

        [HttpDelete]
        [Route("sources")]
        [Authorize(Roles = Role.Admin)]
        public IActionResult DeleteSource([FromQuery] int id)
        {

            var s = database.NewsSources.Find(id);

            if (s != null)
            {
                database.NewsSources.Remove(s);
                database.SaveChanges();
                return Ok();
            }
            return NotFound("Source not found with id " + id);
        }
    }
}
