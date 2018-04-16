using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.Owin.Security;
using Microsoft.Owin;
using System.Web.Caching;

namespace AuthenticationAPP
{
    public class MyAuthorizationServerProvider :OAuthAuthorizationServerProvider 
    {
        public AuthenticationProperties Properties { get; private set; }
        public Task Next { get; private set; }
        public string token = "TOKEN";

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            if (context.UserName == "admin" && context.Password == "admin")
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, "admin"));
                identity.AddClaim(new Claim("username", "user"));
                identity.AddClaim(new Claim(ClaimTypes.Name, "sumishra"));
                AuthenticationTicket ticket = new AuthenticationTicket(identity, Properties);
                context.Response.Cookies.Append("Token", context.Options.AccessTokenFormat.Protect(ticket));
                context.Validated(identity);

            }
            else if (context.UserName == "user" && context.Password == "user")
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, "user"));
                identity.AddClaim(new Claim("username", "user"));
                identity.AddClaim(new Claim(ClaimTypes.Name, "Suman Mishra"));
                AuthenticationTicket ticket = new AuthenticationTicket(identity, Properties);
                context.Response.Cookies.Append("Token", context.Options.AccessTokenFormat.Protect(ticket));
                context.Validated(identity);
            }
            else
            {
                context.SetError("invalid_grant", "Provided username and password is incorrect");
                return;

            }
           
        }

        public override Task TokenEndpointResponse(OAuthTokenEndpointResponseContext context)
        {
            
            var accessToken = context.AccessToken;
            HttpRuntime.Cache.Insert(token, accessToken, null, DateTime.Now.AddHours(1), Cache.NoSlidingExpiration);
            return Task.FromResult<object>(null);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        

      





    }
}