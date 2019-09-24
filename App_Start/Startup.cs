using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Web.Http;

// IMPORTANTE: Chamar o OwinStartup para sinalizar esta classe como inicializadora.
[assembly: OwinStartup(typeof(BearerTokenApp.App_Start.Startup))]

namespace BearerTokenApp.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Habilita o Cors. Pode tirar essa linha daqui e inicializar o CORS na classe WebApiConfig.cs também, passando 
            // endereços e headers específicos, como é feito aqui:
            // (https://docs.microsoft.com/pt-br/aspnet/web-api/overview/security/enabling-cross-origin-requests-in-web-api) 
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            // Cria a central de autenticação
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"), // MAGIA!
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new MinhaOAuthAuthorizationServerProvider()
            };

            // Configura a geração de Tokens
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            // Liga com o WebConfig
            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);

            // Liga com a Configuração global
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}