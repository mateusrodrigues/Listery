using Listery.Domain;
using Listery.Repository.Persistence;
using Listery.Shared;
using Listery.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Listery.API.Controllers
{
    public class UsersController : ApiController
    {
        // GET: /User/{id}
        [HttpGet]
        [Route("api/user/{id:guid}")]
        public IHttpActionResult Get(Guid id, bool withClaims = false)
        {
            using (var db = new UnitOfWork())
            {
                var user = (withClaims) ? db.Users.GetWithClaims(id) : db.Users.Get(id);
                if (user != null)
                    return Ok(user);
            }

            return NotFound();
        }

        // POST: /Users
        [HttpPost]
        public IHttpActionResult Post(AddUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var db = new UnitOfWork())
                {
                    if (db.Users.FindByEmail(model.Email) != null)
                        return BadRequest("Email already taken");

                    if (db.Users.FindByUsername(model.Username) != null)
                        return BadRequest("Username already taken");

                    var user = new User
                    {
                        Username = model.Username,
                        Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
                        IsActive = true,

                        Claims = new List<UserClaim>
                        {
                            new UserClaim { ClaimType = IdentityModel.JwtClaimTypes.Name, ClaimValue = model.FullName },
                            new UserClaim { ClaimType = IdentityModel.JwtClaimTypes.Email, ClaimValue = model.Email }
                        }
                    };

                    db.Users.Add(user);
                    
                    if (db.Complete() > 0)
                    {
                        return Created<User>($"{Constants.APIAddress}api/user/{user.Subject}", user);
                    }
                }
            }

            return BadRequest("Model posted is not valid.");
        }
    }
}
