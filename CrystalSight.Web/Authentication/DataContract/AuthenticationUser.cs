using Microsoft.AspNetCore.Identity;

namespace CrystalSight.Web.Authentication.DataContract
{
    public class ApplicationUser : IdentityUser
    {
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
