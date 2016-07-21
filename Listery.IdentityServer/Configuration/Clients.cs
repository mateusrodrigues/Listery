using IdentityServer3.Core.Models;
using Listery.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Listery.IdentityServer.Configuration
{
    public static class Clients
    {
        public static IEnumerable<Client> Get()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "listeryweb",
                    ClientName = "Listery Web",
                    Flow = Flows.Hybrid,
                    AllowedScopes = new List<string>
                    {
                        "openid", "profile", "email", "offline_access", "api_access"
                    },
                    IdentityTokenLifetime = 3600, // 1 hour
                    AccessTokenLifetime = 3600,   // 1 hour
                    RequireConsent = true,

                    RedirectUris = new List<string>
                    {
                        Constants.WebAddress
                    },

                    ClientSecrets = new List<Secret>
                    {
                        new Secret(Constants.WebClientSecret.Sha256())
                    },

                    PostLogoutRedirectUris = new List<string>
                    {
                        Constants.WebAddress
                    }
                }
            };
        }
    }
}