using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Linq;

namespace DataEstate.Auth.Authorization
{
    public class OAuthClientCredentialHandler: AuthorizationHandler<OAuthRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OAuthRequirement requirement)
        {
            // Unauthorized user, if sent without scope. 
            if (!context.User.HasClaim(c=>c.Type=="scope" && c.Issuer == requirement.Issuer))
            {
                return Task.CompletedTask;
            }
            //Check expiry...

            // Split the scopes string into an array
            var scopes = context.User.FindFirst(c => c.Type == "scope" && c.Issuer == requirement.Issuer).Value.Split(' ');
            // Succeed if the scope array contains the required scope
            if (scopes.Any(s => s == requirement.Scope))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }

}
