using IdentityServer3.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Listery.IdentityServer.Configuration
{
    public static class Scopes
    {
        public static IEnumerable<Scope> Get()
        {
            return new List<Scope>
            {
                new Scope
                {
                    Name = "api_access",
                    DisplayName = "API Access",
                    Description = "Allows the client to call the API",
                    Type = ScopeType.Resource
                },

                StandardScopes.OpenId,
                StandardScopes.EmailAlwaysInclude,
                StandardScopes.ProfileAlwaysInclude,
                StandardScopes.OfflineAccess
            };
        }
    }
}