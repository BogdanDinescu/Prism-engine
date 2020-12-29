using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Prism.Data;
using Prism.Models;
using System;
using System.Linq;
using System.Security.Claims;

namespace Prism.Controllers
{
    [Authorize]
    [ApiController]
    [Route("post")]
    public class PostController : ControllerBase
    {
        private readonly DatabaseCtx database;

        public PostController(DatabaseCtx context)
        {
            this.database = context;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] int number = 100)
        {
            return Ok(new { posts = database.Posts.Select(x => new { x.Id, x.ImageUrl, x.Caption, x.User.Name }).Take(number).ToList() });
        
        }

        [HttpPost]
        public IActionResult Post(Post post)
        {
            try
            {
                int id = GetUserId();
                var user = database.Users.Find(id);
                if (user == null)
                    throw new ApplicationException("User not found");

                post.User = user;
                database.Posts.Add(post);
                database.SaveChanges();
                return Ok();
            }
            catch (ApplicationException)
            {
                return Unauthorized("You're not loged in");
            }
            
        }

        [HttpPut]
        public IActionResult Put(Post newPost)
        {
            try
            {
                int id = GetUserId();
                var user = database.Users.Find(id);
                if (user == null)
                    throw new ApplicationException("User not found");

                var oldPost = database.Posts.Find(newPost.Id);
                if (oldPost.User.UserId != id)
                    throw new ApplicationException("Unauhtorized");

                oldPost.Caption = newPost.Caption;
                oldPost.ImageUrl = newPost.ImageUrl;

                database.Posts.Update(oldPost);
                database.SaveChanges();
                return Ok();
            }
            catch (ApplicationException)
            {
                return Unauthorized("You're not loged in");
            }
        }

        [HttpDelete]
        public IActionResult Delete([FromQuery] int id)
        {
            var databasePost = database.Posts.Find(id);
            if (databasePost == null)
            {
                return NotFound("Post not found");
            }
            database.Posts.Remove(databasePost);
            database.SaveChanges();
            return Ok();
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
    }
}
