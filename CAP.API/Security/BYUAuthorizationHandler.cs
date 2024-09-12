using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace CAP.API.Security

{
    /// <summary>
    /// The actual checking of the permissions happens here in the handler
    /// </summary>
    public class BYUAuthorizationHandler :
        AuthorizationHandler<BYUAuthorizationRequirement>
    {
        public BYUAuthorizationHandler()
        {
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            BYUAuthorizationRequirement requirement)
        {
            var netId = context.User.Identity?.Name;
            if (string.IsNullOrEmpty(netId))
            {
                context.Fail();
                return Task.FromResult(0);
            }

            if (!requirement.AllowedRoles.Any(r => context.User.IsInRole(r)))
            {
                context.Fail();
                return Task.FromResult(0);
            }

            context.Succeed(requirement);
            return Task.FromResult(0);
        }
    }


    public class BYUAuthorizationRequirement : IAuthorizationRequirement
    {
        public string Type { get; set; }
        public string[] AllowedRoles { get; set; }

        public BYUAuthorizationRequirement(string[] allowedRoles, string authorizationType = "JWT")
        {
            Type = authorizationType;
            AllowedRoles = allowedRoles;
        }
    }
}