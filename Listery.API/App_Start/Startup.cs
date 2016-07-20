using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.IdentityModel.Tokens;
using System.Collections.Generic;
using Listery.Shared;
using IdentityServer3.AccessTokenValidation;

[assembly: OwinStartup(typeof(Listery.API.App_Start.Startup))]

namespace Listery.API.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();

            app.UseIdentityServerBearerTokenAuthentication(
                new IdentityServerBearerTokenAuthenticationOptions
                {
                    Authority = Constants.STS,
                    RequiredScopes = new[] { "apiaccess" }
                });
        }
    }
}
