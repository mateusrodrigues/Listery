using IdentityServer3.Core.Services.Default;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IdentityServer3.Core.Models;
using System.Threading.Tasks;
using Listery.Domain;
using Listery.Repository.Persistence;
using IdentityServer3.Core.Extensions;
using System.Security.Claims;

namespace Listery.IdentityServer.Services
{
    public class CustomUserService : UserServiceBase
    {
        public override Task AuthenticateLocalAsync(LocalAuthenticationContext context)
        {
            // Gets user from the database
            User user = null;
            using (var db = new UnitOfWork())
            {
                user = db.Users.GetWithClaimsForIdentity(context.UserName, context.Password);
            }

            // Verify the user returned from the database, if any
            if (user == null)
            {
                context.AuthenticateResult = new AuthenticateResult("Invalid Credentials");
                return Task.FromResult(0);
            }

            context.AuthenticateResult = new AuthenticateResult(
                user.Subject.ToString(),
                user.Claims.First(c => c.ClaimType == IdentityServer3.Core.Constants.ClaimTypes.Name).ClaimValue);

            return Task.FromResult(0);
        }

        public override Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            // Find the user
            User user = null;

            using (var db = new UnitOfWork())
            {
                user = db.Users.GetWithClaims(Guid.Parse(context.Subject.GetSubjectId()));
            }

            // Add subject as claim
            var claims = new List<Claim>
            {
                new Claim(IdentityServer3.Core.Constants.ClaimTypes.Subject, user.Subject.ToString())
            };

            // Add the other user claims
            claims.AddRange(user.Claims.Select<UserClaim, Claim>(
                uc => new Claim(uc.ClaimType, uc.ClaimValue)));

            // Only return the requested claims
            if (!context.AllClaimsRequested)
                claims = claims.Where(x => context.RequestedClaimTypes.Contains(x.Type)).ToList();

            // Set the issued claims -> these are the ones that were requested, if available
            context.IssuedClaims = claims;
            return Task.FromResult(0);
        }

        public override Task IsActiveAsync(IsActiveContext context)
        {
            if (context.Subject == null)
                throw new ArgumentNullException("subject");

            // Find the user
            User user = null;

            using (var db = new UnitOfWork())
            {
                user = db.Users.Get(Guid.Parse(context.Subject.GetSubjectId()));
            }

            // Set whether or not the user is active
            context.IsActive = (user != null) && user.IsActive;

            return Task.FromResult(0);
        }
    }
}