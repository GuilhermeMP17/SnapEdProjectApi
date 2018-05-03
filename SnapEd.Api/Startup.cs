using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;
using SnapEd.Api.App_Start;
using SnapEd.Api.Dependency;
using SnapEd.Api.Providers;
using SnapEd.Infra.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

[assembly: OwinStartup(typeof(SnapEd.Api.Startup))]

namespace SnapEd.Api
{
    public class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthServerOptions { get; private set; }
        public static string PublicClientId { get; private set; }

        public void Configuration(IAppBuilder app)
        {
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            // configurando retorno Json
            ConfigureOAuth(app);
            HttpConfiguration config = new HttpConfiguration();


            var container = new UnityContainer();
            DependencyResolver.Resolve(container);
            config.DependencyResolver = new UnityResolver(container);

            ConfigureWebApi(config);
            //ConfigureOAuth(app, container.Resolve<IUserService>());

            app.UseWebApi(config);
            //HttpConfiguration config = new HttpConfiguration();

            //ConfigureWebApi(config);
            //ConfigureOAuth(app);

            //var container = new UnityContainer();
            //DependencyResolver.Resolve(container);
            //config.DependencyResolver = new UnityResolver(container);

            //app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            //app.CreatePerOwinContext(ApplicationDbContext.Create);
            //app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            //app.UseCookieAuthentication(new CookieAuthenticationOptions());
            //app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
            //app.UseWebApi(config);
        }

        public static void ConfigureWebApi(HttpConfiguration config)
        {
            var formatters = config.Formatters;
            formatters.Remove(formatters.XmlFormatter);

            // Modifica a identação
            var jsonSettings = formatters.JsonFormatter.SerializerSettings;
            jsonSettings.Formatting = Formatting.Indented;
            jsonSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            // Modifica a serialização
            formatters.JsonFormatter.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;

            // Web API routes
            config.MapHttpAttributeRoutes();

            var corsAttr = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(corsAttr);

            //config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);
            config.Routes.MapHttpRoute(
            name: "DefaultApi",
            routeTemplate: "api/{controller}/{id}",
            defaults: new { id = RouteParameter.Optional }
                 );

            config.Routes.MapHttpRoute(
                    name: "ApiByAction",
                    routeTemplate: "api/{controller}/{action}",
                    defaults: new { action = "Get" }
                );

            config.Routes.MapHttpRoute(
                name: "ApiByActionAndName",
                routeTemplate: "api/{controller}/{action}/{name}",
                defaults: new
                {
                    name = RouteParameter.Optional
                });
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            {
                app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
                app.CreatePerOwinContext(ApplicationDbContext.Create);
                app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

                // Enable the application to use a cookie to store information for the signed in user
                // and to use a cookie to temporarily store information about a user logging in with a third party login provider
                app.UseCookieAuthentication(new CookieAuthenticationOptions());
                app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

                // Configure the application for OAuth based flow
                PublicClientId = "self";
                OAuthServerOptions = new OAuthAuthorizationServerOptions
                {
                    AllowInsecureHttp = true,
                    TokenEndpointPath = new PathString("/api/security/token"),
                    Provider = new ApplicationOAuthProvider(PublicClientId),
                    AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
                    AccessTokenExpireTimeSpan = TimeSpan.FromDays(2),
                    // In production mode set AllowInsecureHttp = false
                };

                // Enable the application to use bearer tokens to authenticate users
                app.UseOAuthBearerTokens(OAuthServerOptions);

                // Uncomment the following lines to enable logging in with third party login providers
                //app.UseMicrosoftAccountAuthentication(
                //    clientId: "",
                //    clientSecret: "");

                //app.UseTwitterAuthentication(
                //    consumerKey: "",
                //    consumerSecret: "");

                //app.UseFacebookAuthentication(
                //    appId: "",
                //    appSecret: "");

                //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
                //{
                //    ClientId = "",
                //    ClientSecret = ""
                //});
            }

        }
    }
}