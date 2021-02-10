using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Frens.Entities;
using Frens.Entities.Models;
using Frens.Payloads;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Frens.Enums;

namespace Frens.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserPostsController : ControllerBase
    {
        private readonly FrensContext _db;

        public UserPostsController(FrensContext db)
        {
            _db = db;
        }

        [Route("GetAllPosts")]
        [AllowAnonymous]
        [HttpGet]
        public ActionResult<List<Post>> GetAllPosts()
        {
            var postare = _db.Posts.Include(p => p.User).Include(p => p.Comment).OrderByDescending(p => p.Id).ToList();
            return (postare);
            //return _db.Posts.Include(p => p.User).ToList();

        }

        [Route("GetPostsById")]
        [AllowAnonymous]
        [HttpGet]
        [HttpGet]
        public ActionResult<Post> GetById(int Id)
        {
            var postare= _db.Posts.Include(p=>p.User).Single(post => Id == post.Id);
            return postare;
        }
        [Route("Create")]
        [AllowAnonymous]
        [HttpPost]
        public ActionResult<Post> Create([FromBody] PostsPayload payload)
        {
            try
            {
                var getUserId = int.Parse(HttpContext.User.Claims.First(claim => claim.Type == "Id").Value);
                var postToAdd = new Post
                {

                UserId= getUserId,
                User = _db.Users.Single(u => u.Id == getUserId),
               // PostId = payload.PostId,
               // Post = _db.Posts.Single(p => p.Id == payload.PostId),
                Text=payload.Text

                };

                _db.Posts.Add(postToAdd);
                _db.SaveChanges();

                return Ok(postToAdd);
            }
            catch (Exception)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);

            }
        }//create code end 

        [Route("Comment")]
        [AllowAnonymous]

        public ActionResult<Comment> CreateComment([FromBody] CommentPayload payload)
        {
            try
            {
                var getUserId = int.Parse(HttpContext.User.Claims.First(claim => claim.Type == "Id").Value);
                var commentToAdd = new Comment
                {

                    UserId = getUserId,
                    User = _db.Users.Single(u => u.Id == getUserId),
                    PostId = payload.PostId,
                    Post = _db.Posts.Single(p => p.Id == payload.PostId),
                    Text = payload.Text

                };

                _db.Comments.Add(commentToAdd);
                _db.SaveChanges();

                return Ok(commentToAdd);
            }
            catch (Exception)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);

            }
        }//create code end 

        [Route("GetAllComments")]
        [AllowAnonymous]
        [HttpGet]
        public ActionResult<List<Comment>> GetAllComments()
        {
            return _db.Comments.ToList();
        }

        [Route("GetCommentById")]
        [AllowAnonymous]
        [HttpGet]
        public ActionResult<List<Comment>> GetCommentById(int Id)
        {
            var com = _db.Posts.Include(p=> p.Comment).Single(post => Id == post.Id).Comment;

            return com;
        } 

    }
}
