using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using SnapEd.Api.App_Start;
using SnapEd.Api.Controllers;
using SnapEd.Infra.DataContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace SnapEd.Api.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;

        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }

            _publicClientId = publicClientId;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            //context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            //context.OwinContext.Response.Headers.Add("Access-Control-Allow-Headers", new[] { "authorization", "Content-Type" });
            //context.OwinContext.Response.Headers.Add("Access-Control-Allow-Methods",new[] { "PUT, GET, POST, DELETE, OPTIONS" });


            try
            {
                //var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();
                var user = context.UserName;
                var password = context.Password;
                //ApplicationUser user = await userManager.FindAsync(context.UserName, context.Password);
                SnapEdDataContext db = new SnapEdDataContext();
                //ApplicationDbContext dbContext = context.OwinContext.Get<ApplicationDbContext>();
                ApplicationUserManager userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();

                var dbUser = db.Users.SingleOrDefault(x => x.Login == user);
                var pass = Cryptography.GetMD5Hash(password);
                var perm = dbUser.perm.ToString();

                if (user == null)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }

                if (pass != dbUser.Password)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }


                //if (userManager.PasswordHasher.VerifyHashedPassword(user,password) == Microsoft.AspNet.Identity.PasswordVerificationResult.Failed)
                //{
                //    context.SetError("invalid_grant", "The user name or password is incorrect.");
                //    return;
                //}

                //var roles = new List<string>();
                //roles.Add("Administrator");
                //roles.Add("User");


                var identity = new ClaimsIdentity(context.Options.AuthenticationType);

                //foreach (var role in roles)
                //{
                identity.AddClaim(new Claim(ClaimTypes.Role, perm));
                //}


                identity.AddClaim(new Claim(ClaimTypes.Name, user));
                //identity.AddClaim(new Claim(ClaimTypes.GivenName, user.firstName));

                GenericPrincipal principal = new GenericPrincipal(identity, null);
                Thread.CurrentPrincipal = principal;

                context.Validated(identity);
            }
            catch (Exception ex)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
            }


            //ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager,
            //   OAuthDefaults.AuthenticationType);
            //ClaimsIdentity cookiesIdentity = await user.GenerateUserIdentityAsync(userManager,
            //    CookieAuthenticationDefaults.AuthenticationType);

            //AuthenticationProperties properties = CreateProperties(user.UserName);
            //AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
            //context.Validated(ticket);
            //context.Request.Context.Authentication.SignIn(cookiesIdentity);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {

            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // Resource owner password credentials does not provide a client ID.
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public override Task MatchEndpoint(OAuthMatchEndpointContext context)
        {
            if (context.IsTokenEndpoint && context.Request.Method == "OPTIONS")
            {
                //context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
                //context.OwinContext.Response.Headers.Add("Access-Control-Allow-Headers", new[] { "authorization", "Content-Type" });
                //context.OwinContext.Response.Headers.Add("Access-Control-Allow-Methods", new[] { "PUT, GET, POST, DELETE, OPTIONS" });

                context.RequestCompleted();
                return Task.FromResult(0);
            }

            return base.MatchEndpoint(context);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(string userName)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName }
            };
            return new AuthenticationProperties(data);
        }
    }
}