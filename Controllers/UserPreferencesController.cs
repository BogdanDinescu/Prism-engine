using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    [Route("preferences")]
    public class UserPreferencesController : ControllerBase
    {

        private readonly DatabaseCtx database;

        public UserPreferencesController(DatabaseCtx context)
        {
            this.database = context;
        }


        private int GetUserId()
        {
            try
            {
                return Int32.Parse(User.FindFirst(ClaimTypes.Name)?.Value);
            }
            catch (Exception e)
            {
                throw new ApplicationException("UserId was not found");
            }
        }

        [HttpPost]
        [Route("news")]
        public IActionResult SetNewsPreferences([FromBody] int[] newsSourceIds)
        {
            int userId = GetUserId();
            List<NewsSource> newNewsSources = database.NewsSources.Where(n => newsSourceIds.Contains(n.Id)).ToList();
            var userPreferences = database.UserPreferences.Find(userId);
            if (userPreferences == null)
            {
                userPreferences = new UserPreference { UserId = userId, NewsSources = newNewsSources};
            }
            else
            {
                database.UserPreferences.Remove(userPreferences);
                database.SaveChanges();
            }
            userPreferences = new UserPreference { UserId = userId, NewsSources = newNewsSources };
            database.UserPreferences.Add(userPreferences);
            database.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("news")]
        public IActionResult GetNewsPreferences()
        {
            int userId = GetUserId();
            
            return Ok(database.UserPreferences.Where(u => u.UserId == userId).SelectMany(u => u.NewsSources));

        }
    }
}
