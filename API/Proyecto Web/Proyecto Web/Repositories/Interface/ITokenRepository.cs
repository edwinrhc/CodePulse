using Microsoft.AspNetCore.Identity;

namespace Proyecto_Web.Repositories.Interface
{
    public interface ITokenRepository
    {
        string CreateJwtToken(IdentityUser user, List<string> roles);
    }
}
