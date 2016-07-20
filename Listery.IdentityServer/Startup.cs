using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using IdentityServer3.Core.Configuration;
using Listery.IdentityServer.Configuration;
using Listery.IdentityServer.Services;
using IdentityServer3.Core.Services;
using Listery.Shared;
using System.Security.Cryptography.X509Certificates;
using System.Configuration;

[assembly: OwinStartup(typeof(Listery.IdentityServer.Startup))]

namespace Listery.IdentityServer
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            try
            {
                app.Map("/identity", idsrv =>
                    {
                        var idServerServiceFactory = new IdentityServerServiceFactory()
                                .UseInMemoryClients(Clients.Get())
                                .UseInMemoryScopes(Scopes.Get());

                        var customUserService = new CustomUserService();
                        idServerServiceFactory.UserService =
                                new Registration<IUserService>(resolver => customUserService);

                        var options = new IdentityServerOptions
                        {
                            Factory = idServerServiceFactory,
                            SiteName = "Listery Identity",
                            IssuerUri = Constants.IssuerUri,
                            PublicOrigin = Constants.STSOrigin,
                            SigningCertificate = LoadCertificate(),
                            AuthenticationOptions = new AuthenticationOptions()
                            {
                                EnableSignOutPrompt = true,
                                EnablePostSignOutAutoRedirect = false
                            }
                        };

                        idsrv.UseIdentityServer(options);
                    });
            }
            catch (Exception ex)
            {
                app.Run(context =>
                {
                    string t = "Error during creation of Identity Server middleware!\n\n";

                    string ASPNET_ENV = ConfigurationManager.AppSettings["ASPNET_ENV"] == null ?
                        "null" : ConfigurationManager.AppSettings["ASPNET_ENV"];
                    string CERT_THUMBPRINT = ConfigurationManager.AppSettings["CERT_THUMBPRINT"] == null ?
                        "null" : ConfigurationManager.AppSettings["CERT_THUMBPRINT"];

                    return context.Response.WriteAsync(t + $"ASPNET_ENV: {ASPNET_ENV}\n"
                        + $"CERT_THUMBPRINT: {CERT_THUMBPRINT}\n\n" + "Technical Info:\n" + ex.Message + "\n" + ex.StackTrace);
                });
            }
        }

        X509Certificate2 LoadCertificate()
        {
            if (ConfigurationManager.AppSettings["ASPNET_ENV"].Equals("Development"))
            {
                return new X509Certificate2(
                    $@"{AppDomain.CurrentDomain.BaseDirectory}\Certificates\idsrv3test.pfx",
                        "idsrv3test");
            }

            var thumbprint = Environment.GetEnvironmentVariable("CERT_THUMBPRINT");
            X509Store certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            certStore.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection certCollection = certStore.Certificates.Find(
                X509FindType.FindByThumbprint,
                thumbprint,
                false);

            // Get the first certificate with thumbprint
            if (certCollection.Count > 0)
            {
                X509Certificate2 cert = certCollection[0];
                return cert;
            }
            else
            {
                return null;
            }
        }
    }
}
