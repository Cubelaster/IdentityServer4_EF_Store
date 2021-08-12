using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServerAspNetIdentity.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityServerAspNetIdentity.Services
{
    public class ProfileService : IProfileService
    {
        protected UserManager<ApplicationUser> _userManager;

        public ProfileService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            //>Processing
            var user = await _userManager.GetUserAsync(context.Subject);

            var address = new
            {
                street_address = "One Hacker Way",
                locality = "Heidelberg",
                postal_code = 69118,
                country = "USA"
            };

            var custom_claims = new
            {
                full_name = "Full Cubelaster",
                dummy = "Dummy Cubelaster"
            };

            var claims = new List<Claim>
            {
                new Claim("full_name", "Cubelaster Full Name"),
                new Claim("dummy", "Dummy"),
                new Claim(JwtClaimTypes.Address, JsonSerializer.Serialize(address), IdentityServerConstants.ClaimValueTypes.Json),
                new Claim("custom_claims", JsonSerializer.Serialize(custom_claims), IdentityServerConstants.ClaimValueTypes.Json),
                new Claim("mvc_scope", JsonSerializer.Serialize(custom_claims), IdentityServerConstants.ClaimValueTypes.Json),
                new Claim(JwtClaimTypes.Name, "Cubelaster"),
                new Claim(JwtClaimTypes.GivenName, "Cubelaster"),
                new Claim(JwtClaimTypes.FamilyName, "Cubelaster"),
                new Claim(JwtClaimTypes.Email, "Cubelaster@cubelaster.com"),
                new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                new Claim(JwtClaimTypes.WebSite, "https://github.com/Cubelaster"),
            };

            context.IssuedClaims.AddRange(claims);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            //>Processing
            var user = await _userManager.GetUserAsync(context.Subject);

            context.IsActive = (user != null) && user.EmailConfirmed;
            //&& user.IsActive;
        }
    }
}
