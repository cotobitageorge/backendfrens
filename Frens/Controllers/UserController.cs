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

namespace Frens.Controllers
{
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly FrensContext _db;

        public UserController(FrensContext db)
        {
            _db = db;
        }
        [HttpGet]
        public ActionResult<List<User>> GetAll()
        {
            var currentUser = HttpContext.User;

            if(currentUser.HasClaim(claim => claim.Type == "Role"))
            {
                var role = currentUser.Claims.FirstOrDefault(c => c.Type = "Role").Value;
                if(role == "Admin")
                {
                    return _db.Users.ToList();
                }
            }
            return _db.Users.ToList();
        }
        [HttpGet]
        public ActionResult<User> GetById(int Id)
        {
            return _db.Users.Where(user => Id == user.Id).Single();
        }

        [HttpPost]
        public ActionResult<User> Create([FromBody] UserPayload payload)
        {
            try
            {
                var userToAdd = new User
                {
                    FirstName = payload.FirstName,
                    LastName = payload.LastName,
                    Email = payload.Email,
                    Gender = payload.Gender
                };

                _db.Users.Add(userToAdd);
                _db.SaveChanges();

                return Ok(userToAdd);
            }
            catch (Exception)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);

            }
        }//create code end 


        [HttpPost]
        public ActionResult<User> Update([FromBody] UserPayload payload)
        {
            try
            {
                if (payload.Id.HasValue)
                {
                    var userToUpdate = _db.Users.SingleOrDefault(user => payload.Id.Value == user.Id);

                    userToUpdate.FirstName = payload.FirstName;
                    userToUpdate.LastName = payload.LastName;
                    userToUpdate.Email = payload.Email;
                    userToUpdate.Gender = payload.Gender;

                    _db.SaveChanges();
                    return Ok(userToUpdate);
                }
                else
                {
                    return new StatusCodeResult(StatusCodes.Status400BadRequest);
                }
            }
            catch (Exception)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }//update code ends
        [HttpDelete]
        public ActionResult Delete(int Id)
        {
            try
            {
                var userToDelete = _db.Users.Single(user => Id == user.Id);

                _db.Users.Remove(userToDelete);
                _db.SaveChanges();
                return Ok(new { status = true });
            }
            catch (Exception)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

        }//delete code ends 
    }
}
