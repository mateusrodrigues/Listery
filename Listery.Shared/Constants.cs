using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listery.Shared
{
    public static class Constants
    {
        public static string APIAddress
        {
            get
            {
                var environment = Environment.GetEnvironmentVariable("ASPNET_ENV") != null ?
                    Environment.GetEnvironmentVariable("ASPNET_ENV") : null;

                if (environment == null)
                    environment = "Development";

                if (environment.Equals("Development"))
                {
                    return @"https://localhost:44337/";
                }
                else if (environment.Equals("Staging"))
                {
                    return @"";
                }
                else
                {
                    return @"";
                }
            }
        }

        public static string WebAddress
        {
            get
            {
                var environment = Environment.GetEnvironmentVariable("ASPNET_ENV") != null ?
                    Environment.GetEnvironmentVariable("ASPNET_ENV") : null;

                if (environment == null)
                    environment = "Development";

                if (environment.Equals("Development"))
                {
                    return @"https://localhost:44312/";
                }
                else if (environment.Equals("Staging"))
                {
                    return @"";
                }
                else
                {
                    return @"";
                }
            }
        }

        public static string STSOrigin
        {
            get
            {
                var environment = Environment.GetEnvironmentVariable("ASPNET_ENV") != null ?
                    Environment.GetEnvironmentVariable("ASPNET_ENV") : null;

                if (environment == null)
                    environment = "Development";

                if (environment.Equals("Development"))
                {
                    return @"https://localhost:44393/";
                }
                else if (environment.Equals("Staging"))
                {
                    return @"";
                }
                else
                {
                    return @"";
                }
            }
        }



        public static readonly string WebClientSecret = "b9dd9d5a-cbaf-42f9-8e34-c0d5b3a108f4";

        public static readonly string IssuerUri = "https://liste.ry/identity";

        public static readonly string STS = STSOrigin + "identity";
        public static readonly string STSTokenEndpoint = STS + "/connect/token";
        public static readonly string STSAuthorizationEndpoint = STS + "/connect/authorize";
        public static readonly string STSUserInfoEndpoint = STS + "/connect/userinfo";
        public static readonly string STSEndSessionEndpoint = STS + "/connect/endsession";
        public static readonly string STSRevokeTokenEndpoint = STS + "/connect/revocation";
    }
}
