using Microsoft.AspNetCore.Identity;

namespace Web2_p1.Repositories
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}