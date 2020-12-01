using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Prism.Data;
using Prism.Helpers;
using Prism.Models;
using System.Collections.Generic;

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

        [HttpGet]
        public IActionResult Get([FromQuery] string[] links)
        {
            List<NewsArticle> news = new List<NewsArticle>();

            foreach (string link in links)
            {
                news.AddRange(RSSReader.Read(link));  
            }
            return Ok(new
            {
                news
            });
        }
    }
}
