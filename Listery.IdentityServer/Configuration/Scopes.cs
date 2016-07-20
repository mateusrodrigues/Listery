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
                StandardScopes.OpenId,
                StandardScopes.EmailAlwaysInclude,
                StandardScopes.ProfileAlwaysInclude,
                StandardScopes.OfflineAccess
            };
        }
    }
}