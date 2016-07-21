using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Web.Helpers;
using System.IdentityModel.Tokens;
using System.Collections.Generic;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Listery.Shared;
using System.Security.Claims;
using IdentityModel.Client;
using Microsoft.IdentityModel.Protocols;

[assembly: OwinStartup(typeof(Listery.Web.App_Start.Startup))]

namespace Listery.Web.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            AntiForgeryConfig.UniqueClaimTypeIdentifier =
                IdentityModel.JwtClaimTypes.Name;

            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();

            // Configure cookie authentication, including expiration time
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "Cookies",
                ExpireTimeSpan = new TimeSpan(0, 30, 0),
                SlidingExpiration = true
            });

            // OpenID configuration
            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                ClientId = "listeryweb",
                ClientSecret = Constants.WebClientSecret,
                Authority = Constants.STS,
                RedirectUri = Constants.WebAddress,
                SignInAsAuthenticationType = "Cookies",
                ResponseType = "code id_token token",
                Scope = "profile email openid offline_access api_access",
                UseTokenLifetime = true,
                PostLogoutRedirectUri = Constants.WebAddress,

                Notifications = new OpenIdConnectAuthenticationNotifications()
                {
                    SecurityTokenValidated = n =>
                    {
                        var id = n.AuthenticationTicket.Identity;

                        var subClaim = id.FindFirst(IdentityModel.JwtClaimTypes.Subject);

                        var nameClaim = id.FindFirst(IdentityModel.JwtClaimTypes.Name);
                        var emailClaim = id.FindFirst(IdentityModel.JwtClaimTypes.Email);

                        var newClaimsIdentity = new ClaimsIdentity(
                            n.AuthenticationTicket.Identity.AuthenticationType,
                            IdentityModel.JwtClaimTypes.Name,
                            IdentityModel.JwtClaimTypes.Role);

                        newClaimsIdentity.AddClaim(new Claim("id_token", n.ProtocolMessage.IdToken));

                        if (nameClaim != null)
                            newClaimsIdentity.AddClaim(nameClaim);

                        if (emailClaim != null)
                            newClaimsIdentity.AddClaim(emailClaim);

                        newClaimsIdentity.AddClaim(subClaim);

                        // Request a refresh token
                        var tokenClientForRefreshToken = new TokenClient(
                            Constants.STSTokenEndpoint,
                            "listeryweb",
                            Constants.WebClientSecret);

                        var refreshResponse = tokenClientForRefreshToken.RequestAuthorizationCodeAsync(
                            n.ProtocolMessage.Code,
                            Constants.WebAddress);

                        var expirationDateAsRoundTripString =
                            DateTime.SpecifyKind(DateTime.UtcNow.AddSeconds(refreshResponse.Result.ExpiresIn),
                            DateTimeKind.Utc).ToString("o");

                        newClaimsIdentity.AddClaim(new Claim("refresh_token", refreshResponse.Result.RefreshToken));
                        newClaimsIdentity.AddClaim(new Claim("access_token", refreshResponse.Result.AccessToken));
                        newClaimsIdentity.AddClaim(new Claim("expires_at", expirationDateAsRoundTripString));
                        newClaimsIdentity.AddClaim(new Claim("id_token", refreshResponse.Result.IdentityToken));

                        // Create a new authentication ticket, overwriting the old one
                        n.AuthenticationTicket = new Microsoft.Owin.Security.AuthenticationTicket(
                            newClaimsIdentity,
                            n.AuthenticationTicket.Properties);

                        return Task.FromResult(0);
                    },

                    RedirectToIdentityProvider = n =>
                    {
                        if (n.ProtocolMessage.RequestType == OpenIdConnectRequestType.LogoutRequest)
                        {
                            var idTokenHint = n.OwinContext.Authentication.User.FindFirst("id_token");

                            if (idTokenHint != null)
                                n.ProtocolMessage.IdTokenHint = idTokenHint.Value;
                        }

                        return Task.FromResult(0);
                    }
                }
            });
        }
    }
}