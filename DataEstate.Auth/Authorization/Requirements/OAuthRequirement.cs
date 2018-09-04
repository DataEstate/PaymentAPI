using Microsoft.AspNetCore.Authorization;

namespace DataEstate.Auth.Authorization
{
    public class OAuthRequirement: IAuthorizationRequirement
    {
        public readonly string Issuer;
        public readonly string Scope;

        public OAuthRequirement(string scope, string issuer)
        {
            Issuer = issuer;
            Scope = scope;
        }
    }
}
