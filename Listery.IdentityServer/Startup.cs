using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using IdentityServer3.Core.Configuration;
using Listery.IdentityServer.Configuration;
using Listery.IdentityServer.Services;
using IdentityServer3.Core.Services;

[assembly: OwinStartup(typeof(Listery.IdentityServer.Startup))]

namespace Listery.IdentityServer
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Map("/identity", idsrv =>
            {
                var idServerServiceFactory = new IdentityServerServiceFactory()
                        .UseInMemoryClients(Clients.Get())
                        .UseInMemoryScopes(Scopes.Get());

                var customUserService = new CustomUserService();
                idServerServiceFactory.UserService =
                        new Registration<IUserService>(resolver => customUserService);
                // TODO Create constants library for project and finalize IdSrv configuration
                var options = new IdentityServerOptions
                {
                    Factory = idServerServiceFactory,
                    SiteName = "Listery Identity",
                    // IssuerUri = "",
                    // PublicOrigin = "",
                    // SigningCertificate = LoadCertificate(),
                    AuthenticationOptions = new AuthenticationOptions()
                    {
                        EnableSignOutPrompt = true,
                        EnablePostSignOutAutoRedirect = false
                    }
                };

                idsrv.UseIdentityServer(options);
            });
        }
    }
}
