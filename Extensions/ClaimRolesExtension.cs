using Blog.Models;
using System.Security.Claims;

namespace Blog.Extensions
{
    public static class ClaimRolesExtension
    {
        public static IEnumerable<Claim> GetClaims(this User user)
        {
            var results = new List<Claim>
            {
                new (ClaimTypes.Email, user.Email)
            };
            results.AddRange(user.Roles.Select(x => new Claim(ClaimTypes.Role, x.Slug)));

            return results;
        }
    }
}
